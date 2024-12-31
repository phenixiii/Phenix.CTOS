using Dapr.Actors;

namespace Phenix.iTOS.CollaborativeTruckSchedulingService.Actors;

/// <summary>
/// ������
/// </summary>
public interface ITruckPoolsActor : IActor
{
    /// <summary>
    /// ��ʼ��
    /// </summary>
    Task Init(string[] truckNos);

    /// <summary>
    /// ����
    /// </summary>
    public Task Invalid();

    /// <summary>
    /// �ָ�
    /// </summary>
    public Task Resume();

    /// <summary>
    /// �����µ���������
    /// </summary>
    public Task HandleNewCarryingTaskAsync(OutsideEvents.CarryingTask msg);
}