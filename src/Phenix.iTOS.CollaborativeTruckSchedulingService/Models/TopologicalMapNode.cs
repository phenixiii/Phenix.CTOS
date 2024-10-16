namespace Phenix.iTOS.CollaborativeTruckSchedulingService.Models;

/// <summary>
/// ���˵�ͼ�ڵ�
/// </summary>
public class TopologicalMapNode
{
    /// <summary>
    /// ���˵�ͼ�ڵ�
    /// </summary>
    /// <param name="location">λ�ã���ͼ���λ�ã�</param>
    /// <param name="locationLng">���ȣ���ͼ���λ�ã�</param>
    /// <param name="locationLat">γ�ȣ���ͼ���λ�ã�</param>
    /// <param name="nodeType">�ڵ�����</param>
    public TopologicalMapNode(string location, double locationLng, double locationLat, TopologicalMapNodeType nodeType)
    {
        _location = location;
        _locationLng = locationLng;
        _locationLat = locationLat;
        _nodeType = nodeType;
    }

    private readonly string _location;

    /// <summary>
    /// λ�ã���ͼ���λ�ã�
    /// </summary>
    public string Location
    {
        get { return _location; }
    }

    private readonly double _locationLng;

    /// <summary>
    /// ���ȣ���ͼ���λ�ã�
    /// </summary>
    public double LocationLng
    {
        get { return _locationLng; }
    }

    private readonly double _locationLat;

    /// <summary>
    /// γ�ȣ���ͼ���λ�ã�
    /// </summary>
    public double LocationLat
    {
        get { return _locationLat; }
    }

    private readonly TopologicalMapNodeType _nodeType;

    /// <summary>
    /// �ڵ�����
    /// </summary>
    public TopologicalMapNodeType NodeType
    {
        get { return _nodeType; }
    }
}