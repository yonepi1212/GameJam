using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// 汎用ユーティリティクラス
/// </summary>
public static class GenericUtility
{
    /// <summary>
    /// ランダムな要素を返却する
    /// </summary>
    public static T RandomAt<T>(this IEnumerable<T> ie_)
    {
        return ie_.ElementAt(Random.Range(0, ie_.Count()));
    }

    /// <summary>
    /// ランダムな要素を返却する
    /// </summary>
    public static T RandomAt<T>(this T[] array_)
    {
        return array_.ElementAt(Random.Range(0, array_.Count()));
    }

    /// <summary>
    /// 要素をシャッフルして返却する
    /// </summary>
    public static T[] Shuffle<T>(this T[] array_)
    {
        for (int i = 0; i < array_.Length; ++i)
        {
            T temp = array_[i];
            int randomIndex = Random.Range(0, array_.Length);
            array_[i] = array_[randomIndex];
            array_[randomIndex] = temp;
        }
        return array_;
    }

    /// <summary>
    /// 文字列のNull+空チェック
    /// </summary>
    public static bool IsNullOrEmpty(this string self_)
    {
        return string.IsNullOrEmpty(self_);
    }

    /// <summary>
    /// リストのNull+空チェック
    /// </summary>
    public static bool IsNullOrEmpty<T>(this IList<T> self_)
    {
        return self_ == null || self_.Count == 0;
    }

    /// <summary>
    /// SetParentして、Transformを初期化する
    /// </summary>
    /// <param name="trans_">子となるトランスフォーム</param>
    /// <param name="parent_">親となるトランスフォーム</param>
    public static void SetParentInit(this Transform trans_, Transform parent_)
    {
        trans_.SetParent(parent_);
        trans_.localPosition = Vector3.zero;
        trans_.localScale = Vector3.one;
        trans_.localRotation = Quaternion.identity;
    }

    /// <summary>
    /// リストの要素を重複させずに追加します
    /// 要素を追加を実行した場合はtrue, 重複のため実行をしない場合falseを返します
    /// </summary>
    public static bool AddUnique<T>(this IList<T> list_, T item_)
    {
        if (!list_.Contains(item_))
        {
            list_.Add(item_);
            return true;
        }
        return false;
    }

    /// <summary>
    /// リストの要素を重複させずに複数追加します
    /// </summary>
    public static void AddRangeUnique<T>(this IList<T> list_, IEnumerable<T> collection_)
    {
        foreach (var item in collection_)
        {
            list_.AddUnique(item);
        }
    }

    /// <summary>
    /// 2点間のXZ角度を取得します
    /// </summary>
    public static float GetAngle(Vector3 p1, Vector3 p2, bool toDegree = true)
    {
        float dx = p2.x - p1.x;
        float dz = p2.z - p1.z;
        float rad = Mathf.Atan2(dz, dx);
        if (toDegree) return rad * Mathf.Rad2Deg;
        return rad;
    }

    /// <summary>
    /// SetParentして、Recttransformを初期化する
    /// </summary>
    /// <param name="trans_">子となるRectTransform</param>
    /// <param name="parent_">親となるRectTransform</param>
    public static void SetParentInitUgui(this RectTransform trans_, RectTransform parent_)
    {
        trans_.SetParent(parent_);
        trans_.localPosition = Vector3.zero;
        trans_.offsetMax = Vector2.zero;
        trans_.offsetMin = Vector2.zero;
        trans_.localScale = Vector3.one;
        trans_.localRotation = Quaternion.identity;
    }

    /// <summary>
    /// DateTimeのパースし、結果をNullable型で返します
    /// 結果は.HasValueでチェックすることができます
    /// </summary>
    public static DateTime? DateParseNullable(string s_)
    {
        DateTime tmp;
        if (DateTime.TryParse(s_, out tmp))
        {
            // success
            return tmp;
        }
        // fail
        return null;
    }

    /// <summary>
    /// 末尾から指定数の要素を削除
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="self"></param>
    /// <param name="count"></param>
    public static void DropRight<T>(this List<T> self, int count)
    {
        self.RemoveRange(self.Count - count, count);
    }

    /// <summary>
    /// Y軸は0となるランダムな球を返します
    /// </summary>
    public static Vector3 GetRandomFlatSphere(float value_)
    {
        var ret = Random.insideUnitSphere * value_;
        ret.y = 0f;
        return ret;
    }

    /// <summary>
    /// 目的の値に最も近い値を返します
    /// </summary>
    /// <param name="self">自インスタンス</param>
    /// <param name="targetValue">目的値</param>
    /// <returns></returns>
    public static int Nearest(this IEnumerable<int> self, int targetValue)
    {
        int min = self.Min(x => Math.Abs(x - targetValue));
        return self.First(x => Math.Abs(x - targetValue) == min);
    }

    /// <summary>
    /// 目的の値に最も近い値を返します
    /// </summary>
    /// <param name="self">自インスタンス</param>
    /// <param name="targetValue">目的値</param>
    /// <returns></returns>
    public static float Nearest(this IEnumerable<float> self, float targetValue)
    {
        float min = self.Min(x => Math.Abs(x - targetValue));
        return self.First(x => Math.Abs(x - targetValue) == min);
    }

    /// <summary>
    /// インデックス付きでForEachしたいときに使う
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ie"></param>
    /// <param name="action"></param>
    public static void ForEachWithIndex<T>(this IEnumerable<T> ie, Action<T, int> action)
    {
        var i = 0;
        foreach (var e in ie) action(e, i++);
    }

    /// <summary>
    /// <para>As演算子によるCastとNullチェックを自動で行い、特定の処理を実行する拡張メソッドです.</para>
    /// <para>Chainメソッドで記述することが可能です.</para>
    /// <para>hoge</para>
    /// <para> .DoAs＜Foo＞(foo => Debug.Log(foo.ToString()))       // FooクラスにCastとラムダ式の実行を試行します</para>
    /// <para> .DoAs＜Bar＞(bar => Debug.Log(bar.ToString()));      // FooクラスのCastに失敗した場合、BarクラスのCastとラムダ式の実行を試行します</para>
    /// </summary>
    /// <typeparam name="T">キャストクラス</typeparam>
    /// <param name="self">自インスタンス</param>
    /// <param name="action">任意の処理</param>
    /// <returns>Actionが実行された場合nullが, 失敗した場合はselfが返却されます.</returns>
    public static object DoAs<T>(this object self, Action<T> action) where T : class
    {
        if (self == null) return null;
        var target = self as T;
        if (target != null)
        {
            action.Call(target);
            return null;
        }
        return self;
    }

    #region Min Method

    /// <summary>
    /// 最小値のIndexを取得します.
    /// </summary>
    /// <param name="self">自インスタンス</param>
    /// <param name="selector">比較する値を返す処理</param>
    /// <returns>最小となるIndex</returns>
    public static int IndexOfMin<T>(this IEnumerable<T> self, Func<T, int> selector)
    {
        if (self == null || selector == null) throw new ArgumentNullException();
        int minValue = int.MaxValue;
        int minIndex = -1;
        int index = -1;
        foreach (T obj in self)
        {
            index++;
            var num = selector(obj);
            if (num <= minValue)
            {
                minValue = num;
                minIndex = index;
            }
        }
        if (index == -1) throw new InvalidOperationException("Sequence was empty");
        return minIndex;
    }

    /// <summary>
    /// 最小値のIndexを取得します.
    /// </summary>
    /// <param name="self">自インスタンス</param>
    /// <param name="selector">比較する値を返す処理</param>
    /// <returns>最小となるIndex</returns>
    public static int IndexOfMin<T>(this IEnumerable<T> self, Func<T, float> selector)
    {
        if (self == null || selector == null) throw new ArgumentNullException();
        float minValue = float.MaxValue;
        int minIndex = -1;
        int index = -1;
        foreach (T obj in self)
        {
            index++;
            var num = selector(obj);
            if (num <= minValue)
            {
                minValue = num;
                minIndex = index;
            }
        }
        if (index == -1) throw new InvalidOperationException("Sequence was empty");
        return minIndex;
    }

    /// <summary>
    /// 最小値の要素を取得します.
    /// </summary>
    /// <param name="self">自インスタンス</param>
    /// <param name="selector">比較する値を返す処理</param>
    /// <returns>最小となる要素</returns>
    public static T MinElement<T>(this IEnumerable<T> self, Func<T, int> selector)
    {
        if (self == null || selector == null) throw new ArgumentNullException();
        int minValue = int.MaxValue;
        T minObj = default(T);
        foreach (T obj in self)
        {
            var num = selector(obj);
            if (num <= minValue)
            {
                minValue = num;
                minObj = obj;
            }
        }
        return minObj;
    }

    /// <summary>
    /// 最小値の要素を取得します.
    /// </summary>
    /// <param name="self">自インスタンス</param>
    /// <param name="selector">比較する値を返す処理</param>
    /// <returns>最小となる要素</returns>
    public static T MinElement<T>(this IEnumerable<T> self, Func<T, float> selector)
    {
        if (self == null || selector == null) throw new ArgumentNullException();
        float minValue = float.MaxValue;
        T minObj = default(T);
        foreach (T obj in self)
        {
            var num = selector(obj);
            if (num <= minValue)
            {
                minValue = num;
                minObj = obj;
            }
        }
        return minObj;
    }

    #endregion End Min Method

    
    #region Max Method

    /// <summary>
    /// 最大値のIndexを取得します.
    /// </summary>
    /// <param name="self">自インスタンス</param>
    /// <param name="selector">比較する値を返す処理</param>
    /// <returns>最大となるIndex</returns>
    public static int IndexOfMax<T>(this IEnumerable<T> self, Func<T, int> selector)
    {
        if (self == null || selector == null) throw new ArgumentNullException();
        int maxValue = int.MinValue;
        int maxIndex = -1;
        int index = -1;
        foreach (T obj in self)
        {
            index++;
            var num = selector(obj);
            if (num >= maxValue)
            {
                maxValue = num;
                maxIndex = index;
            }
        }
        if (index == -1) throw new InvalidOperationException("Sequence was empty");
        return maxIndex;
    }

    /// <summary>
    /// 最大値のIndexを取得します.
    /// </summary>
    /// <param name="self">自インスタンス</param>
    /// <param name="selector">比較する値を返す処理</param>
    /// <returns>最大となるIndex</returns>
    public static int IndexOfMax<T>(this IEnumerable<T> self, Func<T, float> selector)
    {
        if (self == null || selector == null) throw new ArgumentNullException();
        float maxValue = float.MinValue;
        int maxIndex = -1;
        int index = -1;
        foreach (T obj in self)
        {
            index++;
            var num = selector(obj);
            if (num >= maxValue)
            {
                maxValue = num;
                maxIndex = index;
            }
        }
        if (index == -1) throw new InvalidOperationException("Sequence was empty");
        return maxIndex;
    }

    /// <summary>
    /// 最大値の要素を取得します.
    /// </summary>
    /// <param name="self">自インスタンス</param>
    /// <param name="selector">比較する値を返す処理</param>
    /// <returns>最大となる要素</returns>
    public static T MaxElement<T>(this IEnumerable<T> self, Func<T, int> selector)
    {
        if (self == null || selector == null) throw new ArgumentNullException();
        int maxValue = int.MinValue;
        T maxObj = default(T);
        foreach (T obj in self)
        {
            var num = selector(obj);
            if (num >= maxValue)
            {
                maxValue = num;
                maxObj = obj;
            }
        }
        return maxObj;
    }

    /// <summary>
    /// 最大値の要素を取得します.
    /// </summary>
    /// <param name="self">自インスタンス</param>
    /// <param name="selector">比較する値を返す処理</param>
    /// <returns>最大となる要素</returns>
    public static T MaxElement<T>(this IEnumerable<T> self, Func<T, float> selector)
    {
        if (self == null || selector == null) throw new ArgumentNullException();
        float maxValue = float.MinValue;
        T maxObj = default(T);
        foreach (T obj in self)
        {
            var num = selector(obj);
            if (num >= maxValue)
            {
                maxValue = num;
                maxObj = obj;
            }
        }
        return maxObj;
    }
    #endregion

    // クラスのDeepコピー
    public static T DeepCopy<T>(T source_) where T : class
    {
        T result;
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        MemoryStream memoryStream = new MemoryStream();

        try
        {
            binaryFormatter.Serialize(memoryStream, source_);
            memoryStream.Position = 0;
            result = binaryFormatter.Deserialize(memoryStream) as T;
        }
        finally
        {
            memoryStream.Close();
        }

        return result;
    }
}