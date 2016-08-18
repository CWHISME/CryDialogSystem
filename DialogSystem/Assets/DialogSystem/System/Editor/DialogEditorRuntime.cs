/**********************************************************
*Author: wangjiaying
*Date: 2016.7.13
*Func:
**********************************************************/
using UnityEngine;
using CryDialog.Runtime;
using UnityEditor;

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
            style.fontSize = 20;
            selectStyle.fontSize = 20;
            style.fontSize = (int)(style.fontSize * Tools.Zoom);
            selectStyle.fontSize = (int)(selectStyle.fontSize * Tools.Zoom);

            style = _currentNode == node ? selectStyle : style;

            GUIContent des = new GUIContent((node as DialogNode).ToDescription());

            //计算额外描述高度
            //GUIStyle desStyle = ResourcesManager.GetInstance.GetOverflowFontStyle(12);
            //float height = desStyle.CalcHeight(des, nodeRect.width);

            //Rect expandRect = new Rect(nodeRect);
            //expandRect.height = nodeRect.height + height + 10;
            Rect expandRect = Tools.GetNodeRect(pos, des.text);
            GUI.Box(expandRect, coreNode ? "<color=#00FF00>" + node._name + "</color>" : node._name, style);

            DragNodeEvent(node, expandRect);

            //处理分割线
            float seperateHeight = 40 * Tools.Zoom;
            Color handleColor = Handles.color;
            if (_currentNode != node && !Tools.IsValidMouseAABB(expandRect))
                Handles.color = Color.black;
            Handles.DrawLine(new Vector3(expandRect.position.x, expandRect.position.y + seperateHeight - 2, 0), new Vector3(expandRect.xMax, expandRect.yMin + seperateHeight - 2));
            Handles.DrawLine(new Vector3(expandRect.position.x, expandRect.position.y + seperateHeight, 0), new Vector3(expandRect.xMax, expandRect.yMin + seperateHeight));
            Handles.color = handleColor;

            DrawRunModeLable(node, nodeRect);

            if (coreNode)
                DrawRunningNodeLabel(expandRect);

            nodeRect.width = nodeRect.width - 10;
            nodeRect.position = new Vector2(nodeRect.position.x + 5, nodeRect.position.y);
            DrawDescription(nodeRect, (node as DialogNode).ToDescription());

            return expandRect;
        }


    }
}