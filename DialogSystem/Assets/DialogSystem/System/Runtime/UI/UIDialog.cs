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
        public GameObject _backGround;

        private Queue<Sentence> _sentenceQueue = new Queue<Sentence>();
        private Sentence _currentSentence;

        void Update()
        {
            if (_currentSentence == null) return;

            if (Input.GetKeyDown(KeyCode.Mouse0))
                NextSentence();
        }

        /// <summary>
        /// 显示一段对话接口
        /// </summary>
        /// <param name="dialogArray"></param>
        /// <param name="callBack"></param>
        public void ShowDialog(string name, string[] dialogArray, System.Action callBack)
        {
            Sentence sen = new Sentence(name, dialogArray, callBack);

            ShowDialog(sen);
        }

        /// <summary>
        /// 显示一段对话接口
        /// </summary>
        /// <param name="dialogArray"></param>
        /// <param name="callBack"></param>
        public void ShowDialog(Sentence sentence)
        {
            if (_currentSentence != null)
            {
                _sentenceQueue.Enqueue(sentence);
                return;
            }

            SetUIActive(true);

            _currentSentence = sentence;

            NextSentence();
        }

        private void NextSentence()
        {
            if (_currentSentence.IsEnd())
            {
                _currentSentence.InvokCallBack();

                if (_sentenceQueue.Count > 0)
                {
                    _currentSentence = _sentenceQueue.Dequeue();
                }
                else {
                    SetUIActive(false);
                    _currentSentence = null;
                    return;
                }
            }

            _content.text = _currentSentence.GetCurrentString();
        }

        private void SetUIActive(bool act)
        {
            _content.gameObject.SetActive(act);
            _backGround.SetActive(act);
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
                            rect.sizeDelta = new Vector2(0, 150);
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

    public class Sentence
    {
        private string _name;
        private bool _haveName;
        private string[] _dialogList;
        private int _index = 0;
        private System.Action _callBack;

        public Sentence(string name, string[] dialogArray, System.Action callBack)
        {
            _name = name;
            _haveName = string.IsNullOrEmpty(_name);
            _dialogList = dialogArray;
            _index = 0;
            _callBack = callBack;
        }

        public bool IsEnd()
        {
            return _index >= _dialogList.Length;
        }

        public string GetCurrentString()
        {
            return _haveName ? _name + ": " + _dialogList[_index++] : _dialogList[_index++];
        }

        public void InvokCallBack()
        {
            if (_callBack != null)
                _callBack.Invoke();
        }
    }
}