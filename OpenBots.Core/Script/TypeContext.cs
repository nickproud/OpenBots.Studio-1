using System;
using System.Collections.Generic;

namespace OpenBots.Core.Script
{
    public class TypeContext
    {
        public Dictionary<string, List<Type>> GroupedTypes { get; set; }
        public Dictionary<string, Type> DefaultTypes { get; set; }

        public TypeContext(Dictionary<string, List<Type>> groupedTypes, Dictionary<string, Type> defaultTypes)
        {
            GroupedTypes = groupedTypes;
            DefaultTypes = defaultTypes;
        }

        public TypeContext()
        {

        }
    }
}
