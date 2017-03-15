using UnityEngine;
using System.Collections.Generic;

public static class DictionaryExtensiton
{
    /// <summary>
    /// 安全に値をDictionaryに格納
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dict_"></param>
    /// <param name="key_"></param>
    /// <param name="value_"></param>
    public static void AddSafely<T>(this Dictionary<string, T> dict_, string key_, T value_)
    {
        if (string.IsNullOrEmpty(key_))
        {
            Debug.LogError("dictionaryのkeyが指定されていません");
            return;
        }
        if (value_ == null)
        {
            Debug.LogError(key_ + "に入れようとしている値が空です");
            return;
        }

        if (!dict_.ContainsKey(key_))
        {
            dict_.Add(key_, value_);
        }
        else
        {
            dict_[key_] = value_;
        }
    }

    /// <summary>
    /// Dictionaryから値を安全に取得
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dict_"></param>
    /// <param name="key_"></param>
    /// <returns></returns>
    public static object GetSafely<T>(this Dictionary<string, T> dict_, string key_)
    {
        if (!dict_.ContainsKey(key_))
        {
            Debug.LogError(string.Format("指定したキー:{0}はありません", key_));
            return null;
        }
        return dict_[key_];
    }
}
