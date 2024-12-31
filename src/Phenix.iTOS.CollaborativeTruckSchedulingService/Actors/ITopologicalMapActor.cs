using Dapr.Actors;
using Phenix.iTOS.CollaborativeTruckSchedulingService.Models;

namespace Phenix.iTOS.CollaborativeTruckSchedulingService.Actors;

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