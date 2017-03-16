using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using UniRx.Triggers;

public class PlayerCharacter : CharacterBase
{

	private Camera _uiCamera;

	private Rigidbody2D _rigidBody;

	void Update ()
	{
		LogUtility.Log ();
	}

	public override void Show ()
	{
		base.Show ();
		LogUtility.Log ();

		_rigidBody = GetComponent<Rigidbody2D> ();

		_uiCamera = Common.Camera.GetCamera ("ui_camera");
		_uiCamera.transform.SetParent (transform);


		Observable.EveryUpdate ().Subscribe (_ => {

			_uiCamera.transform.position = new Vector3 (RectTransform.position.x + 4, 0, 0);

		}).AddTo (_uiCamera);

		IObservable<Unit> keydown = this.UpdateAsObservable ();


		keydown.Where (key => Input.GetKey (KeyCode.D)).Subscribe (_ => {
			_rigidBody.AddForce (new Vector2 (100f, 0f));
			LogUtility.Log ();
	

		});

	}



}
