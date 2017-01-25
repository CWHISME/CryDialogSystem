/**********************************************************
*Author: wangjiaying
*Date: 2017.1.25
*Func:
**********************************************************/

namespace CryDialog.Runtime
{
    public class HeightAttribute : System.Attribute
    {

        private float _height;
        public float Height { get { return _height; } }

        public HeightAttribute(float height)
        {
            _height = height;
        }
    }
}