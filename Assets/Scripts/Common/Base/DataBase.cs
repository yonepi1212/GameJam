using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/* DataBase.cs
 *  ウィンドウが保持するシーンに必要なデータ群の基底クラス
 */
public class DataBase : MonoBehaviour, IBook<WindowBase> {
    
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

    public WindowBase Root { set; get; }
    #endregion

    #region Public Method
    public virtual void Show()
    {

    }

    public void Show(Action callback)
    {
        Show();
        callback.Call();
    }

    public virtual void Close()
    {
        closeNotification();
        Destroy(gameObject);
    }
    #endregion

}
