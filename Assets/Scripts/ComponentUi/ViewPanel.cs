using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// ViewPanel
/// </summary>
public class ViewPanel : ViewPanelBase
{

    //#region declaration

    ///// <summary>
    ///// パラメータークラス
    ///// </summary>
    //[System.Serializable]
    //public class PanelParamater
    //{
    //    /// <summary>
    //    /// マージン
    //    /// </summary>
    //    public float Margin = 4.0f;
    //    /// <summary>
    //    /// Width
    //    /// </summary>
    //    public float Width = 418.0f;
    //    /// <summary>
    //    /// Height
    //    /// </summary>
    //    public float Height = 256.0f;
    //    /// <summary>
    //    /// 親子付けされるサブビューのインスタンス
    //    /// </summary>
    //    public ViewPanel Parent = null;
    //    /// <summary>
    //    /// ノードが追加されたかの判定
    //    /// </summary>
    //    public bool IsChange = false;
    //    /// <summary>
    //    /// ストレッチするか否か
    //    /// </summary>
    //    public bool IsStretch = false;
    //    /// <summary>
    //    /// 折りたたみ判定
    //    /// </summary>
    //    public bool IsFolding = false;

    //    /// <summary>
    //    /// パネルのサイズ
    //    /// </summary>
    //    public void SetSize(float width, float height)
    //    {
    //        Width = width;
    //        Height = height;
    //    }
    //}

    ///// <summary>
    ///// パラメーター
    ///// </summary>
    //public PanelParamater Panel = new PanelParamater();

    ///// <summary>
    ///// コンテナクラス
    ///// </summary>
    //[System.Serializable]
    //public class Containers
    //{
    //    /// <summary>
    //    /// アンカー
    //    /// </summary>
    //    public GameObject Anchor;
    //    /// <summary>
    //    /// リストビューのインスタンス
    //    /// </summary>
    //    public GameObject Background;
    //    /// <summary>
    //    /// リストビューのインスタンス
    //    /// </summary>
    //    public GameObject Nodes;
    //}
    ///// <summary>
    ///// コンテナ
    ///// </summary>
    //public Containers Container;

    ///// <summary>
    ///// ビューの列と行を保持するクラス
    ///// </summary>
    //[System.Serializable]
    //public class Row
    //{
    //    //public bool AlignLeft = true;
    //    public float Height = 0f;
    //    public float Padding = 0f;
    //    public List<GameObject> Columns = new List<GameObject>();
    //    /// <summary>
    //    /// カラムのゲームオブジェクトの表示非表示を切り替える
    //    /// </summary>
    //    public void SetColumnsActive(bool flag)
    //    {
    //        Columns.ForEach(g => { g.SetActive(flag); });
    //    }
    //    /// <summary>
    //    /// カラムの高さを取得する
    //    /// </summary>
    //    public float GetColumnsHeight()
    //    {
    //        float h = 0;
    //        Columns.ForEach(g =>
    //        {
    //            g.SetActive(true);

    //            RectTransform t = g.transform as RectTransform;

    //            if (h < t.sizeDelta.y)
    //            {
    //                h = t.sizeDelta.y;
    //            }
    //        });
    //        return h;
    //    }
    //}

    ///// <summary>
    ///// ビューの列と行を保持する
    ///// </summary>
    //public List<Row> Rows = new List<Row>();

    ///// <summary>
    ///// サブビュー保持用
    ///// </summary>
    //public List<ViewPanel> SubViews = new List<ViewPanel>();

    //#endregion

    //#region MonoBehaviour
    ///// <summary>
    ///// OnDestroy
    ///// </summary>
    //protected override void OnDestroy()
    //{
    //    Panel = null;
    //    Container = null;
    //    Rows = null;
    //    SubViews = null;
    //    base.OnDestroy();
    //}
    ///// <summary>
    ///// Update
    ///// </summary>
    //protected virtual void Update()
    //{
    //    System.Func<List<ViewPanel>, bool> isSubviewChange =  (sub)=>
    //    {
    //        bool flag = true;

    //        foreach (ViewPanel item in sub)
    //        {
    //            if (item.Panel.IsChange != true)
    //            {
    //                flag = false;
    //                break;
    //            }
    //        }

    //        return flag;
    //    };

    //    if (Panel.IsChange)
    //    {
    //        if (isSubviewChange(SubViews))
    //        {
    //            ReBuild();
    //        }
    //    }
    //}
    //#endregion

    //#region static method
    ///// <summary>
    ///// ビューパネルの生成
    ///// </summary>
    //public static new ViewPanel Create(Transform aTransform)
    //{
    //    GameObject instance = LoadCanvasTools().ViewPanel.Standard.gameObject;
    //    GameObject result = Instantiate(instance) as GameObject;
    //    result.name = instance.name;

    //    RectTransform t = result.transform as RectTransform;
    //    t.localScale = Vector3.one;
    //    t.anchoredPosition = Vector2.zero;
    //    t.SetParent(aTransform, false);

    //    ViewPanel view = result.GetComponent<ViewPanel>();

    //    return view;
    //}
    //#endregion

    //#region public method
    ///// <summary>
    ///// Initialization
    ///// </summary>
    //public virtual ViewPanel Initialization(string name, Color aColor, bool isStretch = false, float padding = 0f)
    //{
    //    if (!string.IsNullOrEmpty(name))
    //    {
    //        gameObject.name = name;
    //    }
    //    SetBackgroundColor(aColor);
    //    Stretch(isStretch, padding);

    //    return this;
    //}

    ///// <summary>
    ///// ストレッチの設定
    ///// </summary>
    //public virtual void Stretch(bool isStretch = true, float padding = 0f)
    //{
    //    RectTransform t = transform as RectTransform;

    //    if (isStretch)
    //    {
    //        t.anchorMin = Vector2.up;
    //        t.anchorMax = Vector2.one;
    //        t.anchoredPosition = Vector2.right * padding;
    //        t.sizeDelta = new Vector2(-padding * 2, Panel.Height + Panel.Margin * 2);
    //    }
    //    else
    //    {
    //        t.anchorMin = Vector2.up;
    //        t.anchorMax = Vector2.up;
    //        t.anchoredPosition = Vector2.zero;
    //        t.sizeDelta = new Vector2(Panel.Width, Panel.Height);
    //    }

    //    Panel.IsStretch = isStretch;
    //}
    ///// <summary>
    ///// RectTransformを構築する
    ///// </summary>
    //public virtual void BuildTransform()
    //{
    //    RectTransform t = transform as RectTransform;
    //    Panel.Height = Panel.Margin * 2;

    //    t.anchoredPosition = new Vector2(t.anchoredPosition.x, t.anchoredPosition.y + t.sizeDelta.y);

    //    if (Panel.IsFolding)
    //    {
    //        Panel.Height = Collapse(Panel.Height);
    //    }
    //    else
    //    {
    //        Panel.Height = Spread(Panel.Height);
    //    }

    //    t.anchoredPosition = new Vector2(t.anchoredPosition.x, t.anchoredPosition.y - Panel.Height);
    //    t.sizeDelta = new Vector2(t.sizeDelta.x, Panel.Height);
    //}
    ///// <summary>
    ///// ノードの再構築
    ///// </summary>
    //public virtual void ReBuild()
    //{
    //    //Log.D(gameObject.name);
    //    float height = -Panel.Margin;
    //    float width = Panel.Margin;

    //    foreach (Row row in Rows)
    //    {
    //        float x = 0, y = 0;

    //        row.Columns.ForEach(g =>
    //        {
    //            RectTransform t = g.transform as RectTransform;
    //            y = t.sizeDelta.y;
    //            t.anchoredPosition = new Vector2(x, -y + height);
    //            x += t.sizeDelta.x;
    //        });

    //        if (x > width)
    //        {
    //            width = x + Panel.Margin;
    //        }

    //        height -= y + row.Padding;
    //    }

    //    if (!Panel.IsStretch)
    //    {
    //        RectTransform t = transform as RectTransform;
    //        Panel.Width = width + Panel.Margin * 2;
    //        t.sizeDelta = new Vector2(Panel.Width, Panel.Height);
    //    }

    //    if (width != Panel.Margin)
    //    {
    //        Panel.IsChange = false;

    //        if (Panel.Parent != null)
    //        {
    //            Panel.Parent.ReBuild();
    //        }
    //    }
    //}
    ///// <summary>
    ///// 折りたたみの切り替え
    ///// </summary>
    //public void ChangeFolding()
    //{
    //    Panel.IsFolding = !Panel.IsFolding;
    //    BuildTransform();
    //}

    ///// <summary>
    ///// バックグラウンドカラーを変更する
    ///// </summary>
    //public virtual void SetBackgroundColor(Color32 aColor)
    //{
    //    Image img = Container.Background.GetComponent<Image>();
    //    img.color = aColor;
    //}
    //#endregion

    //#region private method
    ///// <summary>
    ///// Viewを折りたたむ
    ///// </summary>
    //protected virtual float Collapse(float height, int index = 0)
    //{
    //    for (int i = 0; i < Rows.Count; i++)
    //    {
    //        Row row = Rows[i];

    //        if (i <= index)
    //        {
    //            height += row.GetColumnsHeight() + row.Padding;
    //        }
    //        else
    //        {
    //            row.SetColumnsActive(false);
    //        }
    //    }

    //    return height;
    //}
    ///// <summary>
    ///// Viewを広げる
    ///// </summary>
    //protected virtual float Spread(float height)
    //{
    //    //Log.D(gameObject.name);
    //    Rows.ForEach(row =>
    //    {
    //        height += row.GetColumnsHeight() + row.Padding;
    //    });

    //    return height;
    //}
    //#endregion

    //#region columns
    ///// <summary>
    ///// 空列を追加する
    ///// </summary>
    //public void NewColumn(float padding = 8.0f)
    //{
    //    Row row = new Row() { Padding = padding };
    //    Rows.Add(row);
    //}

    ///// <summary>
    ///// ノードを追加する
    ///// </summary>
    //public void NewColumn(GameObject node, float padding = 8.0f)
    //{
    //    Row row = new Row() { Padding = padding };
    //    Rows.Add(row);

    //    if (node != null)
    //    {
    //        AddColumn(node);
    //    }
    //}
    ///// <summary>
    ///// ノードを追加する
    ///// </summary>
    //public void AddColumn(GameObject node)
    //{
    //    if (Rows.Count == 0)
    //    {
    //        NewColumn();
    //    }

    //    Row row = Rows[Rows.Count - 1];
    //    row.Columns.Add(node);

    //    RectTransform rects = node.transform as RectTransform;
    //    rects.SetParent(Container.Nodes.transform, false);

    //    if (rects.sizeDelta.y > row.Height)
    //    {
    //        row.Height = rects.sizeDelta.y;
    //    }

    //    Panel.IsChange = true;
    //}

    ///// <summary>
    ///// ViewPanelを追加する
    ///// </summary>
    //public void NewColumn(ViewPanel view, float padding = 8.0f)
    //{
    //    Row row = new Row() { Padding = padding };
    //    Rows.Add(row);

    //    if (view != null)
    //    {
    //        AddColumn(view);
    //    }
    //}
    ///// <summary>
    ///// ViewPanelを追加する
    ///// </summary>
    //public void AddColumn(ViewPanel view)
    //{
    //    AddColumn(view.gameObject);
    //    view.Panel.Parent = this;
    //    SubViews.Add(view);
    //}

    ///// <summary>
    ///// 標準テキストを追加する
    ///// </summary>
    //public void NewColumn(System.Action<Text, RichTextUtility> action, float padding = 8.0f)
    //{
    //    Row row = new Row() { Padding = padding };
    //    Rows.Add(row);
    //    AddColumn(action);
    //}
    ///// <summary>
    ///// 標準テキストを追加する
    ///// </summary>
    //public void AddColumn(System.Action<Text, RichTextUtility> action)
    //{
    //    GameObject node = CreateStandardWord(action);

    //    ContentSizeFitter contentSizeFitter = node.GetComponent<ContentSizeFitter>();

    //    if (contentSizeFitter != null)
    //    {
    //        contentSizeFitter.SetLayoutHorizontal();
    //    }

    //    AddColumn(node);
    //}
    ///// <summary>
    /////  verticalFitの標準テキストを追加する
    ///// </summary>
    //public void AddColumn(System.Action<Text, RichTextUtility> action, bool verticalFit)
    //{
    //    GameObject node = CreateStandardWord(action);

    //    ContentSizeFitter contentSizeFitter = node.GetComponent<ContentSizeFitter>();

    //    if (contentSizeFitter != null)
    //    {
    //        contentSizeFitter.SetLayoutHorizontal();
    //        if (verticalFit)
    //        {
    //            contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
    //            contentSizeFitter.SetLayoutVertical();
    //        }
    //    }

    //    AddColumn(node);
    //}

    ///// <summary>
    ///// リンク付きテキストを追加する
    ///// </summary>
    //public void NewColumn(System.Action<Text, RichTextUtility, Button> action, float padding = 8.0f)
    //{
    //    Row row = new Row() { Padding = padding };
    //    Rows.Add(row);
    //    AddColumn(action);
    //}
    ///// <summary>
    ///// リンク付きテキストを追加する
    ///// </summary>
    //public void AddColumn(System.Action<Text, RichTextUtility, Button> action)
    //{
    //    GameObject node = CreateLinkedWord(action);

    //    ContentSizeFitter contentSizeFitter = node.GetComponent<ContentSizeFitter>();

    //    if (contentSizeFitter != null)
    //    {
    //        contentSizeFitter.SetLayoutHorizontal();
    //    }

    //    AddColumn(node);
    //}

    ///// <summary>
    ///// 画像付きテキストを追加する
    ///// </summary>
    //public void NewColumn(System.Action<Text, Text, RichTextUtility, Image> action, float padding = 8.0f)
    //{
    //    Row row = new Row() { Padding = padding };
    //    Rows.Add(row);
    //    AddColumn(action);
    //}
    ///// <summary>
    ///// 画像付きテキストを追加する
    ///// </summary>
    //public void AddColumn(System.Action<Text, Text, RichTextUtility, Image> action)
    //{
    //    GameObject node = CreateImageWord(action);

    //    ContentSizeFitter contentSizeFitter = node.GetComponent<ContentSizeFitter>();

    //    if (contentSizeFitter != null)
    //    {
    //        contentSizeFitter.SetLayoutHorizontal();
    //    }

    //    AddColumn(node);
    //}

    ///// <summary>
    ///// アイコンを追加する
    ///// </summary>
    //public void NewColumn(System.Action<Image> action, float padding = 8.0f)
    //{
    //    Row row = new Row() { Padding = padding };
    //    Rows.Add(row);
    //    AddColumn(action);
    //}
    ///// <summary>
    ///// アイコンを追加する
    ///// </summary>
    //public void AddColumn(System.Action<Image> action)
    //{
    //    AddColumn(CreateIcon(action));
    //}
    //#endregion
}

