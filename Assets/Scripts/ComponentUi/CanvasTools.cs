using UnityEngine;

/// <summary>
/// キャンバスで使用するツール群
/// </summary>
public class CanvasTools : MonoBehaviour
{
    public GameObject Event;
    public GameObject White;
    public GameObject ListView;

    [System.Serializable]
    public class ViewPanelHolder
    {
        /// <summary>
        /// 標準ビューパネル
        /// </summary>
        public ViewPanel Standard;

    }

    public ViewPanelHolder ViewPanel;

}