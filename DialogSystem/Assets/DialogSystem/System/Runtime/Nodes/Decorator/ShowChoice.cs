/**********************************************************
*Author: CWHISME with PC DESKTOP-TCL3N2O
*Date: 8/16/2016 4:22:22 PM
*Func:
**********************************************************/

using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace CryDialog.Runtime
{
    [Help("显示一个选项，当选择相应选项之后，会运行接下来相应节点（比如选择2->只运行第二根连线节点），所以，该节点运行完毕后，只会执行一个子节点。")]
    public class ShowChoice : Decorator
    {
        public bool AutoAddNumber = false;
        public string[] ChoicesList;
        private bool _show = false;
        private int _selectIndex = -1;

        private int _tempIndex = 0;

        protected override EnumResult OnProcessing(NodeContent content, NodeModifier[] nextNode)
        {
            if (!_show)
            {
                if (AutoAddNumber)
                    UIChoiceDialog.GetInstance.ShowChoiceWithNumber(ChoicesList, (i) => _selectIndex = i);
                else
                    UIChoiceDialog.GetInstance.ShowChoice(ChoicesList, (i) => _selectIndex = i);
                _show = true;
            }
            if (_selectIndex == -1) return EnumResult.Running;
            else
            {
                _show = false;
                _tempIndex = _selectIndex;
                _selectIndex = -1;
                return EnumResult.Success;
            }
        }

        public override void GetNextNodes(List<NodeModifier> nodes)
        {
            if (_tempIndex > _nextNodeList.Count - 1) return;
            NodeModifier node = _nextNodeList[_tempIndex];
            if (node != null)
                nodes.Add(node);
        }

        public override Color ColorLine
        {
            get
            {
                return new Color32(245, 255, 250, 255);
            }
        }

        public override string ToDescription()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < ChoicesList.Length; i++)
            {
                if (i >= _nextNodeList.Count)
                {
                    builder.AppendLine("<color=red>选择 [" + i + "." + ChoicesList[i] + "]无效！</color>");
                    continue;
                }
                builder.AppendLine("[<color=#00FF7F>" + i + "</color>] " + ChoicesList[i] + "\n(运行节点 <color=#00FF7F>" + _nextNodeList[i]._name + "</color>)");
            }
            return builder.ToString();
        }
    }
}