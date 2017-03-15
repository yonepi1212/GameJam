using System;
using UniRx;
using UnityEngine;
/* QuestSelectChapterAnimation.cs
	クエストセレクト：チャプターポップアップアニメーション
*/
public class QuestSelectChapterAnimation : UiAnimationBase
{
    #region Declaration

    private const string AnimationName = "QuestSelectAnim@episodeStage";

    [SerializeField]
    private PopUpBase _questCharpterPopUp;

    #endregion

    #region Public Method

    public override void Show()
    {
        base.Show();
        _questCharpterPopUp.Show();
    }

    public void PlayPopUpAnimation(bool isOpen, Action call = null)
    {
        _questCharpterPopUp.PlayPopUp(isOpen, call);
    }

    public void PlaySelectChapterAnimation(bool isChapterSelected)
    {
        PlayAnimation(AnimationName, isChapterSelected);
    }

    #endregion

    #region Private Method

    #endregion
}
