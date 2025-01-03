using Phenix.Core.Mapper;
using Phenix.Core.Mapper.Schema;

namespace Phenix.CTOS.CollaborativeTruckSchedulingService.Models;

/// <summary>
/// �����ؼ���
/// </summary>
[Sheet("CTS_TRUCK_POOLS_TRUCK")]
public class TruckPoolsTruck : EntityBase<TruckPoolsTruck>
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

    private readonly long _CTP_ID;

    /// <summary>
    /// ������
    /// </summary>
    public long CTP_ID
    {
        get { return _CTP_ID; }
    }

    private readonly string _truckNo;

    /// <summary>
    /// �������
    /// </summary>
    public string TruckNo
    {
        get { return _truckNo; }
    }

    #endregion
}