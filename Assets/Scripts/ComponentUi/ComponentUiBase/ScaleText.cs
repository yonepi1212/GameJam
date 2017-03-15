using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

/* TextUiBase.cs
  テキストの数値増加するときのアニメーションクラス
*/

public class ScaleText : ComponentUiBase {

    #region Declaration

    private bool _isInitialized;
    public Text Text { get; set; }
    private float _lastVaule;
    private string _stringFormat;
    private AnimationCurve _countUpCurve;
    private AnimationCurve _countDownCurve;
    private enum TextKind
    {
        DefaultType,
        PointType,
        ComboType,
        CurrentHpType,
        DefaultHpType,
        StatusDefaultType,
        TurnInfoType,
    };

    [SerializeField] private TextKind _textKind;

    private const string DefaultFormatString = "{0:0}";
    private const string PointFormatString = "{0:N0}pt";
    private const string ComboFormatString = "{0:000}";
    private const string CurrentHpFormatString = "{0:0}";
    private const string DefaultHpFormatString = "/{0:0}";
    private const string StatusDefaultFormatString = "±{0:00.00}";
    private const string StatusIncreaseFormatString = "+{0:00.00}";
    private const string StatusDecreaseFormatString = "{0:00.00}";
    private const string TurnInfoFormatString = "{0:0}/10";

    private const float DefaultScale = 1f;

    private Color _defaultColor;
    private readonly Color _increaseColor = new Color(48 / 255f, 149 / 255f, 220 / 255f);
    private readonly Color _decreaseColor = new Color(212f / 255f, 21f / 255f, 91f / 255f);

    private List<IDisposable> _animationList = new List<IDisposable>();
    #endregion

    #region Private Method
    
    private void Initialization()
    {
        if (!_isInitialized)
        {
            _countUpCurve = Define.AnimationCurves.CountUpCurve;
            _countDownCurve = Define.AnimationCurves.CountDownCurve;
            Text = GetComponent<Text>();
            _defaultColor = Text.color;
            SetTextFormat(_textKind);
            _isInitialized = true;
        }
    }

    private void SetTextFormat(TextKind textKind)
    {
        switch (textKind)
        {
            case TextKind.DefaultType:
                _stringFormat = DefaultFormatString;
                break;

            case TextKind.PointType:
                _stringFormat = PointFormatString;
                break;

            case TextKind.ComboType:
                _stringFormat = ComboFormatString;
                break;

            case TextKind.CurrentHpType:
                _stringFormat = CurrentHpFormatString;
                break;

            case TextKind.DefaultHpType:
                _stringFormat = DefaultHpFormatString;
                break;

            case TextKind.StatusDefaultType:
                _stringFormat = StatusDefaultFormatString;
                break;

            case TextKind.TurnInfoType:
                _stringFormat = TurnInfoFormatString;
                break;
        }
    }

    private void ScaleTextAnimation(AnimationCurve curve, float startValue, float endValue)
    {
        float curveRate = 0;
        int lastCurveKey = curve.length - 1;
        float timer = curve.keys[lastCurveKey].time;

        // UniRxによるアニメーションが残っている場合はDisposeする
        _animationList.ForEach((disposable)=>disposable.Dispose());
        _animationList.Clear();

        var scaleAnimation = Observable.EveryUpdate()
            .TakeWhile(_ => timer > curveRate)
            .Subscribe(_ =>
            {
                //スケール変化
                curveRate = curveRate + Time.deltaTime;
                float scale = curve.Evaluate(curveRate);
                Vector2 textLocalScale = new Vector2(scale, scale);
                RectTransform.localScale = textLocalScale;
                //テキスト変化
                float tempValue  = Mathf.Lerp(startValue, endValue, curveRate/ (timer * 0.8f));
                SetText(_stringFormat, tempValue);
            }, () =>
            {
                Text.color = _defaultColor;
                RectTransform.localScale = new Vector2(DefaultScale, DefaultScale);
                SetText(_stringFormat, endValue);
            }).AddTo(this);

        _animationList.Add(scaleAnimation);

    }

    private void PlayTextScaleAnimation(float startValue, float endValue)
    {
        if (endValue > startValue)
        {
            SetTextColor(_increaseColor);
            ScaleTextAnimation(_countUpCurve, startValue, endValue);
        }
        else if (endValue < startValue)
        {
            SetTextColor(_decreaseColor);
            ScaleTextAnimation(_countDownCurve, startValue, endValue);
        }
        _lastVaule = endValue;
    }

    private void SetTextColor(Color color)
    {
        if (_textKind != TextKind.PointType && _textKind != TextKind.ComboType)
        {
            Text.color = color;
        }
        else if (_textKind == TextKind.PointType)
        {
            Text.color = Color.white;
        }
    }

    private void CheckIsStatusFormat(float endValue)
    {
        if (_textKind == TextKind.StatusDefaultType)
        {
            _stringFormat = StatusDefaultFormatString;
            if (endValue > 0)
            {
                _stringFormat = StatusIncreaseFormatString;
            }
            else if (endValue < 0)
            {
                _stringFormat = StatusDecreaseFormatString;
            }
        }
    }

    private void PlayTextAnimation(float startValue, float endValue)
    {
        CheckIsStatusFormat(endValue);
        PlayTextScaleAnimation(startValue, endValue);
    }

    private void SetText(string stringFormat, float value)
    {
        Text.text = string.Format(stringFormat, value);
    }


    #endregion

    #region Public Method

    public override void Show()
    {
        base.Show();
        Initialization();
    }

    public void SetText(float endValue)
    {
        Initialization();
        CheckIsStatusFormat(endValue);
        SetText(_stringFormat, endValue);
    }

    public void ResetText()
    {
        Initialization();
        if (_stringFormat == StatusIncreaseFormatString || _stringFormat == StatusDecreaseFormatString)
        {
            _stringFormat = StatusDefaultFormatString;
        }
        SetText(_stringFormat, 0);
    }

    public void PlayTextAnimation(float endValue)
    {

        Initialization();
        PlayTextAnimation(_lastVaule, endValue);
    }
}
#endregion
