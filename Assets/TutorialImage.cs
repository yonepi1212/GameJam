using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using Random = UnityEngine.Random;
using System;
using UniRx;

public class TutorialImage : MonoBehaviour {

	public Text text1;
	public Text text2;
	public Text text3;
	public Text text4;

	// Use this for initialization
	void Start () {
		text1.gameObject.SetActive (true);
		text1.color = new Color (1, 1, 1, 0);
		text2.gameObject.SetActive (false);
		text2.color = new Color (1, 1, 1, 0);
		text3.gameObject.SetActive (false);
		text3.color = new Color (1, 1, 1, 0);
		text4.gameObject.SetActive (false);
		text4.color = new Color (1, 1, 1, 0);

	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void StartTextAndNextScene()
	{
		var span = 3f;

		text1.gameObject.SetActive (true);
		LeanTween.alphaText(text1.gameObject.GetComponent<RectTransform>(),1.0f,0.5f);


		Observable.Timer(TimeSpan.FromSeconds(span)).Subscribe(_=>{
			text2.gameObject.SetActive (true);
			LeanTween.alphaText(text2.gameObject.GetComponent<RectTransform>(),1.0f,0.5f);

		}).AddTo(text2.gameObject);

		Observable.Timer(TimeSpan.FromSeconds(span * 2)).Subscribe(_=>{
			text3.gameObject.SetActive (true);
			LeanTween.alphaText(text3.gameObject.GetComponent<RectTransform>(),1.0f,0.5f);

		}).AddTo(text3.gameObject);

		Observable.Timer(TimeSpan.FromSeconds(span * 3)).Subscribe(_=>{
			text4.gameObject.SetActive (true);
			LeanTween.alphaText(text4.gameObject.GetComponent<RectTransform>(),1.0f,0.5f);

		}).AddTo(text4.gameObject);

		Observable.Timer(TimeSpan.FromSeconds(span * 4.2f)).Subscribe(_=>{
			SceneManager.LoadScene("IngameScene");
		});
	}
}
