using UnityEngine.Events;

/* LazyLoadWindow.cs
	読み込み負荷分散用クラス
*/
public class LazyLoadWindow : WindowBase
{
    #region Declaration
    private LazyLoad _lazyLoad = new LazyLoad();
    #endregion

    #region Public Method
    public override void Show()
    {
        _lazyLoad.Initialization(gameObject);
        base.Show();
    }

    public override void Close()
    {
        _lazyLoad = null;
        base.Close();
    }

    public void AddTask(UnityAction task)
    {
        _lazyLoad.AddTask(task);
    }
    #endregion
}
