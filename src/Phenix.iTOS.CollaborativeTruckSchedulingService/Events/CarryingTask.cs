namespace Phenix.iTOS.CollaborativeTruckSchedulingService.Events;

/// <summary>
/// ��������
/// </summary>
public readonly record struct CarryingTask(
    string TerminalNo, //��ͷ���
    string TruckPoolsNo, //�����غ�
    long TaskId, //����ID
    string TaskType, //�������ͣ����� CarryingTaskType ����ֵ��ת��
    string TaskPostStatus, //���񷢲�״̬������ TaskPostStatus ����ֵ��ת��
    int TaskPriority, //�������ȼ�
    bool IsFullLoad, //�Ƿ�һ�����أ����������ټ�һ��С������
    string LoadCraneNo, //װ�أ��ص���������е��
    string LoadCraneType, //װ�ػ�е���ͣ����� CraneType ����ֵ��ת��
    string LoadLocation, //װ��λ�ã���ͼ���λ�ã�
    long LoadQueueNo, //װ���Ŷ���ţ�ͬһװ��λ���谴����Ŷӣ�ͬ�ſɽ�����
    string UnloadCraneNo, //ж�أ�ж�¼�������е��
    string UnloadCraneType, //ж�ػ�е���ͣ����� CraneType ����ֵ��ת��
    string UnloadLocation, //ж��λ�ã���ͼ���λ�ã�
    long UnloadQueueNo, //ж���Ŷ���ţ�ͬһж��λ���谴����Ŷӣ�ͬ�ſɽ�����
    string YLocation, //����λ
    string VLocation, //����λ
    string QuayCraneProcess, //���Ź��գ����� QuayCraneProcess ����ֵ��ת��
    bool NeedTwistLock, //�Ƿ���ҪŤ��������ťվ��
    DateTime Timestamp //ʱ���
    );