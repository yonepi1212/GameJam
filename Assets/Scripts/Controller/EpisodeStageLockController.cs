using UnityEngine;
using System.Collections.Generic;

/* EpisodeStageLockController.cs
    エピソードとステージのロックをコントロールするクラス
*/
public class EpisodeStageLockController : ControllerBase
{

    #region Public Method

    public void UnLockNextEpisodeStage()
    {
        int unLockIndex = PlayerPrefs.GetInt(Define.UserData.EpisodeStageUnLockIndex);
        if (unLockIndex < 4)
        {
            unLockIndex++;
            PlayerPrefs.SetInt(Define.UserData.EpisodeStageUnLockIndex, unLockIndex);
        }
    }

    public void UnLockEpisodeStage(int selectIndex)
    {
        if (selectIndex + 1 > GetLockStatus())
        {
            // 選択したステージの次のエリアを開放
            PlayerPrefs.SetInt(Define.UserData.EpisodeStageUnLockIndex, selectIndex + 1);
        }
    }

    public void ResetButtons()
    {
        PlayerPrefs.SetInt(Define.UserData.EpisodeStageUnLockIndex, 1);
    }

    public void UnLockButtons()
    {
        PlayerPrefs.SetInt(Define.UserData.EpisodeStageUnLockIndex, 4);
    }

    public int GetLockStatus()
    {
        return PlayerPrefs.GetInt(Define.UserData.EpisodeStageUnLockIndex);
    }

    public override void Show()
    {
        // 一度も開放されていない場合は1を解放済みにする
        var lockStatus = GetLockStatus();
        if (lockStatus == 0)
        {
            UnLockEpisodeStage(0);
        }
        base.Show();
    }
    #endregion
}
