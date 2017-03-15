using UnityEngine;

/// <summary>
/// ネットワーク状況確認クラス
/// </summary>
public static class NetworkUtility
{
    /// <summary>
    /// ネットワークに接続できるかを返す
    /// </summary>
    public static bool IsReachable
    {
        get
        {
            bool reachable = false;

            // ネットワークの状態を出力
            switch (Application.internetReachability)
            {
                case NetworkReachability.NotReachable:
                    //ネットワークには到達不可
                    //reachable = false;
                    break;

                case NetworkReachability.ReachableViaCarrierDataNetwork:
                    //キャリアデータネットワーク経由で到達可能
                    reachable = true;
                    break;

                case NetworkReachability.ReachableViaLocalAreaNetwork:
                    //Wifiまたはケーブル経由で到達可能
                    reachable = true;
                    break;
            }
            return reachable;
        }
    }

    /// <summary>
    /// Wifiネットワークか確認する
    /// </summary>
    public static bool IsWifi
    {
        get
        {
            return (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork) ? true : false;
        }
    }

    /// <summary>
    /// キャリアデータネットワークか確認する
    /// </summary>
    public static bool IsCarrier
    {
        get
        {
            return (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork) ? true : false;
        }
    }

    /// <summary>
    /// ネットワークに到達不可能か確認する
    /// </summary>
    public static bool NotReachable
    {
        get
        {
            return (Application.internetReachability == NetworkReachability.NotReachable) ? true : false;
        }
    }
}