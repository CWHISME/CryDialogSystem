/**********************************************************
*Author: wangjiaying
*Date: 2016.6.17
*Func:
**********************************************************/
using UnityEditor;
using Event = UnityEngine.Event;
using UnityEngine;
using CryDialog.Runtime;

namespace CryDialog.Editor
{
    public class MainPageEditor : Singleton<MainPageEditor>
    {

        public bool OnGUI(DialogEditorWindow window)
        {
            DialogObject dialog = Selection.activeObject as DialogObject;
            window._dialogObject = dialog;
            if (dialog == null)
            {
                ShowTips(window._windowRect.center);
                return true;
            }

            return false;
        }

        private void ShowTips(Vector2 center)
        {
            ShowRightClickPopupMenu();
            EditorGUI.LabelField(new Rect(center.x - 100, center.y - 100, 300, 20), "Cry Dialog System", ResourcesManager.GetInstance.skin.GetStyle("Title"));
            EditorGUI.LabelField(new Rect(center.x - 38, center.y - 50, 500, 20), "—By CWHISME");
            EditorGUI.LabelField(new Rect(center.x - 150, center.y - 20, 500, 20), "You are not select any Dialog file,but you can create a new dialog.");
            if (GUI.Button(new Rect(center.x - 20, center.y + 30, 60, 40), "Create", ResourcesManager.GetInstance.skin.button))
            {
                CreateNewStory();
            }

            EditorGUI.LabelField(new Rect(center.x - 20, center.y + 100, 500, 20), Version.FullVersion);
        }

        private void ShowRightClickPopupMenu()
        {
            if (Event.current != null && Event.current.type == EventType.mouseDown)
            {
                if (Event.current.button == 1)
                {
                    if (DialogEditorWindow.DialogWindow != null)
                        if (Event.current.mousePosition.y < DialogEditorWindow.DialogWindow._contentRect.y) return;
                    GenericMenu menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Create/New Dialog"), false, () =>
                    {
                        CreateNewStory();
                    }
               );
                    menu.ShowAsContext();
                }
            }
        }

        private void CreateNewStory()
        {
            string path = EditorUtility.SaveFilePanelInProject("Create Dialog", "New Dialog", "asset", "Create new dialog");
            if (!string.IsNullOrEmpty(path))
            {
                DialogObject o = ScriptableObject.CreateInstance<DialogObject>();
                AssetDatabase.CreateAsset(o, path);
                Selection.activeObject = o;
            }
        }
    }
}