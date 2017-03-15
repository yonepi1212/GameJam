using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;

/*  AddOnManager

*/
public class AddOnManager : SingletonMonoBehaviour<AddOnManager>
{
    // 重複禁止リスト
    [SerializeField]
    private List<string> _duplicateBan = new List<string>();
    // 重複禁止リスト
    public List<string> DuplicateBan
    {
        get { return _duplicateBan; }
    }

    #region MonoBehaviour
    // OnDestroy
    protected override void OnDestroy()
    {
        _duplicateBan = null;
        base.OnDestroy();
    }
    // Start
    protected virtual void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    #region Add
    // シーンの追加
    public void Add<T>() where T : AddOnBase
    {
        string name = typeof(T).Name;
        T result = null;

        Action sequence = () =>
        {
            GameObject g = GameObject.Find(name);

            if (g != null)
            {
                result = g.GetComponent<T>();

                if (result != null)
                {
                    result.IsAdded = true;
                }
            }
        };

        if (!_duplicateBan.Contains(name))
        {
            _duplicateBan.Add(name);
            Add(name, sequence);
        }
    }
    // シーンの追加
    public void Add<T>(Action<T> complete_) where T : AddOnBase
    {
        string name = typeof(T).Name;
        T result = null;

        Action<T> single = (res) =>
        {
            if (complete_ != null) complete_(res);
        };

        Action sequence = () =>
        {
            GameObject g = GameObject.Find(name);

            if (g != null)
            {
                result = g.GetComponent<T>();

                if (result != null)
                {
                    result.IsAdded = true;
                    single(result);
                }
            }
        };

        if (!_duplicateBan.Contains(name))
        {
            _duplicateBan.Add(name);
            Add(name, sequence);
        }
    }
    // シーンの追加
    public void Add<T>(params Action<T>[] complete_) where T : AddOnBase
    {
        string name = typeof(T).Name;
        T result = null;

        Action<T> multiple = (res) =>
        {
            if (complete_ != null)
            {
                foreach (Action<T> item in complete_) item(res);
            }
        };

        Action sequence = () =>
        {
            GameObject g = GameObject.Find(name);

            if (g != null)
            {
                result = g.GetComponent<T>();

                if (result != null)
                {
                    result.IsAdded = true;
                    multiple(result);
                }
            }
        };

        if (!_duplicateBan.Contains(name))
        {
            _duplicateBan.Add(name);
            Add(name, sequence);
        }
    }
    // シーンの追加
    private void Add(string name_, System.Action complete_)
    {
        StartCoroutine(AddScene(name_, complete_));
    }
    // シーンの追加
    private IEnumerator AddScene(string name_, System.Action complete_)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(name_, LoadSceneMode.Additive);
        yield return async;

        if (complete_ != null)
        {
            complete_();
        }
    }
    #endregion

    #region Unload
    // シーンのアンロード
    public void Unload<T>(float wait_ = 0.1f) where T : AddOnBase
    {
        string name = typeof(T).Name;

        if (_duplicateBan.Contains(name))
        {
            _duplicateBan.Remove(name);
            Unload(name, wait_);
        }
    }
    // シーンのアンロード
    private void Unload(string name_, float wait_)
    {
        StartCoroutine(UnloadScene(name_, wait_));
    }
    // シーンのアンロード
    private IEnumerator UnloadScene(string name_, float wait_)
    {
        var asyncOperation = SceneManager.UnloadSceneAsync(name_);
        while (asyncOperation.isDone && SceneManager.GetSceneByName(name_).IsValid())
        {
            yield return new WaitForSeconds(wait_);
        }
    }
    #endregion
}
