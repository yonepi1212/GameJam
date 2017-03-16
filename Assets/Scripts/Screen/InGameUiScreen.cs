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




	#endregion

	#region Public Method

	public override void Show ()
	{
		base.Show ();

		LogUtility.Log ();

		_stageInfo.gameObject.SetActive (true);


		Observable.Timer (TimeSpan.FromSeconds (0.5f)).Subscribe (_ => { 
			_stageInfo.gameObject.SetActive (false);
		});


		var homeWindow = Root as InGameWindow;
		if ( homeWindow != null ) {

		}


	}

	#endregion

	#region Private Method


	#endregion
}
