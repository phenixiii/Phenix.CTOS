using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Phenix.Core;
using Phenix.Core.Log;
using Phenix.Core.Reflection;
using Phenix.CTOS.CollaborativeTruckSchedulingService.Actors;
using Phenix.CTOS.CollaborativeTruckSchedulingService.Configs;
using Phenix.CTOS.CollaborativeTruckSchedulingService.DomainServices;
using Serilog;
using Serilog.Events;

namespace Phenix.CTOS.CollaborativeTruckSchedulingService;

public static class Program
{
    public static void Main(string[] args)
    {
        //����Linux��CentOS������
        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("zh-CN", true)
        {
            DateTimeFormat = { ShortDatePattern = "yyyy-MM-dd", FullDateTimePattern = "yyyy-MM-dd HH:mm:ss", LongTimePattern = "HH:mm:ss" }
        };

#if DEBUG
        Log.Logger = LogHelper.InitializeConfiguration()
            .CreateLogger();
        AppRun.Debugging = true;
#else
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Exceptionless(
                restrictedToMinimumLevel: AppRun.Debugging ? LogEventLevel.Debug : LogEventLevel.Information, //�������С��־����
                includeProperties: true, //����Serilog����
                serverUrl: LogConfig.ServerUrl,
                apiKey: LogConfig.ApiKey)
            .CreateLogger();
        if (AppRun.Debugging)
            LogHelper.Warning("please set this Phenix.Core.AppRun.Debugging parameter to false in the production environment");
#endif

        try
        {
            using (WebApplication app = CreateWebApplication(args))
            {
                // let's go!
                app.Run("http://localhost:6000");
            }
        }
        catch (Exception ex)
        {
            LogHelper.Fatal(ex, "service terminated unexpectedly.");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static WebApplication CreateWebApplication(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        builder.Host.UseContentRoot(Phenix.Core.AppRun.BaseDirectory);

        //Logging
        builder.Host.ConfigureLogging(logging => logging.ClearProviders());
        builder.Host.UseSerilog(dispose: true);

        //DomainServices
        builder.Services.AddSingleton<IPathPlanningService, PathPlanningService>();
        builder.Services.AddSingleton<ITaskDispatchService, TaskDispatchService>();
        builder.Services.AddSingleton<ITrafficManagementService, TrafficManagementService>();
        builder.Services.AddSingleton<ITrajectoryPlanningService, TrajectoryPlanningService>();

        //DaprClient
        string daprHttpPort = Environment.GetEnvironmentVariable("DAPR_HTTP_PORT") ?? "3600";
        string daprGrpcPort = Environment.GetEnvironmentVariable("DAPR_GRPC_PORT") ?? "60000";
        builder.Services.AddDaprClient(clientBuilder => clientBuilder
            .UseJsonSerializationOptions(
                new JsonSerializerOptions()
                {
                    ReferenceHandler = ReferenceHandler.Preserve //�����л��ͷ����л���ʱ�������ò�����ѭ������
                })
            .UseHttpEndpoint($"http://localhost:{daprHttpPort}")
            .UseGrpcEndpoint($"http://localhost:{daprGrpcPort}"));

        //Dapr
        builder.Services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.DateFormatString = Utilities.JsonDateFormatString;
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                options.SerializerSettings.Formatting = Formatting.None;
                options.UseMemberCasing();
            })
            .AddDapr(clientBuilder =>
                clientBuilder.UseJsonSerializationOptions(
                    new JsonSerializerOptions()
                    {
                        ReferenceHandler = ReferenceHandler.Preserve //�����л��ͷ����л���ʱ�������ò�����ѭ������
                    }));

        //Actors
        builder.Services.AddActors(options => { options.Actors.RegisterActor<TopologicalMapActor>(); });
        builder.Services.AddActors(options => { options.Actors.RegisterActor<TruckActor>(); });
        builder.Services.AddActors(options => { options.Actors.RegisterActor<TruckPoolsActor>(); });
        builder.Services.AddActors(options => { options.Actors.RegisterActor<CarryingTaskActor>(); });

        WebApplication result = builder.Build();
        //configure middleware
        result.UseMiddleware<Phenix.Core.Net.Http.ExceptionHandlerMiddleware>(); //ʹ���쳣�����м��
        //configure web-app
        if (result.Environment.IsDevelopment())
            result.UseDeveloperExceptionPage();
        result.UseRouting();
        result.UseCloudEvents(); //CloudEvents��һ�ֱ�׼������Ϣ���ݸ�ʽ���ṩ��һ��ͨ�õķ�����������ƽ̨���¼���Ϣ������Dapr��pubsub���ܡ� 
        result.UseAuthorization();

        //configure routing
        result.MapSubscribeHandler(); //���һ��Dapr���Ķ˵㣬����˵㽫��Ӧ/dapr/subscribe�ϵ������Զ��ҵ�������Topic����װ�ε�WebAPI������������ָʾDaprΪ���Ǵ������ġ�
        result.MapActorsHandlers(); //Register actors handlers that interface with the Dapr runtime.
        result.MapControllers();

        return result;
    }
}