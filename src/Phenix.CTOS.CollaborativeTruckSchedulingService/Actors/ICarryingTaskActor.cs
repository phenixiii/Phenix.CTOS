using Dapr.Actors;
using Phenix.CTOS.CollaborativeTruckSchedulingService.Events;
using Phenix.CTOS.CollaborativeTruckSchedulingService.Models;

namespace Phenix.CTOS.CollaborativeTruckSchedulingService.Actors;

/// <summary>
/// ��������
/// </summary>
public interface ICarryingTaskActor : IActor
{
    /// <summary>
    /// ����
    /// </summary>
    public Task CarryAsync(CarryingTask task, TaskRequestType status);
}
