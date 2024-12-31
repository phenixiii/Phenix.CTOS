using System.Data.Common;
using Phenix.Core.Mapper;
using Phenix.Core.Mapper.Schema;

namespace Phenix.iTOS.CollaborativeTruckSchedulingService.Models;

/// <summary>
/// ��������
/// </summary>
[Sheet("CTS_CARRYING_TASK")]
public class CarryingTask : EntityBase<CarryingTask>
{
    /// <summary>
    /// ����
    /// </summary>
    public static CarryingTask Create(string terminalNo, string truckPoolsNo,
        long taskId, CarryingTaskType taskType, int taskPriority,
        string containerNumber, bool isBigSize,
        string loadLocation, long loadQueueNo, string? loadCraneNo,
        string unloadLocation, long unloadQueueNo, string? unloadCraneNo, bool needTwistLock,
        QuayCraneProcess? quayCraneProcess)
    {
        if (taskType == CarryingTaskType.Shift && quayCraneProcess != null)
            throw new InvalidOperationException($"ת����ҵʱ��Ӧ���а��Ź���!");
        if (taskType != CarryingTaskType.Shift && quayCraneProcess == null)
            throw new InvalidOperationException($"��ת����ҵʱӦ���а��Ź���!");

        CarryingTask result = New(
            Set(p => p.TerminalNo, terminalNo).
                Set(p => p.TruckPoolsNo, truckPoolsNo).
                Set(p => p.TaskId, taskId).
                Set(p => p.TaskType, taskType).
                Set(p => p.TaskPriority, taskPriority).
                Set(p => p.OriginateTime, DateTime.Now));

        result._containerDict[true] = result.NewDetail<CarryingContainer>(
            CarryingContainer.Set(p => p.IsPlan, true).
                Set(p => p.ContainerNumber, containerNumber).
                Set(p => p.IsBigSize, isBigSize));
        result._containerDict[false] = null;

        result._orderDict[CarryingTaskOrderType.Load] = result.NewDetail<CarryingTaskOrder>(
            CarryingTaskOrder.Set(p => p.OrderType, CarryingTaskOrderType.Load).
                Set(p => p.LoadUnloadLocation, loadLocation).
                Set(p => p.LoadUnloadQueueNo, loadQueueNo).
                Set(p => p.CraneNo, loadCraneNo).
                Set(p => p.CraneType, taskType is CarryingTaskType.Discharge or CarryingTaskType.Transship ? CraneType.QuayCrane : CraneType.GantryCrane).
                Set(p => p.QuayCraneProcess, taskType is CarryingTaskType.Discharge or CarryingTaskType.Transship ? quayCraneProcess : null).
                Set(p => p.NeedTwistLock, false));
        result._orderDict[CarryingTaskOrderType.Unload] = result.NewDetail<CarryingTaskOrder>(
            CarryingTaskOrder.Set(p => p.OrderType, CarryingTaskOrderType.Unload).
                Set(p => p.LoadUnloadLocation, unloadLocation).
                Set(p => p.LoadUnloadQueueNo, unloadQueueNo).
                Set(p => p.CraneNo, unloadCraneNo).
                Set(p => p.CraneType, taskType is CarryingTaskType.Shipment or CarryingTaskType.Transship ? CraneType.QuayCrane : CraneType.GantryCrane).
                Set(p => p.QuayCraneProcess, taskType is CarryingTaskType.Shipment or CarryingTaskType.Transship ? quayCraneProcess : null).
                Set(p => p.NeedTwistLock, needTwistLock));

        result.Database.Execute((DbTransaction transaction) =>
        {
            result.InsertSelf(transaction);
            foreach (KeyValuePair<bool, CarryingContainer?> kvp in result._containerDict)
                if (kvp.Value != null)
                    kvp.Value.InsertSelf(transaction);
            foreach (KeyValuePair<CarryingTaskOrderType, CarryingTaskOrder> kvp in result._orderDict)
                kvp.Value.InsertSelf(transaction);
        });

        return result;
    }

    #region ����

    private readonly long _ID;

    /// <summary>
    /// ID
    /// </summary>
    public long ID
    {
        get { return _ID; }
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

    private readonly int _taskPriority;

    /// <summary>
    /// �������ȼ�
    /// </summary>
    public int TaskPriority
    {
        get { return _taskPriority; }
    }

    private readonly DateTime _originateTime;

    /// <summary>
    /// �Ƶ�ʱ��
    /// </summary>
    public DateTime OriginateTime
    {
        get { return _originateTime; }
    }

    private readonly CarryingTaskStatus _status;

    /// <summary>
    /// ����״̬
    /// </summary>
    public CarryingTaskStatus Status
    {
        get { return _status; }
    }

    private readonly bool _suspending;

    /// <summary>
    /// �Ƿ���ͣ
    /// </summary>
    public bool Suspending
    {
        get { return _suspending; }
    }

    private readonly DateTime _suspendingChangeTime;

    /// <summary>
    /// ��ͣ���ʱ��
    /// </summary>
    public DateTime SuspendingChangeTime
    {
        get { return _suspendingChangeTime; }
    }

    #region Detail

    [NonSerialized]
    private readonly Dictionary<bool, CarryingContainer?> _containerDict = new Dictionary<bool, CarryingContainer?>(2)
    {
        [true] = null,
        [false] = null
    };

    [Newtonsoft.Json.JsonIgnore]
    private IDictionary<bool, CarryingContainer?> ContainerDict
    {
        get
        {
            if (_containerDict[true] == null)
                foreach (CarryingContainer item in this.FetchDetails<CarryingContainer>())
                    _containerDict[item.IsPlan] = item;
            return _containerDict;
        }
    }

    /// <summary>
    /// �ƻ��������
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public CarryingContainer PlanContainer
    {
        get { return ContainerDict[true]!; }
    }

    /// <summary>
    /// ʵ���������
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public CarryingContainer? ActualContainer
    {
        get { return ContainerDict[false]; }
    }

    /// <summary>
    /// �Ǵ���
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public bool IsBigSize
    {
        get { return ActualContainer != null ? ActualContainer.IsBigSize : PlanContainer.IsBigSize; }
    }

    [NonSerialized]
    private readonly Dictionary<CarryingTaskOrderType, CarryingTaskOrder> _orderDict = new Dictionary<CarryingTaskOrderType, CarryingTaskOrder>(2);

    [Newtonsoft.Json.JsonIgnore]
    private IDictionary<CarryingTaskOrderType, CarryingTaskOrder> OrderDict
    {
        get
        {
            if (_orderDict.Count == 0)
                foreach (CarryingTaskOrder item in this.FetchDetails<CarryingTaskOrder>())
                    _orderDict[item.OrderType] = item;
            return _orderDict;
        }
    }

    /// <summary>
    /// װ��ָ��
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public CarryingTaskOrder LoadOrder
    {
        get { return OrderDict[CarryingTaskOrderType.Load]; }
    }

    /// <summary>
    /// ж��ָ��
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public CarryingTaskOrder UnloadOrder
    {
        get { return OrderDict[CarryingTaskOrderType.Unload]; }
    }

    #endregion

    #endregion

    #region ����

    /// <summary>
    /// ����ƻ�����
    /// </summary>
    /// <param name="containerNumber">���</param>
    /// <param name="isBigSize">�Ǵ���</param>
    public void ChangePlanContainer(string containerNumber, bool isBigSize)
    {
        ContainerDict[true]!.UpdateSelf(
            CarryingContainer.Set(p => p.ContainerNumber, containerNumber).
                Set(p => p.IsBigSize, isBigSize));
    }

    /// <summary>
    /// װ�ػ���
    /// </summary>
    /// <param name="containerNumber">���</param>
    /// <param name="isBigSize">�Ǵ���</param>
    public void LoadContainer(string containerNumber, bool isBigSize)
    {
        if (ContainerDict[false] != null)
            ContainerDict[false]!.UpdateSelf(
                CarryingContainer.Set(p => p.ContainerNumber, containerNumber).
                    Set(p => p.IsBigSize, isBigSize));
        else
        {
            CarryingContainer container = this.NewDetail<CarryingContainer>(
                CarryingContainer.Set(p => p.IsPlan, false).
                    Set(p => p.ContainerNumber, containerNumber).
                    Set(p => p.IsBigSize, isBigSize));
            container.InsertSelf();
            ContainerDict[false] = container;
        }
    }

    /// <summary>
    /// ���װ��ָ��
    /// </summary>
    /// <param name="loadLocation">װжλ��</param>
    /// <param name="loadQueueNo">װж�Ŷ����</param>
    /// <param name="loadCraneNo">װж��е��</param>
    /// <param name="quayCraneProcess">���Ź���</param>
    public void ChangeLoadOrder(string loadLocation, long loadQueueNo, string? loadCraneNo, QuayCraneProcess? quayCraneProcess)
    {
        CarryingTaskOrder loadOrder = LoadOrder;
        if (loadOrder.Completed)
            throw new InvalidOperationException($"װ��ָ�������ִ��, �޷��滻!");

        loadOrder.UpdateSelf(
            CarryingTaskOrder.Set(p => p.OrderType, CarryingTaskOrderType.Load).
                Set(p => p.LoadUnloadLocation, loadLocation).
                Set(p => p.LoadUnloadQueueNo, loadQueueNo).
                Set(p => p.CraneNo, loadCraneNo).
                Set(p => p.CraneType, TaskType is CarryingTaskType.Discharge or CarryingTaskType.Transship ? CraneType.QuayCrane : CraneType.GantryCrane).
                Set(p => p.QuayCraneProcess, TaskType is CarryingTaskType.Discharge or CarryingTaskType.Transship ? quayCraneProcess : null).
                Set(p => p.NeedTwistLock, false));
    }

    /// <summary>
    /// ���ж��ָ��
    /// </summary>
    /// <param name="unloadLocation">װжλ��</param>
    /// <param name="unloadQueueNo">װж�Ŷ����</param>
    /// <param name="unloadCraneNo">װж��е��</param>
    /// <param name="quayCraneProcess">���Ź���</param>
    /// <param name="needTwistLock">�Ƿ���Ҫװж��ť</param>
    public void ChangeUnloadOrder(string unloadLocation, long unloadQueueNo, string? unloadCraneNo, QuayCraneProcess? quayCraneProcess, bool needTwistLock)
    {
        CarryingTaskOrder unloadOrder = UnloadOrder;
        if (unloadOrder.Executing)
            throw new InvalidOperationException($"ж��ָ���ѿ�ʼִ��, �޷��滻!");
        if (unloadOrder.Completed)
            throw new InvalidOperationException($"ж��ָ�������ִ��, �޷��滻!");

        unloadOrder.UpdateSelf(
            CarryingTaskOrder.Set(p => p.OrderType, CarryingTaskOrderType.Unload).
                Set(p => p.LoadUnloadLocation, unloadLocation).
                Set(p => p.LoadUnloadQueueNo, unloadQueueNo).
                Set(p => p.CraneNo, unloadCraneNo).
                Set(p => p.CraneType, TaskType is CarryingTaskType.Shipment or CarryingTaskType.Transship ? CraneType.QuayCrane : CraneType.GantryCrane).
                Set(p => p.QuayCraneProcess, TaskType is CarryingTaskType.Shipment or CarryingTaskType.Transship ? quayCraneProcess : null).
                Set(p => p.NeedTwistLock, needTwistLock));
    }

    /// <summary>
    /// ִ����һ����ҵ
    /// </summary>
    /// <returns>����������ҵ</returns>
    public CarryingTaskOperation? ExecuteNextOperation()
    {
        Resume();

        CarryingTaskOrder loadOrder = LoadOrder;
        if (!loadOrder.Completed)
            return loadOrder.ExecuteNextOperation(this);

        CarryingTaskOrder unloadOrder = UnloadOrder;
        if (!unloadOrder.Completed)
            return unloadOrder.ExecuteNextOperation(this);

        return null;
    }

    /// <summary>
    /// ��ͣ����
    /// </summary>
    public void Suspend()
    {
        if (Suspending)
            return;

        this.UpdateSelf(Set(p => p.Suspending, true).
            Set(p => p.SuspendingChangeTime, DateTime.Now));
    }

    /// <summary>
    /// �ָ�����
    /// </summary>
    public void Resume()
    {
        if (!Suspending)
            return;

        this.UpdateSelf(Set(p => p.Suspending, false).
            Set(p => p.SuspendingChangeTime, DateTime.Now));
    }

    #endregion
}