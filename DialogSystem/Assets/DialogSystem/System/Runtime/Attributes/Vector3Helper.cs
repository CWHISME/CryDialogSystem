/**********************************************************
*Author: wangjiaying
*Date: 2017.2.7
*Func:
**********************************************************/

namespace CryDialog.Runtime
{
    public class Vector3Helper : System.Attribute
    {

        private VectorSyceMode _mode;
        public VectorSyceMode Mode { get { return _mode; } }

        public Vector3Helper(VectorSyceMode mode)
        {
            _mode = mode;
        }
    }

    public enum VectorSyceMode
    {
        Position,
        EulerAngle,
    }
}