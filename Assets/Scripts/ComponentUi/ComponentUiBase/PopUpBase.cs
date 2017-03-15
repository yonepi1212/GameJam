using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Events;

/* PopUpBase.cs
	ポップアップベースクラス
    ・ポップアップは、押下したボタンの中心付近から出現します。
    　約50%縮小->100%の大きさに拡縮します。
*/
public class PopUpBase : ComponentUiBase
{
    #region Declaration

    private const float PopUpTime = 0.1f;
    private const float ReductionScale = 0.5f;
    private const float DefaultScale = 1.0f;

    #endregion

    #region Public Method

    public override void Show()
    {
        base.Show();
        gameObject.SetActive(false);
        RectTransform.localScale = new Vector2(ReductionScale, ReductionScale);
       
    }


    public void PlayPopUp(bool isOpen, Action call = null)
    {
        if (isOpen)
        {
            PlayPopUpShow();
        }
        else
        {
            PlayPopUpClose();
        }

        Observable.Timer(TimeSpan.FromSeconds(PopUpTime))
            .Subscribe(_ =>
            {
                if (call != null)
                {
                    call.Invoke();
                }
            });
    }


    #endregion

    #region Private Method

    #region ShowPopUpAnimation

    private void PlayPopUpShow()
    {
        gameObject.SetActive(true);
        //1 拡大 0.5 -> 1.0
        PlayAnimation(ReductionScale, DefaultScale, PopUpTime);
    }

    #endregion

    #region ClosePopUpAnimation


    private void PlayPopUpClose()
    {
        //1 縮小 1.0 -> 0.5
        PlayAnimation(DefaultScale, ReductionScale, PopUpTime, CloseBase);
    }

    private void CloseBase()
    {
        gameObject.SetActive(false);
    }

    #endregion

    private void PlayAnimation(float beginScale, float finalScale, float scaleTime, Action call = null)
    {

        float timer = 0f;
        Observable.EveryUpdate()
            .TakeWhile(_ => timer < scaleTime)
            .Subscribe(_ =>
            {
                timer += Time.deltaTime;
                float scale = Mathf.Lerp(beginScale, finalScale, timer / scaleTime);
                RectTransform.localScale = new Vector2(scale, scale);
            }, () =>
            {
                RectTransform.localScale = new Vector2(finalScale, finalScale);
                if (call != null)
                {
                    call.Invoke();
                }
            }).AddTo(this);
    }
    #endregion
}
