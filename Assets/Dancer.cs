using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dancer : MonoBehaviour {

	public bool IsReverse;

	public float shakeX = 50;

	// Use this for initialization
	void Start () {
		StartShake ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void StartShake()
	{
		var shakePositionX = shakeX;
		if (IsReverse) {
			shakePositionX *= -1;
		}
		LeanTween.moveLocalX (gameObject, transform.localPosition.x + shakePositionX, 3f).setEaseShake().setLoopPingPong();
	}
}
