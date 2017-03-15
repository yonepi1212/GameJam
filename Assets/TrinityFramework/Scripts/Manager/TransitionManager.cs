using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/* TransitionManager.cs 
   マスク画像を使ったトランジション制御用クラスです。
   TODO:ScreenSpaceをOverlayではなくてCamera制御にする
*/
public class TransitionManager : UnityEngine.UI.Graphic
{
    #region Declaration

    private Texture _maskTexture;

    [Serializable]
    public class MaskTexture
    {
        public Texture BattleMaskTexture;
        public Texture FocusMaskTexture;
    }

    [SerializeField]
    private MaskTexture _maskTextures;

    // 暗転か明転か
    private bool _isInvert;
    // Cutout量
    private ReactiveProperty<float> _cutoutRange = new ReactiveProperty<float>(0);
    private ReactiveProperty<float> _alpha = new ReactiveProperty<float>(0);

    public enum Kind
    {
        White,
        Black,
        Battle,
        Focus
    }

    #endregion

    #region Monobehavior

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        // カットアウト量を監視する
        _cutoutRange.Subscribe(UpdateMaskCutout);

        // α量を監視する
        _alpha.Subscribe(UpdateAlpha);
    }

    #endregion

    #region Public Methods
    public void TransitionOut(Kind kind, float time, Action callback = null)
    {
        switch (kind)
        {
            case Kind.Black:
                FadeOut(false, time, callback);
                break;
            case Kind.White:
                FadeOut(true, time, callback);
                break;
            case Kind.Battle:
                _maskTexture = _maskTextures.BattleMaskTexture;
                CutOut(time, callback);
                break;
            case Kind.Focus:
                _maskTexture = _maskTextures.FocusMaskTexture;
                CutOut(time, callback);
                break;
        }
    }

    public void TransitionIn(Kind kind, float time, Action callback = null)
    {
        switch (kind)
        {
            case Kind.Black:
                FadeIn(false, time, callback);
                break;
            case Kind.White:
                FadeIn(true, time, callback);
                break;
            case Kind.Battle:
                _maskTexture = _maskTextures.BattleMaskTexture;
                CutIn(time, callback);
                break;
            case Kind.Focus:
                _maskTexture = _maskTextures.FocusMaskTexture;
                CutIn(time, callback);
                break;
        }
    }

    private void FadeIn(bool isWhiteOut, float time, Action action = null)
    {
        // トランジション初期化
        _alpha.Value = 0f;
        gameObject.SetActive(true);
        _isInvert = isWhiteOut;

        // トランジション終了時に呼ぶ処理
        Action complete = () =>
        {
            gameObject.SetActive(false);
            action.Call();
        };

        // α量を徐々に増やす
        Observable.EveryUpdate()
            .TakeWhile(_ => _alpha.Value <= 1f)
            .Subscribe((_) => _alpha.Value += Time.deltaTime / time, complete)
            .AddTo(this);
    }

    private void FadeOut(bool isWhiteOut, float time, Action action = null)
    {
        // トランジション初期化
        _alpha.Value = 1f;
        gameObject.SetActive(true);
        _isInvert = isWhiteOut;

        // トランジション終了時に呼ぶ処理
        Action complete = () =>
        {
            gameObject.SetActive(false);
            action.Call();
        };

        // α量を徐々に増やす
        Observable.EveryUpdate()
            .TakeWhile(_ => _alpha.Value >= 0f)
            .Subscribe((_) => _alpha.Value -= Time.deltaTime / time, complete)
            .AddTo(this);
    }

    private void CutIn(float time, Action action = null)
    {
        // トランジション初期化
        gameObject.SetActive(true);
        _isInvert = false;
        UpdateMaskCutout(0);
        _cutoutRange.Value = 0f;

        // トランジション終了時に呼ぶ処理
        Action complete = () =>
        {
            _cutoutRange.Value = 1f;
            gameObject.SetActive(false);
            action.Call();
        };

        // カットアウト量を徐々に増やす
        Observable.EveryUpdate()
            .TakeWhile(_ => _cutoutRange.Value <= 1f)
            .Subscribe((_) => _cutoutRange.Value += Time.deltaTime / time, complete)
            .AddTo(this);        
    }

    private void CutOut(float time, Action action = null)
    {
        // トランジション初期化
        gameObject.SetActive(true);
        _isInvert = true;
        UpdateMaskCutout(0);
        _cutoutRange.Value = 0f;


        // トランジション終了時に呼ぶ処理
        Action complete = () =>
        {
            _cutoutRange.Value = 0;
            gameObject.SetActive(false);
            action.Call();
        };

        // カットアウト量を徐々に減らす
         Observable.EveryUpdate()
            .TakeWhile(_ => _cutoutRange.Value <= 1f)
            .Subscribe((_) => _cutoutRange.Value += Time.deltaTime / time, complete)
            .AddTo(this);        
    }

    private void UpdateMaskCutout(float range)
    {
        UpdateMaskTexture(_maskTexture);

        material.SetFloat("_Range", 1 - range);
        material.SetFloat("_IsInvert", Convert.ToSingle(_isInvert));
    }

    private void UpdateMaskTexture(Texture texture)
    {
        material.SetTexture("_MaskTex", texture);
        material.SetColor("_Color", color);
    }

    private void UpdateAlpha(float alpha)
    {
        material.SetFloat("_IsInvert", Convert.ToSingle(_isInvert));
        material.SetFloat("_Range", 1);
        var black = _isInvert ? 1f : 0f;
        material.SetColor("_Color", new Color(black, black, black, alpha));
    }
    #endregion

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        UpdateMaskTexture(_maskTexture);
    }
#endif
}
