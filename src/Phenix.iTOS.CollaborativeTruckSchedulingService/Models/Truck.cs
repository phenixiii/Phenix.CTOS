namespace Phenix.iTOS.CollaborativeTruckSchedulingService.Models;

/// <summary>
/// ����
/// </summary>
public class Truck
{
    /// <summary>
    /// ����
    /// </summary>
    /// <param name="truckNo">�������</param>
    /// <param name="truckType">��������</param>
    public Truck(string truckNo, TruckType truckType)
    {
        _truckNo = truckNo;
        _truckType = truckType;
    }

    private readonly string _truckNo;

    /// <summary>
    /// �������
    /// </summary>
    public string TruckNo
    {
        get { return _truckNo; }
    }

    private readonly TruckType _truckType;

    /// <summary>
    /// ��������
    /// </summary>
    public TruckType TruckType
    {
        get { return _truckType; }
    }

    private bool _loggedIn;

    /// <summary>
    /// �ѵ�¼
    /// </summary>
    public bool LoggedIn
    {
        get { return _loggedIn; }
    }

    private TruckHealthStatus _healthStatus;

    /// <summary>
    /// ����״̬
    /// </summary>
    public TruckHealthStatus HealthStatus
    {
        get { return _healthStatus; }
    }

    private string? _location;

    /// <summary>
    /// λ�ã���ͼ���λ�ã�
    /// </summary>
    public string? Location
    {
        get { return _location; }
    }

    private double _locationLng;

    /// <summary>
    /// ���ȣ���ͼ���λ�ã�
    /// </summary>
    public double LocationLng
    {
        get { return _locationLng; }
    }

    private double _locationLat;

    /// <summary>
    /// γ�ȣ���ͼ���λ�ã�
    /// </summary>
    public double LocationLat
    {
        get { return _locationLat; }
    }

    private CarryingTaskOperationStatus? _currentTaskOperationStatus;

    /// <summary>
    /// ��ǰ������ҵ״̬
    /// </summary>
    public CarryingTaskOperationStatus? CurrentTaskOperationStatus
    {
        get { return _currentTaskOperationStatus; }
    }

    private TruckLoadingPosition _currentTaskPosition = TruckLoadingPosition.Fore;

    /// <summary>
    /// ��ǰ��������λ��
    /// </summary>
    public TruckLoadingPosition CurrentTaskPosition
    {
        get { return _currentTaskPosition; }
    }

    private readonly Dictionary<TruckLoadingPosition, CarryingTask?> _taskDict = new Dictionary<TruckLoadingPosition, CarryingTask?>(2)
    {
        [TruckLoadingPosition.Fore] = null,
        [TruckLoadingPosition.After] = null
    };

    /// <summary>
    /// ��ǰ����
    /// </summary>
    public CarryingTask? CurrentTask
    {
        get { return _taskDict[_currentTaskPosition]; }
    }

    private bool IsHealthy(bool throwIfUnhealthy = false)
    {
        switch (_healthStatus)
        {
            case TruckHealthStatus.Charging:
                if (throwIfUnhealthy)
                    throw new InvalidOperationException($"����{_truckNo}�����ڳ������״̬, ���Ȼָ�������״̬����ܼ�������!");
                return false;
            case TruckHealthStatus.Maintain:
                if (throwIfUnhealthy)
                    throw new InvalidOperationException($"����{_truckNo}������ά�޻���״̬, ���Ȼָ�������״̬����ܼ�������!");
                return false;
            default:
                return true;
        }
    }

    /// <summary>
    /// ��¼
    /// </summary>
    public void Login()
    {
        _loggedIn = IsHealthy(true);
    }

    /// <summary>
    /// ���½���״̬
    /// </summary>
    public void ChangeHealthStatus(TruckHealthStatus healthStatus)
    {
        _healthStatus = healthStatus;

        switch (healthStatus)
        {
            case TruckHealthStatus.Charging:
            case TruckHealthStatus.Maintain:
                _loggedIn = false;
                break;
        }
    }

    /// <summary>
    /// �ƶ���
    /// </summary>
    /// <param name="location">λ�ã���ͼ���λ�ã�</param>
    /// <param name="locationLng">���ȣ���ͼ���λ�ã�</param>
    /// <param name="locationLat">γ�ȣ���ͼ���λ�ã�</param>
    public void MoveTo(string location, double locationLng, double locationLat)
    {
        _location = location;
        _locationLng = locationLng;
        _locationLat = locationLat;
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="carryingTask">��������</param>
    /// <param name="specifyPosition">ָ������λ�ã�Ϊ�մ������ض�Ҫ��</param>
    /// <returns>����λ��</returns>
    public TruckLoadingPosition? AcceptTask(CarryingTask carryingTask, TruckLoadingPosition? specifyPosition = null)
    {
        if (carryingTask == null)
            throw new ArgumentNullException(nameof(carryingTask), $"������{_truckNo}�����ܿյ�����!");

        if (!IsHealthy(true))
            return null;

        CarryingTask? currentTask = _taskDict[_currentTaskPosition];
        TruckLoadingPosition otherPosition = _currentTaskPosition == TruckLoadingPosition.Fore ? TruckLoadingPosition.After : TruckLoadingPosition.Fore;
        CarryingTask? otherTask = _taskDict[otherPosition];

        //Ѱ������λ��
        if (specifyPosition == null)
        {
            if (currentTask != null && currentTask.TaskId != carryingTask.TaskId && currentTask.IsFullLoad)
                throw new InvalidOperationException($"������{_truckNo}����{currentTask.TaskId}�����أ��޷������������!");
            if (currentTask == null && otherTask != null && otherTask.TaskId != carryingTask.TaskId && otherTask.IsFullLoad)
                throw new InvalidOperationException($"������{_truckNo}����{otherTask.TaskId}�����أ��޷������������!");
            specifyPosition = currentTask != null && currentTask.TaskId == carryingTask.TaskId 
                ? _currentTaskPosition
                : currentTask == null && otherTask != null && otherTask.TaskId == carryingTask.TaskId
                ? otherPosition
                : ;
        }

        if (specifyPosition != null)
            if (specifyPosition.Value == _currentTaskPosition) //�滻��ǰ��ҵ������ʱ
            {
                if (currentTask != null && currentTask.TaskId != carryingTask.TaskId &&
                    _currentTaskOperationStatus.HasValue && _currentTaskOperationStatus < CarryingTaskOperationStatus.Unloaded)
                    throw new InvalidOperationException($"������{_truckNo}λ��{specifyPosition.Value}������ҵ�������滻����!");
            }
            else //�滻�ǵ�ǰ��ҵ������ʱ
            {
                if (currentTask != null && currentTask.TaskId != carryingTask.TaskId &&
                    _currentTaskOperationStatus.HasValue && _currentTaskOperationStatus < CarryingTaskOperationStatus.Unloaded)
                    throw new InvalidOperationException($"������{_truckNo}λ��{specifyPosition.Value}������ҵ�������滻����!");
            }

        if (_taskDict[specifyPosition.Value].IsFullLoad)
        CarryingTask? currentTask = CurrentTask;
        if (currentTask != null && currentTask.IsFullLoad)
            throw new InvalidOperationException($"����{_truckNo}���������������������ҵ��ָ�������״̬����ܼ�������!");

        foreach (KeyValuePair<TruckLoadingPosition, CarryingTask> kvp in _taskDict)
        {

        }

        _taskDict[position] = carryingTask;
        return true;
    }

    /// <summary>
    /// ��ǰ����
    /// </summary>
    /// <param name="loadingPosition">����λ��</param>
    /// <returns>��������</returns>
    public CarryingTask? FindTask(TruckLoadingPosition loadingPosition)
    {
        return _taskDict.TryGetValue(loadingPosition, out CarryingTask? result) ? result : null;
    }
}