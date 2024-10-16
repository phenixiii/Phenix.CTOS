namespace Phenix.iTOS.CollaborativeTruckSchedulingService.Models;

/// <summary>
/// ��������
/// </summary>
public class CarryingTask
{
    public CarryingTask(string terminalNo, string truckPoolsNo,
        long taskId, CarryingTaskType taskType, bool suspend, int taskPriority, bool isFullLoad,
        string? loadCraneNo, CraneType loadCraneType, string loadLocation, long loadQueueNo,
        string? unloadCraneNo, CraneType unloadCraneType, string unloadLocation, long unloadQueueNo,
        string? yLocation, string? vLocation, QuayCraneProcess quayCraneProcess, bool needTwistLock,
        DateTime timestamp)
    {
        _terminalNo = terminalNo;
        _truckPoolsNo = truckPoolsNo;
        _taskId = taskId;
        _taskType = taskType;
        _suspend = suspend;
        _taskPriority = taskPriority;
        _isFullLoad = isFullLoad;
        _loadCraneNo = loadCraneNo;
        _loadCraneType = loadCraneType;
        _loadLocation = loadLocation;
        _loadQueueNo = loadQueueNo;
        _unloadCraneNo = unloadCraneNo;
        _unloadCraneType = unloadCraneType;
        _unloadLocation = unloadLocation;
        _unloadQueueNo = unloadQueueNo;
        _yLocation = yLocation;
        _vLocation = vLocation;
        _quayCraneProcess = quayCraneProcess;
        _needTwistLock = needTwistLock;
        _timestamp = timestamp;
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

    private readonly CarryingTaskType _taskType;

    /// <summary>
    /// ��������
    /// </summary>
    public CarryingTaskType TaskType
    {
        get { return _taskType; }
    }

    private readonly bool _suspend;

    /// <summary>
    /// �Ƿ���ͣ
    /// </summary>
    public bool Suspend
    {
        get { return _suspend; }
    }

    private readonly int _taskPriority;

    /// <summary>
    /// �������ȼ�
    /// </summary>
    public int TaskPriority
    {
        get { return _taskPriority; }
    }

    private readonly bool _isFullLoad;

    /// <summary>
    /// �Ƿ�һ�����أ����������ټ�һ��С������
    /// </summary>
    public bool IsFullLoad
    {
        get { return _isFullLoad; }
    }

    private readonly string? _loadCraneNo;

    /// <summary>
    /// װ�أ��ص���������е��
    /// </summary>
    public string? LoadCraneNo
    {
        get { return _loadCraneNo; }
    }

    private readonly CraneType _loadCraneType;

    /// <summary>
    /// װ�ػ�е����
    /// </summary>
    public CraneType LoadCraneType
    {
        get { return _loadCraneType; }
    }

    private readonly string _loadLocation;

    /// <summary>
    /// װ��λ�ã���ͼ���λ�ã�
    /// </summary>
    public string LoadLocation
    {
        get { return _loadLocation; }
    }

    private readonly long _loadQueueNo;

    /// <summary>
    /// װ���Ŷ���ţ�ͬһװ��λ���谴����Ŷӣ�ͬ�ſɽ�����
    /// </summary>
    public long LoadQueueNo
    {
        get { return _loadQueueNo; }
    }

    private readonly string? _unloadCraneNo;

    /// <summary>
    /// ж�أ�ж�¼�������е��
    /// </summary>
    public string? UnloadCraneNo
    {
        get { return _unloadCraneNo; }
    }

    private readonly CraneType _unloadCraneType;

    /// <summary>
    /// ж�ػ�е����
    /// </summary>
    public CraneType UnloadCraneType
    {
        get { return _unloadCraneType; }
    }

    private readonly string _unloadLocation;

    /// <summary>
    /// ж��λ�ã���ͼ���λ�ã�
    /// </summary>
    public string UnloadLocation
    {
        get { return _unloadLocation; }
    }

    private readonly long _unloadQueueNo;

    /// <summary>
    /// ж���Ŷ���ţ�ͬһж��λ���谴����Ŷӣ�ͬ�ſɽ�����
    /// </summary>
    public long UnloadQueueNo
    {
        get { return _unloadQueueNo; }
    }

    private readonly string? _yLocation;

    /// <summary>
    /// ����λ
    /// </summary>
    public string? YLocation
    {
        get { return _yLocation; }
    }

    private readonly string? _vLocation;

    /// <summary>
    /// ����λ
    /// </summary>
    public string? VLocation
    {
        get { return _vLocation; }
    }

    private readonly QuayCraneProcess _quayCraneProcess;

    /// <summary>
    /// ���Ź���
    /// </summary>
    public QuayCraneProcess QuayCraneProcess
    {
        get { return _quayCraneProcess; }
    }

    private readonly bool _needTwistLock;

    /// <summary>
    /// �Ƿ���ҪŤ��������ťվ��
    /// </summary>
    public bool NeedTwistLock
    {
        get { return _needTwistLock; }
    }

    private readonly DateTime _timestamp;

    /// <summary>
    /// ʱ���
    /// </summary>
    public DateTime Timestamp
    {
        get { return _timestamp; }
    }
}