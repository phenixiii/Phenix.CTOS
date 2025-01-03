using Dapr.Actors;
using Phenix.CTOS.CollaborativeTruckSchedulingService.Models;

namespace Phenix.CTOS.CollaborativeTruckSchedulingService.Actors;

/// <summary>
/// ����
/// </summary>
public interface ITruckActor : IActor
{
    /// <summary>
    /// ����������
    /// </summary>
    public Task<TruckCarryingTask> NewTaskAsync(CarryingTask task, TruckLoadingPosition? position = null);
}