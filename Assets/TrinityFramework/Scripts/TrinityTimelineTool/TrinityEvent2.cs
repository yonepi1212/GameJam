// UnityEventとScriptableObjectの親和性が悪すぎるので
// 一旦保存形式をMonoBehaviorにする
//#define USE_MONOBEHAVIOUR_INSTEAD_OF_SCRIPTBLEOBJECT

using System;
using UnityEngine;
using UnityEngine.Events;
/* TrinityEvent
    タイムラインで発火するイベント
*/
[Serializable]
public class TrinityEvent2
{
#region Declaration
    public UnityEvent FireEvent = new UnityEvent();
    [HideInInspector]
    public float FireTime = 0f;
    [HideInInspector]
    public bool IsFired;
    // エディタ拡張用にイベントごとに色を付ける
    public Color Color;
#endregion
}
