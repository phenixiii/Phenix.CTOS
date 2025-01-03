namespace Phenix.CTOS.CollaborativeTruckSchedulingService.Common;

/// <summary>
/// ͨ����ͼ�ϵ���������������
/// </summary>
public class MapHelper
{
    private const double EARTH_RADIUS = 6378137;

    /// <summary>
    /// ��������λ�õľ���
    /// �ù�ʽΪGOOGLE�ṩ
    /// ���С��0.2��
    /// </summary>
    /// <param name="lat1">��һ��γ��</param>
    /// <param name="lng1">��һ�㾭��</param>
    /// <param name="lat2">�ڶ���γ��</param>
    /// <param name="lng2">�ڶ��㾭��</param>
    /// <returns>��������ľ��루�ף�</returns>
    public static double GetDistance(double lat1, double lng1, double lat2, double lng2)
    {
        double radLat1 = Rad(lat1);
        double radLng1 = Rad(lng1);
        double radLat2 = Rad(lat2);
        double radLng2 = Rad(lng2);
        double a = radLat1 - radLat2;
        double b = radLng1 - radLng2;
        return 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) + Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2))) * EARTH_RADIUS;
    }

    /// <summary>
    /// ��γ��ת���ɻ���
    /// </summary>
    /// <param name="value">��γ��</param>
    /// <returns>����</returns>
    private static double Rad(double value)
    {
        return value * Math.PI / 180d;
    }
}