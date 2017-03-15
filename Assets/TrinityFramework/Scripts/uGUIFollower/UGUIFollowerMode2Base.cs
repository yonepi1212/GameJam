using UnityEngine;
using System.Collections;

public class UGUIFollowerMode2Base : MonoBehaviourExtend
{
    #region Declaration
    [Header("UGUIFollower")]
    // 追跡対象のゲームオブジェクト
    [SerializeField]
    public Transform targetObject;
    [Header("オフセット")]
    [SerializeField]
    public Vector3 offset;
    // 制限方法 
    public enum LimitKind
    {
        ParentRect,
        Radius,
        None
    }
    [Header("制限方法"), SerializeField]
    protected LimitKind limitKind;
    // オフセットをローカル基準にするか
    [SerializeField, Header("ローカル")]
    public bool isLocalOffset = true;
    // 動ける範囲の中心座標
    protected Vector2 centerPosition;
    // 動ける範囲の半径
    [SerializeField]
    protected float radius;
    // 親カンバス
    protected Canvas rootCanvas;
    // 親オブジェクトのRectTransform
    protected RectTransform parentRectTransform;

    // このスクリプトアタッチされているオブジェクトのRectTransform
    protected RectTransform _rectTransform;
    // nullチェックのためのRectTransformプロパティ
    protected RectTransform rectTransform
    {
        get
        {
            if (_rectTransform == null)
            {
                _rectTransform = GetComponent<RectTransform>();
            }
            if (_rectTransform == null)
            {
                Debug.LogError("uGUIコンポーネント(RectTransform)のあるオブジェクトにアタッチしてください。");
                return null;
            }
            return _rectTransform;
        }
    }

    public RectTransform _rangeObject;
    // nullチェックのためのRangeObjectプロパティ
    protected RectTransform rangeObject
    {
        get
        {
            if (_rangeObject == null)
            {
                _rangeObject = rectTransform.parent as RectTransform;
            }
            if (_rangeObject == null)
            {
                Debug.LogError("動ける範囲を親がありません");
                return null;
            }
            return _rangeObject;
        }
    }
    // canvas worldカメラ
    private Camera _uiCamera;
    protected Camera uiCamera
    {
        get
        {
            if (_uiCamera == null)
            {
                _uiCamera = rootCanvas.worldCamera;
            }
            return _uiCamera;

        }
    }

    // 使用中のカメラ
    protected virtual Camera currentCamera
    {
        get { return Camera.current; }
    }
    #endregion

    #region Monobehavior
    protected virtual void Start()
    {
        _rectTransform = gameObject.GetComponent<RectTransform>();

        parentRectTransform = rectTransform.parent as RectTransform;

        SetRootCanvas();

        // 画面の中央を保存
        centerPosition = new Vector2(Screen.width / 2, Screen.height / 2);

    }

    protected virtual void Update()
    {
        if (rectTransform != null && currentCamera != null)
        {
            UpdateUIPosition();
        }
    }
    #endregion

    #region Private Method
    // フォローするUIオブジェクトの位置を更新
    private void UpdateUIPosition()
    {
        //スクリーンポイントを取得
        var screenPosition = Vector3.zero;
        if (isLocalOffset)
        {
            var targetPosition = targetObject.transform.TransformPoint(offset);
            screenPosition = currentCamera.WorldToScreenPoint(targetPosition);
        }
        else
        {
            screenPosition = currentCamera.WorldToScreenPoint(targetObject.position + offset);
        }

        Vector2 localPos = Vector2.zero;

        switch (limitKind)
        {

            // 指定領域で移動範囲を制限する場合
            case LimitKind.ParentRect:
                RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, screenPosition, uiCamera, out localPos);
                localPos = ClampLocalPosition(localPos);
                break;

            // 円で移動範囲を制限する場合
            case LimitKind.Radius:
                centerPosition = new Vector2(Screen.width / 2, Screen.height / 2);
                var position = Vector2.zero;
                if (Vector2.Distance(screenPosition, centerPosition) < radius)
                {
                    position = screenPosition;
                }
                else
                {
                    position = GetFollowerPosition();
                }
                RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, position, uiCamera, out localPos);
                break;
            default:
                RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, screenPosition, uiCamera, out localPos);
                break;
        }
        rectTransform.localPosition = localPos;
    }
    // フォロー対象のオブジェクトが範囲外に出た場合、フォローするUIの座標を取得
    private Vector2 GetFollowerPosition()
    {
        var v = Vector2.zero;
        if (isLocalOffset)
        {
            var targetPosition = targetObject.transform.TransformPoint(offset);
            v = Camera.main.WorldToScreenPoint(targetPosition);
        }
        else
        {
            v = Camera.main.WorldToScreenPoint(targetObject.position + offset);
        }

        float dd = (v.y - centerPosition.y) / (v.x - centerPosition.x);
        float xx = radius / Mathf.Sqrt(1 + dd * dd);

        //float yy;
        float x;
        if (v.x > centerPosition.x)
        {
            x = centerPosition.x + xx;
        }
        else
        {
            x = centerPosition.x - xx;
        }

        float y = dd * x + centerPosition.y - dd * centerPosition.x;

        return new Vector2(x, y);
    }

    // 領域内に入るようにポジションを調整
    private Vector2 ClampLocalPosition(Vector2 localPosition_)
    {
        var rect = rangeObject.rect;
        var rectHarfWidth = rect.width / 2;
        var rectHarfHeight = rect.height / 2;
        var rect2 = rectTransform.rect;
        var rectHarfWidth2 = rect2.width;
        var rectHarfHeight2 = rect2.height;
        var pivot2 = rectTransform.pivot;
        var x = Mathf.Clamp(localPosition_.x, -rectHarfWidth + rectHarfWidth2 * pivot2.x, rectHarfWidth - rectHarfWidth2 * (1 - pivot2.x));
        var y = Mathf.Clamp(localPosition_.y, -rectHarfHeight + rectHarfHeight2 * pivot2.y, rectHarfHeight - rectHarfHeight2 * (1 - pivot2.y));
        return new Vector2(x, y);
    }

    // このオブジェクトが配置されているカンバスを取得
    private void SetRootCanvas()
    {
        var canvasArr = GetComponentsInParent<Canvas>();
        foreach (var canvas in canvasArr)
        {
            if (canvas.isRootCanvas)
            {
                rootCanvas = canvas;
            }
        }
    }
    #endregion
}
