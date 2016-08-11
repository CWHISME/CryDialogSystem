/**********************************************************
*Author: wangjiaying
*Date: 2016.8.11
*Func:
**********************************************************/

using System.Collections.Generic;

namespace CryDialog.Runtime
{
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public class ValueNameSelectAttribute : System.Attribute
    {

        public string[] GetValueNameList(ValueContainer container)
        {
            List<string> names = new List<string>(5);
            foreach (var item in container._valueContainer)
            {
                names.Add(item.Key);
            }

            return names.ToArray();
        }

    }

}