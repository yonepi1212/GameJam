using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour {

	private AudioSource audioSource;

	public List<AudioClip> BGMList;

	public int CurrentPlayingIndex = -1;

	// Use this for initialization
	void Awake () {
		audioSource = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PlayBGM(int id)
	{
		if (audioSource != null) {
			if (CurrentPlayingIndex != id) {
				audioSource.clip = BGMList [id];
				audioSource.Play ();
				CurrentPlayingIndex = id;
			}
		}
	}
}
