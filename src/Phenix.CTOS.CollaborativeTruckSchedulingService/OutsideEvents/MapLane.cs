namespace Phenix.CTOS.CollaborativeTruckSchedulingService.OutsideEvents;

/// <summary>
/// ��ͼ����
/// </summary>
public readonly record struct MapLane(
    long Id, //ID
    int Count, //��������
    string EntryJunctionLocation, //��ڵ���λ�ã���ͼ���λ�ã�
    string ExitJunctionLocation, //���ڵ���λ�ã���ͼ���λ�ã�
    string[] NodeLocations //�ڵ�λ�ã���������������
    );