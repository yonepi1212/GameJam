using System.Text;

/*  RichTextUtility
    リッチテキストユーティリティー
*/
public class RichTextUtility
{
    #region Declaration
    // Unityマニュアルに記載されている色
    public static class HtmlColor
    {
        public const string Aqua = @"#00ffffff";
        public const string Black = @"#000000ff";
        public const string Blue = @"#0000ffff";
        public const string Brown = @"#a52a2aff";
        public const string Cyan = Aqua;
        public const string Darkblue = @"#0000a0ff";
        public const string Fuchsia = @"#ff00ffff";
        public const string Green = @"#008000ff";
        public const string Grey = @"#808080ff";
        public const string Lightblue = @"#add8e6ff";
        public const string Lime = @"#00ff00ff";
        public const string Magenta = Fuchsia;
        public const string Maroon = @"#800000ff";
        public const string Navy = @"#000080ff";
        public const string Olive = @"#808000ff";
        public const string Orange = @"#ffa500ff";
        public const string Purple = @"#800080ff";
        public const string Red = @"#ff0000ff";
        public const string Silver = @"#c0c0c0ff";
        public const string Teal = @"#008080ff";
        public const string White = @"#ffffffff";
        public const string Yellow = @"#ffff00ff";
    }
    // タグ
    private struct Tag
    {
        public const string BracketBegin = @"<";
        public const string BracketEnd = @">";
        public const string Bold = @"<b>";
        public const string BoldEnd = @"</b>";
        public const string Italic = @"<i>";
        public const string ItalicEnd = @"</i>";
        public const string SizeBegin = @"<size=";
        public const string SizeEnd = @"</size>";
        public const string ColorBegin = @"<color=";
        public const string ColorEnd = @"</color>";
        public const string ColorFormat = @"{0}#{1:X2}{2:X2}{3:X2}{4:X2}{5}";
    }
    // 全ての文字例保持用
    private StringBuilder text = new StringBuilder();
    // 装飾する文字用
    private StringBuilder decoration = new StringBuilder();
    #endregion

    // コンストラクター
    public RichTextUtility()
    {
        text.Capacity = 512;
        decoration.Capacity = 512;
    }
    // デストラクター
    ~RichTextUtility()
    {
        text = decoration = null;
    }

    #region Public Method
    // 次の文字例に移ります
    public RichTextUtility Next()
    {
        if (decoration.Length > 0)
        {
            text.Append(decoration);
            decoration.Remove(0, decoration.Length);
        }
        return this;
    }
    // 装飾する文字を追加します
    public RichTextUtility Decorate(string value_)
    {
        Next();
        decoration.Append((string.IsNullOrEmpty(value_)) ? string.Empty : value_);
        return this;
    }
    // 装飾する文字を追加します
    public RichTextUtility Decorate(int value_)
    {
        Next();
        decoration.Append(value_.ToString());
        return this;
    }
    // 装飾する文字を追加します
    public RichTextUtility Decorate(float value_)
    {
        Next();
        decoration.Append(value_.ToString());
        return this;
    }
    // 文字例にして追加します
    public RichTextUtility Add(string value_)
    {
        decoration.Append(value_.ToString());
        return this;
    }
    // 数値を文字例にして追加します
    public RichTextUtility Add(int value_)
    {
        decoration.Append(value_.ToString());
        return this;
    }
    // 数値を文字例にして追加します
    public RichTextUtility Add(float value_)
    {
        decoration.Append(value_.ToString());
        return this;
    }
    // 太字のテキストを描画します
    public RichTextUtility Bold()
    {
        System.Action<StringBuilder> task = (b) =>
        {
            b.Insert(0, Tag.Bold);
            b.Append(Tag.BoldEnd);
        };

        if (decoration.Length > 0)
        {
            task(decoration);
        }
        else
        {
            task(text);
        }

        return this;
    }
    // テキストをイタリックにレンダリング
    public RichTextUtility Itaric()
    {
        System.Action<StringBuilder> task = (b) =>
        {
            b.Insert(0, Tag.Italic);
            b.Append(Tag.ItalicEnd);
        };

        if (decoration.Length > 0)
        {
            task(decoration);
        }
        else
        {
            task(text);
        }

        return this;
    }
    // テキストサイズをパラメーター値にもとづいてピクセル単位でセット
    public RichTextUtility Size(int size_)
    {
        System.Action<StringBuilder> task = (b) =>
        {
            string s = GetSizeTag(size_);
            b.Insert(0, s);
            b.Append(Tag.SizeEnd);
        };

        if (decoration.Length > 0)
        {
            task(decoration);
        }
        else
        {
            task(text);
        }

        return this;
    }
    // 色を設定します
    // #rrggbbaa … 文字は色の、赤、緑、青およびアルファ（透明度）の値を表す 16 進数の対に対応します
    public RichTextUtility Color(string color_)
    {
        if (string.IsNullOrEmpty(color_))
        {
            return this;
        }

        System.Action<StringBuilder> task = (b) =>
        {
            string s = GetColorTag(color_);
            b.Insert(0, s);
            b.Append(Tag.ColorEnd);
        };

        if (decoration.Length > 0)
        {
            task(decoration);
        }
        else
        {
            task(text);
        }

        return this;
    }
    // テキストの色はパラメーター値に応じて設定されます。
    public RichTextUtility Color(UnityEngine.Color32 color_)
    {
        System.Action<StringBuilder> task = (b) =>
        {
            string s = string.Format(Tag.ColorFormat, Tag.ColorBegin, color_.r, color_.g, color_.b, color_.a, Tag.BracketEnd);
            b.Insert(0, s);
            b.Append(Tag.ColorEnd);
        };

        if (decoration.Length > 0)
        {
            task(decoration);
        }
        else
        {
            task(text);
        }

        return this;
    }
    // テキストの色はパラメーター値に応じて設定されます。
    public RichTextUtility Color(byte r_, byte g_, byte b_, byte a_ = 255)
    {
        System.Action<StringBuilder> task = (b) =>
        {
            string s = string.Format(Tag.ColorFormat, Tag.ColorBegin, r_, g_, b_, a_, Tag.BracketEnd);
            b.Insert(0, s);
            b.Append(Tag.ColorEnd);
        };

        if (decoration.Length > 0)
        {
            task(decoration);
        }
        else
        {
            task(text);
        }

        return this;
    }
    // 改行します
    public RichTextUtility CR()
    {
        Next();
        text.Append(Define.Newline);
        return this;
    }
    // すべての文字例を返します
    public string Build()
    {
        Next();
        return text.ToString();
    }
    // クリアー
    public RichTextUtility Clear()
    {
        text.Remove(0, text.Length);
        decoration.Remove(0, decoration.Length);
        return this;
    }
    #endregion

    #region Private Method
    // カラータグを返す
    private string GetColorTag(string color_)
    {
        return Tag.ColorBegin + color_ + Tag.BracketEnd;
    }
    // サイズタグを返す
    private string GetSizeTag(int size_)
    {
        return Tag.SizeBegin + size_.ToString() + Tag.BracketEnd;
    }
    #endregion
}