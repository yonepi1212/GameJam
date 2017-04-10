using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TapManager : MonoBehaviour {

	[SerializeField]
	private GameManager _gameManager;

	[SerializeField]
	private Button _tapButton;


	// Use this for initialization
	void Start () {
		_tapButton.onClick.AddListener (() => {
			Tap();
		});
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void Tap()
	{
		_gameManager.OnTaped ();
	}
}
