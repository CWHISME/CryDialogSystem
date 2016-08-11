/**********************************************************
*Author: wangjiaying
*Date: 2016.8.11
*Func:
**********************************************************/

using System.IO;

namespace CryDialog.Runtime
{

    abstract public class DialogNode : UpdateNode
    {
        protected ValueContainer GetValueContainer { get { return GetContent() as ValueContainer; } }

        public virtual string ToDescription() { return ""; }

        public sealed override void Serialize(BinaryWriter w)
        {
            base.Serialize(w);
        }

        public sealed override void Deserialize(BinaryReader r)
        {
            base.Deserialize(r);
        }

    }
}