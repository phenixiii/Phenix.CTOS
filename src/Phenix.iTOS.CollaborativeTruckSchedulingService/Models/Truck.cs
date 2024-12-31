using Phenix.Core.Mapper;
using Phenix.Core.Mapper.Schema;

namespace Phenix.iTOS.CollaborativeTruckSchedulingService.Models;

/// <summary>
/// ����
/// </summary>
[Sheet("CTS_TRUCK")]
public class Truck : EntityBase<Truck>
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

    private readonly string _truckNo;

    /// <summary>
    /// �������
    /// </summary>
    public string TruckNo
    {
        get { return _truckNo; }
    }

    private readonly TruckDriveType _driveType;

    /// <summary>
    /// ��ʻ����
    /// </summary>
    public TruckDriveType DriveType
    {
        get { return _driveType; }
    }

    private readonly TruckHealthStatus _healthStatus;

    /// <summary>
    /// ����״̬
    /// </summary>
    public TruckHealthStatus HealthStatus
    {
        get { return _healthStatus; }
    }

    private readonly DateTime _healthStatusChangeTime;

    /// <summary>
    /// ����״̬���ʱ��
    /// </summary>
    public DateTime HealthStatusChangeTime
    {
        get { return _healthStatusChangeTime; }
    }

    #region Detail

    [NonSerialized]
    private readonly Dictionary<TruckLoadingPosition, TruckCarryingTask?> _taskDict = new Dictionary<TruckLoadingPosition, TruckCarryingTask?>(2)
    {
        [TruckLoadingPosition.Fore] = null,
        [TruckLoadingPosition.Back] = null
    };

    [Newtonsoft.Json.JsonIgnore]
    private IDictionary<TruckLoadingPosition, TruckCarryingTask?> TaskDict
    {
        get
        {
            if (_taskDict[TruckLoadingPosition.Fore] == null)
                foreach (TruckCarryingTask item in this.FetchDetails<TruckCarryingTask>())
                    _taskDict[item.LoadingPosition] = item;
            return _taskDict;
        }
    }

    /// <summary>
    /// ǰ������
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public CarryingTask? ForeTask
    {
        get { return TaskDict[TruckLoadingPosition.Fore] != null ? TaskDict[TruckLoadingPosition.Fore]?.Task : null; }
    }

    /// <summary>
    /// ��������
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public CarryingTask? BackTask
    {
        get { return TaskDict[TruckLoadingPosition.Back] != null ? TaskDict[TruckLoadingPosition.Back]?.Task : null; }
    }

    #endregion

    #region ʱ������

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

    #endregion

    #endregion

    #region ����

    private bool IsHealthy(bool throwIfUnhealthy = false)
    {
        switch (HealthStatus)
        {
            case TruckHealthStatus.Charging:
                if (throwIfUnhealthy)
                    throw new InvalidOperationException($"����{TruckNo}�����ڳ������״̬, ���Ȼָ�������״̬����ܼ�������!");
                return false;
            case TruckHealthStatus.Maintaining:
                if (throwIfUnhealthy)
                    throw new InvalidOperationException($"����{TruckNo}������ά�޻���״̬, ���Ȼָ�������״̬����ܼ�������!");
                return false;
            default:
                return true;
        }
    }

    private void SetHealthStatus(TruckHealthStatus status)
    {
        if (HealthStatus == status)
            return;

        this.UpdateSelf(Set(p => p.HealthStatus, status).
            Set(p => p.HealthStatusChangeTime, DateTime.Now));
    }

    /// <summary>
    /// ���
    /// </summary>
    public void Charge()
    {
        SetHealthStatus(TruckHealthStatus.Charging);
    }

    /// <summary>
    /// ά��
    /// </summary>
    public void Maintain()
    {
        SetHealthStatus(TruckHealthStatus.Maintaining);
    }

    /// <summary>
    /// �ָ�����
    /// </summary>
    public void Resume()
    {
        SetHealthStatus(TruckHealthStatus.Normal);
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
    /// ����������
    /// </summary>
    /// <param name="task">��������</param>
    /// <param name="position">ָ������λ�ã�Ϊ�մ������ض�Ҫ��</param>
    /// <returns>ȷ������λ��</returns>
    public TruckCarryingTask NewTask(CarryingTask task, TruckLoadingPosition? position = null)
    {
        if (!IsHealthy(true))
            return null;

        CarryingTask? foreTask = ForeTask;
        CarryingTask? backTask = BackTask;
        if (foreTask != null)
            if (foreTask.TaskId == task.TaskId)
                throw new InvalidOperationException($"������{TruckNo}ǰ��λ��������ͬ{task.TaskId}������ʹ��ForeTask����غ�����������!");
            else if (position == TruckLoadingPosition.Fore && foreTask.Status != CarryingTaskStatus.Unloaded)
                throw new InvalidOperationException($"������{TruckNo}ǰ��λ�õ�{foreTask.TaskId}����δ��ɣ��޷��н�������!");
        if (backTask != null)
            if (backTask.TaskId == task.TaskId)
                throw new InvalidOperationException($"������{TruckNo}����λ��������ͬ{task.TaskId}������ʹ��BackTask����غ�����������!");
            else if (position == TruckLoadingPosition.Fore && backTask.Status != CarryingTaskStatus.Unloaded)
                throw new InvalidOperationException($"������{TruckNo}����λ�õ�{backTask.TaskId}����δ��ɣ��޷��н�������!");

        if (position == null)
            foreach (KeyValuePair<TruckLoadingPosition, TruckCarryingTask?> kvp in TaskDict)
                if (kvp.Value == null || kvp.Value.Task.Status == CarryingTaskStatus.Unloaded)
                {
                    position = kvp.Key;
                    break;
                }

        if (position == null)
            throw new InvalidOperationException($"������{TruckNo}��������λ�ÿɹ���������!");

        TruckCarryingTask result = this.NewDetail<TruckCarryingTask>(
            TruckCarryingTask.Set(p => p.TerminalNo, task.TerminalNo).
                Set(p => p.TruckPoolsNo, task.TruckPoolsNo).
                Set(p => p.TaskId, task.TaskId).
                Set(p => p.LoadingPosition, position).
                Set(p => p.OriginateTime, DateTime.Now));
        result.InsertSelf();
        TaskDict[position.Value] = result;
        return result;
    }

    /// <summary>
    /// ȡ�����񣨺�ǿ��ȡ����
    /// </summary>
    /// <param name="taskId">����ID</param>
    /// <returns>ȷ������λ��</returns>
    public TruckLoadingPosition CancelTask(long taskId)
    {
        TruckLoadingPosition? position = null;
        foreach (KeyValuePair<TruckLoadingPosition, TruckCarryingTask?> kvp in TaskDict)
            if (kvp.Value != null && kvp.Value.TaskId == taskId)
            {
                position = kvp.Key;
                break;
            }

        if (position == null)
            throw new InvalidOperationException($"������{TruckNo}������{taskId}�����޴�ȡ��!");

        this.DeleteDetails<TruckCarryingTask>(p => p.LoadingPosition == position);
        TaskDict[position.Value] = null;
        return position.Value;
    }

    #endregion
}