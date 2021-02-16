using System;

namespace OpenBots.Core.Attributes.PropertyAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class OutputType : Attribute
    {
        public Type OutType { get; private set; }
        public OutputType(Type outType)
        {
            OutType = outType;
        }
    }
}
