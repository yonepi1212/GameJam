using System.Collections.Generic;
using UniRx;
using System;
using UnityEngine;
using UnityEngine.UI;


public class InGameScreen : ScreenBase
{
	#region Declaration

	private bool _isInitialize;

	public InGameUiScreen IngameUIScreen;

	[NonSerialized]
	public Canvas UiCanvas;
	[SerializeField]
	private GameObject _uiCanvasOrigin;


	[SerializeField]
	private InGameUiScreen _ingameUiScreenOrigin;

	[SerializeField]
	private PlayerCharacter _player;




	#endregion

	#region Public Method

	public override void Show ()
	{
		base.Show ();

		_player.Show ();

		UiCanvas = Instantiate (_uiCanvasOrigin).GetComponent<Canvas> ();

		IngameUIScreen = Instantiate (_ingameUiScreenOrigin.gameObject).GetComponent<InGameUiScreen> ();
		IngameUIScreen.transform.SetParent (UiCanvas.transform);
		IngameUIScreen.RectTransform.anchoredPosition = Vector2.zero;


		IngameUIScreen.Show ();

	



		var ingameWindow = Root as InGameWindow;
		if ( ingameWindow != null ) {

		}


	}

	#endregion

	#region Private Method


	#endregion
}
