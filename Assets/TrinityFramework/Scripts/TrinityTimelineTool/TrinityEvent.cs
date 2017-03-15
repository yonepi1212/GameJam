// UnityEventとScriptableObjectの親和性が悪すぎるので
// 一旦保存形式をMonoBehaviorにする
//#define USE_MONOBEHAVIOUR_INSTEAD_OF_SCRIPTBLEOBJECT
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
/* TrinityEvent
    タイムラインで発火するイベント
*/
#if USE_MONOBEHAVIOUR_INSTEAD_OF_SCRIPTBLEOBJECT
public class TrinityEvent : MonoBehaviour
#else
public class TrinityEvent : ScriptableObject
#endif
{
#region Declaration
    public UnityEvent FireEvent = new UnityEvent();
    public float FireTime = 0f;
    public bool IsFired;
    // エディタ拡張用にイベントごとに色を付ける
    public Color Color;
#endregion
}
