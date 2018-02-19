using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEManager : MonoBehaviour {

	private AudioSource audioSource;

	public List<AudioClip> SEList;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PlaySE(int id)
	{
		audioSource.clip = SEList [id];
		audioSource.Play ();
	}
}
