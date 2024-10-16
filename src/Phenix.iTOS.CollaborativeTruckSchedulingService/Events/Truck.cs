namespace Phenix.iTOS.CollaborativeTruckSchedulingService.Events;

/// <summary>
/// ����
/// </summary>
public readonly record struct Truck(
    string TruckNo, //�������
    MapLane[] Lanes, //����
    MapNode[] Nodes, //�ڵ�
    DateTime Timestamp //ʱ���
    );