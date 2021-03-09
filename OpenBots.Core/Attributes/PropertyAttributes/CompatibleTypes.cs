using System;

namespace OpenBots.Core.Attributes.PropertyAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CompatibleTypes : Attribute
    {
        public Type[] CompTypes { get; private set; }
        public bool IsStringOrPrimitive { get; private set; }

        public CompatibleTypes(Type[] compTypes, bool isStringOrPrimitive = false)
        {
            CompTypes = compTypes;
            IsStringOrPrimitive = isStringOrPrimitive;
        }
    }
}
