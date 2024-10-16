using Phenix.Core.SyncCollections;
using Phenix.iTOS.CollaborativeTruckSchedulingService.Common;

namespace Phenix.iTOS.CollaborativeTruckSchedulingService.Models;

/// <summary>
/// ���˵�ͼ����������
/// </summary>
public class TopologicalMapLane
{
    /// <summary>
    /// ���˵�ͼ����������
    /// </summary>
    /// <param name="id">ID</param>
    /// <param name="count">��������</param>
    /// <param name="entryJunction">��ڵ���</param>
    /// <param name="exitJunction">���ڵ���</param>
    /// <param name="nodes">�ڵ㣨��������������</param>
    public TopologicalMapLane(long id, int count, TopologicalMapJunction entryJunction, TopologicalMapJunction exitJunction, TopologicalMapNode[] nodes)
    {
        _id = id;
        _count = count;
        _entryJunction = entryJunction;
        _exitJunction = exitJunction;
        _nodes = nodes;

        entryJunction.AddExitLane(this);
        exitJunction.AddEntryLane(this);
    }

    private readonly long _id;

    /// <summary>
    /// ID
    /// </summary>
    public long Id
    {
        get { return _id; }
    }

    private readonly int _count;

    /// <summary>
    /// ��������
    /// </summary>
    public int Count
    {
        get { return _count; }
    }

    private readonly TopologicalMapJunction _entryJunction;

    /// <summary>
    /// ��ڵ���
    /// </summary>
    public TopologicalMapJunction EntryJunction
    {
        get { return _entryJunction; }
    }

    private readonly TopologicalMapJunction _exitJunction;

    /// <summary>
    /// ���ڵ���
    /// </summary>
    public TopologicalMapJunction ExitJunction
    {
        get { return _exitJunction; }
    }

    private readonly TopologicalMapNode[] _nodes;

    /// <summary>
    /// �ڵ㣨��������������
    /// </summary>
    public TopologicalMapNode[] Nodes
    {
        get { return _nodes; }
    }

    private readonly SynchronizedSortedDictionary<TopologicalMapNode, double> _reachNodeDistanceDict = new SynchronizedSortedDictionary<TopologicalMapNode, double>();

    /// <summary>
    /// ��ڵ��ڵ���ʻ���루�������нڵ㣩
    /// </summary>
    public IDictionary<TopologicalMapNode, double> ReachNodeDistanceDict
    {
        get
        {
            if (_reachNodeDistanceDict.Count == 0)
                for (int i = 0; i < _nodes.Length; i++)
                    _reachNodeDistanceDict[_nodes[i]] = i == 0
                        ? MapHelper.GetDistance(_entryJunction.Node.LocationLat, _entryJunction.Node.LocationLng, _nodes[i].LocationLat, _nodes[i].LocationLng)
                        : _reachNodeDistanceDict[_nodes[i - 1]] + MapHelper.GetDistance(_nodes[i - 1].LocationLat, _nodes[i - 1].LocationLng, _nodes[i].LocationLat, _nodes[i].LocationLng);

            return _reachNodeDistanceDict.AsReadOnly();
        }
    }

    private double? _reachExitJunctionDistance;

    /// <summary>
    /// ��ڵ�������ʻ���루�������нڵ㣩
    /// </summary>
    public double ReachExitJunctionDistance
    {
        get
        {
            if (!_reachExitJunctionDistance.HasValue)
                _reachExitJunctionDistance = _nodes.Length > 0
                    ? ReachNodeDistanceDict[_nodes[^1]] + MapHelper.GetDistance(_nodes[^1].LocationLat, Nodes[^1].LocationLng, _exitJunction.Node.LocationLat, _exitJunction.Node.LocationLng)
                    : MapHelper.GetDistance(_entryJunction.Node.LocationLat, _entryJunction.Node.LocationLng, _exitJunction.Node.LocationLat, _exitJunction.Node.LocationLng);

            return _reachExitJunctionDistance.Value;
        }
    }
}