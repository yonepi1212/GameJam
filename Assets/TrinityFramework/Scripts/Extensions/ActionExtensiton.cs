using System;
using System.Collections;

public static class ActionExtensiton
{
    /// <summary>
    /// Actionデリゲートを安全に呼び出します.
    /// </summary>
    public static void Call(this Action action)
    {
        if (action != null) { action(); }
    }

    /// <summary>
    /// Actionデリゲートを安全に呼び出します.
    /// </summary>
    public static void Call<T>(this Action<T> action, T arg)
    {
        if (action != null) { action(arg); }
    }

    /// <summary>
    /// Actionデリゲートを安全に呼び出します.
    /// </summary>
    public static void Call<T1, T2>(this Action<T1, T2> action, T1 arg1, T2 arg2)
    {
        if (action != null) { action(arg1, arg2); }
    }

    /// <summary>
    /// Actionデリゲートを安全に呼び出します.
    /// </summary>
    public static void Call<T1, T2, T3>(this Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3)
    {
        if (action != null) { action(arg1, arg2, arg3); }
    }
}
