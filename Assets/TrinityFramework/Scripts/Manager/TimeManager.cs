using UniRx;
using UnityEngine;
/* TimeManager.cs
    ゲーム内の経過時間を保持する
*/
public class TimeManager : SingletonMonoBehaviour<TimeManager>
{
    #region Declration
    private float _deltaTime;
    /// <summary>
    /// 前フレームからの経過時間
    /// </summary>
    public float deltaTime
    {
        get { return _deltaTime; }
    }
    private float _timeSinceLevelLoad;
    /// <summary>
    /// ゲームが起動してからの経過時間
    /// </summary>
    public float timeSinceLevelLoad
    {
        get { return _timeSinceLevelLoad; }
    }
    #endregion

    #region Monobehavior
    protected override void Awake()
    {
        base.Awake();
        _timeSinceLevelLoad = Time.timeSinceLevelLoad;
        Observable.EveryUpdate()
                  .Subscribe((_) =>
                  {
                      // TODO:ゲームの速度をDataManagerに持たせるか悩み中
                      _deltaTime = Time.deltaTime * Common.GameSpeed.Value;
                      _timeSinceLevelLoad += _deltaTime;
                  }).AddTo(this);
        DontDestroyOnLoad(gameObject);
    }
    #endregion
}
