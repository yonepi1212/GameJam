using UniRx;
using UnityEngine;
using System;

/* InputManager.cs

*/
public class InputManager : SingletonMonoBehaviour<InputManager>
{
	#region Declration

	// ジャンプキー
	public ReactiveProperty<bool> IsJumpKey = new BoolReactiveProperty (true);

	public ReactiveProperty<bool> IsLeftKey = new BoolReactiveProperty (true);

	public ReactiveProperty<bool> IsRightKey = new BoolReactiveProperty (true);


	#endregion

	#region Monobehavior

	protected virtual void Start ()
	{
		DontDestroyOnLoad (gameObject);
	}

	#endregion

	#region Public Method

  
	#endregion
}
