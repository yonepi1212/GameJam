﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UniRx.Triggers;

public class TitleWindow : MonoBehaviour {

	public Button NewGameButton;

	public Button ContinueButton;

	private SoundManager _soundManager;

	public TutorialImage _tutorialImage;

	void Awake()
	{
		_soundManager = GameObject.Find ("SoundManager").GetComponent<SoundManager> ();
	}

	// Use this for initialization
	void Start () {
		NewGameButton.onClick.AddListener (StartNewGame);
		ContinueButton.onClick.AddListener (StartContinue);
		_soundManager.BgmManager.PlayBGM (0);

	}
	
	// Update is called once per frame
	void Update () {
		
	}


	private void StartNewGame()
	{
		Debug.Log ("初めから");
		PlayerPrefs.DeleteAll ();
		//SceneManager.LoadScene ("IngameScene");
		_tutorialImage.gameObject.SetActive(true);
		_tutorialImage.StartTextAndNextScene();
	}

	private void StartContinue()
	{
		Debug.Log ("続きから");
		SceneManager.LoadScene ("IngameScene");
	}
}
