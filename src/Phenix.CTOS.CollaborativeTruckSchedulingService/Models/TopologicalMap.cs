namespace Phenix.CTOS.CollaborativeTruckSchedulingService.Models;

/// <summary>
/// ���˵�ͼ
/// </summary>
public class TopologicalMap
{
    /// <summary>
    /// ���˵�ͼ
    /// </summary>
    /// <param name="terminalNo">��ͷ���</param>
    /// <param name="inGateDict">����բ��ö��</param>
    /// <param name="nodeDict">�ڵ�ö��</param>
    public TopologicalMap(string terminalNo, IDictionary<string, TopologicalMapJunction> inGateDict, IDictionary<string, TopologicalMapNode> nodeDict)
    {
        _terminalNo = terminalNo;
        _inGateDict = inGateDict;
        _nodeDict = nodeDict;
    }

    private readonly string _terminalNo;

    /// <summary>
    /// ��ͷ���
    /// </summary>
    public string TerminalNo
    {
        get { return _terminalNo; }
    }

    private readonly IDictionary<string, TopologicalMapJunction> _inGateDict;

    /// <summary>
    /// ����բ��ö��
    /// </summary>
    public IDictionary<string, TopologicalMapJunction> InGateDict
    {
        get { return _inGateDict; }
    }

    private readonly IDictionary<string, TopologicalMapNode> _nodeDict;

    /// <summary>
    /// �ڵ�ö��
    /// </summary>
    public IDictionary<string, TopologicalMapNode> NodeDict
    {
        get { return _nodeDict; }
    }
}