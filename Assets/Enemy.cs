using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Enemy : CharacterBase
{

	[SerializeField]
	Sprite _damageSprite;

	Image _bodyImage;

	// Use this for initialization
	void Start ()
	{
		_bodyImage = GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void OnCollisionEnter2D (Collision2D coll)
	{
		if ( coll.gameObject.tag == "Player" ) {

			if ( coll.transform.position.y > transform.position.y + 0.3f ) {

				LogUtility.Log ();

				// damage
				_bodyImage.sprite = _damageSprite;

				Invoke ("DestroyObject", 0.1f);
			}
			else {
				Common.Stage.IsGameOver.Value = true;
				coll.transform.GetComponent<PlayerCharacter> ().Damage ();
			}
		}

	}

	private void DestroyObject ()
	{
		Destroy (gameObject);

	}
}
