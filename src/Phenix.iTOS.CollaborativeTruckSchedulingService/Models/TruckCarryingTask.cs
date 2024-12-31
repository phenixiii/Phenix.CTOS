using Phenix.Core.Mapper;
using Phenix.Core.Mapper.Schema;

namespace Phenix.iTOS.CollaborativeTruckSchedulingService.Models;

/// <summary>
/// ������������
/// </summary>
[Sheet("CTS_TRUCK_CARRYING_TASK")]
public class TruckCarryingTask : EntityBase<TruckCarryingTask>
{
    #region ����

    private readonly long _ID;

    /// <summary>
    /// ID
    /// </summary>
    public long ID
    {
        get { return _ID; }
    }

    private readonly long _CTC_ID;

    /// <summary>
    /// ����
    /// </summary>
    public long CTC_ID
    {
        get { return _CTC_ID; }
    }

    private readonly string _terminalNo;

    /// <summary>
    /// ��ͷ���
    /// </summary>
    public string TerminalNo
    {
        get { return _terminalNo; }
    }

    private readonly string _truckPoolsNo;

    /// <summary>
    /// �����غ�
    /// </summary>
    public string TruckPoolsNo
    {
        get { return _truckPoolsNo; }
    }

    private readonly long _taskId;

    /// <summary>
    /// ����ID
    /// </summary>
    public long TaskId
    {
        get { return _taskId; }
    }

    private readonly TruckLoadingPosition _loadingPosition;

    /// <summary>
    /// ����λ��
    /// </summary>
    public TruckLoadingPosition LoadingPosition
    {
        get { return _loadingPosition; }
    }

    private readonly DateTime _originateTime;

    /// <summary>
    /// �Ƶ�ʱ��
    /// </summary>
    public DateTime OriginateTime
    {
        get { return _originateTime; }
    }

    #region Relate

    [NonSerialized]
    private CarryingTask? _task;

    /// <summary>
    /// ��������
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public CarryingTask Task
    {
        get
        {
            if (_task == null)
                _task = CarryingTask.FetchRoot(p => p.TerminalNo == TerminalNo && p.TruckPoolsNo == TruckPoolsNo && p.TaskId == TaskId);
            return _task;
        }
    }

    #endregion

    #endregion
}