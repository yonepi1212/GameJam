using UnityEngine;

/// <summary>
/// 文言
/// </summary>
public static class Word
{
    /// <summary>
    /// ネットワーク
    /// </summary>
    public static string Network = "ネットワーク";
    /// <summary>
    /// 接続
    /// </summary>
    public static string Connect = "接続";
    /// <summary>
    /// タイトル
    /// </summary>
    public static string Title = "タイトル";

    #region Server
    /// <summary>
    /// サーバー
    /// </summary>
    public static class Server
    {
        /// <summary>
        /// Error
        /// </summary>
        public static class Error
        {
            /// <summary>
            /// ネットワークに接続することが出来ませんでした
            /// </summary>
            public static string NotConnectNetwork = Network + "に" + Connect +"することが" + Define.Newline + "出来ませんでした";
            public static string ToTitle = Title + "へ";
            public static string Retry = "リトライ";
        }
    }
    #endregion

}
