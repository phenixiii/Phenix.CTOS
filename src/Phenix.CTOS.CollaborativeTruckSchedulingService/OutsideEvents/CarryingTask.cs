using System.Runtime.Serialization;

namespace Phenix.CTOS.CollaborativeTruckSchedulingService.OutsideEvents;

/// <summary>
/// ��������
/// </summary>
[DataContract]
public readonly record struct CarryingTask(
    [property: DataMember] string TerminalNo, //��ͷ���
    [property: DataMember] string TruckPoolsNo, //�����غ�
    [property: DataMember] long TaskId, //����ID
    [property: DataMember] string TaskType, //�������ͣ����� CarryingTaskType ����ֵ��ת��
    [property: DataMember] int TaskPriority, //�������ȼ�
    [property: DataMember] string PlanLoadingPosition, //�ƻ�����λ�ã����� TruckLoadingPosition ����ֵ��ת��
    [property: DataMember] string PlanContainerNumber, //�ƻ����
    [property: DataMember] bool PlanIsBigSize, //�ƻ��Ǵ���
    [property: DataMember] string LoadLocation, //װ��λ�ã���ͼ���λ�ã�
    [property: DataMember] long LoadQueueNo, //װ���Ŷ���ţ�ͬһװ��λ���谴����Ŷӣ�ͬ�ſɽ�����
    [property: DataMember] string? LoadCraneNo, //װ�أ��ص���������е��
    [property: DataMember] string UnloadLocation, //ж��λ�ã���ͼ���λ�ã�
    [property: DataMember] long UnloadQueueNo, //ж���Ŷ���ţ�ͬһװ��λ���谴����Ŷӣ�ͬ�ſɽ�����
    [property: DataMember] string? UnloadCraneNo, //ж�أ�ж�¼�������е��
    [property: DataMember] bool NeedTwistLock, //�Ƿ���ҪŤ��������ťվ��
    [property: DataMember] string? QuayCraneProcess //���Ź��գ����� QuayCraneProcess ����ֵ��ת��
);