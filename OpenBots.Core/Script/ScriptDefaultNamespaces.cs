using System;
using System.Collections.Generic;
using System.Reflection;

namespace OpenBots.Core.Script
{
    
    public class ScriptDefaultNamespaces
    {
        private static Assembly _systemAssembly = Assembly.GetAssembly(typeof(string));

        public static Dictionary<string, Assembly> DefaultNamespaces = new Dictionary<string, Assembly>()
        {
            //all default namespaces are part of mscorlib
            { "System", _systemAssembly },
            { "System.Collections.Generic", _systemAssembly },
         // { "System.Linq", _systemAssembly }, //not mscorlib
            { "System.Text", _systemAssembly },
            { "System.Threading.Tasks", _systemAssembly }
        };
    }
}
