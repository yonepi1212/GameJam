using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// シーンで使用するテキストを保持するクラス
/// </summary>
[ExecuteInEditMode]
public class ClientText
{
    #region Declaration

    /// <summary>
    /// 文字列辞書のロード済みフラグ
    /// </summary>
    public bool DictionaryHasBeenSet;

    public string CsvId = "";

    private string newLineTag = "<BR>";

    /// <summary>
    /// 文字列を保持するDictionary
    /// </summary>
    private  Dictionary<string,string> textDictionary = new Dictionary<string, string>();
    private  Dictionary<string, string> TextDictionary
    {
        get
        {
            if (!DictionaryHasBeenSet) Load(Define.Path.CsvRoot + Define.Name.Csv.ClientText);
            return textDictionary;
        }
        set
        {
            DictionaryHasBeenSet = (value != null);
            textDictionary = value;
        }
    }
    #endregion

    #region PublicMethod

    public void ReloadDictionary()
    {
        if (CsvId.IsNullOrEmpty())
        {
            Debug.LogWarning("シーンの種類が指定されてません");
            return;
        }
        Load(Define.Path.CsvRoot + Define.Name.Csv.ClientText + "_" + CsvId); 
    }

    #endregion

    #region Private Method

    public ClientText(string csvId_)
    {
        CsvId = csvId_;
        ReloadDictionary();
    }


    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Initialize()
    {
        try
        {
            Load(Define.Path.CsvRoot + Define.Name.Csv.ClientText + "_" + CsvId);
        }
        catch (Exception e)
        {
            Debug.Log("ロード失敗" + e.StackTrace);
        }
    }

    /// <summary>
    /// 文字列ファイルを読み込み
    /// </summary>
    /// <param name="filePath"></param>
    private void Load(string filePath_)
    {
        textDictionary.Clear();
        var asset = Resources.Load<TextAsset>(filePath_);
        if (asset == null)
        {
            Debug.LogError("対象のcsvファイルの読み込み失敗" + filePath_);
            return;
        }
        var text = asset.text;
        char[] newLine = { '\n', '\r'};//'\r'に対応 2016.8.4.
        var lines = text.Split(newLine).ToList();
        string header = lines[0];

        //2列以上必須
        if (header.Split(',').Length < 2)
        {
            Debug.LogError("テキストcsvのヘッダー項目が足らない");
            return;
        }

        if (lines.Count >= 2)
        {
            //ヘッダー削除
            lines.RemoveAt(0);
        }
        else
        {
            Debug.LogError("テキストcsvが2行未満");
            return;
        }

        Parse(lines);
    }

    /// <summary>
    /// 引数の文字をパースして辞書に格納
    /// </summary>
    /// <param name="line"></param>
    private void Parse(List<string> lines_)
    {
        Dictionary<string, string> tempDictionary = new Dictionary<string, string>();
        for (var i = 0; i < lines_.Count; i++)
        {
            var line = lines_[i];

            //空白行はスキップ
            if (string.IsNullOrEmpty(line.Trim())) continue;

            // コメントアウトしている行はスキップ
            if (line.StartsWith("//")) continue;

            var defines = line.Split(',');
            if (defines.Length < 2)
            {
                Debug.LogError(string.Format("テキストcsvの{0}行目のカラムが足りません", i + 1));
                continue;
            }
            string key = defines[0];
            string value = defines[1];

            if (!tempDictionary.ContainsKey(key))
            {
                tempDictionary.Add(key, value);
            }
            else
            {
                Debug.LogWarning(string.Format("{0}のキーが重複したので{1}を{2}で上書きました/{3}行目)", key, tempDictionary[key], value, i + 1));
                tempDictionary[key] = value;
            }
        }
        TextDictionary = tempDictionary;
    }
    #endregion

    #region Public Method

    /// <summary>
    /// 指定したキーの文字を取得
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public string Get(string key_)
    {
        if (TextDictionary.ContainsKey(key_))
        {
            var text = TextDictionary[key_];
            return RepleceNewLine(text);
        }
        Debug.LogWarning(string.Format("キー:{0}に該当する値がありません", key_));
        return "";
    }

    private string RepleceNewLine(string source_)
    {
        return source_.Replace(newLineTag, Environment.NewLine);
    }

    /// <summary>
    /// 破棄字処理
    /// </summary>
    public void Release()
    {
        DictionaryHasBeenSet = false;
        textDictionary.Clear();
        textDictionary = null;
    }

    #endregion
}

/// <summary>
/// クライアントで使用するテキストを管理するクラス
/// </summary>
public static class TextManager
{
    private static Dictionary<string, ClientText> ClientTextMasterDictionary = new Dictionary<string, ClientText>();

    public static string Get(string csvId_,string textKey_)
    {
        try
        {
            if (ClientTextMasterDictionary.ContainsKey(csvId_) && ClientTextMasterDictionary[csvId_].DictionaryHasBeenSet)
            {
                var clientText = ClientTextMasterDictionary[csvId_];
                return clientText.Get(textKey_);
            }
            else
            {
                //ClientText初期化
                var clientText = new ClientText(csvId_);
                if (clientText.DictionaryHasBeenSet)
                {
                    ClientTextMasterDictionary.AddSafely(csvId_,clientText);
                    return clientText.Get(textKey_);
                }
            }
        }
        catch (Exception e)
        {
            string log = string.Format("sceneKey:{0},valueKey:{1}のロード失敗", csvId_, textKey_);
            Debug.LogError(log + Environment.NewLine + e.StackTrace);
        }
        return "";
    }

    public static string Get(string textKey_)
    {
        //TODO:複数シーンが重なっている箇所もあるので、確実にキーとなるシーンを取得できるようにする
        return Get(SceneManager.GetActiveScene().name,textKey_);
    }

    /// <summary>
    /// シーンの破棄時に呼ぶ
    /// </summary>
    /// <param name="csvId_"></param>
    public static void Release(string csvId_)
    {
        if (!ClientTextMasterDictionary.ContainsKey(csvId_)) return;
        ClientTextMasterDictionary[csvId_].Release();
        ClientTextMasterDictionary.Remove(csvId_);
    }

    public static void Reload(string csvId_)
    {
        if (ClientTextMasterDictionary.ContainsKey(csvId_))
        {
            ClientTextMasterDictionary[csvId_].ReloadDictionary();
        }
        else
        {
            var clientText = new ClientText(csvId_);
            ClientTextMasterDictionary.AddSafely(csvId_,clientText);
        }
    }
}