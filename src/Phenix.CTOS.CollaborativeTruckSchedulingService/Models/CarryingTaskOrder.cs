using System.Data.Common;
using Phenix.Core.Mapper;
using Phenix.Core.Mapper.Schema;

namespace Phenix.CTOS.CollaborativeTruckSchedulingService.Models;

/// <summary>
/// ��������ָ��
/// </summary>
[Sheet("CTS_CARRYING_TASK_ORDER")]
public class CarryingTaskOrder : EntityBase<CarryingTaskOrder>
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

    private readonly long _CCT_ID;

    /// <summary>
    /// ��������
    /// </summary>
    public long CCT_ID
    {
        get { return _CCT_ID; }
    }

    private readonly CarryingTaskOrderType _orderType;

    /// <summary>
    /// ָ������
    /// </summary>
    public CarryingTaskOrderType OrderType
    {
        get { return _orderType; }
    }

    private readonly string _loadUnloadLocation;

    /// <summary>
    /// װжλ�ã���ͼ���λ�ã�
    /// </summary>
    public string LoadUnloadLocation
    {
        get { return _loadUnloadLocation; }
    }

    private readonly long _loadUnloadQueueNo;

    /// <summary>
    /// װж�Ŷ���ţ�ͬ��ָ�װжλ�ú��Ŷ������ͬ���£�ͬ��֧��˫�������ͬ���ȵ�����ҵ��
    /// </summary>
    public long LoadUnloadQueueNo
    {
        get { return _loadUnloadQueueNo; }
    }

    private readonly string? _craneNo;

    /// <summary>
    /// װж��е��
    /// </summary>
    public string? CraneNo
    {
        get { return _craneNo; }
    }

    private readonly CraneType _craneType;

    /// <summary>
    /// װж��е����
    /// </summary>
    public CraneType CraneType
    {
        get { return _craneType; }
    }

    private readonly QuayCraneProcess? _quayCraneProcess;

    /// <summary>
    /// ���Ź���
    /// </summary>
    public QuayCraneProcess? QuayCraneProcess
    {
        get { return _quayCraneProcess; }
    }

    private readonly bool _needTwistLock;

    /// <summary>
    /// �Ƿ���Ҫװж��ť������ťվ��
    /// </summary>
    public bool NeedTwistLock
    {
        get { return _needTwistLock; }
    }

    #region Detail

    [NonSerialized]
    private IList<CarryingTaskOperation>? _operationList;

    [Newtonsoft.Json.JsonIgnore]
    private IList<CarryingTaskOperation> OperationList
    {
        get
        {
            if (_operationList == null)
                _operationList = this.FetchDetails<CarryingTaskOperation>();
            return _operationList;
        }
    }

    /// <summary>
    /// ��ǰ��ҵ
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public CarryingTaskOperation? CurrentOperation
    {
        get { return OperationList.Count > 1 ? OperationList[^1] : null; }
    }

    /// <summary>
    /// ִ����
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public bool Executing
    {
        get { return CurrentOperation != null && CurrentOperation.Status > CarryingTaskOperationStatus.UnStart && CurrentOperation.Status < CarryingTaskOperationStatus.LoadUnloaded; }
    }

    /// <summary>
    /// �����
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public bool Completed
    {
        get { return CurrentOperation != null && CurrentOperation.Status == CarryingTaskOperationStatus.LoadUnloaded; }
    }

    #endregion

    #endregion

    #region ����

    /// <summary>
    /// ִ����һ����ҵ
    /// </summary>
    /// <returns>����������ҵ</returns>
    public CarryingTaskOperation? ExecuteNextOperation(CarryingTask task)
    {
        CarryingTaskOperation? result = null;
        CarryingTaskOperation? currentOperation = CurrentOperation;
        switch (OrderType)
        {
            case CarryingTaskOrderType.Load:
                if (!Completed)
                    task.Database.Execute((DbTransaction transaction) =>
                    {
                        result = CarryingTaskOperation.New(
                            CarryingTaskOperation.Set(p => p.Status,
                                    currentOperation == null || currentOperation.Status == CarryingTaskOperationStatus.UnStart
                                        ? CarryingTaskOperationStatus.ToLocation
                                        : currentOperation.Status + 1).
                                Set(p => p.Timestamp, DateTime.Now));
                        result.InsertSelf(transaction);
                        task.UpdateSelf(transaction,
                            CarryingTask.Set(p => p.Status,
                                result.Status == CarryingTaskOperationStatus.LoadUnloaded ? CarryingTaskStatus.Loaded : CarryingTaskStatus.Executing));
                    });
                break;
            case CarryingTaskOrderType.Unload:
                if (!Completed)
                    task.Database.Execute((DbTransaction transaction) =>
                    {
                        result = CarryingTaskOperation.New(
                            CarryingTaskOperation.Set(p => p.Status,
                                    currentOperation == null || currentOperation.Status == CarryingTaskOperationStatus.UnStart
                                        ? task.TaskType != CarryingTaskType.Shift && NeedTwistLock
                                            ? CarryingTaskOperationStatus.ToTwistLockStop
                                            : CarryingTaskOperationStatus.ToLocation
                                        : currentOperation.Status + 1).
                                Set(p => p.Timestamp, DateTime.Now));
                        result.InsertSelf(transaction);
                        task.UpdateSelf(transaction,
                            CarryingTask.Set(p => p.Status,
                                result.Status == CarryingTaskOperationStatus.LoadUnloaded ? CarryingTaskStatus.Unloaded : CarryingTaskStatus.Loaded));
                    });
                break;
        }

        if (result != null)
            OperationList.Add(result);
        return result;
    }

    #endregion
}