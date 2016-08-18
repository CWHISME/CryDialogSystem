/**********************************************************
*Author: wangjiaying
*Date: 2016.7.7
*Func:
**********************************************************/
using UnityEngine;
using Event = UnityEngine.Event;
using UnityEditor;
using System.Reflection;
using System;
using CryDialog.Runtime;

namespace CryDialog.Editor
{
    public class DialogEditor : NodeContentEditor<DialogEditor>
    {
        protected override void InternalOnGUI()
        {
            base.InternalOnGUI();
            ShowRightClickMenu();

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

            //处理分割线
            float seperateHeight = 40 * Tools.Zoom;
            Color handleColor = Handles.color;
            if (_currentNode != node && !Tools.IsValidMouseAABB(expandRect))
                Handles.color = Color.black;
            Handles.DrawLine(new Vector3(expandRect.position.x, expandRect.position.y + seperateHeight, 0), new Vector3(expandRect.xMax, expandRect.yMin + seperateHeight));
            Handles.color = handleColor;

            DrawRunModeLable(node, expandRect);

            nodeRect.width = nodeRect.width - 10;
            nodeRect.position = new Vector2(nodeRect.position.x + 5, nodeRect.position.y);
            DrawDescription(nodeRect, des.text);

            return expandRect;
        }

        protected override void ShowConnectLine()
        {
            if (_isConnecting && _currentNode != null)
            {
                Vector2 pos1 = new Vector2(_currentNodeRect.max.x, _currentNodeRect.min.y + Tools.NodeHalfHeightZoomed);
                Tools.DrawBazier(pos1, Event.current.mousePosition);
                if (Tools.MouseUp)
                {
                    _isConnecting = false;
                    if (_currentNode == _currentHover) return;
                    string des = (_currentHover as DialogNode).ToDescription();
                    if (Tools.IsValidMouseAABB(Tools.GetNodeRect(CalcRealPosition(_currentHover._position), des)))
                    {
                        LinkCurrentNode();
                    }
                }
            }
        }

        private Vector2 _mousePosition;

        private void ShowRightClickMenu()
        {
            if (Event.current != null)
            {
                if (Event.current.type == EventType.MouseDown)
                {
                    _mousePosition = Event.current.mousePosition;
                    if (Event.current.button == 1)
                    {
                        if (_currentHover == null)
                        {
                            CreateNodeMenu();
                            return;
                        }

                        if (!Tools.IsValidMouseAABB(Tools.GetNodeRect(CalcRealPosition(_currentHover._position), (_currentHover as DialogNode).ToDescription())))
                            CreateNodeMenu();
                        else ShowDeleteNodeMenu();
                    }
                }
            }
        }

        private void CreateNodeMenu()
        {
            Vector2 mousePos = Event.current.mousePosition;
            if (mousePos.x < _contentRect.x) return;
            if (mousePos.y < _contentRect.y) return;
            GenericMenu menu = new GenericMenu();

            Assembly asm = ReflectionHelper.Asm;
            Type[] types = asm.GetTypes();

            AddEventMenu(menu, types, asm);
            AddConditionMenu(menu, types, asm);
            AddActionMenu(menu, types, asm);
            AddDecoratorMenu(menu, types, asm);

            menu.ShowAsContext();
        }

        /// <summary>
        /// 显示删除复制等菜单
        /// </summary>
        private void ShowDeleteNodeMenu()
        {
            GenericMenu menu = new GenericMenu();

            menu.AddItem(new GUIContent("Duplicate"), false, () =>
            {
                DuplicateNode(_currentHover);
            });

            menu.AddItem(new GUIContent("Delete"), false, () =>
            {
                Runtime.NodeModifier.Delete(_currentHover);
                if (_currentNode == _currentHover) _currentNode = null;
                _currentHover = null;
            });
            menu.ShowAsContext();
        }

        private void AddEventMenu(GenericMenu menu, Type[] type, Assembly asm)
        {
            Type baseType = typeof(CryDialog.Runtime.Event);
            Type[] typeList = Array.FindAll<Type>(type, (t) => t.IsSubclassOf(baseType));

            AddMenu(typeList, menu, "Create/Event/");

        }

        private void AddConditionMenu(GenericMenu menu, Type[] type, Assembly asm)
        {
            Type baseType = typeof(CryDialog.Runtime.Condition);
            Type[] typeList = Array.FindAll<Type>(type, (t) => t.IsSubclassOf(baseType));

            AddMenu(typeList, menu, "Create/Condition/");

        }

        private void AddActionMenu(GenericMenu menu, Type[] type, Assembly asm)
        {
            Type baseType = typeof(CryDialog.Runtime.Action);
            Type[] typeList = Array.FindAll<Type>(type, (t) => t.IsSubclassOf(baseType));

            AddMenu(typeList, menu, "Create/Action/");
        }

        private void AddDecoratorMenu(GenericMenu menu, Type[] type, Assembly asm)
        {
            Type baseType = typeof(CryDialog.Runtime.Decorator);
            Type[] typeList = Array.FindAll<Type>(type, (t) => t.IsSubclassOf(baseType));

            AddMenu(typeList, menu, "Create/Decorator/");
        }

        private void AddMenu(Type[] typeList, GenericMenu menu, string prefix)
        {
            for (int i = 0; i < typeList.Length; i++)
            {
                Type nodeType = typeList[i];
                if (nodeType.Name.StartsWith("_")) continue;
                CategoryAttribute[] category = nodeType.GetCustomAttributes(typeof(CategoryAttribute), true) as CategoryAttribute[];
                string cat = category.Length > 0 ? category[0].Category + "/" : "";
                menu.AddItem(new GUIContent(prefix + cat + nodeType.Name), false, () =>
               {
                   CreateNode(nodeType, _window._Dialog, CalcVirtualPosition(_mousePosition));
               });
            }
        }

        private void CreateNode(Type type, UniqueIDCalculator content, Vector3 pos)
        {
            object o = ReflectionHelper.Asm.CreateInstance(type.FullName);
            if (o != null)
            {
                Runtime.DialogNode node = o as Runtime.DialogNode;
                if (node == null) return;
                node._name = type.Name;
                node._position = pos;
                node.SetID(content.GenerateID());
                Runtime.NodeModifier.SetContent(node, content);
                _currentNode = node;
            }
        }
    }
}