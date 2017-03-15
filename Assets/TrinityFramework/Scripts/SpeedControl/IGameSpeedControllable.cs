/* IGameSpeedControllable.cs
    ゲームスピードを管理されるクラスのインターフェース
*/
public interface IGameSpeedControllable
{
    // 速度変化通知を登録
    void AddChangeSpeedNotify();
    // ゲームスピードが変化したら、このメソッドが呼び出される
    void ChangedSpeedNotification(float gameSpeed);
}
