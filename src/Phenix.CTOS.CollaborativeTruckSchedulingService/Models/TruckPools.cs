using System.Data.Common;
using Phenix.Core.Mapper;
using Phenix.Core.Mapper.Schema;

namespace Phenix.CTOS.CollaborativeTruckSchedulingService.Models;

/// <summary>
/// ������
/// </summary>
[Sheet("CTS_TRUCK_POOLS")]
public class TruckPools : EntityBase<TruckPools>
{
    /// <summary>
    /// ����
    /// </summary>
    public static TruckPools Create(string terminalNo, string truckPoolsNo, string[] truckNos)
    {
        TruckPools result = New(
            Set(p => p.TerminalNo, terminalNo).
                Set(p => p.TruckPoolsNo, truckPoolsNo).
                Set(p => p.OriginateTime, DateTime.Now));

        if (truckNos.Length > 0)
        {
            result._truckList = new List<TruckPoolsTruck>(truckNos.Length);
            foreach (string truckNo in truckNos)
                result._truckList.Add(result.NewDetail<TruckPoolsTruck>(
                    TruckPoolsTruck.Set(p => p.TruckNo, truckNo)));
        }

        result.Database.Execute((DbTransaction transaction) =>
        {
            result.InsertSelf(transaction);
            if (result._truckList != null)
                foreach (TruckPoolsTruck item in result._truckList)
                    item.InsertSelf(transaction);
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

    private readonly DateTime _originateTime;

    /// <summary>
    /// �Ƶ�ʱ��
    /// </summary>
    public DateTime OriginateTime
    {
        get { return _originateTime; }
    }

    private readonly bool _invalided;

    /// <summary>
    /// �Ƿ�����
    /// </summary>
    public bool Invalided
    {
        get { return _invalided; }
    }

    private readonly DateTime _invalidedChangeTime;

    /// <summary>
    /// ���ϱ��ʱ��
    /// </summary>
    public DateTime InvalidedChangeTime
    {
        get { return _invalidedChangeTime; }
    }

    #region Detail

    [NonSerialized]
    private IList<TruckPoolsTruck>? _truckList;

    /// <summary>
    /// �����嵥
    /// </summary>
    [Newtonsoft.Json.JsonIgnore]
    public IList<TruckPoolsTruck> TruckList
    {
        get
        {
            if (_truckList == null)
                _truckList = this.FetchDetails<TruckPoolsTruck>();
            return _truckList.AsReadOnly();
        }
        set
        {
            this.Database.Execute((DbTransaction transaction) =>
            {
                this.DeleteDetails<TruckPoolsTruck>(transaction);
                foreach (TruckPoolsTruck item in value)
                    item.InsertSelf(transaction);
            });
            _truckList = value;
        }
    }

    #endregion

    #endregion

    #region ����

    /// <summary>
    /// ����
    /// </summary>
    public void Invalid()
    {
        if (Invalided)
            return;

        this.UpdateSelf(Set(p => p.Invalided, true).
            Set(p => p.InvalidedChangeTime, DateTime.Now));
    }

    /// <summary>
    /// �ָ�
    /// </summary>
    public void Resume()
    {
        if (!Invalided)
            return;

        this.UpdateSelf(Set(p => p.Invalided, false).
            Set(p => p.InvalidedChangeTime, DateTime.Now));
    }

    /// <summary>
    /// �滻�����嵥
    /// </summary>
    public void ReplaceTruckList(string[] truckNos)
    {
        List<TruckPoolsTruck> truckList = new List<TruckPoolsTruck>(truckNos.Length);
        foreach (string truckNo in truckNos)
            truckList.Add(this.NewDetail<TruckPoolsTruck>(
                TruckPoolsTruck.Set(p => p.TruckNo, truckNo)));
        TruckList = truckList;
    }

    #endregion
}