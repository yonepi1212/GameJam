using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

/* 
    汎用ダイアログ
    文言、はい、いいえを設定して使用する
*/
public class CommonDialogScreen : ScreenBase
{

	#region Declaration

	[SerializeField]
	private Text _descriptionText;

	[SerializeField]
	private ButtonBase _yesButton;

	[SerializeField]
	private ButtonBase _noButton;

	private Action _yesCallBack;

	private Action _noCallBack;

	#endregion

	#region Public Method

	public void Initialization (string description, Action yesCallBack, Action noCallBack)
	{
		_descriptionText.text = description;
		_yesCallBack = yesCallBack;
		_noCallBack = noCallBack;
		_yesButton.OnClickAsObservable ().Subscribe (_ => _yesCallBack.Call ());
		_noButton.OnClickAsObservable ().Subscribe (_ => _noCallBack.Call ());
	}

	/// <summary>
	/// 事前にかならずInitialization()で動作を登録してください。
	/// </summary>
	public override void Show ()
	{
		_yesButton.Show ();
		_noButton.Show ();

		if ( _descriptionText.text.Equals ("") ) {
			var errorText = "文言が登録されていません。";
			_descriptionText.text = errorText;
			Debug.Log (errorText);
		}

		if ( _yesCallBack == null ) {
			Debug.Log ("[Dialog] Yesのコールバックが登録されていません。");
		}

		if ( _noCallBack == null ) {
			Debug.Log ("[Dialog] Noのコールバックが登録されていません。");
		}

		base.Show ();
	}

	#endregion
}
