using UniRx;
using UnityEngine;
using UnityEngine.UI;

/* TitleWindow.cs
    タイトルウィンドウ
*/
public class TitleWindow : WindowBase
{
	#region Declaration

	// スタートボタン
	[SerializeField]
	private Button _startButton;

	#endregion

	#region MonoBehaviour

	/// <summary>
	/// OnDestroy
	/// </summary>
	protected override void OnDestroy ()
	{
		//Log.D();
		_startButton = null;
		base.OnDestroy ();
	}

	#endregion

	#region Public Method

	public override void Show ()
	{
		base.Show ();
		_startButton.OnClickAsObservable ()
            .Subscribe (_ => {
			Common.MainCanvas.TitleWindow.Close ();
			Common.MainCanvas.InGameWindow.Show ();
		});
	}

	#endregion


}