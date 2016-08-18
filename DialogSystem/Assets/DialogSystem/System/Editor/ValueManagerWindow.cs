/**********************************************************
*Author: wangjiaying
*Date: 2016.7.20
*Func:
**********************************************************/
using UnityEditor;
using UnityEngine;
using CryDialog.Runtime;

namespace CryDialog.Editor
{
    /// <summary>
    /// 管理Story及Mission的各种变量
    /// </summary>
    public class ValueManagerWindow : EditorWindow
    {

        private static ValueManagerWindow _instance;
        public static ValueContainer missionContainer;

        [MenuItem("Dialog System/Open Value Manager")]
        public static void Open()
        {
            if (_instance != null)
            {
                _instance.Focus();
                return;
            }
            _instance = EditorWindow.CreateInstance<ValueManagerWindow>();
            _instance.titleContent = new GUIContent("Value Manager");
            float h = 600;
            float w = 350;
            _instance.position = new Rect(Screen.width - w, Screen.height - h, w, h);
            //_instance.maxSize = new Vector2(w, Screen.height);

            _instance.Show();
        }

        void OnGUI()
        {
            GUILayout.Space(5);
            EditorGUILayout.LabelField("Value Manager", ResourcesManager.GetInstance.GetFontStyle(22));
            GUILayout.Space(10);
            StoryValue();
        }

        void Update()
        {
            Repaint();
        }

        private void StoryValue()
        {
            DialogObject dialog = Selection.activeObject as DialogObject;
            if (dialog == null)
            {
                EditorGUILayout.LabelField("Not select dialog!");
                return;
            }
            if (dialog.Dialog != null)
            {
                EditorGUILayout.BeginVertical(GUILayout.Width(300));

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("<color=#FF00FF>" + dialog.name + "</color>", ResourcesManager.GetInstance.GetFontStyle(18));
                //GUILayout.Space(10);
                if (GUILayout.Button("<color=#00FF00>Add Value</color>", ResourcesManager.GetInstance.skin.button, GUILayout.Height(25)))
                    ValueAdder.Open(dialog.Dialog);
                EditorGUILayout.EndHorizontal();

                GUILayout.Space(10);

                ShowContainer(dialog.Dialog);

                EditorGUILayout.EndVertical();
            }
        }

        private void ShowContainer(ValueContainer container)
        {
            foreach (var val in container._valueContainer)
            {
                if (ShowValue(val.Key, val.Value))
                {
                    container._valueContainer.Remove(val.Key);
                    return;
                }
            }
        }

        private bool ShowValue(string name, Value val)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("<color=#00FFFF>" + name + "</color>[<color=yellow>" + val.ValueType + "</color>]", ResourcesManager.GetInstance.GetFontStyle(12));
            switch (val.ValueType)
            {
                case VarType.INT:
                    val.IntValue = EditorGUILayout.IntField(val.IntValue);
                    break;
                case VarType.FLOAT:
                    val.FloatValue = EditorGUILayout.FloatField(val.FloatValue);
                    break;
                case VarType.BOOL:
                    val.BoolValue = EditorGUILayout.Toggle(val.BoolValue);
                    break;
                case VarType.STRING:
                    val.StringValue = EditorGUILayout.TextField(val.StringValue);
                    break;
            }

            if (GUILayout.Button("<color=red>X</color>", ResourcesManager.GetInstance.skin.button, GUILayout.Height(18), GUILayout.Width(18)))
            {
                if (EditorUtility.DisplayDialog("Caution!", "You will delete value [" + name + "] !", "Confirm", "Cancel"))
                    return true;
            }

            EditorGUILayout.EndHorizontal();
            return false;
        }

    }
}
