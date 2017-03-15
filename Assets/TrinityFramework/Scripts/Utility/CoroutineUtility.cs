using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// コルーチンに関する処理をまとめた汎用クラス
/// </summary>
public static class CoroutineUtility
{
    /// <summary>
    /// コルーチンデータ構造体
    /// </summary>
    public struct RoutineData
    {
        public MonoBehaviour monoObj;
        public MethodInfo methodInfo;
        public Coroutine coroutine;
        public IEnumerator iterator;
        public string methodName;
        //public object[] args;

        #region Method

        // コルーチンの再開
        public void Resume()
        {
            if (monoObj)
            {
                if (monoObj.gameObject.activeInHierarchy && iterator != null)
                {
                    coroutine = monoObj.StartCoroutine(iterator);
                }
            }
        }

        // コルーチンの一時停止
        public void Pause()
        {
            if (monoObj != null && iterator != null)
            {
                monoObj.StopCoroutine(iterator);
            }
        }

        // コルーチンの停止
        public void Stop()
        {
            if (monoObj)
            {
                if (iterator != null)
                {
                    monoObj.StopCoroutine(iterator);
                    monoObj.StopAllCoroutines();
                    iterator = null;
                    return;
                }
                if (!string.IsNullOrEmpty(methodName))
                {
                    monoObj.StopCoroutine(methodName);
                    return;
                }
            }
        }

        // 空かどうか、空ならtrue
        public bool IsEmpty()
        {
            if (!monoObj) return true;
            if (iterator == null && string.IsNullOrEmpty(methodName)) return true;
            return false;
        }

        #endregion
    }

    /// <summary>
    /// コルーチンを代理で実行するモノビヘイビア
    /// </summary>
    public static readonly MonoBehaviour Mono;

    private static List<RoutineData> _coroutineList = new List<RoutineData>();

    /// <summary>
    /// コルーチンを管理するゲームオブジェクトを生成するコンストラクタ
    /// </summary>
    static CoroutineUtility()
    {
        var gameObject = new GameObject("CoroutineProxy");
        GameObject.DontDestroyOnLoad(gameObject);
        Mono = gameObject.AddComponent<CoroutineProxy>();
    }

    /// <summary>
    /// 代理コルーチン稼働
    /// </summary>
    /// <param name="enumerator">稼働させるイテレーター</param>
    public static void StartCoroutine(IEnumerator enumerator) { Mono.StartCoroutine(enumerator); }

    /// <summary>
    /// 代理コルーチン稼働
    /// </summary>
    /// <param name="enumerator">稼働させるイテレーター</param>
    public static void StartCoroutineSafe(IEnumerator enumerator) { Mono.StartCoroutineSafe(enumerator); }

    /// <summary>
    /// コルーチン停止
    /// </summary>
    /// <param name="enumerator">停止させるイテレーター</param>
    public static void StopCoroutine(IEnumerator enumerator) { Mono.StopCoroutine(enumerator); }

    #region Delay Method

    /// <summary>
    /// 1 フレーム待機してから Action デリゲートを呼び出します
    /// </summary>
    public static void CallWaitForOneFrame(Action act_)
    {
        Mono.StartCoroutineSafe(DoCallWaitForOneFrame(act_));
    }

    /// <summary>
    /// 指定された秒数待機してから Action デリゲートを呼び出します
    /// </summary>
    public static void CallWaitForSeconds(float seconds_, Action act_)
    {
        Mono.StartCoroutineSafe(DoCallWaitForSeconds(seconds_, act_));
    }

    /// <summary>
    /// 1 フレーム待機してから Action デリゲートを呼び出します
    /// </summary>
    private static IEnumerator DoCallWaitForOneFrame(Action act_)
    {
        yield return null;
        if (act_ != null)
        {
            act_();
        }
    }

    /// <summary>
    /// 指定された秒数待機してから Action デリゲートを呼び出します
    /// </summary>
    private static IEnumerator DoCallWaitForSeconds(float seconds_, Action act_)
    {
        var t = 0f;
        while (t < seconds_)
        {
            yield return null;
            t += Time.deltaTime;
        }

        if (act_ != null)
        {
            act_();
        }
    }

    #endregion

    #region Manage Method

    /// <summary>
    /// すべてのコルーチンを再び再生する
    /// </summary>
    public static void ResumeCoroutineAll()
    {
        _coroutineList.RemoveAll(data => data.IsEmpty());

        for (int i = 0; i < _coroutineList.Count; ++i)
        {
            if (_coroutineList[i].iterator == null) continue;
            _coroutineList[i].Resume();
        }
    }

    /// <summary>
    /// コルーチンを再開する
    /// </summary>
    public static void ResumeCoroutine(this MonoBehaviour mono_, Coroutine coroutine_)
    {
        var targets = _coroutineList.FindAll(data => data.coroutine == coroutine_);
        foreach (var target in targets)
        {
            target.Resume();
        }
    }

    /// <summary>
    /// すべてのコルーチンを一時停止する
    /// </summary>
    public static void PauseCoroutineAll()
    {
        _coroutineList.RemoveAll(data => data.IsEmpty());

        foreach (var data in _coroutineList)
        {
            data.Pause();
        }
    }

    /// <summary>
    /// コルーチンを一時停止する
    /// </summary>
    public static void PauseCoroutine(this MonoBehaviour mono_, Coroutine coroutine_)
    {
        var targets = _coroutineList.FindAll(data => data.coroutine == coroutine_);
        foreach (var target in targets)
        {
            target.Pause();
        }
    }

    /// <summary>
    /// すべてのコルーチンを停止する
    /// </summary>
    public static void StopCoroutineAll()
    {
        _coroutineList.RemoveAll(data => data.IsEmpty());

        foreach (var data in _coroutineList)
        {
            data.Stop();
        }

        _coroutineList.Clear();
    }

    /// <summary>
    /// 開放
    /// </summary>
    public static void Release()
    {
        StopCoroutineAll();
        _coroutineList.Clear();
    }

    #endregion

    #region StartCoroutineSafe

    public static Coroutine StartCoroutineSafe<T>(this T mono_, IEnumerator routine_) where T : MonoBehaviour
    {
        if (CheckStartCoroutine(mono_) == false) return null;
        return ExecCoroutine(mono_, null, routine_, null);
    }

#if false
    /// <summary>
    /// コルーチンを安全に再生する
    /// </summary>
    /// <param name="mono_">コルーチンを実行するモノビヘイビア</param>
    /// <param name="routine_">コルーチンメソッド</param>
    /// <returns>再生されたコルーチンを返します</returns>
    public static Coroutine StartCoroutineSafe(this MonoBehaviour mono_, Func<IEnumerator> routine_)
    {
        if (CheckStartCoroutine(mono_) == false) return null;
        IEnumerator iterator = routine_.Invoke();
        var method = routine_.Method;
        return ExecCoroutine(mono_, method, iterator, method.Name, null);
    }

    // 引数1つver
    public static Coroutine StartCoroutineSafe<T1>(this MonoBehaviour mono_, Func<T1, IEnumerator> routine_, T1 arg1)
    {
        if (CheckStartCoroutine(mono_) == false) return null;
        IEnumerator iterator = routine_.Invoke(arg1);
        var method = routine_.Method;
        return ExecCoroutine(mono_, method, iterator, routine_.Method.Name, new object[] {arg1});
    }

    // 引数2つver
    public static Coroutine StartCoroutineSafe<T1, T2>(this MonoBehaviour mono_, Func<T1, T2, IEnumerator> routine_,
        T1 arg1, T2 arg2)
    {
        if (CheckStartCoroutine(mono_) == false) return null;
        IEnumerator iterator = routine_.Invoke(arg1, arg2);
        var method = routine_.Method;
        return ExecCoroutine(mono_, method, iterator, method.Name, new object[] {arg1, arg2});
    }

    // 引数3つver
    public static Coroutine StartCoroutineSafe<T1, T2, T3>(this MonoBehaviour mono_,
        Func<T1, T2, T3, IEnumerator> routine_, T1 arg1, T2 arg2, T3 arg3)
    {
        if (CheckStartCoroutine(mono_) == false) return null;
        IEnumerator iterator = routine_.Invoke(arg1, arg2, arg3);
        var method = routine_.Method;
        return ExecCoroutine(mono_, method, iterator, method.Name, new object[] {arg1, arg2, arg3});
    }

    // 引数4つver
    public static Coroutine StartCoroutineSafe<T1, T2, T3, T4>(this MonoBehaviour mono_,
        Func<T1, T2, T3, T4, IEnumerator> routine_, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        if (CheckStartCoroutine(mono_) == false) return null;
        IEnumerator iterator = routine_.Invoke(arg1, arg2, arg3, arg4);
        var method = routine_.Method;
        return ExecCoroutine(mono_, method, iterator, method.Name, new object[] {arg1, arg2, arg3, arg4});
    }
#endif

    /// <summary>
    /// コルーチンを安全に再生する 文字列指定ver
    /// </summary>
    /// <param name="mono_">コルーチンを実行するモノビヘイビア</param>
    /// <param name="methodName_">コルーチンメソッド名</param>
    /// <param name="arg_">引数</param>
    /// <returns>再生されたコルーチンを返します</returns>
    public static Coroutine StartCoroutineSafe(this MonoBehaviour mono_, string methodName_, object arg_ = null)
    {
        if (CheckStartCoroutine(mono_) == false) return null;

        // コルーチンのメソッドを取得
        MethodInfo method = arg_ == null
            ? mono_.GetType().GetMethod(methodName_,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static |
                BindingFlags.DeclaredOnly, null, new Type[] {}, new ParameterModifier[] {})
            : mono_.GetType().GetMethod(methodName_,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static |
                BindingFlags.DeclaredOnly, null, new Type[] {arg_.GetType()}, new ParameterModifier[] {});

        // コルーチンの実行&登録
        IEnumerator iterator = null;
        if (method != null)
        {
            // メソッドを実行し、IEnumratorを取得
            iterator = arg_ == null
                ? method.Invoke(mono_, null) as IEnumerator
                : method.Invoke(mono_, new[] {arg_}) as IEnumerator;
        }

        Coroutine ret = null;
        if (iterator != null)
        {
            ret = mono_.StartCoroutine(iterator);
        }
        else
        {
            ret = arg_ == null ? mono_.StartCoroutine(methodName_) : mono_.StartCoroutine(methodName_, arg_);
        }

        if (ret != null)
        {
            _coroutineList.Add(new RoutineData()
            {
                monoObj = mono_,
                methodInfo = method,
                coroutine = ret,
                iterator = iterator,
                methodName = methodName_,
                //args = new object[] {arg_}
            });
        }
        else
        {
            Debug.LogWarning("Cotourine Not Found.");
        }
        return ret;
    }


    // コルーチンを実行可能かチェックする
    static bool CheckStartCoroutine(MonoBehaviour mono_)
    {
        if (!mono_)
        {
            Debug.LogWarning("StartCoroutine Canceled! MonoBehabiour is Null");
            return false;
        }

        if (!mono_.gameObject.activeInHierarchy)
        {
            Debug.LogWarning("StartCoroutine Canceled! " + mono_.name + " is InActive");
            return false;
        }
        return true;
    }

    // コルーチンを実行する
    static Coroutine ExecCoroutine(MonoBehaviour mono_, MethodInfo method_, IEnumerator iterator_, string methodName_)
    {
        Coroutine ret = mono_.StartCoroutine(iterator_);
        if (ret != null)
        {
            _coroutineList.Add(new RoutineData()
            {
                monoObj = mono_,
                methodInfo = method_,
                coroutine = ret,
                iterator = iterator_,
                methodName = methodName_,
                //args = args_
            });
        }
        return ret;
    }

    #endregion
}