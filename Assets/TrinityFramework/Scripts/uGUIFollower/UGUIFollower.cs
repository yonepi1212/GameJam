using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SocialPlatforms;

public class UGUIFollower : MonoBehaviour
{
	/// <summary>
	/// 追跡対象のゲームオブジェクト
	/// </summary>
	[SerializeField]
	private Transform followTargetObject;

	[SerializeField]
	private Vector3 offset;

	[SerializeField]
	private RectTransform rangeObject;

	[Header("uGUIオブジェクトが動ける範囲がスクリーン全体かどうか")]
	[SerializeField]
	private bool isScreenRange = true;

	// 親カンバス
	private Canvas rootCanvas;

	/// <summary>
	/// このスクリプトアタッチされているオブジェクトのRectTransform
	/// </summary>
	private RectTransform rectTransform;

	/// <summary>
	/// 親オブジェクトのRectTransform
	/// </summary>
	private RectTransform parentRectTransform;

	/// <summary>
	/// canvas worldカメラ
	/// </summary>
	private Camera uiCamera;

	// Use this for initialization
	void Start ()
	{
		rectTransform = gameObject.GetComponent<RectTransform> ();

		if (rectTransform == null)
		{
			Debug.LogError ("uGUIコンポーネント(RectTransform)のあるオブジェクトにアタッチしてください。");
			return;
		}

		if (isScreenRange && rangeObject == null)
		{
			Debug.LogError ("uGUIコンポーネント(RectTransform)のあるオブジェクトの動ける範囲を正しくセットしてください。");
			return;
		}

		parentRectTransform = rectTransform.parent as RectTransform;

		SetRootCanvas ();
		uiCamera = rootCanvas.worldCamera;
	}

	void Update ()
	{
		if (rectTransform == null)
		{
			return;
		}

		UpdateUIPosition ();
	}

	/// <summary>
	/// このオブジェクトが配置されているカンバスを取得
	/// </summary>
	private void SetRootCanvas()
	{
		var canvasArr = GetComponentsInParent<Canvas> ();
		foreach (var canvas in canvasArr)
		{
			if (canvas.isRootCanvas)
			{
				rootCanvas = canvas;
			}
		}
	}

	private void UpdateUIPosition()
	{
		// uGUIオブジェクトが移動できる範囲をスクリーン全体として、各変数を初期化
		float rangeWidth = Screen.width;;
		float rangeHeight = Screen.height;
		Vector2 centerPosition;
		float rangeLeftEdgeX = 0;
		float rangeRightEdgeX = rangeWidth;
		float rangeBottomEdgeY = 0;
		float rangeTopEdgeY = rangeHeight;

		if (!isScreenRange)
		{
			rangeWidth = rangeObject.rect.width * rootCanvas.scaleFactor;
			rangeHeight = rangeObject.rect.height * rootCanvas.scaleFactor;
			centerPosition = rangeObject.position;
			rangeLeftEdgeX = centerPosition.x - rangeWidth / 2;
			rangeRightEdgeX = centerPosition.x + rangeWidth / 2;
			rangeBottomEdgeY = centerPosition.y - rangeHeight / 2;
			rangeTopEdgeY = centerPosition.y + rangeHeight / 2;
		}

		float componentHeight = rectTransform.rect.height * rootCanvas.scaleFactor;
		float componentWidth = rectTransform.rect.width * rootCanvas.scaleFactor;

		//Rect componentRect = rectTransform.rect;

		// オフセットを含めたローカル座標からワールド座標へ変換
		Vector3 targetPosition = Vector3.zero;
		targetPosition = followTargetObject.transform.TransformPoint (offset);
		
		//スクリーンポイントを取得
		var screenPosition = Camera.main.WorldToScreenPoint(targetPosition);

		if (screenPosition.x + componentWidth / 2 > rangeRightEdgeX) {
			screenPosition.x = rangeRightEdgeX - componentWidth / 2;
		} else if (screenPosition.x - componentWidth / 2 < rangeLeftEdgeX) {
			screenPosition.x = rangeLeftEdgeX + componentWidth / 2;
		}
		
		if (screenPosition.y + componentHeight / 2 > rangeTopEdgeY) {
			screenPosition.y = rangeTopEdgeY - componentHeight / 2;
		} else if (screenPosition.y - componentHeight / 2 < rangeBottomEdgeY) {
			screenPosition.y = rangeBottomEdgeY + componentHeight / 2;
		}

		Vector2 localPos;
		RectTransformUtility.ScreenPointToLocalPointInRectangle (parentRectTransform, screenPosition, uiCamera, out localPos);
		rectTransform.localPosition = localPos;
	}
}
