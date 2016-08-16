/**********************************************************
*Author: wangjiaying
*Date: 2016.7.29
*Func:
**********************************************************/

using System;
using System.Collections.Generic;

namespace CryDialog.Runtime
{

    abstract public class Decorator : DialogNode
    {
        public override EnumResult Tick(NodeContent content)
        {
            return OnProcessing(content, NextNodes);
        }

        protected sealed override EnumResult OnStart()
        {
            return base.OnStart();
        }

        protected sealed override EnumResult OnUpdate()
        {
            return base.OnUpdate();
        }

        protected sealed override void OnEnd()
        {
        }

        protected virtual EnumResult OnProcessing(NodeContent content, NodeModifier[] nextNode)
        {
            return EnumResult.Success;
        }

        //public virtual void GetNextNodesFromDecorator(List<NodeModifier> nodes)
        //{
        //    GetNextNodes(nodes);
        //}

        public virtual UnityEngine.Color ColorLine
        {
            get { return UnityEngine.Color.white; }
        }
    }
}