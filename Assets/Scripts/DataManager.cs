using System;
/* DataManager.cs
    Windowをまたいで一時的に置いておきたいデータとマスターを管理するクラス
    値が常に確認できるようにSingletonMonobehaviourを利用
*/
public class DataManager : SingletonMonoBehaviour<DataManager>
{
    #region Declaration
    [Serializable]
    public class Progress
    {
        public int SelectedStage = -1;
    }
    public Progress ProgressInfo = new Progress();
    #endregion

    #region Monobehaviour
    protected virtual void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    #endregion
}
