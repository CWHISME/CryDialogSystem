/**********************************************************
*Author: wangjiaying
*Date: 2016.8.16
*Func:
**********************************************************/
using CryDialog.Runtime;
using System.Collections.Generic;

/// <summary>
/// 运行对话的一个管理器
/// </summary>
[System.Serializable]
public class DialogManager
{
    private Dialog _currentDialog;

    private List<Dialog> _dialogList = new List<Dialog>();

    private bool _accomplish = false;

    public void Tick()
    {
        if (_currentDialog == null || _accomplish) return;
        if (_currentDialog.Tick() != EnumResult.Running)
            _accomplish = true;
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

    public void RemoveDialog(DialogObject dialog)
    {
        RemoveDialog(dialog.name);
    }

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


}
