using UnityEngine;

public static class ScreenUtility
{

    /// <summary>
    /// スクリーンの高さ
    ///     Editor上ではGameView左上の数値(w×h)のhを返す
    /// 　　実機では実際の高さを返す
    /// </summary>
    public static float Height
    {
        get
        {
#if UNITY_EDITOR
        SetScreenWidthAndHeightFromEditorGameViewViaReflection();
        return editorScreenHeight;
#else
        return Screen.height;
#endif
        }
    }

    /// <summary>
    /// スクリーンの幅
    ///     Editor上ではGameView左上の数値(w×h)のwを返す
    /// 　　実機では実際の幅を返す
    /// </summary>
    public static float Width
    {
        get
        {
#if UNITY_EDITOR
            SetScreenWidthAndHeightFromEditorGameViewViaReflection();
            return editorScreenWidth;
#else
        return Screen.width;
#endif
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// エディターのゲームview上のスクリーンの高さ
    /// </summary>
    private static float editorScreenHeight;
    /// <summary>
    /// エディターのゲームview上のスクリーンの幅
    /// </summary>
    private static float editorScreenWidth;

    /// <summary>
    ///　ゲームビューのサイズを取得する
    /// </summary>
    private static void SetScreenWidthAndHeightFromEditorGameViewViaReflection()
    {
        //ゲームビューを取得
        var gameView = GetMainGameView();
        var prop = gameView.GetType().GetProperty("currentGameViewSize", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var gvsize = prop.GetValue(gameView, new object[0]{});
        var gvSizeType = gvsize.GetType();
 
        //ゲームビューのプロパティから高さと幅を取得
        editorScreenHeight = (int)gvSizeType.GetProperty("height", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).GetValue(gvsize, new object[0] { });
        editorScreenWidth = (int)gvSizeType.GetProperty("width", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).GetValue(gvsize, new object[0]{});
    }

    /// <summary>
    /// ゲームビューを取得
    /// </summary>
    private static UnityEditor.EditorWindow GetMainGameView()
    {
        System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
        System.Reflection.MethodInfo GetMainGameView = T.GetMethod("GetMainGameView",System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        System.Object Res = GetMainGameView.Invoke(null,null);
        return (UnityEditor.EditorWindow)Res;
    }
#endif

}