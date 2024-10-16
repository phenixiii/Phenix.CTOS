namespace Phenix.iTOS.CollaborativeTruckSchedulingService.Models;

/// <summary>
/// ���˵�ͼ����
/// </summary>
public class TopologicalMapJunction
{
    /// <summary>
    /// ���˵�ͼ����
    /// </summary>
    /// <param name="node">�ڵ�</param>
    public TopologicalMapJunction(TopologicalMapNode node)
    {
        _node = node;
    }

    private readonly TopologicalMapNode _node;

    /// <summary>
    /// �ڵ�
    /// </summary>
    public TopologicalMapNode Node
    {
        get { return _node; }
    }

    private readonly List<TopologicalMapLane> _entryLaneList = new List<TopologicalMapLane>();

    /// <summary>
    /// ��ڳ���
    /// </summary>
    public TopologicalMapLane[] EntryLanes
    {
        get { return _entryLaneList.ToArray(); }
    }

    private readonly List<TopologicalMapLane> _exitLaneList = new List<TopologicalMapLane>();

    /// <summary>
    /// ���ڳ���
    /// </summary>
    public TopologicalMapLane[] ExitLanes
    {
        get { return _exitLaneList.ToArray(); }
    }

    internal void AddEntryLane(TopologicalMapLane lane)
    {
        _entryLaneList.Add(lane);
    }

    internal void AddExitLane(TopologicalMapLane lane)
    {
        _exitLaneList.Add(lane);
    }
}