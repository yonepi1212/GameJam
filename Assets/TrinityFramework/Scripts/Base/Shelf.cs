using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/* Shelf.cs
  Controller,Windowなどをユニークに保存するクラス
  Tは入れるオブジェクト、TRootは入れ物を保存しておきます
*/
public class Shelf<T,TRoot> where T : MonoBehaviour, IBook<TRoot>
{
    #region Declaration
    public Dictionary<string, T> Hanger = new Dictionary<string, T>();

    // bookを置く親オブジェクト
    private Transform _rootTransform;
    public Transform RootTransform
    {
        set
        {
            _rootTransform = value;
            foreach (var book in Hanger)
            {
                book.Value.transform.SetParent(_rootTransform);
            }
        }
        get
        {
            return _rootTransform;
        }
    }

    public TRoot Root ;
    #endregion

    #region Public Method
    // ユニークにbookを配置する
    public T SetBook(T instance)
    {
        T book;
        if (Hanger.ContainsKey(instance.name))
        {
            book = Hanger[instance.name];
        }
        else
        {
            book = CreateBook(instance);
            book.Root = Root;
            Hanger.Add(instance.name, book);
        }
        return book;
    }

    public void CloseBook(string key)
    {
        Hanger[key].Close();
        Hanger.Remove(key);
    }

    // 全てのウィンドウをクローズする
    public void CloseAllBook(List<string> ignoreNameList = null)
    {
        List<string> removeKeys = Hanger.Keys.ToList();

        if (ignoreNameList != null && ignoreNameList.Count > 0)
        {
            foreach (var itemName in ignoreNameList)
            {
                if (removeKeys.Contains(itemName))
                {
                    removeKeys.Remove(itemName);
                }
            }
        }
        foreach (var key in removeKeys)
        {
            CloseBook(key);
        }
    }
    #endregion

    #region Private Method
    private T CreateBook(T instance)
    {
        GameObject g = Object.Instantiate(instance.gameObject);
        g.name = instance.gameObject.name;

        Transform t = g.transform;
        if (RootTransform != null)
        {
            t.SetParent(RootTransform, false);
        }

        T book = g.GetComponent<T>();
        book.CloseNotification += () => Hanger.Remove(instance.name);

        return book;
    }
    #endregion
}
