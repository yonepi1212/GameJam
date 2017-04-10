using UniRx;
using UnityEngine;
using System;

/* InputManager.cs

*/
public class StageManager : SingletonMonoBehaviour<StageManager>
{
	#region Declration

	public ReactiveProperty<bool> IsGameOver = new ReactiveProperty<bool> (false);


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
