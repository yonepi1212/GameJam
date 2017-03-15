using UnityEditor;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.Events;

/* TrinityTimelineToolInspector.cs
    TimeLinePlayerを設定するためのカスタムエディタ拡張
*/
[CustomEditor((typeof(TrinityTimelineTool)))]
public class TrinityTimelineToolInspector : Editor
{
    #region Declaration
    private TrinityTimelineTool _timeLine;
    #endregion

    #region Editor
    private void OnEnable()
    {
        _timeLine = target as TrinityTimelineTool;
    }

    public override void OnInspectorGUI()
    {
        if(_timeLine == null)
        {
            _timeLine = target as TrinityTimelineTool;
        }

        if (_timeLine.Events.Count > 0)
        {
            AddTimeInfoField();
            AddTimeLine();
            AddEventField();

            ChangeCurrentTimeWithKey();
            FireEvent();
        }

        AddEventButton();
    }

    #endregion

    #region Private Method

    private void AddEventButton()
    {
        if (GUILayout.Button("追加"))
        {
            var trinityEvent = _timeLine.AddEvent();
            UnityEventTools.AddVoidPersistentListener(trinityEvent.FireEvent, _timeLine.Pause);
            trinityEvent.FireEvent.SetPersistentListenerState(0, UnityEventCallState.EditorAndRuntime);
        }
    }

    private void AddTimeInfoField()
    {
        _timeLine.TimeLength = EditorGUILayout.FloatField("再生時間", _timeLine.TimeLength);
        _timeLine.CurrentTime = EditorGUILayout.FloatField("現在時間", _timeLine.CurrentTime);
    }

    private void AddTimeLine()
    {
        var timeLineArea = GUILayoutUtility.GetRect(Screen.width, 40f);
        _timeLine.CurrentTime = GUI.HorizontalSlider(timeLineArea, _timeLine.CurrentTime, 0.0F, _timeLine.TimeLength, "box", "box");
        GUILayout.Space(5);
        // イベントの位置に線を引く
        _timeLine.Events.ForEach((trinityEvent) =>
        {
            var ratio = trinityEvent.FireTime / _timeLine.TimeLength;
            var x = timeLineArea.x + timeLineArea.width * ratio;
            Handles.color = trinityEvent.Color;
            Handles.DrawLine(
               new Vector2(x, timeLineArea.y),
               new Vector2(x, timeLineArea.y + 45));
        });

        var buttonArea = GUILayoutUtility.GetRect(Screen.width, 20f);

        var button = GUI.Button(new Rect(buttonArea.x, buttonArea.y, buttonArea.width / 2, buttonArea.height), "Play");
        if (button)
        {
            _timeLine.Play();
        }

        var stopButton = GUI.Button(new Rect(buttonArea.x + buttonArea.width / 2, buttonArea.y, buttonArea.width / 2, buttonArea.height), "Stop");
        if (stopButton)
        {
            _timeLine.Stop();
        }
    }

    private void AddEventField()
    {
        _timeLine.Events.ForEach((trinityEvent) =>
        {
            var guiStyle = new GUIStyle
            {
                normal = new GUIStyleState()
                {
                    textColor = trinityEvent.Color
                }
            };
            EditorGUILayout.LabelField("イベント",guiStyle);
            var fireTime = EditorGUILayout.FloatField("発火タイミング", trinityEvent.FireTime, guiStyle);
            fireTime = Mathf.Clamp(fireTime, 0f, _timeLine.TimeLength);
            trinityEvent.FireTime = fireTime;
            // TrinityEventはScriptableObjectなので、Serialized出来る
            // ※UnityEventのフィールドを出すためにSerializeしている
            var serializedTrinityEvent = new SerializedObject(trinityEvent);
            SerializedProperty trinityEventProperty = serializedTrinityEvent.FindProperty("FireEvent");
            EditorGUILayout.PropertyField(trinityEventProperty);
            serializedTrinityEvent.ApplyModifiedProperties();

            if (GUILayout.Button("削除"))
            {
                _timeLine.Events.Remove(trinityEvent);
                DestroyImmediate(trinityEvent);
            }
        });
    }

    private void FireEvent()
    {
        // エディタ拡張上でUniRxが使えないので、無理やり再現
        if (_timeLine.IsPlaying && _timeLine.CurrentTime <= _timeLine.TimeLength)
        {
            _timeLine.FireEvent();
        }
    }

    private void ChangeCurrentTimeWithKey()
    {
        if (Event.current.type == EventType.KeyDown)
        {

            if (Event.current.keyCode == KeyCode.RightArrow)
                _timeLine.CurrentTime += 0.01f;

            if (Event.current.keyCode == KeyCode.LeftArrow)
                _timeLine.CurrentTime -= 0.01f;

            GUI.changed = true;
            Event.current.Use();
            Repaint();
        }
    }
    #endregion
}