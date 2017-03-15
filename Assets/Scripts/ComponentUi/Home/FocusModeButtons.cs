using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

/* FocusModeButtons.cs
	FocusModeボタン：アイコンと戻る
*/
public class FocusModeButtons : UiAnimationBase
{
    #region Declaration

    public ReactiveProperty<bool> IsEyeMode = new BoolReactiveProperty();
    public ReactiveProperty<bool> IsFocusButtonShow = new BoolReactiveProperty();
    private const string FocusModeAnimationName = "FocusModeButtonAnim";


    [SerializeField]
    private ButtonBase _eyeButton;
    [SerializeField]
    private ButtonBase _returnButton;

    #endregion

    #region Public Method

    public override void Show()
    {
        _eyeButton.Show();
        _returnButton.Show();

        IsEyeMode.Value = false;

        _returnButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                PlayFocusModeInOutAnimation(false);
                IsFocusButtonShow.Value = false;
            });

        _eyeButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                IsEyeMode.Value = !IsEyeMode.Value;
            });
    }

    public void EnterFocusMode()
    {
        PlayFocusModeInOutAnimation(true);
        IsFocusButtonShow.Value = true;
    }
    #endregion

    #region Private Method

    private void PlayFocusModeInOutAnimation(bool isFocusMode)
    {
        Observable.Timer(TimeSpan.FromSeconds(Define.Window.FocusTransitionTime))
            .Subscribe(time =>
            {
                 PlayAnimation(FocusModeAnimationName, isFocusMode);
            }).AddTo(this);

    }

    #endregion
}
