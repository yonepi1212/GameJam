using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;


public class InGameUiScreen : ScreenBase
{
	#region Declaration

	private bool _isInitialize;

	[SerializeField]
	private ButtonBase _chatButton;


	#endregion

	#region Public Method

	public override void Show ()
	{
		base.Show ();

		LogUtility.Log ();

		var homeWindow = Root as InGameWindow;
		if ( homeWindow != null ) {

		}


	}

	#endregion

	#region Private Method


	#endregion
}
