/**********************************************************
*Author: wangjiaying
*Date: 2016.8.16
*Func:
**********************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

namespace CryDialog.Runtime
{
    public class UIChoiceDialog : MonoBehaviour
    {

        public GameObject _buttonOringin;
        public GridLayoutGroup _gridLayout;
        public float _defaultWidth = 200f;

        private List<ButtonInfo> _buttonList = new List<ButtonInfo>();

#if UNITY_EDITOR
        [UnityEngine.ContextMenu("显示一下")]
        public void Show()
        {
            ShowChoice(new string[] { "XXX", "HHHH", "GGG", "GOGOGO" }, (v) => { UnityEditor.EditorUtility.DisplayDialog("Result", "You Select :" + v, "OK"); });
        }
#endif

        private static UIChoiceDialog _instance;

        public static UIChoiceDialog GetInstance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<UIChoiceDialog>();

                    if (_instance == null)
                    {
                        GameObject prefab = Resources.Load<GameObject>("[ChoiceDialog]");
                        if (prefab != null)
                        {
                            GameObject go = Instantiate(prefab) as GameObject;
                            go.transform.SetParent(GetCanvas().transform);
                            go.name = "[ChoiceDialog]";
                            _instance = go.GetComponent<UIChoiceDialog>();
                            RectTransform rect = (_instance.transform as RectTransform);
                            rect.anchoredPosition = Vector2.zero;
                            rect.sizeDelta = Vector2.zero;
                        }
                    }
                }

                return _instance;
            }
        }

        private static GameObject GetCanvas()
        {
            Canvas canvas = GameObject.FindObjectOfType<Canvas>();

            if (!canvas)
            {
                Debug.LogError("Canvas Not Found!");
            }
            return canvas.gameObject;
        }

        /// <summary>
        /// 显示一个选项
        /// </summary>
        /// <param name="choiceArray"></param>
        /// <param name="callBack">当玩家选择了某项之后，将会回调传入选择的下标</param>
        public void ShowChoice(string[] choiceArray, Action<int> callBack)
        {
            int diffCount = choiceArray.Length - _buttonList.Count;
            if (diffCount > 0)
            {
                for (int i = 0; i < diffCount; i++)
                {
                    _buttonList.Add(CreateButton());
                }
            }

            for (int i = 0; i < choiceArray.Length; i++)
            {
                ButtonInfo info = _buttonList[i];
                info._text.text = choiceArray[i];
                info._button.onClick.RemoveAllListeners();
                info._button.onClick.AddListener(() => { callBack.Invoke(_buttonList.FindIndex(b => b._button == info._button)); Hiden(); });
                info.SetActive(true);
            }

            ResetGridWidth();

        }

        /// <summary>
        /// 显示一个对话，自动加上序号
        /// </summary>
        public void ShowChoiceWithNumber(string[] choiceArray, Action<int> callBack)
        {
            string[] newChoice = choiceArray.Clone() as string[];
            for (int i = 0; i < newChoice.Length; i++)
            {
                newChoice[i] = (i + 1) + "." + newChoice[i];
            }

            ShowChoice(newChoice, callBack);
        }
        /// <summary>
        /// 创建选择按钮
        /// </summary>
        /// <returns></returns>
        private ButtonInfo CreateButton()
        {
            GameObject button = GameObject.Instantiate<GameObject>(_buttonOringin);
            button.transform.SetParent(_buttonOringin.transform.parent);
            button.SetActive(false);
            ButtonInfo info = new ButtonInfo();
            info._button = button.GetComponent<Button>();
            info._text = button.GetComponentInChildren<Text>();
            return info;
        }

        /// <summary>
        /// 隐藏选项
        /// </summary>
        private void Hiden()
        {
            for (int i = 0; i < _buttonList.Count; i++)
            {
                _buttonList[i].SetActive(false);
            }
        }

        /// <summary>
        /// 适应UI
        /// </summary>
        public void ResetGridWidth()
        {
            float w = _defaultWidth;
            for (int i = 0; i < _buttonList.Count; i++)
            {
                float pw = _buttonList[i]._text.preferredWidth;
                if (pw > w)
                    w = pw + 5f;
            }

            _gridLayout.cellSize = new Vector2(w, _gridLayout.cellSize.y);
        }

        public struct ButtonInfo
        {
            public Button _button;
            public Text _text;

            public void SetActive(bool act)
            {
                _button.gameObject.SetActive(act);
            }

        }
    }
}