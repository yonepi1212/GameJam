/* ComponentUiBase.cs
  個別のUiのベースクラス(ボタンなど)
  TODO:後に速度調整出来る機能も継承する
*/
public class ComponentUiBase : UGuiMonoBehaviour
{
    #region Public Method
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        Destroy(gameObject);
    }
    #endregion
}