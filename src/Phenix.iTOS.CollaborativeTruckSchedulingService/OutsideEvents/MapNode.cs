namespace Phenix.iTOS.CollaborativeTruckSchedulingService.OutsideEvents;

/// <summary>
/// ��ͼ�ڵ�
/// </summary>
public readonly record struct MapNode(
    string Location, //λ�ã���ͼ���λ�ã�
    double LocationLng, //���ȣ���ͼ���λ�ã�
    double LocationLat, //γ�ȣ���ͼ���λ�ã�
    string NodeType //�ڵ����ͣ����� TopologicalMapNodeType ����ֵ��ת��
    );