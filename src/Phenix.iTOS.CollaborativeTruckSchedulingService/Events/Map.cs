namespace Phenix.iTOS.CollaborativeTruckSchedulingService.Events;

/// <summary>
/// ��ͼ
/// </summary>
public readonly record struct Map(
    string TerminalNo, //��ͷ���
    MapLane[] Lanes, //����
    MapNode[] Nodes, //�ڵ�
    DateTime Timestamp //ʱ���
    );