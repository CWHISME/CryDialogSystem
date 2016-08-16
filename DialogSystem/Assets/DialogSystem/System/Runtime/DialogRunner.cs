/**********************************************************
*Author: wangjiaying
*Date: 2016.8.16
*Func:
**********************************************************/
using UnityEngine;
using System.Collections;
using CryDialog.Runtime;

/// <summary>
/// Just for test
/// </summary>
public class DialogRunner : MonoBehaviour
{

    public DialogObject _defaltDialog;

    private DialogManager _dialogManager;

    void Start()
    {
        _dialogManager = new DialogManager();
        _dialogManager.LoadDialog(_defaltDialog);
    }

    void Update()
    {
        _dialogManager.Tick();
    }
}
