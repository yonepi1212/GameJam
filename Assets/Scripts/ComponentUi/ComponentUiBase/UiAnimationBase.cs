using UnityEngine;
/* UiAnimationBase.cs
	クエストバトルUIアニメーションベースクラス

*/
public class UiAnimationBase : ComponentUiBase
{
    #region Declaration
    public Animation UiAnimation;
    private string _animationName;
    private bool _showing;
    private bool _closing;
    #endregion

    #region Public Method

    //TODO: α3関数名見直す
    public void PlayForceAnimation(string animationName, bool isPlayingForward)
    {
        _animationName = animationName;
        UiAnimation.clip = UiAnimation.GetClip(_animationName);
        if (!isPlayingForward)
        {
            SetCloseAnimation();
        }
        else
        {
            SetShowAnimaiton();
        }
        UiAnimation.Play();
    }

    public void PlayAnimation(string animationName, bool isPlayingForward)
    {
        if (CanPlayAnimation(isPlayingForward))
        {
            _animationName = animationName;
            UiAnimation.clip = UiAnimation.GetClip(_animationName);
            if (!isPlayingForward)
            {
                SetCloseAnimation();
            }
            else
            {
                SetShowAnimaiton();
            }
            UiAnimation.Play();
        }
    }

    public void StopAnimation(string animationName)
    {
        _animationName = animationName;
        UiAnimation.clip = UiAnimation.GetClip(_animationName);
        UiAnimation.Stop();
    }
    #endregion

    #region Private Method
    private bool CanPlayAnimation(bool isPlayingForward)
    {
        bool canPlay;
        // 表示アニメーション命令
        if (isPlayingForward)
        {
            // show中でなければ再生できる
            canPlay = !_showing;
        }
        // 非表示アニメーション命令
        else
        {
            // close中でな無ければ再生できる
            canPlay = !_closing;
        }
        return canPlay;
    }

    private void SetShowAnimaiton()
    {
        _showing = true;
        _closing = false;
        UiAnimation[_animationName].time = 0;
        UiAnimation[_animationName].speed = 1;
    }

    private void SetCloseAnimation()
    {
        _showing = false;
        _closing = true;
        UiAnimation[_animationName].time = UiAnimation[_animationName].length;
        UiAnimation[_animationName].speed = -1;
    }

    #endregion
}
