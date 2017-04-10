using UnityEngine;

/*  Common.cs
    汎用クラス
*/

public static class Common
{
	#region MainCanvas

	// MainCanvasのインスタンス
	private static MainCanvas _mainCanvas;

	public static MainCanvas MainCanvas {
		get {
			if ( _mainCanvas == null ) {
				GameObject canvasObject = GameObject.Find (Define.Name.MainCanvas) ?? PrefabUtility.LoadGameObject (Define.Path.Prefab.Canvas.Main, Define.Name.MainCanvas);
				_mainCanvas = canvasObject.GetComponent<MainCanvas> ();
			}
			return _mainCanvas;
		}
	}

	#endregion

	#region World Object

	private static Transform _controllers;

	public static Transform Controllers {
		get {
			if ( _controllers == null ) {
				var g = new GameObject { name = "controllers" };
				Object.DontDestroyOnLoad (g);
				_controllers = g.transform;
			}

			return _controllers;
		}
	}

	private static Transform _world;

	public static Transform World {
		get {
			if ( _world == null ) {
				var g = new GameObject { name = "world" };
				Object.DontDestroyOnLoad (g);
				_world = g.transform;
			}

			return _world;
		}
	}




	#endregion

	#region Singleton Instance

	// AddOnManagerrのインスタンス
	public static AddOnManager AddOn = null;


	// カメラマネージャーのインスタンス
	public static CameraManager Camera {
		get { return CameraManager.Instance; }
	}

	public static GameSpeedManager GameSpeed {
		get { return GameSpeedManager.Instance; }
	}

	public static TimeManager Time {
		get { return TimeManager.Instance; }
	}

	public static InputManager Input {
		get { return InputManager.Instance; }
	}

	public static StageManager Stage {
		get { return StageManager.Instance; }
	}

	#endregion






}
