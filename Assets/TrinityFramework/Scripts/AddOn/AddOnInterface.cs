/*  AddOnInterface
    AddOnのインターフェイス
*/
interface AddOnInterface
{
    // シーンが加算されたかの判定
    bool IsAdded { set; }
    // クローズが完了したときに呼ばれるイベント
    event System.Action UnloadNotification;
}