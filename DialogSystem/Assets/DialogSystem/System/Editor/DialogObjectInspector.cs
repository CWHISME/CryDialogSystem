/**********************************************************
*Author: wangjiaying
*Date: 2016.8.11
*Func:
**********************************************************/
using CryDialog.Runtime;
using UnityEditor;
using UnityEngine;

namespace CryDialog.Editor
{
    [CustomEditor(typeof(DialogObject))]
    public class DialogObjectInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("<color=#EE82EE>Dialog Source</color>", ResourcesManager.GetInstance.skin.GetStyle("Title"));

            GUILayout.Space(10);

            EditorGUILayout.HelpBox(((DialogObject)target)._description, MessageType.Info);

            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Edit >>>", GUILayout.Height(40), GUILayout.Width(80)))
                DialogEditorWindow.Open();

            if (GUILayout.Button("Value >>>", GUILayout.Height(40), GUILayout.Width(80)))
                ValueManagerWindow.Open();
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);
            EditorGUILayout.LabelField("Archive Version: ", ((DialogObject)target)._saveVersion.ToString());

            string path = Application.dataPath + "/../" + AssetDatabase.GetAssetPath(target);
            System.DateTime modifyDate = System.IO.File.GetLastAccessTime(path);
            EditorGUILayout.LabelField("Modify Date:", modifyDate.ToString("yyyy.MM.dd   H:m:s"));
        }
    }
}