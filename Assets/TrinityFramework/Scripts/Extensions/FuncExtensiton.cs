using System;
using System.Collections;

public static  class FuncExtensiton
{
    /// <summary>
    /// Funcデリゲートを安全に呼び出します.
    /// </summary>
    public static TResult Call<TResult>(this Func<TResult> func, TResult result = default(TResult))
    {
        return func != null ? func() : result;
    }

    /// <summary>
    /// Funcデリゲートを安全に呼び出します.
    /// </summary>
    public static TResult Call<T, TResult>(this Func<T, TResult> func, T arg, TResult result = default(TResult))
    {
        return func != null ? func(arg) : result;
    }

    /// <summary>
    /// Funcデリゲートを安全に呼び出します.
    /// </summary>
    public static TResult Call<T1, T2, TResult>(this Func<T1, T2, TResult> func, T1 arg1, T2 arg2, TResult result = default(TResult))
    {
        return func != null ? func(arg1, arg2) : result;
    }

    /// <summary>
    /// Funcデリゲートを安全に呼び出します.
    /// </summary>
    public static TResult Call<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> func, T1 arg1, T2 arg2, T3 arg3, TResult result = default(TResult))
    {
        return func != null ? func(arg1, arg2, arg3) : result;
    }
}
