using System;

namespace OpenBots.Core.Attributes.PropertyAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CompatibleTypes : Attribute
    {
        public Type[] CompTypes { get; private set; }

        public CompatibleTypes(Type[] compTypes)
        {
            CompTypes = compTypes;
        }
    }
}
