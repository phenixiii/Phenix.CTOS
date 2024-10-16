namespace Phenix.iTOS.CollaborativeTruckSchedulingService.Events;

/// <summary>
/// ����������ҵ
/// </summary>
public record struct CarryingTaskOperation(
    long TaskId, //����ID
    string OperationStatus, //��ҵ״̬������ CarryingTaskOperationStatus ����ֵ��ת��
    string TruckNo, //������
    string LoadingPosition, //����λ�ã����� LoadingPosition ����ֵ��ת��
    bool ShoreShiftUp, //���ϵ�
    DateTime Timestamp);