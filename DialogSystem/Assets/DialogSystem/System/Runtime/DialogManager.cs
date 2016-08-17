/**********************************************************
*Author: wangjiaying
*Date: 2016.8.16
*Func:
**********************************************************/
using System;
using System.Collections.Generic;
using System.IO;

namespace CryDialog.Runtime
{
    /// <summary>
    /// 运行对话的一个管理器
    /// </summary>
    [System.Serializable]
    public class DialogManager : ISerialize
    {

        /// <summary>
        /// 当一整个对话逻辑结束的事件
        /// 指定参数为：对话（文件）的名字
        /// </summary>
        public static event System.Action<string> OnDialogEnd;

        /// <summary>
        /// 当前对话，一次仅且只运行当前一个对话逻辑
        /// </summary>
        private Dialog _currentDialog;

        /// <summary>
        /// 对话列表，若当前对话目标具有了多个对话逻辑（一个及以上）时
        /// 额外的对话逻辑将按照添加顺序，加入该列表
        /// </summary>
        private List<Dialog> _dialogList = new List<Dialog>();

        /// <summary>
        /// 当当前对话逻辑结束后，该项为true
        /// 所以，每次对话开始之前
        /// 请确保该值为false
        /// 一般可调用方法 ReloadDialog() 进行重置
        /// </summary>
        private bool _accomplish = false;

        public void Tick()
        {
            if (_accomplish || _currentDialog == null) return;
            if (_currentDialog.Tick() != EnumResult.Running)
            {
                _accomplish = true;
                if (OnDialogEnd != null)
                    OnDialogEnd.Invoke(_currentDialog._name);
            }
        }

        /// <summary>
        /// 若当前存在对话，则重新运行对话
        /// 若不存在，则加载对话列表最前面一个
        /// 若都不存在，返回false，重载失败
        /// </summary>
        public bool ReloadDialog()
        {
            if (_currentDialog != null)
            {
                _accomplish = false;
                return true;
            }

            if (_dialogList.Count > 0)
            {
                _currentDialog = _dialogList[_dialogList.Count - 1];
                _accomplish = false;
                return true;
            }

            return false;
        }

        /// <summary>
        /// 清除所有痕迹，从最开始加载一个Dialog
        /// </summary>
        /// <param name="dia"></param>
        public void LoadDialog(DialogObject dia)
        {
            if (!dia) return;
            _dialogList.Clear();
            _currentDialog = dia.Dialog;
            _accomplish = false;
        }

        /// <summary>
        /// 添加一个Dialog，若当前有Dialog，则缓存当前对话，并将新增对话当作当前对话
        /// </summary>
        /// <param name="dialog"></param>
        public void AddDialog(DialogObject dialog)
        {
            if (_currentDialog == null)
            {
                _currentDialog = dialog.Dialog;
                return;
            }

            _dialogList.Add(_currentDialog);
            _currentDialog = dialog.Dialog;
            //删除可能已存在与此的同一节点
            RemoveDialog(dialog);
            _accomplish = false;
        }

        /// <summary>
        /// 从该对话目标之上删除一个对话逻辑
        /// </summary>
        /// <param name="dialog">对话逻辑文件</param>
        public void RemoveDialog(DialogObject dialog)
        {
            RemoveDialog(dialog.name);
        }

        /// <summary>
        /// 从该对话目标之上删除一个对话逻辑
        /// </summary>
        /// <param name="name">对话逻辑名字</param>
        public void RemoveDialog(string name)
        {
            if (_currentDialog._name == name)
            {
                if (_dialogList.Count > 0)
                    _currentDialog = _dialogList[_dialogList.Count - 1];
                else _currentDialog = null;
            }

            int index = _dialogList.FindIndex(d => d._name == name);
            if (index != -1)
                _dialogList.RemoveAt(index);
        }

        /// <summary>
        /// 将当前对话管理器中的所有对话序列化为二进制数据
        /// </summary>
        /// <param name="w"></param>
        public void Serialize(BinaryWriter w)
        {
            WriteDialog(_currentDialog, w);

            w.Write(_dialogList.Count);
            for (int i = 0; i < _dialogList.Count; i++)
            {
                WriteDialog(_dialogList[i], w);
            }
        }

        /// <summary>
        /// 反序列化当前对话管理器的对话信息
        /// </summary>
        public void Deserialize(BinaryReader r)
        {
            _currentDialog = ReadDialog(r);
            int count = r.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                Dialog dialog = ReadDialog(r);
                if (dialog != null)
                    _dialogList.Add(dialog);
            }
        }

        private void WriteDialog(Dialog dialog, BinaryWriter w)
        {
            w.Write(dialog == null);
            if (dialog != null)
            {
                byte[] datas = dialog.Save();
                int count = datas.Length;
                w.Write(count);
                w.Write(datas);
            }
        }

        private Dialog ReadDialog(BinaryReader r)
        {
            Dialog d = null;
            bool cur = r.ReadBoolean();
            if (cur)
            {
                d = new Dialog();
                d.Load(r.ReadBytes(r.ReadInt32()));
            }
            return d;
        }
    }
}