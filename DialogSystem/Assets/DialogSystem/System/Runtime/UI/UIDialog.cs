/**********************************************************
*Author: wangjiaying
*Date: 2016.8.17
*Func:
**********************************************************/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CryDialog.Runtime
{
    public class UIDialog : MonoBehaviour
    {

        public Text _content;

        private bool _show = false;
        private List<string> _dialogList = new List<string>(5);
        private int _index = 0;
        private System.Action _callBack;

        void Update()
        {
            if (!_show) return;

            if (Input.GetKeyDown(KeyCode.Mouse0))
                NextSentence();
        }

        public void ShowDialog(string[] dialogArray, System.Action callBack)
        {
            if (!_show)
            {
                _index = 0;
                _dialogList.Clear();
            }
            _dialogList.AddRange(dialogArray);
            _callBack = callBack;
            _content.gameObject.SetActive(true);
            _show = true;

            NextSentence();
        }

        private void NextSentence()
        {

            if (_index >= _dialogList.Count)
            {
                _show = false;
                _content.gameObject.SetActive(false);
                if (_callBack != null)
                    _callBack.Invoke();

                return;
            }
            _content.text = _dialogList[_index++];
        }

        private static UIDialog _instance;

        public static UIDialog GetInstance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<UIDialog>();

                    if (_instance == null)
                    {
                        GameObject prefab = Resources.Load<GameObject>("[SayDialog]");
                        if (prefab != null)
                        {
                            GameObject go = Instantiate(prefab) as GameObject;
                            go.transform.SetParent(GetCanvas().transform);
                            go.name = "[SayDialog]";
                            _instance = go.GetComponent<UIDialog>();
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

    }
}