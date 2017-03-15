#if false
using System;
using System.Reflection;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

/* LogUtility.cs
    ログを見やすくするクラス

    DLLを生成するときは#ifをTrueにし、同フォルダに含まれるbuild.batをダブルクリックしてください
　  DLLを生成後は再び#ifをFalseにするようにしてください。　

    ex:
    LogUtility.Log("message",);
    LogUtility.LogWarning("message","tag");
    LogUtility.LogError("message","tag");
    LogUtility.LogException(exceptoin);
*/
public class LogUtility
{
#region Declaration
    private static ILogger _logger;
    private static ILogger Logger
    {
        get { return _logger ?? (_logger = Debug.logger); }
    }

    /// <summary>
    /// ログの出力を有効にする
    /// </summary>
    public static bool Enabled
    {
        get { return Logger.logEnabled; }
        set { Logger.logEnabled = value; }
    }

    /// <summary>
    /// Logの見た目をトリニティ仕様にする
    /// </summary>
    public static bool IsTrinityFormat = true;
    
    /// <summary>
    /// Logのフィルタリング
    /// </summary>
    public static LogType FilterLogType
    {
        set { Logger.filterLogType = value; }
        get { return Logger.filterLogType; }
    }
    private static bool _isProSkin;
    private static bool _isInitialization;

    private static bool _isEditor;
    public static bool IsEditor
    {
        get
        {
            try
            {
                var asm = Assembly.Load("UnityEditor.dll");
                _isEditor = asm != null;
            }
            catch (Exception)
            {
                _isEditor = false;
            }
            return _isEditor;
        }
    }
    /// <summary>
    /// プロ用のスキンかどうか
    /// </summary>
    public static bool IsProSkin
    {
        get
        {
            if (!_isInitialization && IsEditor)
            {
                var asm = Assembly.Load("UnityEditor.dll");
                Type myType = asm.GetType("UnityEditor.EditorGUIUtility");
                var isProSkin = myType.GetProperty("isProSkin");
                _isProSkin = Convert.ToBoolean(isProSkin.GetValue(asm, null));
            }

            _isInitialization = true;

            return _isProSkin;
        }
    }
    #endregion

    #region Public Method
    public static void Log(string aMessage = null, int skipFrames = 2)
    {
        Logger.Log(aMessage == null ? GetMessage(skipFrames) : GetMessage(aMessage, skipFrames));
    }

    public static void LogWarning(string aMessage = null, string tag = "", int skipFrames = 2)
    {
        Logger.LogWarning(GetTagFormat(tag), aMessage == null ? GetMessage(skipFrames) : GetMessage(aMessage, skipFrames));
    }

    public static void LogError(string aMessage = null, string tag = "", int skipFrames = 2)
    {
        Logger.LogError(GetTagFormat(tag), aMessage == null ? GetMessage(skipFrames) : GetMessage(aMessage, skipFrames));
    }

    public void LogException(Exception exception, Object context = null)
    {
        if (context)
        {
            Logger.LogException(exception, context);
        }
        else
        {
            Logger.LogException(exception);
        }
    }
    #endregion

    #region Private Method for info of method
    // MethodBaseクラスの取得
    private static MethodBase GetMethodBase()
    {
        MethodBase methodBase = null;
        StackTrace stackTrace = new StackTrace();
        for (int frame = 0; frame < stackTrace.FrameCount; frame++)
        {
            StackFrame stackFrame = stackTrace.GetFrame(frame);
            methodBase = stackFrame.GetMethod();
            var declaringType = MethodBase.GetCurrentMethod().DeclaringType;
            if (methodBase.DeclaringType != null && (declaringType != null && declaringType.FullName != methodBase.DeclaringType.FullName))
            {
                break;
            }
        }
        return methodBase;
    }

    // MethodFullNameの取得
    private static string GetMethodFullName()
    {
        MethodBase methodBase = GetMethodBase();
        var methodFullName = "";
        if (methodBase.DeclaringType != null)
        {
            methodFullName = methodBase.DeclaringType.FullName + "." + methodBase.Name;
        }
        return methodFullName;
    }
    // FileLineNumberの取得
    private static int GetFileLineNumber(int skipFrames = 2)
    {
        return new StackFrame(skipFrames, true).GetFileLineNumber();
    }
    #endregion

    #region Private Method for format of message
    private static string GetMessage(int skipFrames)
    {
        return IsTrinityFormat && IsEditor ? string.Format(GetColorText("{0}({1})"), GetMethodFullName(), GetFileLineNumber(skipFrames)) : "";
    }

    private static string GetMessage(string aMessage, int skipFrames)
    {
        return IsTrinityFormat && IsEditor ? string.Format(GetColorText("{0}({1}) : {2}"), GetMethodFullName(), GetFileLineNumber(skipFrames), aMessage) : aMessage;
    }

    private static string GetTagFormat(string tag)
    {
        return IsTrinityFormat && IsEditor ? string.Format(GetColorText("[{0}]"), tag) : tag;
    }

    private static string GetColorText(string message)
    {
        var colorName = IsProSkin ? "lightblue" : "navy";
        return string.Format("<color={0}>{1}</color>", colorName, message);
    }
    #endregion
}
#endif
