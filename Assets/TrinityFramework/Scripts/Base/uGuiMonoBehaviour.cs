using UnityEngine;

/// <summary>
/// uGUIの汎用プロパティー
/// </summary>
public class UGuiMonoBehaviour : MonoBehaviour
{
    #region Transform
    /// <summary>
    /// Transform
    /// </summary>
    [SerializeField, HideInInspector]
    private Transform _transform;
    /// <summary>
    /// Transform
    /// </summary>
    public Transform Transform
    {
        get
        {
            if (_transform == null)
            {
                _transform = gameObject.transform;
            }

            return _transform;
        }
    }
    #endregion

    #region RectTransform
    /// <summary>
    /// RectTransform
    /// </summary>
    [SerializeField, HideInInspector]
    private RectTransform _rectTransform;
    /// <summary>
    /// RectTransform
    /// </summary>
    public RectTransform RectTransform
    {
        get
        {
            if (_rectTransform == null)
            {
                _rectTransform = Transform as RectTransform;
            }

            return _rectTransform;
        }
    }
    /// <summary>
    /// AnchoredPosition
    /// </summary>
    public Vector2 AnchoredPosition
    {
        get
        {
            return RectTransform.anchoredPosition;
        }
    }
    /// <summary>
    /// SizeDelta
    /// </summary>
    public Vector2 SizeDelta
    {
        get
        {
            return RectTransform.sizeDelta;
        }
    }
    #endregion

    #region MonoBehaviour
    /// <summary>
    /// OnDestroy
    /// </summary>
    protected virtual void OnDestroy()
    {
        _rectTransform = null;
        _transform = null;
    }
    #endregion

    #region method
    /// <summary>
    /// RectTransformの位置とスケールを初期化する
    /// </summary>
    /// <param name="transform_">Parentしたいtransform_</param>
    /// <param name="worldPositionStays">worldPositionStays</param>
    /// <returns></returns>
    public RectTransform Beginning(Transform transform_ = null, bool worldPositionStays = false)
    {
        if (transform_ != null)
        {
            RectTransform.SetParent(transform_, worldPositionStays);
        }
        RectTransform.localScale = Vector3.one;
        RectTransform.anchoredPosition = Vector2.zero;
        return _rectTransform;
    }
    #endregion
}
