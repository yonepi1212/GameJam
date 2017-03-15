using UnityEngine;
using UniRx;

/* GaugeBase.cs
  数値を変化するともに、ゲージをアニメーションベースクラス（スケール変化）
*/

public class GaugeBase : ComponentUiBase {

    #region Declaration

    //ゲージのスケール変化がかかる時間
    [SerializeField]
    private float _gaugeTime = 0.2f;

    #endregion

    #region Public Method
    //スケール変化（アニメーションしない）
    public void SetGaugeScale(float newValue, float totalValue)
    {
        float gaugeScale = ComputeScaleRatio(newValue, totalValue);
        RectTransform.localScale = new Vector2(gaugeScale, RectTransform.localScale.y);
    }

    //スケール変化（アニメーションする）
    public virtual void PlayGaugeAnimation(float newValue, float totalValue)
    {
        Vector2 startScale = RectTransform.localScale;
        float newScaleValue = ComputeScaleRatio(newValue, totalValue);
        Vector2 endScale = new Vector2(newScaleValue, RectTransform.localScale.y);
        float erapsedTime = 0;
        Observable.EveryUpdate()
            .TakeWhile(_ => erapsedTime < _gaugeTime)
            .Subscribe(_ =>
            {
                RectTransform.localScale = Vector2.Lerp(startScale, endScale, erapsedTime / _gaugeTime);
                erapsedTime += Time.deltaTime;
            }, () =>
            {
                SetGaugeScale(newValue, totalValue);
            }).AddTo(this);
    }
    #endregion

    #region Private Method

    private float ComputeScaleRatio(float newValue, float totalValue)
    {
        float gaugeScale = 1f;
        if (totalValue > 0 || totalValue < 0)
        {
            gaugeScale = Mathf.Abs(newValue) / Mathf.Abs(totalValue);
        }
        return gaugeScale;
    }
    #endregion

}
