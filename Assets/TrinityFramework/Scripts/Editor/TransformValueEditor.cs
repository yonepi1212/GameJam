using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

/* TransformValueEditor.cs
	キャラクターの位置を保存、ロードします。
*/
public class TransformValueEditor : EditorWindow
{
    #region Declaration

    private int _toolbar;
    private string[] _toolbarList = { "ロード", "保存"};
    private bool _loadToggle;
    private bool _saveToggle;

    // ScriptableObjectのデータ
    private TransformValueMaster _transformValueMaster;

    // ロード時に使う変数
    private string _loadTransformName;
    private string[] _loadTransformNameList;
    private int _loadTransformNameListIndex;
    private List<TransformValueMaster.TransformInfo> _loadTransformList = new List<TransformValueMaster.TransformInfo>();
    private List<GameObject> _loadTargetObjList = new List<GameObject>();

    // 保存時に使う変数
    private string _saveTransformName;
    private List<string> _saveKeyList;
    private List<Transform> _saveTransformList;
    private List<GameObject> _saveTargetObjList;

    #endregion

    #region EditorWindow

    [MenuItem("Custom/TransformValue")]
    public static void Init()
    {
        var window = GetWindow<TransformValueEditor>();
        window.titleContent = new GUIContent("TransformValue");
    }

    void OnGUI()
    {

        // タブ切り替え
        _toolbar = GUILayout.Toolbar(_toolbar, _toolbarList);
        SetToggle(_toolbar);
        EditorGUILayout.Space();

        // ロード
        if (_loadToggle)
        {
            EditorGUILayout.BeginHorizontal(GUI.skin.label);

            // ScriptableObjectから配置名を取得
            if (_loadTransformNameList == null || _loadTransformNameList.Length <= 0)
            {
                EditorGUILayout.HelpBox("データがありません。", MessageType.Info);
                if (GUILayout.Button("リロード", GUILayout.Width(100)))
                {
                    ReloadTransformNameList();
                }
                return;
            }

            // 配置名のプルダウン表示
            _loadTransformNameListIndex = 
                EditorGUILayout.Popup("ロードする配置名", _loadTransformNameListIndex, _loadTransformNameList, GUILayout.Width(300));
            if (_loadTransformNameList.Length <= _loadTransformNameListIndex)
            {
                ReloadTransformNameList();
                return;
            }
            _loadTransformName = _loadTransformNameList[_loadTransformNameListIndex];


            if (GUILayout.Button("ロード", GUILayout.Width(100)))
            {
                OnClickLoadTransform(_loadTransformName);
            }
            if (GUILayout.Button("リロード", GUILayout.Width(100)))
            {
                ReloadTransformNameList();
                if (_loadTransformList != null) _loadTransformList.Clear();
                if (_loadTargetObjList != null) _loadTargetObjList.Clear();
            }

            EditorGUILayout.EndHorizontal();


            if (_loadTransformList != null && _loadTransformList.Count > 0)
            {
                // ロードした要素を表示
                for (int i = 0; i < _loadTransformList.Count; i++)
                {
                    var info = _loadTransformList[i];

                    EditorGUILayout.BeginHorizontal(GUI.skin.label);
                    EditorGUILayout.LabelField(info.Key);
                    EditorGUILayout.LabelField(GetPositionStr(info), GUILayout.Width(200), GUILayout.Height(50));
                    if (_loadTargetObjList != null)
                    {
                        _loadTargetObjList[i] = (GameObject)EditorGUILayout.ObjectField(_loadTargetObjList[i], typeof(GameObject), true);
                        if (GUILayout.Button("適用", GUILayout.Width(100)))
                        {
                            if (_loadTargetObjList[i] != null)
                            {
                                OnClickSetTransform(_loadTargetObjList[i], info);
                            }
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
        }
        // 保存
        else if (_saveToggle)
        {
            _saveTransformName = EditorGUILayout.TextField("保存する配置名　", _saveTransformName, GUILayout.Width(300));

            if (GUILayout.Button("要素追加", GUILayout.Width(200)))
            {
                if(_saveKeyList == null)
                {
                    _saveKeyList = new List<string>();
                    _saveTransformList = new List<Transform>();
                    _saveTargetObjList = new List<GameObject>();
                }
                _saveKeyList.Add("");
                _saveTransformList.Add(null);
                _saveTargetObjList.Add(null);
            }
            if (_saveKeyList != null && _saveKeyList.Count > 0)
            {

                // 要素を表示
                for (int i = 0; i < _saveKeyList.Count; i++)
                {
                    if (i == 0)
                    {
                        // ヘッダー表示
                        EditorGUILayout.BeginHorizontal(GUI.skin.label);
                        EditorGUILayout.LabelField("Key", GUILayout.Width(200));
                        EditorGUILayout.LabelField("Value", GUILayout.Width(200));
                        EditorGUILayout.EndHorizontal();
                    }

                    // 要素を表示
                    EditorGUILayout.BeginHorizontal(GUI.skin.label);
                    _saveKeyList[i] = EditorGUILayout.TextField(_saveKeyList[i], GUILayout.Width(200));
                    if (_saveTransformList[i] != null)
                    {
                        EditorGUILayout.LabelField(GetPositionStr(_saveTransformList[i]), GUILayout.Width(200), GUILayout.Height(50));
                    }
                    _saveTargetObjList[i] = (GameObject)EditorGUILayout.ObjectField(_saveTargetObjList[i], typeof(GameObject), true, GUILayout.Width(200));
                    if (_saveTargetObjList[i] != null)
                    {
                        _saveTransformList[i] = _saveTargetObjList[i].transform;
                    }
                    EditorGUILayout.EndHorizontal();
                }

                // 保存ボタン
                if (_saveTransformName.IsNullOrEmpty())
                {
                    EditorGUILayout.HelpBox("配置名が不正です。保存できません。", MessageType.Info);
                }
                else if (IsNullList(_saveKeyList))
                {
                    EditorGUILayout.HelpBox("Keyが不正です。保存できません。", MessageType.Info);
                }
                else if (IsNullList(_saveTransformList))
                {
                    EditorGUILayout.HelpBox("valueが不正です。保存できません。", MessageType.Info);
                }
                else
                {
                    if (GUILayout.Button("保存", GUILayout.Width(200)))
                    {
                        OnClickSaveTransform();
                    }
                }
            }

            if (GUILayout.Button("クリア", GUILayout.Width(200)))
            {
                ClearSaveInfo();
            }
        }
    }
    #endregion

    #region Public Method
    #endregion

    #region Private Method

    private void ClearSaveInfo()
    {
        _saveTransformName = "";
        _saveKeyList.Clear();
        if (_saveTransformList != null) _saveTransformList.Clear();
        if (_saveTargetObjList != null) _saveTargetObjList.Clear();
    }
    

    // ロードボタン押下時の処理　ScriptableObjectからロードする
    private void OnClickLoadTransform(string loadTransformName)
    {
        _transformValueMaster = Resources.Load("TransformValueMaster") as TransformValueMaster;
        if (_transformValueMaster == null) return;
        var transformInfo = _transformValueMaster.GetEnemyTransform(loadTransformName);
        if (transformInfo == null) return;
        _loadTransformList = transformInfo.TransformList;


        _loadTargetObjList = new List<GameObject>();

        for (int i = 0; i < _loadTransformList.Count; i++)
        {
            _loadTargetObjList.Add(null);
        }
    }

    // 保存ボタン押下時の処理　ScriptableObjectに保存する
    private void OnClickSaveTransform()
    {
        _transformValueMaster = Resources.Load("TransformValueMaster") as TransformValueMaster;
        if (_transformValueMaster == null) return;
        AssetDatabase.StartAssetEditing();
        for (int i = 0; i < _saveKeyList.Count; i++)
        {
            _transformValueMaster.AddEnemyTransform
                (_saveTransformName, _saveKeyList[i], _saveTransformList[i]);
        }
        AssetDatabase.StopAssetEditing();
        EditorUtility.SetDirty(_transformValueMaster);
    }

    // 適用ボタン押下時の処理　対象のオブジェクトにポジションと傾きを適用
    private void OnClickSetTransform(GameObject obj, TransformValueMaster.TransformInfo transform)
    {
        obj.transform.localPosition = transform.LocalPosition;
        obj.transform.localRotation = transform.LocalRotation;
        obj.transform.localScale = transform.LocalScale;
    }

    // ロード用の配置名をScriptableObjectから取得する
    private void ReloadTransformNameList()
    {
        _loadTransformNameList = null;
        _transformValueMaster = Resources.Load("TransformValueMaster") as TransformValueMaster;
        if (_transformValueMaster != null && _transformValueMaster.EnemyTransformList != null && _transformValueMaster.EnemyTransformList.Count > 0)
        {
            _loadTransformNameList = _transformValueMaster.EnemyTransformList.Select(t => t.Name).ToArray();
        }
    }


    // リスト内の全要素にnullがないかチェック
    private bool IsNullList(List<string> list)
    {
        if (list == null || list.Count <= 0)
        {
            return true;
        }
        for (int i = 0; i < list.Count; i++)
        {
            var value = list[i];
            if (value.IsNullOrEmpty())
            {
                return true;
            }

            for (int j = i+1; j < list.Count; j++)
            {
                if (list[j].Equals(value))
                {
                    return true;
                }
            }
        }
        return false;
    }

    // リスト内の全要素にnullがないかチェック
    private bool IsNullList(IList<Transform> list)
    {
        if (list == null || list.Count <= 0)
        {
            return true;
        }
        return list.Any(t => t == null);
    }

    // Transformのポジションをストリングにして返す
    private string GetPositionStr(TransformValueMaster.TransformInfo transform)
    {
        string ret = "LocalPosition X:" + transform.LocalPosition.x + " y:" + transform.LocalPosition.y + " z:" + transform.LocalPosition.z + "\n" +
                     "LocalRotation X:" + transform.LocalRotation.x + " y:" + transform.LocalRotation.y + " z:" + transform.LocalRotation.z + "\n" +
                     "LocalScale    X:" + transform.LocalScale.x + " y:" + transform.LocalScale.y + " z:" + transform.LocalScale.z + "";
        return ret;
    }
    private string GetPositionStr(Transform transform)
    {
        string ret = "LocalPosition X:" + transform.localPosition.x + " y:" + transform.localPosition.y + " z:" + transform.localPosition.z + "\n" +
                     "LocalRotation X:" + transform.localRotation.x + " y:" + transform.localRotation.y + " z:" + transform.localRotation.z + "\n" +
                     "LocalScale    X:" + transform.localScale.x + " y:" + transform.localScale.y + " z:" + transform.localScale.z + "";
        return ret;
    }


    // タブの設定切り替え
    private void SetToggle(int selectIndex)
    {
        _loadToggle = false;
        _saveToggle = false;
        switch (selectIndex)
        {
            case 0:
                _loadToggle = true;
                break;
            case 1:
                _saveToggle = true;
                break;
        }
    }
    #endregion
}
