using System;

namespace OpenBots.Core.Attributes.PropertyAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class CompatibleTypes : Attribute
    {
        public Type[] CompTypes { get; private set; }
        public bool IsPrimitive { get; private set; }

        //handled by VariableMethods.ConvertUserVariableToObject() and VariableMethods.StoreInUserVariable()
        public CompatibleTypes(Type[] compTypes)
        {
            CompTypes = compTypes;
        }

        //handled by VariableMethods.ConvertUserVariableToString()
        public CompatibleTypes(bool isPrimitive)
        {
            IsPrimitive = isPrimitive;
        }
    }
}
