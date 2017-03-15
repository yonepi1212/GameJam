using UnityEngine;
/* AnimationSpeedControllable.cs
 アニメーション（Legacy）のポーズ処理
*/
public class AnimationSpeedControllable : GameSpeedControllable
{
    #region Declration
    private Animation _animation;
    #endregion

    #region Private Method
    private Animation Animation
    {
        get { return _animation ?? (_animation = GetComponent<Animation>()); }
    }
    #endregion

    #region Public Method
    public override void ChangedSpeedNotification(float gameSpeed)
    {
        base.ChangedSpeedNotification(gameSpeed);

        if (Animation != null && Animation.clip != null)
        {
            Animation[Animation.clip.name].speed = gameSpeed;
        }
    }
    #endregion
}