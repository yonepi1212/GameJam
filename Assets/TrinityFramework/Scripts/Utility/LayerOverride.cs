using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;

/// <summary>
/// エディタ拡張
/// </summary>
[CustomEditor(typeof(LayerOverride), true)]
public class LayerOverrideInspector : Editor
{
    /// <summary>
    /// ソーティングレイヤ名一覧を取得する
    /// </summary>
    /// <returns>レイヤ名一覧</returns>
    public string[] GetSortingLayerNames()
    {
        Type internalEditorUtilityType = typeof(InternalEditorUtility);
        PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
        return (string[])sortingLayersProperty.GetValue(null, new object[0]);
    }

    /// <summary>
    /// インスペクタ拡張
    /// </summary>
    public override void OnInspectorGUI()
    {
        var layerOverride = target as LayerOverride;

        // ソーティングレイヤー
        {
            var layerNames = GetSortingLayerNames();
            var currentIndex = Array.FindIndex(layerNames, a => a == layerOverride.SortingLayerName);
            var index = EditorGUILayout.Popup("SortingLayer", currentIndex, layerNames);
            var selectedName = layerNames[index];
            if (layerOverride.SortingLayerName != selectedName)
            {
                layerOverride.SortingLayerName = selectedName;
            }
        }

        // レイヤオーダー
        {
            layerOverride.SortingOrder = EditorGUILayout.IntField("SortingOrder", layerOverride.SortingOrder);
        }
    }
}

#endif

/// <summary>
/// Canvas ParticleSystem以外のレンダラに対して
/// ソーティングレイヤを設定するためのコンポーネント
/// （CanvasやParticleSystemはソーティングレイヤを選択するGUIが存在するため不要）
/// </summary>
[ExecuteInEditMode]
public class LayerOverride : MonoBehaviour
{
    #region Private Declaration

    [SerializeField] private int _sortingLayerId;
    [SerializeField] private int _sortingOrder;

    private List<Renderer> _renderers; 

    #endregion

    #region Public Declaration

    /// <summary>
    /// レンダラ
    /// </summary>
    public List<Renderer> Renderers
    {
        get
        {
            if (_renderers.IsNullOrEmpty())
                _renderers = new List<Renderer>(GetComponentsInChildren<Renderer>(true));
            return _renderers;
        }
    }

    /// <summary>
    /// ソーティングレイヤID
    /// </summary>
    public int SortingLayerId
    {
        get { return _sortingLayerId; }
        set
        {
            _sortingLayerId = value;
            Apply();
        }
    }

    /// <summary>
    /// ソーティングレイヤ名
    /// </summary>
    public string SortingLayerName
    {
        get
        {
            return SortingLayer.IDToName(_sortingLayerId);
        }
        set
        {
            int id = SortingLayer.NameToID(value);
            if (_sortingLayerId != id)
            {
                _sortingLayerId = id;
                Apply();
            }
        }
    }

    /// <summary>
    /// ソーティングオーダー
    /// 同レイヤ内の描画順番
    /// </summary>
    public int SortingOrder
    {
        get { return _sortingOrder; }
        set
        {
            if (_sortingOrder != value)
            {
                _sortingOrder = value;
                Apply();
            }
        }
    }

    #endregion

    #region MonoBehaviour

    void Start()
    {
        if (_renderers.IsNullOrEmpty())
            _renderers = new List<Renderer>(GetComponentsInChildren<Renderer>(true));
    }

    void OnDestroy()
    {
        _renderers = null;
    }

    #endregion

    /// <summary>
    /// 設定を適用します.
    /// </summary>
    [ContextMenu("Apply")]
    public void Apply()
    {
        foreach (var ren in _renderers)
        {
            ren.sortingLayerID = _sortingLayerId;
            ren.sortingOrder = _sortingOrder;
        }
    }

    /// <summary>
    /// Rendererの再取得を実行します.
    /// Rendererの構成が変更されたタイミングで呼び出してください.
    /// </summary>
    [ContextMenu("Refresh")]
    public void Refresh()
    {
        _renderers = new List<Renderer>(GetComponentsInChildren<Renderer>(true));
        Apply();
    }
}