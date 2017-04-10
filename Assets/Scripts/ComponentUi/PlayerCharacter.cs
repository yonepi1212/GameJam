using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UniRx;
using UniRx.Triggers;

public class PlayerCharacter : CharacterBase
{

	private Camera _uiCamera;
	private Rigidbody2D _rigidBody;

	private Image _bodyImage;
	[SerializeField]
	private Sprite _defaultSprite;

	[SerializeField]
	private Sprite _jumpSprite;

	[SerializeField]
	private Sprite _damageSprite;

	bool isJumping = false;

	void Update ()
	{
	}

	public override void Show ()
	{
		base.Show ();
		LogUtility.Log ();

		_rigidBody = GetComponent<Rigidbody2D> ();

		_bodyImage = GetComponent<Image> ();

		_uiCamera = Common.Camera.GetCamera ("ui_camera");
		_uiCamera.transform.SetParent (transform);


		Observable.EveryUpdate ().Subscribe (_ => {

			_uiCamera.transform.position = new Vector3 (RectTransform.position.x + 4, 0, 0);
		

		}).AddTo (_uiCamera);

		IObservable<Unit> keydown = this.UpdateAsObservable ();


		Common.Input.IsLeftKey.Subscribe (_ => {
			if ( Mathf.Abs (_rigidBody.velocity.x) < 2.5f ) {
				_rigidBody.AddForce (new Vector2 (-120f, 0f));
			}
		}).AddTo (_rigidBody);

		Common.Input.IsRightKey.Subscribe (_ => {
			if ( Mathf.Abs (_rigidBody.velocity.x) < 2.5f ) {
				_rigidBody.AddForce (new Vector2 (120f, 0f));
			}
		}).AddTo (_rigidBody);

		Common.Input.IsJumpKey.ThrottleFirstFrame (30).Subscribe (_ => {
			_rigidBody.AddForce (new Vector2 (0f, 340f));
			_bodyImage.sprite = _jumpSprite;
			Invoke ("SetDefaultSprite", 1f);
		}).AddTo (_rigidBody);

	}


	public void Damage ()
	{
		_bodyImage.sprite = _damageSprite;
	}

	private void SetDefaultSprite ()
	{
		_bodyImage.sprite = _defaultSprite;
	}
}
