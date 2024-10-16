using Dapr.Actors;
using Phenix.iTOS.CollaborativeTruckSchedulingService.Models;

namespace Phenix.iTOS.CollaborativeTruckSchedulingService.Actors;

/// <summary>
/// ��������
/// </summary>
public interface ICarryingTaskActor : IActor
{
    /// <summary>
    /// ����
    /// </summary>
    public Task CarryAsync(CarryingTask task, TaskPostStatus status);
}
