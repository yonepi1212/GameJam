using UnityEngine;

/// <summary>
/// モノビヘイビアの拡張クラス
/// </summary>
public class MonoBehaviourExtend : MonoBehaviour
{
    private Transform _cachedTransform;
    private RectTransform _cachedRectTransform;

    /// <summary>
    /// Transformキャッシュ
    /// 通常のTransformはパフォーマンスが悪いため、特に理由がなければこちらを使用してください
    /// </summary>
    public Transform TransformCached
    {
        get { return _cachedTransform ?? (_cachedTransform = transform); }
    }

    /// <summary>
    /// RectTransformのキャッシュ
    /// uGUIのコンポーネントのみ使用してください
    /// </summary>
    public RectTransform RectTransformCached
    {
        get { return _cachedRectTransform ?? (_cachedRectTransform = GetComponent<RectTransform>()); }
    }
}