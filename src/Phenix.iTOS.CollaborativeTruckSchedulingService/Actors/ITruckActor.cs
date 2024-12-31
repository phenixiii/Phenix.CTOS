using Dapr.Actors;
using Phenix.iTOS.CollaborativeTruckSchedulingService.Models;

namespace Phenix.iTOS.CollaborativeTruckSchedulingService.Actors;

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