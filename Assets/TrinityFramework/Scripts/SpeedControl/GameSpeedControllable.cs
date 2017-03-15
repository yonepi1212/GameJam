/* GameSpeedControllable.cs
    ゲームスピードを管理されるクラス
    GameSpeedが変わるとChangeSpeedNotify()が呼ばれます
*/
public class GameSpeedControllable : MonoBehaviourExtend, IGameSpeedControllable
{
    #region Declration
    protected float GameSpeed;
    #endregion

    #region Protected Method
    protected virtual void Start()
    {
        AddChangeSpeedNotify();
    }

    // 速度変化通知を登録します
    public virtual void AddChangeSpeedNotify()
    {
        Common.GameSpeed.AddObserve(this);
    }

    // ゲームスピードが変化したら、このメソッドが呼び出されます
    public virtual void ChangedSpeedNotification(float gameSpeed)
    {
        GameSpeed = gameSpeed;
    }
    #endregion
}
