/**********************************************************
*Author: wangjiaying
*Date: 2016.6.16
*Func:
**********************************************************/

using System;

namespace CryDialog.Runtime
{

    abstract public class Condition : DialogNode
    {

        public abstract bool OnCheck();

        public sealed override EnumResult Tick(NodeContent content)
        {
            if (OnCheck())
                return EnumResult.Success;
            return EnumResult.Failed;
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
    }
}