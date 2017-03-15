using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ViewPanelBase
/// </summary>
public class ViewPanelBase : MonoBehaviour
{
    ///// <summary>
    ///// キャンバスツール
    ///// </summary>
    //[SerializeField]
    //private CanvasTools canvasTools;

    ///// <summary>
    ///// キャンバスツールのプロパティ
    ///// </summary>
    //protected CanvasTools CanvasTools
    //{
    //    get
    //    {
    //        if (canvasTools == null)
    //        {
    //            canvasTools = LoadCanvasTools();
    //        }

    //        return canvasTools;
    //    }
    //}

    ///// <summary>
    ///// リッチテキストユーティリティー
    ///// </summary>
    //protected RichTextUtility richTextUtility = new RichTextUtility();

    //#region MonoBehaviour
    ///// <summary>
    ///// OnDestroy
    ///// </summary>
    //protected virtual void OnDestroy()
    //{
    //    richTextUtility = null;
    //    canvasTools = null;
    //}
    //#endregion

    //#region static method
    ///// <summary>
    ///// キャンバスツールのロード
    ///// </summary>
    //protected static System.Func<CanvasTools> LoadCanvasTools = () =>
    //{
    //    GameObject instance = Resources.Load(Define.Path.Prefab.Canvas.Tools + "CanvasTools") as GameObject;
    //    CanvasTools tools = instance.GetComponent<CanvasTools>();
    //    return tools;
    //};
    ///// <summary>
    ///// ビューパネルの生成
    ///// </summary>
    //public static ViewPanelBase Create(Transform aTransform)
    //{
    //    GameObject instance = LoadCanvasTools().ViewPanel.Base.gameObject;
    //    GameObject result = Instantiate(instance) as GameObject;
    //    result.name = instance.name;

    //    RectTransform t = result.transform as RectTransform;
    //    t.localScale = Vector3.one;
    //    t.anchoredPosition = Vector2.zero;
    //    t.SetParent(aTransform, false);

    //    ViewPanelBase view = result.GetComponent<ViewPanelBase>();

    //    return view;
    //}
    //#endregion

    //#region public method
    ///// <summary>
    ///// ノードの生成
    ///// </summary>
    //protected virtual GameObject CreateNode(GameObject instance, float height = -16f)
    //{
    //    GameObject result = Instantiate(instance) as GameObject;
    //    result.name = instance.gameObject.name;

    //    RectTransform t = result.transform as RectTransform;

    //    t.localScale = Vector3.one;
    //    t.anchoredPosition = Vector2.up * height;

    //    t.SetParent(transform, false);

    //    return result;
    //}

    ///// <summary>
    ///// 標準テキストの生成
    ///// </summary>
    //protected virtual GameObject CreateStandardWord(System.Action<Text, RichTextUtility> action)
    //{
    //    GameObject result = CreateNode(CanvasTools.ViewPanelItem.NormalWord);

    //    if (action != null)
    //    {
    //        Text text = result.GetComponent<Text>();
    //        action(text, richTextUtility.Clear());
    //    }

    //    return result;
    //}

    ///// <summary>
    ///// リンク付きテキストの生成
    ///// </summary>
    //protected virtual GameObject CreateLinkedWord(System.Action<Text, RichTextUtility, Button> action)
    //{
    //    GameObject result = CreateNode(CanvasTools.ViewPanelItem.LinkedWord);

    //    if (action != null)
    //    {
    //        Text text = result.GetComponent<Text>();
    //        GameObject buttons = result.transform.Find("Button").gameObject;
    //        Button button = buttons.GetComponent<Button>();

    //        action(text, richTextUtility.Clear(), button);
    //    }

    //    return result;
    //}

    ///// <summary>
    ///// 画像付きテキストの生成
    ///// </summary>
    //protected virtual GameObject CreateImageWord(System.Action<Text, Text, RichTextUtility, Image> action)
    //{
    //    GameObject result = CreateNode(CanvasTools.ViewPanelItem.ImageWord);

    //    if (action != null)
    //    {
    //        Text text = result.GetComponent<Text>();

    //        GameObject second = result.transform.Find("Text").gameObject;
    //        Text secondText = second.GetComponent<Text>();

    //        GameObject images = result.transform.Find("Image").gameObject;
    //        Image image = images.GetComponent<Image>();

    //        action(text, secondText, richTextUtility.Clear(), image);
    //    }

    //    return result;
    //}

    ///// <summary>
    ///// アイコンの生成
    ///// </summary>
    //protected virtual GameObject CreateIcon(System.Action<Image> action)
    //{
    //    GameObject result = CreateNode(CanvasTools.ViewPanelItem.Icon);

    //    if (action != null)
    //    {
    //        Image image = result.GetComponent<Image>();
    //        action(image);
    //    }

    //    return result;
    //}
    //#endregion
}
