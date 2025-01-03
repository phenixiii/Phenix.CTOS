using Dapr.Actors;

namespace Phenix.CTOS.CollaborativeTruckSchedulingService.Actors;

/// <summary>
/// ������
/// </summary>
public interface ITruckPoolsActor : IActor
{
    /// <summary>
    /// ��ʼ��
    /// </summary>
    Task InitAsync(string[] truckNos);

    /// <summary>
    /// ����
    /// </summary>
    public Task InvalidAsync();

    /// <summary>
    /// �ָ�
    /// </summary>
    public Task ResumeAsync();

    /// <summary>
    /// �����µ���������
    /// </summary>
    public Task HandleNewCarryingTaskAsync(OutsideEvents.CarryingTask msg);
}