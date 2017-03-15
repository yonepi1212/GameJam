/* Singleton.cs
    通常のシングルトンクラス
*/
public class Singleton<T> where T : class, new()
{
    // 万一、外からコンストラクタを呼ばれたときに、ここで引っ掛ける
    #region Constructor
    protected Singleton()
    {
        LogUtility.Log((null == _instance).ToString());
    }
    #endregion

    #region Declaration
    private static readonly T _instance = new T();
    public static T Instance {
         get {
             return _instance;
         }
    }
    #endregion
}
