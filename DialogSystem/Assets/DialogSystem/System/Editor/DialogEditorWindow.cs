/**********************************************************
*Author: wangjiaying
*Date: 2016.8.11
*Func:
**********************************************************/
using UnityEngine;
using UnityEditor;
using CryDialog.Runtime;

namespace CryDialog.Editor
{
    public class DialogEditorWindow : EditorWindow
    {

        public static DialogEditorWindow DialogWindow;
        [MenuItem("Dialog System/Open Dialog Editor")]
        public static void Open()
        {
            if (DialogWindow != null)
            {
                DialogWindow.Show();
                return;
            }
            DialogWindow = EditorWindow.GetWindow<DialogEditorWindow>();
            DialogWindow.titleContent = new GUIContent("Dialog Editor");
            //float h = Screen.height * 0.7f;
            //float w = Screen.width * 0.7f;
            //DialogWindow.position = new Rect(Screen.width - w, Screen.height - h, w, h);
            //DialogWindow.position=new 

            DialogWindow.Show();
        }

        private DialogObject _lastDialogObject;
        public DialogObject _dialogObject;
        public Dialog _Dialog { get { return _dialogObject.Dialog; } }
        public Vector2 CurrentContentCenter
        {
            get
            {
                return _Dialog.graphCenter;
            }
            set
            {
                _Dialog.graphCenter = value;
            }
        }

        public Rect _windowRect;
        public Rect _contentRect;

        public float _leftWidth = 230f;
        public float _topHeight = 60f;
        public float _titleHeight = 35f;
        public float _selectionGridHeight { get { return _titleHeight + 5; } }

        public const float _zoomMin = 0.4f;
        public const float _zoomMax = 2f;
        public float Zoom
        {
            get
            {
                if (_dialogObject)
                    return _dialogObject._zoom;
                return 1;
            }
            set { if (_dialogObject) _dialogObject._zoom = value; }
        }

        void OnEnable()
        {
            DialogWindow = this;
            Repaint();
        }

        void Update()
        {
            Repaint();
            DialogWindow = this;
        }

        void OnGUI()
        {
            _windowRect = new Rect(0, 0, position.width, position.height);
            _contentRect = new Rect(_leftWidth, _topHeight, position.width, position.height);

            //========Story Editor ==============
            if (MainPageEditor.GetInstance.OnGUI(this))
            {
                return;
            }

            if (_lastDialogObject != _dialogObject)
            {
                _lastDialogObject = _dialogObject;
                DialogEditor.GetInstance.Reset();
            }

            //Show Pages
            MainPage();

            //========Top Button ==============
            ShowTitle();
            //========Version          ==============
            ShowVersion();
        }

        private void MainPage()
        {
            EditorGUI.DrawTextureTransparent(_contentRect, ResourcesManager.GetInstance.texBackground);

            //发现Dialog可不一定一定是使用的DialogObject的引用，所以有问题
            //暂时没时间管了
            if (Application.isPlaying) return; //DialogEditorRuntime.GetInstance.OnGUI(this, _Dialog.Nodes);
            else DialogEditor.GetInstance.OnGUI(this, _Dialog.Nodes);


            //处理缩放
            if (UnityEngine.Event.current != null)
            {
                if (UnityEngine.Event.current.type == EventType.ScrollWheel)
                {
                    _dialogObject._zoom -= UnityEngine.Event.current.delta.y * 0.01f;
                    _dialogObject._zoom = Mathf.Clamp(_dialogObject._zoom, _zoomMin, _zoomMax);
                    //Debug.Log(_zoom);
                }
            }

        }

        private void ShowVersion()
        {
            GUI.Label(new Rect(_windowRect.xMax - 120, _windowRect.yMax - 40, 120, 20), new GUIContent("Made By CWHISME"));
            GUI.Label(new Rect(_windowRect.xMax - 90, _windowRect.yMax - 20, 100, 20), new GUIContent(Version.FullVersion));
        }

        private void ShowTitle()
        {
            EditorGUI.DrawTextureTransparent(new Rect(0, 0, position.width, _topHeight), ResourcesManager.GetInstance.texBackground);

            GUIStyle style = ResourcesManager.GetInstance.skin.GetStyle("Title");
            float btnW = 0;

            EditorGUI.LabelField(new Rect(btnW, 0, _windowRect.xMax, _titleHeight), new GUIContent(" Dialog Editor" + "->" + _dialogObject.name), style);

            GUIStyle buttonStyle = new GUIStyle(ResourcesManager.GetInstance.skin.button);
            if (GUI.Button(new Rect(_contentRect.width - 180, 3, 80, _titleHeight - 3), "Values", buttonStyle))
            {
                ValueManagerWindow.Open();
            }

            if (GUI.Button(new Rect(_contentRect.width - 90, 3, 80, _titleHeight - 3), "About", buttonStyle))
            {
                About.ShowAbout();
            }

            _dialogObject._debugMode = EditorGUI.ToggleLeft(new Rect(10f, _titleHeight, _windowRect.xMax, _titleHeight), "Debug Mode", _dialogObject._debugMode);

            //分割线
            Handles.DrawLine(new Vector3(_windowRect.xMin, _windowRect.yMin + _topHeight - 2, 0), new Vector3(_windowRect.xMax, _windowRect.yMin + _topHeight - 2));
            Handles.DrawLine(new Vector3(_windowRect.xMin, _windowRect.yMin + _topHeight, 0), new Vector3(_windowRect.xMax, _windowRect.yMin + _topHeight));

            //运行时不允许存储加载
            if (Application.isPlaying) return;

            buttonStyle.normal.textColor = new Color32(255, 64, 180, 255);
            if (GUI.Button(new Rect(_contentRect.width - 270, 3, 80, _titleHeight - 3), "Reload", buttonStyle))
            {
                _dialogObject.Load();
            }

            buttonStyle.normal.textColor = new Color32(0, 255, 0, 255);
            if (GUI.Button(new Rect(_contentRect.width - 360, 3, 80, _titleHeight - 3), "Save Dialog", buttonStyle))
                _dialogObject.Save();
        }

    }
}
