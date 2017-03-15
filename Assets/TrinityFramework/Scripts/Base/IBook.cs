using System;
using UnityEngine.Events;
/* IBook.cs
  コントローラーとウィンドウをユニークに管理するShelfクラスのためのインターフェース
*/
public interface IBook<T>
{
    void Show();
    void Show(Action callback);
    void Close();
    void Close(Action callback);
    event UnityAction CloseNotification;
    T Root { set; get; }
}