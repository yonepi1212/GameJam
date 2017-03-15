using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/* ButtonBase.cs
  ボタンのクベースクラス
  拡大縮小基本機能
*/
public class ButtonBase : ComponentUiBase, IPointerExitHandler, IPointerEnterHandler, IPointerUpHandler, IPointerDownHandler, IPointerClickHandler
{

    #region Declaration

    public bool IsActive = true;
    private bool _isScaling;
    private bool _isPointerDown;
    private bool _isPointerHover;

    [SerializeField]
    private bool _withButtonAnimation = true;
    private AnimationCurve _scaleAfterClickCurveX;
    private AnimationCurve _scaleAfterClickCurveY;

    [SerializeField]
    private float _defauleScaleX = 1.0f;
    [SerializeField]
    private float _defauleScaleY = 1.0f;
    public float _upScaleX = 1.05f;
    public float _upScaleY = 1.05f;
    private const float UpScaleTime = 0.2f;
    public float _downScaleX = 0.95f;
    public float _downScaleY = 0.95f;

    public UnityEvent OnClick;
    #endregion

    #region Public Method
    public override void Show()
    {
        base.Show();
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (IsActive)
        {
            _isPointerHover = false;
            if (!_isPointerDown)
            {
                RectTransform.localScale = new Vector2(_defauleScaleX, _defauleScaleY);
            }
        }
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (IsActive)
        {
            _isPointerHover = true;
            if (!_isPointerDown)
            {
                float timer = 0f;
                Observable.EveryUpdate()
                    .TakeWhile(_ => timer < UpScaleTime && !_isPointerDown && _isPointerHover)
                    .Subscribe(_ =>
                    {
                        float newScaleX = Mathf.Lerp(gameObject.transform.localScale.x, _upScaleX, UpScaleTime);
                        float newScaleY = Mathf.Lerp(gameObject.transform.localScale.y, _upScaleY, UpScaleTime);
                        RectTransform.localScale = new Vector2(newScaleX, newScaleY);
                        timer += Time.deltaTime;
                    }).AddTo(this);
            }
        }

    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        if (IsActive)
        {
            _isPointerDown = false;
            if (_isPointerHover)
            {
                if (_withButtonAnimation)
                {
                    FireEventAfterScale(eventData);
                }
                else
                {
                    RectTransform.localScale = new Vector2(_upScaleX, _upScaleY);
                }
                
            }
            else
            {
                RectTransform.localScale = new Vector3(_defauleScaleX, _defauleScaleY);
            }
        }
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (IsActive)
        {
            if (!_isScaling)
            {
                _isPointerDown = true;
                RectTransform.localScale = new Vector3(_downScaleX, _downScaleY);
            }
        }
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (IsActive)
        {
            if (_isPointerHover)
            {
                OnClick.Invoke();
            }
        }
    }


    private void FireEventAfterScale(PointerEventData eventData)
    {
        //アニメーションある場合は連打を禁止する
        _isScaling = true;
        float curveRate = 0;
        _scaleAfterClickCurveX = ButtonCurveX();
        _scaleAfterClickCurveY = ButtonCurveY();
        int lastCurveKey = _scaleAfterClickCurveX.length - 1;
        float timer = _scaleAfterClickCurveX.keys[lastCurveKey].time;

        Observable.EveryUpdate()
            .Where(_ => _withButtonAnimation)
            .TakeWhile(_ => timer > curveRate)
            .Subscribe(_ =>
            {
                //スケール変化
                curveRate = curveRate + Time.deltaTime;
                float scaleX = _scaleAfterClickCurveX.Evaluate(curveRate);
                float scaleY = _scaleAfterClickCurveY.Evaluate(curveRate);
                Vector2 newScale = new Vector2(scaleX, scaleY);
                RectTransform.localScale = newScale;
            }, () =>
            {
                if (_isPointerHover)
                {
                    OnPointerEnter(eventData);
                }
                else
                {
                    OnPointerExit(eventData);
                }
                _isScaling = false;
            }).AddTo(this);
    }

    public IObservable<Unit> OnClickAsObservable()
    {
        return OnClick.AsObservable();
    }

    private AnimationCurve ButtonCurveX()
    {
        AnimationCurve curve = new AnimationCurve();
        float firstScaleTime = 0.15f;
        float secondScaleTime = 0.15f;
        curve.AddKey(new Keyframe(0f, _downScaleX));
        curve.AddKey(new Keyframe(firstScaleTime, _upScaleX));
        curve.AddKey(new Keyframe(firstScaleTime + secondScaleTime, _defauleScaleX));
        return curve;
    }

    private AnimationCurve ButtonCurveY()
    {
        AnimationCurve curve = new AnimationCurve();
        float firstScaleTime = 0.15f;
        float secondScaleTime = 0.15f;
        curve.AddKey(new Keyframe(0f, _downScaleY));
        curve.AddKey(new Keyframe(firstScaleTime, _upScaleY));
        curve.AddKey(new Keyframe(firstScaleTime + secondScaleTime, _defauleScaleY));
        return curve;
    }
}

    #endregion