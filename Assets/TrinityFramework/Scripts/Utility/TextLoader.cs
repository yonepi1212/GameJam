using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Textの値を外部から読み込むクラス
/// </summary>
[ExecuteInEditMode]
[RequireComponent(typeof(Text))]
[AddComponentMenu("UI/TextLoader")]
public class TextLoader : MonoBehaviour
{
    #region Private Declaration

    /// <summary>
    /// 対象のテキストコンポーネント
    /// </summary>
    private Text t;
    #endregion

    #region Public Declaration

    public Text TargetText
    {
        get { return t ?? (t = GetComponent<Text>()); }
    }

    /// <summary>
    /// キー
    /// </summary>
    public string Key;

    /// <summary>
    /// 値
    /// </summary>
    public string Value
    {
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (TargetText != null)
                {
                    TargetText.text = value;
                }
            }
        }
    }

    /// <summary>
    /// 対象のcsvの識別子
    /// </summary>
    public string CsvId;

    /// <summary>
    /// 読み込み成功フラグ
    /// </summary>
    [NonSerialized]
    public bool loadSuccess = false;
    #endregion

    #region MonoBehaviour

    #if UNITY_EDITOR
    void OnValidate()
    {
        loadSuccess = LoadTextValue();
    }
    #endif  

    void Start ()
    {
        loadSuccess = LoadTextValue();
    }
    #endregion

    #region PublicMethod
    public void ReloadTextDictionary()
    {
        TextManager.Reload(CsvId);
        loadSuccess = LoadTextValue();
    }

    public void ReloadText()
    {
        loadSuccess = LoadTextValue();
    }
    #endregion

    #region Private Method
    private bool LoadTextValue()
    {
        //もしキーが設定されてなかったらデフォルトでTextに入ってる文言をキーとして使う
        if (string.IsNullOrEmpty(Key))
        {
            if (TargetText != null) Key = TargetText.text;
        }
        if (!string.IsNullOrEmpty(Key))
        {
            var textValue = TextManager.Get(CsvId, Key);
            if (string.IsNullOrEmpty(textValue))
            {
                return false;
            }
            Value = textValue;
            return true;
        }
        return false;
    }
    #endregion
}