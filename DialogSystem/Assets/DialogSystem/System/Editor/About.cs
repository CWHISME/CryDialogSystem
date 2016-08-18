/**********************************************************
*Author: wangjiaying
*Date: 2016.8.18
*Func:
**********************************************************/
using UnityEditor;
using UnityEngine;

namespace CryDialog.Editor
{
    public class About : EditorWindow
    {
        [MenuItem("Dialog System/About")]
        public static void ShowAbout()
        {
            About a = EditorWindow.GetWindow<About>();
            a.titleContent = new GUIContent("About");
            a.position = new Rect(100, 100, 600, 500);
            a.Show();
        }

        void OnGUI()
        {
            AboutPage();
        }

        private void AboutPage()
        {
            Rect _windowRect = new Rect(0, 0, position.width, position.height);

            EditorGUI.LabelField(new Rect(_windowRect.center.x - 100, _windowRect.center.y - 100, 500, 20), "Cry Dialog System", ResourcesManager.GetInstance.skin.GetStyle("Title"));
            EditorGUI.LabelField(new Rect(_windowRect.center.x - 33, _windowRect.center.y - 50, 200, 20), "By CWHISME");
            EditorGUI.LabelField(new Rect(_windowRect.center.x - 55, _windowRect.center.y - 20, 200, 20), "Email: cwhisme@qq.com");
            EditorGUI.LabelField(new Rect(_windowRect.center.x - 20, _windowRect.center.y, 200, 20), Version.FullVersion);

            //Web Github and Blog
            EditorGUI.LabelField(new Rect(_windowRect.center.x - 80, _windowRect.center.y + 30, 500, 20), "It's a Open Source project.\n    For more information:", ResourcesManager.GetInstance.GetFontStyle(14, Color.green));
            EditorGUI.SelectableLabel(new Rect(_windowRect.center.x - 60, _windowRect.center.y + 80, 300, 20), "http://www.cwhisme.com");
            EditorGUI.SelectableLabel(new Rect(_windowRect.center.x - 110, _windowRect.center.y + 100, 300, 20), "https://github.com/CWHISME/CryDialogSystem");
        }



    }
}
