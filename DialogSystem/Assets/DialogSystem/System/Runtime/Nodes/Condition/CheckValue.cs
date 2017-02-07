/**********************************************************
*Author: CWHISME with PC DESKTOP-TCL3N2O
*Date: 7/27/2016 5:18:05 PM
*Func:
**********************************************************/

namespace CryDialog.Runtime
{
    [Category("System/Values")]
    [Help("判断某个变量是否符合条件")]
    public class CheckValue : Condition
    {
        [ValueNameSelect]
        public string ValueName;
        [ValueKeyReference("ValueName")]
        public ValueCompare Compare;
        [ValueKeyReference("ValueName")]
        public string Value;

        public override bool OnCheck()
        {
            Value v = GetValueContainer.GetValue(ValueName);
            if (v != null)
                return v.Compare(Value, Compare);
            return false;
        }

        public override string ToDescription()
        {
            return "检查变量 [" + ValueName + "] " + Compare + " " + Value;
        }
    }
}