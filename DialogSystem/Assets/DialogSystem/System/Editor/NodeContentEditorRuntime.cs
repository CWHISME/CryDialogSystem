/**********************************************************
*Author: wangjiaying
*Date: 2016.7.13
*Func:
**********************************************************/
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using CryDialog.Runtime;

namespace CryDialog.Editor
{
    public class NodeContentEditorRuntime<T> : NodeEditor<T> where T : class, new()
    {

        private List<NodeModifier> _alreadyDrawNode = new List<NodeModifier>(10);

        protected override void DrawNodes(NodeModifier[] nodeList, bool coreNode = false)
        {
            _alreadyDrawNode.Clear();

            for (int i = 0; i < nodeList.Length; i++)
            {
                NodeModifier node = nodeList[i];

                DrawParentNodes(node, coreNode);
            }
        }

        private void DrawChildNodes(NodeModifier[] nodeList, bool coreNode = false)
        {
            for (int i = 0; i < nodeList.Length; i++)
            {
                NodeModifier node = nodeList[i];

                if (_alreadyDrawNode.Contains(node)) continue;

                DrawNodeRect(node, coreNode);

                //Draw conection bazier line
                if (node is Decorator)
                {
                    DrawBazierLine(node, (node as Decorator).ColorLine);
                }
                else
                    DrawBazierLine(node);

                _alreadyDrawNode.Add(node);

                DrawChildNodes(node.GetNextNodes(node));

            }
        }

        private void DrawParentNodes(NodeModifier node, bool coreNode = false)
        {
            if (node == null) return;

            DrawNodeRect(node, coreNode);

            //Draw conection bazier line
            DrawDebugBazierLine(node);

            DrawChildNodes(node.NextNodes);

            DrawParentNodes(node.Parent);

        }

        protected override Rect DrawNodeRect(NodeModifier node, bool coreNode = false)
        {
            Rect rect = base.DrawNodeRect(node, coreNode);
            DragNodeEvent(node, rect);

            if (coreNode) DrawRunningNodeLabel(rect);
            return rect;
        }

        protected virtual void DrawRunningNodeLabel(Rect rect)
        {
            Rect labelRect = new Rect(rect);
            labelRect.position = new Vector2(labelRect.x + (labelRect.width / 2 - 30 * Tools.Zoom), labelRect.y + labelRect.height);
            GUIStyle s = ResourcesManager.GetInstance.GetFontStyle((int)(13 * Tools.Zoom));
            s.fontStyle = FontStyle.Bold;
            EditorGUI.LabelField(labelRect, "<color=#7CFC00>Running...</color>", s);
            s.fontStyle = FontStyle.Normal;
        }
    }
}