namespace Phenix.iTOS.CollaborativeTruckSchedulingService.OutsideEvents;

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