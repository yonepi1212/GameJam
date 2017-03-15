using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// フェードイン・アウトするcanvas
/// </summary> 
public class FadeCanvas : CanvasBase
{
    /// <summary>
    /// コルーチンに渡すメソッド名
    /// </summary> 
    private enum Method
    {
        AlphaIncremental,
        AlphaDecrement
    }

    /// <summary>
    /// スレッド処理の排他制御用ダミーオブジェクト
    /// </summary>
    private object _objectLock = new Object();

    /// <summary>
    /// エフェクトのインスタンスを保持しておくクラス
    /// </summary>
    [System.Serializable]
    public class Effects
    {
        public Image White;
    }

    /// <summary>
    /// エフェクトのインスタンス
    /// </summary>
    public Effects Effect;

    /// <summary>
    /// Canvas状態の列挙型
    /// </summary>
    public enum CanvasState
    {
        None = -1,
        WhiteIn,
        WhiteOut
    }

    /// <summary>
    /// Canvasの状態
    /// </summary>
    [SerializeField] 
    private CanvasState _state;

    /// <summary>
    /// Canvasの状態
    /// </summary>
    public CanvasState State
    {
        get{ return _state; }
    }

    //public WindowBase Window;

    /// <summary>
    /// ホワイトInOutのImage
    /// </summary>
    public virtual Image WhiteEffect
    {
        get
        {
            if (Effect.White == null)
            {
                GameObject g = Instantiate(Tools.White.gameObject);
                g.name = Tools.White.gameObject.name;

                Transform t = g.transform;
                t.SetParent(transform, false);

                Effect.White = g.GetComponent<Image>();
            }

            return Effect.White;
        }
    }

    #region MonoBehaviour

    /// <summary>
    /// Awake
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        //Log.D();
        _state = CanvasState.None;
    }

    /// <summary>
    /// OnDestroy
    /// </summary>
    protected override void OnDestroy()
    {
        base.OnDestroy();
        _objectLock = null;
        Effect = null;
        whiteInNotification = null;
        whiteOutNotification = null;
    }

    #endregion

    #region WhiteInOut

    /// <summary>
    /// ホワイトイン完了時に呼ばれるイベント
    /// </summary>
    private event CompleteHandler whiteInNotification = delegate () { };

    /// <summary>
    /// ホワイトインのコンプリートハンドラー
    /// </summary>
    public event CompleteHandler WhiteInComplete
    {
        add
        {
            lock (_objectLock)
            {
                whiteInNotification += value;
            }
        }
        remove
        {
            lock (_objectLock)
            {
                whiteInNotification -= value;
            }
        }
    }

    /// <summary>
    /// ホワイトアウト完了時に呼ばれるイベント
    /// </summary>
    private event CompleteHandler whiteOutNotification = delegate () { };

    /// <summary>
    /// ホワイトアウトのコンプリートハンドラー
    /// </summary>
    public event CompleteHandler WhiteOutComplete
    {
        add
        {
            lock (_objectLock)
            {
                whiteOutNotification += value;
            }
        }
        remove
        {
            lock (_objectLock)
            {
                whiteOutNotification -= value;
            }
        }
    }

    /// <summary>
    /// ホワイトイン
    /// </summary>
    public virtual void WhiteIn(CompleteHandler callback = null, float z = 100f)
    {
        switch (_state)
        {
            case CanvasState.None:

                if (!WhiteEffect.IsActive())
                {
                    WhiteEffect.enabled = true;
                }

                WhiteInComplete += callback;
                StartCoroutine(Method.AlphaDecrement.ToString());

                break;

            case CanvasState.WhiteIn:
                break;

            case CanvasState.WhiteOut:

                whiteInNotification = delegate { };
                StopCoroutine(Method.AlphaIncremental.ToString());

                WhiteOutComplete += callback;
                StartCoroutine(Method.AlphaDecrement.ToString());

                break;
        }
    }

    /// <summary>
    /// ホワイトアウト
    /// </summary>
    public virtual void WhiteOut(CompleteHandler callback = null, float z = 100f)
    {
        switch (_state)
        {
            case CanvasState.None:

                if (!WhiteEffect.IsActive())
                {
                    WhiteEffect.enabled = true;
                }

                WhiteOutComplete += callback;
                StartCoroutine(Method.AlphaIncremental.ToString());

                break;

            case CanvasState.WhiteIn:

                whiteInNotification = delegate { };
                StopCoroutine(Method.AlphaDecrement.ToString());

                WhiteOutComplete += callback;
                StartCoroutine(Method.AlphaIncremental.ToString());

                break;

            case CanvasState.WhiteOut:
                break;
        }
    }

    /// <summary>
    /// アルファを増加させる
    /// </summary> 
    private IEnumerator AlphaIncremental()
    {
        _state = CanvasState.WhiteIn;

        WhiteEffect.transform.SetAsLastSibling();

        Color color = WhiteEffect.color;
        float alpha = color.a;

        while (alpha < 1.0f)
        {
            yield return new WaitForSeconds(0.025f);
            alpha = Mathf.Clamp01(alpha + 0.1f);
            color.a = alpha;
            WhiteEffect.color = color;
        }

        _state = CanvasState.None;

        yield return new WaitForSeconds(0.25f);

        if (whiteOutNotification != null)
        {
            whiteOutNotification();
            whiteOutNotification = delegate { };
        }

        WhiteEffect.transform.SetAsLastSibling();
    }

    /// <summary>
    /// アルファを減少させる
    /// </summary> 
    private IEnumerator AlphaDecrement()
    {
        _state = CanvasState.WhiteOut;

        WhiteEffect.transform.SetAsLastSibling();

        Color color = WhiteEffect.color;
        float alpha = color.a;

        while (alpha > 0.0f)
        {
            yield return new WaitForSeconds(0.025f);

            alpha = Mathf.Clamp01(alpha - 0.1f);
            color.a = alpha;
            WhiteEffect.color = color;
        }

        _state = CanvasState.None;

        yield return new WaitForSeconds(0.25f);

        WhiteEffect.enabled = false;

        if (whiteInNotification != null)
        {
            whiteInNotification();
            whiteInNotification = delegate { };
        }
    }

    #endregion

    #region Method


#if false
    /// <summary>
    /// ホワイトのポジションを設定する
    /// </summary> 
    public void SetWhitePosition(float x, float y, float z)
    {
        RectTransform rt = WhiteEffect.GetComponent<RectTransform>();
        rt.localPosition = new Vector3(0f, 0f, z);
    }
#endif
    #endregion
}