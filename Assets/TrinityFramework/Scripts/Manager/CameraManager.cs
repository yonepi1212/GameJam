using System.Collections;
using System.Collections.Generic;
using MKGlowSystem;
﻿using System.Collections.Generic;
using UniRx;
using UnityEngine;

/* CameraManager.cs
    カメラを生成して制御するクラス
    使える関数リスト:
    void ShowCamera(string cameraName)
    void HideCamera(string cameraName)
    Camera CreateOthographicCameraWithScript<T>(string cameraName) where T : MonoBehaviour
    Camera CreateOthographicCamera(string cameraName)
    Camera CreateCameraWithScript<T>(string cameraName) where T : MonoBehaviour
    Camera CreateCamera(string cameraName)
    void DeleteCamera(string cameraName)
    void DeleteCamera(Camera selectCamera)
    Camera GetCamera(string cameraName)
    Camera Grow(string cameraName)
    void Shake(string cameraName, float shakeTime)
*/
public class CameraManager : SingletonMonoBehaviour<CameraManager>
{
	#region Declaration

	// カメラのリスト
	[SerializeField]
	private List<Camera> _cameraList = new List<Camera> ();

	public List<Camera> CameraList {
		get { return _cameraList; }
	}

	private const string CameraShakeMaster = "cameraShakeMaster";

	#endregion

	#region Monobehavior

	private void Start ()
	{
		DontDestroyOnLoad (this);
	}

	#endregion

	#region Public Method

	// カメラを隠す
	public void ShowCamera (string cameraName)
	{
		var selectCamera = GetCamera (cameraName);
		if ( selectCamera ) {
			selectCamera.gameObject.SetActive (true);
		}
	}

	// カメラを隠す
	public void HideCamera (string cameraName)
	{
		var selectCamera = GetCamera (cameraName);
		if ( selectCamera ) {
			selectCamera.gameObject.SetActive (false);
		}
	}

	// 平行投影カメラの作成(スクリプトにアタッチもする)
	public T CreateOrthographicCameraWithScript<T> (string cameraName) where T : MonoBehaviour
	{
		var cameraScript = CreateCameraWithScript<T> (cameraName);
		var selectCamera = cameraScript.GetComponent<Camera> ();
		selectCamera.orthographic = true;
		return cameraScript;
	}

	// 平行投影カメラの作成
	public Camera CreateOrthographicCamera (string cameraName)
	{
		var addCamera = CreateCamera (cameraName);
		addCamera.orthographic = true;
		return addCamera;
	}

	// カメラの作成(スクリプトにアタッチもする)
	public T CreateCameraWithScript<T> (string cameraName) where T : MonoBehaviour
	{
		T cameraScript = null;
		var foundCamera = GetCamera (cameraName);
		if ( foundCamera ) {
			cameraScript = foundCamera.GetComponent<T> ();
		}
		else {
			foundCamera = CreateCamera (cameraName);
		}
		return cameraScript ?? (foundCamera.gameObject.AddComponent<T> ());

	}

	// カメラの作成
	public Camera CreateCamera (string cameraName)
	{
		var addCamera = GetCamera (cameraName);
		if ( addCamera == null ) {
			var g = new GameObject { name = cameraName };
			addCamera = g.AddComponent<Camera> ();
			addCamera.clearFlags = CameraClearFlags.Depth;
			AddMKGlowComponent (g);
			_cameraList.Add (addCamera);
		}
		return addCamera;
	}

	// カメラ登録削除
	public void DeleteCamera (string cameraName)
	{
		var foundCamera = GetCamera (cameraName);
		if ( foundCamera != null ) {
			DeleteCamera (foundCamera);
		}
	}

	// カメラ登録削除
	public void DeleteCamera (Camera selectCamera)
	{
		_cameraList.Remove (selectCamera);
		Destroy (selectCamera.gameObject);
	}

	// カメラの検索
	public Camera GetCamera (string cameraName)
	{
		// ヒエラルキーに存在しないが、管理されている場合
		// 管理しないようにカメラリストから削除する
		_cameraList.RemoveAll (aCamera => aCamera == null);

		var foundCamera = _cameraList.Find (currentCamera => currentCamera.name == cameraName);
		if ( foundCamera == null ) {
			// カメラマネージャーで管理されていないが、ヒエラルキーにある場合
			// 管理カメラリストに追加する
			var hierarchyCameraObject = GameObject.Find (cameraName);
			if ( hierarchyCameraObject != null ) {
				foundCamera = hierarchyCameraObject.GetComponent<Camera> ();
				_cameraList.Add (foundCamera);
			}
		}
		return foundCamera;
	}

	#endregion

	#region FOV

	public void SetFov (string cameraName, float targetFov, float duration)
	{
		var foundCamera = _cameraList.Find (currentCamera => currentCamera.name == cameraName);
		if ( foundCamera != null ) {
			var startFov = foundCamera.fieldOfView;
			var curve = Define.AnimationCurves.SmoothCurve;
			float timer = 0f;
			Observable.EveryUpdate ()
                .TakeWhile (_ => timer < duration)
                .Subscribe (_ => {
				timer += Time.deltaTime;
				var rate = curve.Evaluate (timer / duration);
				float fov = Mathf.Lerp (startFov, targetFov, rate);
				foundCamera.fieldOfView = fov;
			}, () => foundCamera.fieldOfView = targetFov)
                .AddTo (foundCamera);
		}
	}
    

	// 既存カメラに対してグローを有効にする
	public Camera Grow (string cameraName)
	{
		var addCamera = GetCamera (cameraName);
		if ( addCamera != null ) {
			var g = addCamera.gameObject;
			var glow = AddMKGlowComponent (g);
			glow.enabled = true;
		}
		return addCamera;
	}


	#endregion

	#region Private Method

	// カメラに対してMKGlowを追加する
	private MKGlow AddMKGlowComponent (GameObject cameraObject)
	{
		var mkGlow = cameraObject.GetComponent<MKGlow> ();
		if ( mkGlow == null ) {
			mkGlow = cameraObject.AddComponent<MKGlow> ();
			mkGlow.GlowLayer = LayerMask.NameToLayer ("Everything");
			mkGlow.GlowType = MKGlowType.Selective;
			mkGlow.BlurSpread = 0.05f;
			mkGlow.BlurIterations = 4;
			mkGlow.Samples = 4;
			mkGlow.GlowIntensity = 0.45f;
			mkGlow.GlowTint = Color.white;

			// OFFにしておく
			mkGlow.enabled = false;
		}
		return mkGlow;
	}

	#endregion

}
