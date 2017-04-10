using System.Collections.Generic;
using UniRx;
using System;
using UnityEngine;
using UnityEngine.UI;


public class InGameUiScreen : ScreenBase
{
	#region Declaration

	private bool _isInitialize;

	[SerializeField]
	private RectTransform _stageInfo;

	[SerializeField]
	private ControlButtons _controllButtons;


	[SerializeField]
	private RectTransform _gameOver;

	#endregion

	#region Public Method

	public override void Show ()
	{
		base.Show ();

		_controllButtons.Show ();
	
		_gameOver.gameObject.SetActive (false);

		_stageInfo.gameObject.SetActive (true);

		var waitTime = 5f;

		#if UNITY_EDITOR 
		waitTime = 0.5f;
		#endif

		Observable.Timer (TimeSpan.FromSeconds (waitTime)).Subscribe (_ => { 
			_stageInfo.gameObject.SetActive (false);
		});





		var homeWindow = Root as InGameWindow;
		if ( homeWindow != null ) {

		}


		Common.Stage.IsGameOver.Subscribe (ShowGameOver);

	}

	private void ShowGameOver (bool isOver)
	{
		if ( _gameOver != null ) {
			_gameOver.gameObject.SetActive (isOver);
		}
	}

	#endregion

	#region Private Method


	#endregion
}
