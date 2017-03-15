using System.Collections.Generic;
using UniRx;
using UnityEngine;

/* InGameController.cs
*/
public class InGameController : ControllerBase
{
	#region Declaration

	public ReactiveProperty<bool> IsFocusMode = new ReactiveProperty<bool> ();

	#endregion

	#region Public Method

	public override void Show ()
	{
		base.Show ();
	}

	public override void Close ()
	{
		base.Close ();

	}



	#endregion

	#region Private Method



	#endregion
}
