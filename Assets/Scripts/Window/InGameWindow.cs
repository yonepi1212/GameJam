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
		public ScreenBase InGameUiScreen;
		//public ControllerBase TalkController;
	}

	// Prefabsのインスタンス
	[SerializeField]
	private Holder _prefabs = null;

	// スクリーンのプロパティ
	public InGameUiScreen InGameUiScreen {
		get {
			return ScreenShelf.SetBook (_prefabs.InGameUiScreen) as InGameUiScreen;
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
		InGameUiScreen.Show ();
	}

	#endregion

	#region Public Method


	#endregion
}
