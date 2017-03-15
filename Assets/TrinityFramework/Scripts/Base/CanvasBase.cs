using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// キャンバスの基底クラス
/// </summary> 
[RequireComponent(typeof(Canvas))]
public class CanvasBase : UGuiMonoBehaviour
{
    #region Declaration
    /// <summary>
    /// コンプリートハンドラー
    /// </summary> 
    public delegate void CompleteHandler();

    /// <summary>
    /// スレッド処理の排他制御用ダミーオブジェクト
    /// </summary>
    private object _objectLock = new Object();

    /// <summary>
    /// Canvasツール群
    /// </summary>
    public CanvasTools Tools;

    //public WindowBase Window;

    /// <summary>
    /// イベントシステム
    /// </summary>
    private EventSystem _eventSystem;

    // Width
    public float Width
    {
        get
        {
            return RectTransform.sizeDelta.x;
        }
    }
    // Height
    public float Height
    {
        get
        {
            return RectTransform.sizeDelta.y;
        }
    }
    #endregion

    #region Canvas

    /// <summary>
    /// イベントシステムを探す
    /// </summary>
    public virtual EventSystem InitializeEventSystem()
    {
        if (_eventSystem == null)
        {
            GameObject eventSystem = GameObject.Find(Define.Name.EventSystem);

            if (eventSystem == null)
            {
                eventSystem = new GameObject { name = Define.Name.EventSystem };
                eventSystem.AddComponent<EventSystem>();
                eventSystem.AddComponent<StandaloneInputModule>();
            }
            _eventSystem = eventSystem.GetComponent<EventSystem>();
            _eventSystem.transform.SetParent(Transform);
        }

        return _eventSystem;
    }

    private Canvas _canvas;
    public Canvas Canvas
    {
        get { return _canvas ?? (_canvas = GetComponent<Canvas>()); }
    }

    // Canvasのカメラをセットアップする
    private Camera _uiCamera;
    public virtual Camera InitializeUiCamera()
    {
        if (_uiCamera == null)
        {
            _uiCamera = Common.Camera.CreateOrthographicCamera(Define.Name.Camera.UI);
            _uiCamera.InitializeLayer("UI");
        }

        // canvasにカメラを使うように設定
        Canvas.renderMode = RenderMode.ScreenSpaceCamera;
        Canvas.worldCamera = _uiCamera;
        return _uiCamera;
    }

    /// <summary>
    /// クローズが完了したときに呼ばれるイベント
    /// </summary>
    private event CompleteHandler closeNotification = delegate () { };

    /// <summary>
    /// クローズが完了したときに呼ばれるイベント
    /// </summary>
    public event CompleteHandler CloseNotification
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

    /// <summary>
    /// キャンバスを表示する
    /// </summary>
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// キャンバスを消す
    /// </summary>
    public virtual void Close()
    {
        if (closeNotification != null)
        {
            closeNotification();
        }

        Destroy(gameObject);
    }

    /// <summary>
    /// キャンバスを非表示にする
    /// </summary>
    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }

    #endregion

    #region MonoBehaviour

    /// <summary>
    /// Awake
    /// </summary>
    protected virtual void Awake()
    {
        //Log.D();
        InitializeEventSystem();
        InitializeUiCamera();
    }

    /// <summary>
    /// Start
    /// </summary>
    protected virtual void Start()
    {
        //Log.D();
    }

    /// <summary>
    /// Update
    /// </summary>
    protected virtual void Update()
    {
        // Log.D();
    }

    /// <summary>
    /// OnDestroy
    /// </summary>
    protected override void OnDestroy()
    {
        base.OnDestroy();
        _objectLock = null;
        Tools = null;
        _eventSystem = null;
        closeNotification = null;
    }

    #endregion

    #region Method

    /// <summary>
    /// ウインドウを作成する
    /// </summary>  
    protected virtual WindowBase CreateWindow(WindowBase instance)
    {
        GameObject g = Instantiate(instance.gameObject);
        g.name = instance.gameObject.name;

        Transform t = g.transform;
        t.SetParent(Transform, false);

        WindowBase window = g.GetComponent<WindowBase>();

        return window;
    }

    #endregion

}