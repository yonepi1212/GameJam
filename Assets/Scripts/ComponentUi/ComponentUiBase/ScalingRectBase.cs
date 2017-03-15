using UniRx;
using UniRx.Triggers;
using UnityEngine;
/* ScalingRectBase.cs
    スケールアニメーションを行うRectTransformのベースクラス
*/
[RequireComponent(typeof(RectTransform))]
public class ScalingRectBase : ComponentUiBase
{
    #region Declaration
    [SerializeField]
    private AnimationCurve _scalingCurve;
    #endregion

    #region MonoBehaviour
    #endregion

    #region Public Method
    public override void Show()
    {
        base.Show();
    }
    #endregion

    #region Private Method

    public void PlayScaleAnimation()
    {
        PlayScaleAnimation(_scalingCurve);
    }

    public void PlayScaleAnimation(AnimationCurve curve)
    {
        float curveRate = 0;
        int lastCurveKey = curve.length - 1;
        float timer = curve.keys[lastCurveKey].time;

        this.UpdateAsObservable()
            .TakeWhile(_ => timer > curveRate)
            .Subscribe(_ =>
            {
                //スケール変化
                curveRate = curveRate + Time.deltaTime;
                float scale = curve.Evaluate(curveRate);
                Vector2 localScale = new Vector2(scale, scale);
                RectTransform.localScale = localScale;
            }, () =>
            {
            });
    }
    #endregion
}
