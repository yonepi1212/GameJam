using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/* TitleCanvas.cs
    ゲームの始まり
    緊急時にタイトルに飛ばしてメモリを開放したいので、
    タイトルはMainCanvasで管理せずに、単一シーンとして扱う
*/
public class TitleCanvas : CanvasBase
{
	#region Declaration

	public Button StartButton;

	#endregion

	#region Monobehaviour

	protected override void Start ()
	{
		base.Start ();
		// maincanvasの削除(メモリ解放用)
		var mainCanvas = GameObject.Find ("main_canvas");
		if ( mainCanvas != null ) {
			Destroy (mainCanvas.gameObject);
		}

		// ボタンのイベント登録
		StartButton.OnClickAsObservable ().Subscribe (_ => {
			SceneManager.LoadScene ("InGame");
		});
	}

	#endregion
}
