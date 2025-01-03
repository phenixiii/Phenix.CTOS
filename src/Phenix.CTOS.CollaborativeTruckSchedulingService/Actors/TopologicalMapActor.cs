using Dapr.Actors.Runtime;
using Phenix.CTOS.CollaborativeTruckSchedulingService.Configs;
using Phenix.CTOS.CollaborativeTruckSchedulingService.Models;

namespace Phenix.CTOS.CollaborativeTruckSchedulingService.Actors;

/// <summary>
/// ���˵�ͼ
/// </summary>
public class TopologicalMapActor : Actor, ITopologicalMapActor
{
    public TopologicalMapActor(ActorHost host)
        : base(host)
    {
    }

    private TopologicalMap? _map;

    protected override async Task OnActivateAsync()
    {
        _map = await this.StateManager.GetOrAddStateAsync(StoreConfig.TopologicalMap, _map);
        await base.OnActivateAsync();
    }

    /// <summary>
    /// �����򸲸ǵ�ͼ
    /// </summary>
    public async Task PutAsync(TopologicalMap map)
    {
        _map = map;
        await this.StateManager.SetStateAsync(StoreConfig.TopologicalMap, _map);
    }
}