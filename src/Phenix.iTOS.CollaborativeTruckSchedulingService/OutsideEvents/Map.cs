namespace Phenix.iTOS.CollaborativeTruckSchedulingService.OutsideEvents;

/// <summary>
/// ��ͼ
/// </summary>
public readonly record struct Map(
    string TerminalNo, //��ͷ���
    MapLane[] Lanes, //����
    MapNode[] Nodes, //�ڵ�
    DateTime Timestamp //ʱ���
    );