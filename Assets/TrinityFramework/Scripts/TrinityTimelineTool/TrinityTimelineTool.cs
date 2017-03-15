// UnityEventとScriptableObjectの親和性が悪すぎるので
// 一旦保存形式をMonoBehaviorにする
//#define USE_MONOBEHAVIOUR_INSTEAD_OF_SCRIPTBLEOBJECT
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
/* TrinityTimelineTool.cs
    タイムラインツールの再生機能
*/
[ExecuteInEditMode]
public class TrinityTimelineTool : MonoBehaviour
{
#region Declration
    private float _startTime;
    public float TimeLength = 10f;
    public float CurrentTime;
    public bool IsPlaying;
#if USE_MONOBEHAVIOUR_INSTEAD_OF_SCRIPTBLEOBJECT
    [SerializeField]
    private List<TrinityEvent> _events = new List<TrinityEvent>();
    public List<TrinityEvent> Events
    {
        get
        {
            if (_events == null || _events.Contains(null))
            {
                _events = GetComponents<TrinityEvent>().ToList();
            }
            return _events;
        }
    }
#else
    public List<TrinityEvent> Events = new List<TrinityEvent>();
#endif
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
            CurrentTime = Time.realtimeSinceStartup - _startTime;
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

        _startTime = Time.realtimeSinceStartup;
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

    public TrinityEvent AddEvent()
    {
#if USE_MONOBEHAVIOUR_INSTEAD_OF_SCRIPTBLEOBJECT
        var trinityEvent = gameObject.AddComponent<TrinityEvent>();
#else
        var trinityEvent = ScriptableObject.CreateInstance<TrinityEvent>();
#endif
        // たまにバグってUnityEventのシリアライズが失敗する
        // そのときに、一瞬Monobehaviorを使うと直る。理由は不明
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
