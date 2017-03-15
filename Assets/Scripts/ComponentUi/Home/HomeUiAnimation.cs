using UnityEngine;
/* HomeUiAnimation.cs
	ホームUiアニメーションクラス
*/
public class HomeUiAnimation : UiAnimationBase
{
    #region Declaration

    private const string HomeUiStagingAnimationName = "HomeUiAnim@staging";

    #endregion

    #region Public Method

    public void PlayAnimation(bool isEyeMode)
    {
        PlayAnimation(HomeUiStagingAnimationName, isEyeMode);
    }

    #endregion

    #region Private Method

    #endregion
}
