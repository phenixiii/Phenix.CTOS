using Dapr.Actors;
using Phenix.CTOS.CollaborativeTruckSchedulingService.Models;

namespace Phenix.CTOS.CollaborativeTruckSchedulingService.Actors;

/// <summary>
/// ���˵�ͼ
/// </summary>
public interface ITopologicalMapActor : IActor
{
    /// <summary>
    /// �����򸲸ǵ�ͼ
    /// </summary>
    public Task PutAsync(TopologicalMap map);
}