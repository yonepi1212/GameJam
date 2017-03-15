using UnityEngine;
using System.Collections.Generic;
using System.IO;

/*  FileCache
    ファイルキャッシュです。
*/
public class FileCache<T>   
{
    #region Declaration
    // パス
    protected string cachePath = "/FileCache";
    // 文鎮
    protected Object paperWeight = new Object();
    // 実行アクションを登録する辞書
    protected Dictionary<string, List<System.Action<T>>> executes = new Dictionary<string, List<System.Action<T>>>();
    // プログレスを登録する辞書
    protected Dictionary<string, List<System.Action<float>>> progress = new Dictionary<string, List<System.Action<float>>>();
    #endregion

    #region Public Method
    // リソースを開放する
    public virtual void Release()
    {
        progress = null;
        executes = null;
        paperWeight = null;
    }
    // URLからハッシュキーを作る
    public virtual string KeyForURL(string url_)
    {
        System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();

        byte[] source = System.Text.Encoding.UTF8.GetBytes(url_);
        byte[] result = md5.ComputeHash(source);

        System.Text.StringBuilder stringBuilder;
        stringBuilder = new System.Text.StringBuilder();

        foreach (byte curByte in result)
        {
            stringBuilder.Append(curByte.ToString("x2"));
        }

        return stringBuilder.ToString();
    }
    // キーからパスを生成する
    public virtual string PathForKey(string key)
    {
#if UNITY_EDITOR
        string directory = Application.dataPath + cachePath;
#else
        string directory = Application.temporaryCachePath + cachePath;
#endif
        return string.Format("{0}/{1}/{2}", directory, key.Substring(0, 2), key);
    }
    // ファイルの有無を確認する
    public bool Exists(string path_)
    {
        return File.Exists(path_);
    }
    // 実行アクションの登録
    public bool RegisterExecutes(string key_, System.Action<T> complete_)
    {
        bool result = false;

        if (!executes.ContainsKey(key_))
        {
            List<System.Action<T>> page = new List<System.Action<T>>();
            page.Add(complete_);
            executes.Add(key_, page);

            result = true;
        }
        else
        {
            lock (paperWeight)
            {
                executes[key_].Add(complete_);
            }
        }

        return result;
    }

    // 実行アクションが登録されているかを返す
    public bool ContainsExecutes(string key_)
    {
        return executes.ContainsKey(key_);
    }
    // 実行アクションを実行する
    public virtual void Execute(string key_, T data_)
    {
        if (executes.ContainsKey(key_))
        {
            executes[key_].ForEach(action =>
            {
                action(data_);
            });

            executes[key_] = null;
            executes.Remove(key_);
        }
    }
    // プログレスの登録
    public bool RegisterProgress(string key_, System.Action<float> progress_)
    {
        bool result = false;

        if (!progress.ContainsKey(key_))
        {
            List<System.Action<float>> page = new List<System.Action<float>>();
            page.Add(progress_);
            progress.Add(key_, page);

            result = true;
        }
        else
        {
            lock (paperWeight)
            {
                progress[key_].Add(progress_);
            }
        }

        return result;
    }
    //プログレスを削除する
    public virtual void RemoveProgress(string key_)
    {
        if (progress.ContainsKey(key_))
        {
            progress[key_] = null;
            progress.Remove(key_);
        }
    }
    // プログレスが登録されているかを返す
    public bool ContainsProgress(string key_)
    {
        return progress.ContainsKey(key_);
    }
    //プログレスを実行する
    public virtual void ExecuteProgress(string key_, float progress_)
    {
        if (progress.ContainsKey(key_))
        {
            progress[key_].ForEach(action => { action(progress_); });
        }
    }
    #endregion
}