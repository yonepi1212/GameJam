using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

using UniRx;
using UniRx.Triggers;

public class ControlButtons : ComponentUiBase
{

	[SerializeField]
	private Button _leftButton;

	[SerializeField]
	private Button _rightButton;

	[SerializeField]
	private Button _jumpButton;


	public override void Show ()
	{
//		_leftButton.OnClickAsObservable ().Subscribe (_ => {
//			Common.Input.IsLeftKey.Value = true;
//		});
//


//		var left = _leftButton.gameObject.AddComponent<ObservableEventTrigger> ();
//
//		this.UpdateAsObservable ()
//			.SkipUntil (left.OnPointerDownAsObservable ())  // ポインタが下がるまでスキップ
//			.TakeUntil (left.OnPointerUpAsObservable ())    // ポインタが上がると終了
//			.Repeat ()   // 繰り返す
//			.Select (_ => Input.mousePosition)
//			.Subscribe (pos => {
//			Common.Input.IsLeftKey.Value = true;
//			LogUtility.Log ("Left押している");
//		})
//			.AddTo (_leftButton.gameObject);
//
//		left.OnPointerUpAsObservable.Subscribe (_ => {
//			Common.Input.IsLeftKey.Value = false;
//			LogUtility.Log ("Left離した");
//		});
//

		SetButtonPushing (_leftButton, () => {
			Common.Input.IsLeftKey.Value = true;
			LogUtility.Log ("Left押している");
		}, () => {
			Common.Input.IsLeftKey.Value = false;
			LogUtility.Log ("Left離した");
		}, null);

		SetButtonPushing (_rightButton, () => {
			Common.Input.IsRightKey.Value = true;
		}, () => {
			Common.Input.IsRightKey.Value = false;
		}, null);

		SetButtonPushing (_jumpButton, () => {
			Common.Input.IsJumpKey.Value = true;
		}, () => {
			Common.Input.IsJumpKey.Value = false;
		}, null);

	}



	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		#if UNITY_EDITOR

		if ( Input.GetKey (KeyCode.RightArrow) ) {
			Common.Input.IsRightKey.Value = true;
		}
		else {
			Common.Input.IsRightKey.Value = false;

		}

		if ( Input.GetKey (KeyCode.LeftArrow) ) {
			Common.Input.IsLeftKey.Value = true;
		}
		else {
			Common.Input.IsLeftKey.Value = false;

		}

		if ( Input.GetKey (KeyCode.Space) ) {
			Common.Input.IsJumpKey.Value = true;
		}
		else {
			Common.Input.IsJumpKey.Value = false;

		}
			

		#endif
	}

	/// <summary>
	/// 長押し中のイベント
	/// </summary>
	/// <param name="button">Button.</param>
	/// <param name="actDown">ボタンが押された時のAction</param>
	/// <param name="actUp">ボタンが離された時のAction</param>
	/// <param name="actPush">ボタンが押されている間のAction</param>
	private void SetButtonPushing (Button button, Action actDown = null, Action actUp = null, Action actPush = null)
	{
		ReactiveProperty<bool> flag = new ReactiveProperty<bool> (false);

		// ボタンダウン
		button.OnPointerDownAsObservable ()
			.Subscribe (_ => {
			flag.Value = true;
			if ( actDown != null ) {
				actDown ();
			}
		});

		// ボタンアップ
		button.OnPointerUpAsObservable ()
			.Subscribe (_ => {
			flag.Value = false;
			if ( actUp != null ) {
				actUp ();
			}
		});

		// 押している間のイベント
		this.UpdateAsObservable ()
			.Select (_ => flag)
			.Where (_ => _.Value)
			.Subscribe (_ => {
			if ( actPush != null ) {
				actPush ();
			}
		});
	}
}
