using System;
using UniRx;
using UnityEngine;
/* LoadingWindow.cs
	ローディングウィンドクラス
    ローディング表示
*/
public class LoadingWindow : WindowBase
{
    #region Declaration

    public UiAnimationBase LoadingAnimation;
    private const string LoadingAnimationName = "LoadingAnim";

    [SerializeField]
    private float _waitTime = 2f;

    private IDisposable _loadingDisposable;
    #endregion

    #region Public Method

    public override void Show()
    {
        base.Show();
        LoadingAnimation.gameObject.SetActive(false);
        _loadingDisposable = Observable.Timer(TimeSpan.FromSeconds(_waitTime))
            .Subscribe(_ =>
            {
                LoadingAnimation.gameObject.SetActive(true);
                LoadingAnimation.PlayAnimation(LoadingAnimationName, true);
            });
    }

    public override void Close()
    {
        base.Close();
        _loadingDisposable.Dispose();
    }
    #endregion

    #region Private Method
    #endregion
}
