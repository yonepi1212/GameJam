// ---------------------------------------------------------
// HierarchyWindowExtention.cs
// 
// ヒエラルキービューの拡張クラス
//
// Author: Haruki Tachihara
// ---------------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace Griphone.EditorExtension
{
    /// <summary>
    /// ヒエラルキービューの拡張クラス
    /// </summary>
    [InitializeOnLoad]
    public static class HierarchyWindowExtention
    {
        #region Settings Declaration

        /// <summary>
        /// SortingLayer情報の表示フラグ
        /// </summary>
        private static bool DispSortingLayer = true;

        #endregion

        #region Private Declaration

        private static int _updateThrottle;
        private static EditorWindow _hierarchyWindow = null;

        #endregion

        #region Public Declaration

        /// <summary>
        /// ヒエラルキーウインドウプロパティ
        /// </summary>
        public static EditorWindow HierarchyWindow
        {
            get
            {
                _hierarchyWindow = _hierarchyWindow
                                   ?? GetWindowByName("UnityEditor.HierarchyWindow")
                                   ?? GetWindowByName("UnityEditor.SceneHierarchyWindow");

                return (_hierarchyWindow);
            }
        }

        #endregion

        #region Editor Events

        /// <summary>
        /// ウインドウ名からウインドウインスタンスを取得する
        /// </summary>
        /// <returns></returns>
        public static EditorWindow GetWindowByName(string name_)
        {
            UnityEngine.Object[] objectList = Resources.FindObjectsOfTypeAll(typeof (EditorWindow));

            foreach (UnityEngine.Object obj in objectList)
            {
                if (obj.GetType().ToString() == name_)
                    return ((EditorWindow) obj);
            }

            return (null);
        }

        /// <summary>
        /// コンストラクタ
        /// InitializeOnLoad属性をつけているのでエディタ起動時に呼ばれる
        /// </summary>
        static HierarchyWindowExtention()
        {
            // 設定の読み込み
            ReadSettings();

            // ヒエラルキーウインドウ描画イベントを登録
            EditorApplication.hierarchyWindowItemOnGUI += Draw;
            EditorApplication.update += Update;
        }

        /// <summary>
        /// ヒエラルキー描画
        /// </summary>
        /// <param name="instanceId">GameObjectのインスタンスID</param>
        /// <param name="selectionRect">ヒエラルキーウインドウ上の表示範囲</param>
        private static void Draw(int instanceId_, Rect selectionRect_)
        {
            var gameObject = EditorUtility.InstanceIDToObject(instanceId_) as GameObject;

            DrawSortingLayerInfo(gameObject, selectionRect_);
        }

        /// <summary>
        /// 更新
        /// </summary>
        private static void Update()
        {
			if (DispSortingLayer == false) return;
            ++_updateThrottle;
            if (_updateThrottle > 20)
            {
                HierarchyWindow.Repaint();
                _updateThrottle = 0;
            }
        }

        /// <summary>
        /// SortingLayerに関する情報を表示する
        /// </summary>
        static void DrawSortingLayerInfo(GameObject gameObject_, Rect selectionRect_)
        {
            if (DispSortingLayer == false) return;

            if (gameObject_ != null)
            {
                var canvas = gameObject_.GetComponent<Canvas>();
                var renderer = gameObject_.GetComponent<Renderer>();

                string layerName = "";
                int sortingOrder = 0;

                if (canvas != null)
                {
                    layerName = canvas.sortingLayerName;
                    sortingOrder = canvas.sortingOrder;
                }
                else if (renderer != null)
                {
                    layerName = renderer.sortingLayerName;
                    sortingOrder = renderer.sortingOrder;
                }

                if (!string.IsNullOrEmpty(layerName))
                {
                    Rect rect = selectionRect_;
                    var dispString = string.Format("{0} : {1}", layerName, sortingOrder);
                    var size = GUI.skin.label.CalcSize(new GUIContent() {text = dispString});
                    rect.x = selectionRect_.x + selectionRect_.width - size.x;
                    rect.y--;
                    rect.width = size.x;
                    rect.height = 16;

                    // SortingLayer表示
                    var col = GUI.color;
                    GUI.color = new Color(0f, 0.7f, 0.7f, 1f);
                    GUI.Label(rect, dispString);
                    GUI.color = col;
                }
            }
        }

        #endregion

        #region Setting Method

        /// <summary>
        /// Perferenceに設定を表示
        /// </summary>
        [PreferenceItem("Hierarchy")]
        public static void DrawPrefs()
        {
            DispSortingLayer = EditorGUILayout.Toggle("Show SortingLayer", DispSortingLayer);

            if (GUI.changed)
            {
                SaveSettings();
            }
        }

        /// <summary>
        /// 設定の読み込み
        /// </summary>
        private static void ReadSettings()
        {
            DispSortingLayer = EditorPrefs.GetBool("GriphoneHierarchyWindow_DispSortingLayer", true);
        }

        /// <summary>
        /// 設定の保存
        /// </summary>
        private static void SaveSettings()
        {
            EditorPrefs.SetBool("GriphoneHierarchyWindow_DispSortingLayer", DispSortingLayer);
        }

        #endregion
    }
}
