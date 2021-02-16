using System;

namespace OpenBots.Core.Attributes.PropertyAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class InputType : Attribute
    {
        public Type InType { get; private set; }
        public InputType(Type inType)
        {
            InType = InType;
        }
    }
}
