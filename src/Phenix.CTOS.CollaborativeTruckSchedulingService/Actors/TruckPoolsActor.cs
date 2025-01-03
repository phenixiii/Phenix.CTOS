using Dapr.Actors;
using Dapr.Actors.Runtime;
using Newtonsoft.Json;
using Phenix.Core.Mapper.Expressions;
using Phenix.CTOS.CollaborativeTruckSchedulingService.Models;

namespace Phenix.CTOS.CollaborativeTruckSchedulingService.Actors;

/// <summary>
/// ������
/// </summary>
/// ID: $"{{\"TruckNo\":\"{msg.TerminalNo}\",\"DriveType\":\"{msg.TruckPoolsNo}\"}}"
public class TruckPoolsActor : Actor, IRemindable, ITruckPoolsActor
{
    public TruckPoolsActor(ActorHost host)
        : base(host)
    {
        dynamic? truckPools = JsonConvert.DeserializeObject<dynamic>(this.Id.ToString());
        if (truckPools == null)
            throw new NotSupportedException($"��{this.GetType().FullName}��֧����'{this.Id}'��ʽ����TruckPools����!");

        _truckPools = TruckPools.FetchRoot(p => p.TerminalNo == truckPools.TerminalNo && p.TruckPoolsNo == truckPools.TruckPoolsNo);
        _taskList = CarryingTask.FetchList(p => p.TerminalNo == truckPools.TerminalNo && p.TruckPoolsNo == truckPools.TruckPoolsNo && p.Status == CarryingTaskStatus.UnStart,
            OrderBy.Ascending<CarryingTask>(p => p.OriginateTime));
    }

    private TruckPools? _truckPools;
    private readonly IList<CarryingTask> _taskList;

    private const string _truckPoolsReminderName = "TruckPoolsReminder";
    private const string _truckPoolsTimerName = "TruckPoolsTimer";
    private ActorTimer? _timer;

    #region ����

    protected override async Task OnActivateAsync()
    {
        if (_truckPools != null && !_truckPools.Invalided)
            _timer = await this.RegisterTimerAsync(_truckPoolsTimerName, nameof(OnTimerCallBack), null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));
        await base.OnActivateAsync();
    }

    private async Task RegisterReminderAsync()
    {
        await this.RegisterReminderAsync(_truckPoolsReminderName, null, TimeSpan.FromMinutes(0), TimeSpan.FromMinutes(1));
    }

    private async Task UnRegisterReminderAsync()
    {
        await this.UnregisterReminderAsync(_truckPoolsReminderName);
    }

    Task IRemindable.ReceiveReminderAsync(string reminderName, byte[] state, TimeSpan dueTime, TimeSpan period)
    {
        return Task.CompletedTask;
    }
    
    private Task OnTimerCallBack(byte[] data)
    {
        Console.WriteLine("OnTimerCallBack is called!");
        return Task.CompletedTask;
    }

    #region API

    /// <summary>
    /// ��ʼ��
    /// </summary>
    public async Task InitAsync(string[] truckNos)
    {
        if (_truckPools == null)
        {
            dynamic? truckPools = JsonConvert.DeserializeObject<dynamic>(this.Id.ToString());
            _truckPools = TruckPools.Create(truckPools?.TerminalNo, truckPools?.TruckPoolsNo, truckNos);
            await RegisterReminderAsync();
        }
        else
            _truckPools.ReplaceTruckList(truckNos);
    }

    /// <summary>
    /// ����
    /// </summary>
    public async Task InvalidAsync()
    {
        if (_truckPools == null)
            throw new NotSupportedException($"��'{this.Id}'�����ػ�δ����, �޷�����!");

        _truckPools.Invalid();
        await UnRegisterReminderAsync();
    }

    /// <summary>
    /// �ָ�
    /// </summary>
    public async Task ResumeAsync()
    {
        if (_truckPools == null)
            throw new NotSupportedException($"��'{this.Id}'�����ػ�δ����, �޷��ָ�!");

        _truckPools.Resume();
        await RegisterReminderAsync();
    }

    /// <summary>
    /// �����µ���������
    /// </summary>
    public async Task HandleNewCarryingTaskAsync(OutsideEvents.CarryingTask msg)
    {
        if (_truckPools == null)
            throw new NotSupportedException($"��'{this.Id}'�����ػ�δ����, �޷������µ���������!");
        if (_truckPools.Invalided)
            throw new InvalidOperationException($"��ͷ'{_truckPools.TerminalNo}'������'{_truckPools.TruckPoolsNo}'����{_truckPools.InvalidedChangeTime}����, ���ܴ����µ���������!");

        CarryingTask task = CarryingTask.Create(
            msg.TerminalNo,
            msg.TruckPoolsNo,
            msg.TaskId,
            (Models.CarryingTaskType)Int32.Parse(msg.TaskType),
            msg.TaskPriority,
            msg.PlanContainerNumber,
            msg.PlanIsBigSize,
            msg.LoadLocation,
            msg.LoadQueueNo,
            msg.LoadCraneNo,
            msg.UnloadLocation,
            msg.UnloadQueueNo,
            msg.UnloadCraneNo,
            msg.NeedTwistLock,
            msg.QuayCraneProcess != null ? (Models.QuayCraneProcess)Int32.Parse(msg.QuayCraneProcess) : null);

        List<Task<TruckCarryingTask>> tasks = new List<Task<TruckCarryingTask>>(_truckPools.TruckList.Count);
        foreach (TruckPoolsTruck item in _truckPools.TruckList)
            tasks.Add(Task.Run(() => this.ProxyFactory.CreateActorProxy<ITruckActor>(new ActorId(item.TruckNo), nameof(TruckActor)).
                NewTaskAsync(task, (Models.TruckLoadingPosition)Int32.Parse(msg.PlanLoadingPosition))));
        await Task.WhenAll(tasks);
        foreach (Task<TruckCarryingTask> item in tasks)
        {

        }
    }

    #endregion

    #endregion
}