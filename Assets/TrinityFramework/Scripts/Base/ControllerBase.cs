using System;
using UnityEngine;
using UnityEngine.Events;
/* ControllerBase.cs
  3Dやロジックを管理するクラス
*/
[Serializable]
public class ControllerBase : MonoBehaviourExtend, IBook<WindowBase>
{
    #region Declaration
    /// <summary>
    /// スレッド処理の排他制御用ダミーオブジェクト
    /// </summary>
    private object _objectLock = new System.Object();
    /// クローズが完了したときに呼ばれるイベント
    /// </summary>
    private event UnityAction closeNotification = delegate () { };
    public void Close(Action callback)
    {
        Close();
        callback.Call();
    }

    /// <summary>
    /// クローズが完了したときに呼ばれるイベント
    /// </summary>
    public event UnityAction CloseNotification
    {
        add
        {
            lock (_objectLock)
            {
                closeNotification += value;
            }
        }
        remove
        {
            lock (_objectLock)
            {
                closeNotification -= value;
            }
        }
    }

    public Shelf<DataBase, WindowBase> DataShelf;
    public WindowBase Root { get; set; }
    #endregion

    #region Public Method
    public virtual void Show()
    {
        if (!IsOpen)
        {
            DataShelf = new Shelf<DataBase, WindowBase>()
            {
                RootTransform = TransformCached,
                Root = Root
            };;
        }
        IsOpen = true;
    }

    public void Show(Action callback)
    {
        Show();
        callback.Call();
    }

    public virtual void Close()
    {
        closeNotification();
        DataShelf.CloseAllBook();
        Destroy(gameObject);
    }
    #endregion

    #region Public Prop

    /// <summary>ウィンドウが開かれているか否か</summary>
    public bool IsOpen { get; protected set; }

    #endregion

    #region Debug Method
    public bool PlayOnShow;
    protected virtual void Start()
    {
        if (PlayOnShow && Root == null)
        {
            Show();
        }
    }
    #endregion

}