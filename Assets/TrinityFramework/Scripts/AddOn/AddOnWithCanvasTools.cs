using UnityEngine;

/*  AddOnWithCanvasTools

*/
public class AddOnWithCanvasTools : AddOnBase
{
    #region CanvasTools
    // キャンバスツール
    [SerializeField]
    private CanvasTools _canvasTools;
    // キャンバスツールのプロパティ
    protected CanvasTools CanvasTools
    {
        get
        {
            if (_canvasTools == null)
            {
                _canvasTools = LoadCanvasTools();
            }

            return _canvasTools;
        }
    }
    // キャンバスツールのロード
    protected static System.Func<CanvasTools> LoadCanvasTools = () =>
    {
        GameObject instance = Resources.Load(Define.Path.Prefab.Canvas.Tools + Define.Name.CanvasTools) as GameObject;
        CanvasTools tools = instance.GetComponent<CanvasTools>();
        return tools;
    };
    #endregion

    #region MonoBehaviour
    // OnDestroy
    protected override void OnDestroy()
    {
        _canvasTools = null;
        base.OnDestroy();
    }
    #endregion
}
