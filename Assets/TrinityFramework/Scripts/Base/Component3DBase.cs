using UnityEngine;
/* Component3DBase.cs
  3Dのオブジェクトのベースクラス
  TODO:後に速度調整出来る機能も継承する
*/
public class Component3DBase : GameSpeedControllable
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
