/**********************************************************
*Author: CWHISME with PC DESKTOP-TCL3N2O
*Date: 8/15/2016 3:02:46 PM
*Func:
**********************************************************/

using UnityEngine;

namespace CryDialog.Runtime
{
    [Help("一直返回Running")]
    [Category("System")]
    public class ReturnRunning : Action
    {

        protected override EnumResult OnUpdate()
        {
            return EnumResult.Running;
        }

        public override string ToDescription()
        {
            return "阻塞";
        }
    }
}