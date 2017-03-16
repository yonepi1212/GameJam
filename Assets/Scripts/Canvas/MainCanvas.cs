using UnityEngine;
using UnityEngine.SceneManagement;

/*  MainCanvas
    メインキャンバス
*/
public class MainCanvas : CanvasBase
{
	#region Declaration

	// 二重起動防止用
	private static MainCanvas _mainCanvasInstance;

	[System.Serializable]
	public class Holder
	{
		public WindowBase TitleWindow;
		public WindowBase InGameWindow;
	}
	// Prefabsのインスタンス
	[SerializeField]
	private Holder _prefabs;
	// ウィンドウインスタンス
	public Shelf<WindowBase, WindowBase> WindowShelf = new Shelf<WindowBase, WindowBase> ();
	[Header ("最初に起動したいWindowを選択")]
	public WindowBase FirstWindow;


	#endregion

	#region Window Property

	// タイトルウインドウのプロパティ
	public TitleWindow TitleWindow {
		get {
			return WindowShelf.SetBook (_prefabs.TitleWindow) as TitleWindow;
		}
	}

	public InGameWindow InGameWindow {
		get {
			return WindowShelf.SetBook (_prefabs.InGameWindow) as InGameWindow;
		}
	}





	#endregion

	#region Public Method

	public void Close (WindowBase window)
	{
		WindowShelf.SetBook (window);
	}

	#endregion

	#region MonoBehaviour

	// Awake
	protected override void Awake ()
	{
		if ( _mainCanvasInstance == null ) {
			_mainCanvasInstance = this;
			base.Awake ();
			DontDestroyOnLoad (gameObject);
		}
		else {
			Destroy (gameObject);
			Destroy (this);
		}
	}
	// Start
	protected override void Start ()
	{
		WindowShelf.RootTransform = Transform;
		if ( FirstWindow != null ) {
			WindowShelf.SetBook (FirstWindow).Show ();
		}

	}
	// OnDestroy
	protected override void OnDestroy ()
	{
		//Log.D();
		_prefabs = null;
		_mainCanvasInstance = null;
		WindowShelf = null;
		base.OnDestroy ();
	}
	#if DEBUG
	private void OnGUI ()
	{
		if ( GUI.Button (new Rect (1036f, 0f, 100f, 25f), "GoTitle") ) {
			SceneManager.LoadScene ("Title");
		}
	}
	#endif
	#endregion

}
