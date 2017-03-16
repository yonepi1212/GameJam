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

	bool isJumping = false;

	void Update ()
	{
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



		Common.Input.IsLeftKey.ThrottleFirstFrame (2).Subscribe (_ => {
			_rigidBody.AddForce (new Vector2 (-140f, 0f));
		});

		Common.Input.IsRightKey.ThrottleFirstFrame (2).Subscribe (_ => {
			_rigidBody.AddForce (new Vector2 (140f, 0f));
		});

		Common.Input.IsJumpKey.ThrottleFirstFrame (30).Subscribe (_ => {
			_rigidBody.AddForce (new Vector2 (0f, 340f));
		});


	}



}
