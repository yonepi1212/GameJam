using UnityEngine;
using UnityEngine.UI;

/* EpisodeStageLock.cs
    エピソードとステージのクエスト開始ボタンのクラス
*/

public class EpisodeStageLock : ComponentUiBase {

    #region Declaration
    [SerializeField]
    private bool _isLock;
    #endregion

    #region Private Method

    private void CheckLockStatus()
    {
        if (_isLock)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }

    }
    #endregion

    #region Public Method

    public void Lock()
    {
        _isLock = true;
        CheckLockStatus();
    }

    public void UnLock()
    {
        _isLock = false;
        CheckLockStatus();
    }

    #endregion
}
