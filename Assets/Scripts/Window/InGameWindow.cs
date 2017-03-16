using UnityEngine;

/* HomeWindow.cs
    ホームウィンドウ
*/
public class InGameWindow : WindowBase
{

	#region Declaration

	[System.Serializable]
	public class Holder
	{
		public ScreenBase InGameScreen;
		//public ControllerBase TalkController;
	}

	// Prefabsのインスタンス
	[SerializeField]
	private Holder _prefabs = null;

	// スクリーンのプロパティ
	public InGameScreen InGameScreen {
		get {
			return ScreenShelf.SetBook (_prefabs.InGameScreen) as InGameScreen;
		}
	}


	//	public TalkController TalkController {
	//		get {
	//			return ControllerShelf.SetBook (_prefabs.TalkController) as TalkController;
	//		}
	//	}

	#endregion

	#region MonoBehaviour

	public override void Show ()
	{
		base.Show ();
		InGameScreen.Show ();
	}

	#endregion

	#region Public Method


	#endregion
}
