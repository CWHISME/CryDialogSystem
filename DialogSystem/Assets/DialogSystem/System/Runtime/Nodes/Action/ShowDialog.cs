/**********************************************************
*Author: CWHISME with PC DESKTOP-TCL3N2O
*Date: 8/17/2016 6:13:27 PM
*Func:
**********************************************************/

namespace CryDialog.Runtime
{
    [Help("显示一段对话")]
    [Category("System")]
    public class ShowDialog : Action
    {
        public string[] DialogList;

        private bool _end = false;

        protected override EnumResult OnStart()
        {
            UIDialog.GetInstance.ShowDialog(DialogList, () => _end = true);
            return EnumResult.Success;
        }

        protected override EnumResult OnUpdate()
        {
            if (!_end) return EnumResult.Running;
            return EnumResult.Success;
        }

        protected override void OnEnd()
        {
            _end = false;
        }
    }
}