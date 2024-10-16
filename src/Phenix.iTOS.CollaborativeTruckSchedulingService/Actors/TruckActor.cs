using Dapr.Actors.Runtime;
using Newtonsoft.Json;
using Phenix.iTOS.CollaborativeTruckSchedulingService.Configs;
using Phenix.iTOS.CollaborativeTruckSchedulingService.Models;

namespace Phenix.iTOS.CollaborativeTruckSchedulingService.Actors;

/// <summary>
/// ����
/// ID: @"{'TruckNo':'','TruckType":1}"
/// </summary>
public class TruckActor : Actor, ITruckActor
{
    public TruckActor(ActorHost host)
        : base(host)
    {
    }

    private Truck? _truck;

    protected override async Task OnActivateAsync()
    {
        dynamic? truck = JsonConvert.DeserializeObject<dynamic>(this.Id.ToString());
        if (truck == null) 
            throw new NotSupportedException($"��{this.GetType().FullName}��֧����'{this.Id}'��ʽ��ID���й���!");

        _truck = await this.StateManager.GetOrAddStateAsync(StoreConfig.CarryingTask, new Truck(truck.TruckNo, truck.TruckType));
        await base.OnActivateAsync();
    }

    /// <summary>
    /// ����
    /// </summary>
    public async Task CarryAsync(CarryingTask task, TaskPostStatus status)
    {
    }
}