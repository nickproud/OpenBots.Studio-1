using System.Collections.Generic;
using System.Reflection;

namespace OpenBots.Core.Script
{

    public class ScriptDefaultNamespaces
    {
        private static AssemblyReference _systemAssembly = new AssemblyReference(Assembly.GetAssembly(typeof(string)).GetName().Name, 
                                                                                 Assembly.GetAssembly(typeof(string)).GetName().Version.ToString());

        public static Dictionary<string, AssemblyReference> DefaultNamespaces = new Dictionary<string, AssemblyReference>()
        {
            //all default namespaces are part of mscorlib
            { "System", _systemAssembly },
            { "System.Collections.Generic", _systemAssembly },
            { "System.Text", _systemAssembly },
            { "System.Threading.Tasks", _systemAssembly }
        };
    }
}
