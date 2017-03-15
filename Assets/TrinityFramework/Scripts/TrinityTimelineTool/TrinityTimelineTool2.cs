// UnityEventとScriptableObjectの親和性が悪すぎるので
// 一旦保存形式をMonoBehaviorにする
//#define USE_MONOBEHAVIOUR_INSTEAD_OF_SCRIPTBLEOBJECT

using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

/* TrinityTimelineTool.cs
    タイムラインツールの再生機能
*/
[ExecuteInEditMode]
public class TrinityTimelineTool2 : MonoBehaviour
{
#region Declration
    private float _startTime;
    [HideInInspector]
    public float TimeLength = 10f;
    [HideInInspector]
    public float CurrentTime;
    [HideInInspector]
    public bool IsPlaying;
    public List<TrinityEvent2> Events = new List<TrinityEvent2>();
#endregion

    #region Monobehaviour
    protected virtual void Start()
    {
        Observable.EveryUpdate()
                  .Where(_ => IsPlaying && CurrentTime <= TimeLength)
                  .Subscribe(_ => FireEvent());
    }
#endregion

#region Public Method
    public void FireEvent()
    {
        if (IsPlaying)
        {
            CurrentTime = (Common.Time.timeSinceLevelLoad - _startTime);
            Events.ForEach(te =>
            {
                if (te.IsFired == false && te.FireTime <= CurrentTime)
                {
                    te.IsFired = true;
                    te.FireEvent.Invoke();
                }
            });
        }
    }

    public void Play()
    {
        _startTime = Common.Time.timeSinceLevelLoad;
        IsPlaying = true;
    }

    public void Pause()
    {
        IsPlaying = false;
    }

    public void Stop()
    {
        IsPlaying = false;
        Events.ForEach(trinityEvent => trinityEvent.IsFired = false);
        CurrentTime = 0f;
    }

    public TrinityEvent2 AddEvent()
    {
        var trinityEvent = new TrinityEvent2();
        // インスペクタ用にイベントのテーマカラーをランダムに設定
        trinityEvent.Color = GetRandomColor();
        Events.Add(trinityEvent);
        return trinityEvent;
    }
#endregion

#region Private Method
    private Color GetRandomColor()
    {
        var r = Random.Range(0f, 1f);
        var b = Random.Range(0f, 1f);
        var g = Random.Range(0f, 1f);
        // 色をはっきりさせるために、rgbを近い値にさせない
        while (Mathf.Abs(r - b) <= 0.3f)
        {
            b = Random.Range(0f, 1f);
        }
        while (Mathf.Abs(b - g) <= 0.15f)
        {
            g = Random.Range(0f, 1f);
        }
        return new Color(r,b,g);
    }
#endregion

}
