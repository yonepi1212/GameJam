using UnityEngine.Events;

public static class UnityActionExtensiton
{
    /// <summary>
    /// UnityActionデリゲートを安全に呼び出します.
    /// </summary>
    public static void Call(this UnityAction action)
    {
        if (action != null) { action(); }
    }

    /// <summary>
    /// UnityActionデリゲートを安全に呼び出します.
    /// </summary>
    public static void Call<T>(this UnityAction<T> action, T arg)
    {
        if (action != null) { action(arg); }
    }

    /// <summary>
    /// UnityActionデリゲートを安全に呼び出します.
    /// </summary>
    public static void Call<T1, T2>(this UnityAction<T1, T2> action, T1 arg1, T2 arg2)
    {
        if (action != null) { action(arg1, arg2); }
    }

    /// <summary>
    /// UnityActionデリゲートを安全に呼び出します.
    /// </summary>
    public static void Call<T1, T2, T3>(this UnityAction<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3)
    {
        if (action != null) { action(arg1, arg2, arg3); }
    }
}
