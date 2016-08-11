/**********************************************************
*Author: wangjiaying
*Date: 2016.8.11
*Func:
**********************************************************/

namespace CryDialog.Runtime
{
    /// <summary>
    /// 该属性针对Enum及Bool类型的自定义变量处理
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public class ValueKeyReferenceAttribute : System.Attribute
    {

        public string _keyRef;

        public ValueKeyReferenceAttribute(string keyRefernce)
        {
            _keyRef = keyRefernce;
        }

        public Value GetValue(ValueContainer container, string key)
        {
            Value value = null;
            container._valueContainer.TryGetValue(key, out value);

            return value;
        }
    }

}