using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

/* LazyLoad.cs
    LazyLoadの基本機能
    ControllerやScreenにもたせて使う
*/
public class LazyLoad
{
    #region Declaration
    private Queue<UnityAction> _tasks = new Queue<UnityAction>();
    public bool IsLazy = true;
    #endregion

    #region Public Method
    public void Initialization(GameObject g)
    {
        Observable.EveryUpdate()
                  .Where((_) => _tasks.Count > 0)
                  .Subscribe((_) =>
                  {
                      var task = _tasks.Dequeue();
                      task.Call();
                  })
                  .AddTo(g);
    }

    public void AddTask(UnityAction task)
    {
        if (IsLazy)
        {
            _tasks.Enqueue(task);
        }
        else
        {
            task.Call();
        }
    }
    #endregion
}
