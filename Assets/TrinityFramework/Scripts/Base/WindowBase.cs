using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ウィンドウ基底クラス
/// </summary>
public abstract class WindowBase : UGuiMonoBehaviour, IBook<WindowBase>
{
    #region Declaration
    public Shelf<ControllerBase, WindowBase> ControllerShelf;
    public Shelf<ScreenBase, WindowBase> ScreenShelf;
    public Shelf<DataBase, WindowBase> DataShelf;

    public WindowBase Root { set; get; }

    // スレッド処理の排他制御用ダミーオブジェクト
    private object _objectLock = new System.Object();
    // クローズが完了したときに呼ばれるイベント
    private event UnityAction closeNotification = delegate () { };
    // クローズが完了したときに呼ばれるイベント
    public void Close(Action callback)
    {
        Close();
        callback.Call();
    }

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

    #endregion

    #region Public Method

    /// <summary>
    /// コントローラーとウィンドウを表示する
    /// </summary>
    public virtual void Show()
    {
        if(Root == null)
        {
            Root = this;
        }

        if (!IsOpen)
        {
            ControllerShelf = new Shelf<ControllerBase, WindowBase>()
            {
                RootTransform = Transform,
                Root = Root
            };
            ScreenShelf = new Shelf<ScreenBase, WindowBase>()
            {
                RootTransform = Transform,
                Root = Root
            };
            DataShelf = new Shelf<DataBase, WindowBase>()
            {
                RootTransform = Transform,
                Root = Root
            };
            gameObject.SetActive(true);
        }

        IsOpen = true;
    }

    public void Show(Action callback)
    {
        Show();
        callback.Call();
    }

    /// <summary>
    /// ウィンドウを消す
    /// </summary>
    public virtual void Close()
    {
        // 強制終了するとClose前にwindowが消されてしまうので
        // thisをnull判定しておく
        if (this)
        {
            closeNotification();
            ControllerShelf.CloseAllBook();
            ScreenShelf.CloseAllBook();
            DataShelf.CloseAllBook();
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// ウィンドウを非表示にする
    /// </summary>
    public virtual void Hide()
    {
        IsOpen = false;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// ウィンドウを消す
    /// </summary>
    public virtual void Close(GameObject instance)
    {
        if (instance != null)
        {
            WindowBase window = instance.GetComponent<WindowBase>();
            window.Close();
        }
    }

    /// <summary>
    /// ウィンドウを非表示にする
    /// </summary>
    public virtual void Hide(GameObject instance)
    {
        if (instance != null)
        {
            instance.SetActive(false);
        }
    }

    #endregion

    #region MonoBehaviour
    /// <summary>
    /// OnDestroy
    /// </summary>
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    #endregion

    #region Public Prop

    /// <summary>ウィンドウが開かれているか否か</summary>
    public bool IsOpen { get; protected set; }

    #endregion

    #region Debug Method
    public bool PlayOnShow;

    private void Start()
    {
        // Play On Showが出来る条件
        // 1.明示的にPlay On Showを選択する
        // 2.他のWindowなどから呼び出されていない
        if(PlayOnShow && Root == null)
        {
            Show();
        }
    }
    #endregion
}
