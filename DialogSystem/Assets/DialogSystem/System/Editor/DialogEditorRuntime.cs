/**********************************************************
*Author: wangjiaying
*Date: 2016.7.13
*Func:
**********************************************************/
using UnityEngine;
using CryDialog.Runtime;

namespace CryDialog.Editor
{
    public class DialogEditorRuntime : NodeContentEditorRuntime<DialogEditorRuntime>
    {
        protected override void InternalOnGUI()
        {
            base.InternalOnGUI();

            DrawHelp(_currentNode);
        }


        protected override Rect DrawNodeRect(NodeModifier node, bool coreNode = false)
        {
            Vector2 pos = CalcRealPosition(node._position);
            Rect nodeRect = Tools.GetNodeRect(pos);

            GUIStyle style = ResourcesManager.GetInstance.DefaultNode;
            GUIStyle selectStyle = ResourcesManager.GetInstance.DefaultNodeOn;
            if (node is CryDialog.Runtime.Event)
            {
                style = ResourcesManager.GetInstance.EventNode;
                selectStyle = ResourcesManager.GetInstance.EventNodeOn;
            }
            else if (node is CryDialog.Runtime.Condition)
            {
                style = ResourcesManager.GetInstance.ConditionNode;
                selectStyle = ResourcesManager.GetInstance.ConditionNodeOn;
            }
            else if (node is CryDialog.Runtime.Action)
            {
                style = ResourcesManager.GetInstance.ActionNode;
                selectStyle = ResourcesManager.GetInstance.ActionNodeOn;
            }

            style = new GUIStyle(style);
            selectStyle = new GUIStyle(selectStyle);
            style.fontSize = (int)(style.fontSize * Tools.Zoom);
            selectStyle.fontSize = (int)(selectStyle.fontSize * Tools.Zoom);

            GUI.Box(nodeRect, coreNode ? "<color=#00FF00>" + node._name + "</color>" : node._name, _currentNode == node ? selectStyle : style);

            DragNodeEvent(node, nodeRect);

            DrawRunModeLable(node, nodeRect);

            if (coreNode) DrawRunningNodeLabel(nodeRect);
            else DrawDescription(nodeRect, (node as DialogNode).ToDescription());

            return nodeRect;
        }


    }
}