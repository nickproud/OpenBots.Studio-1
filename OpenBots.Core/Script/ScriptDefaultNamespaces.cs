using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace OpenBots.Core.Script
{
    public class ScriptDefaultNamespaces
    {
        private static AssemblyReference _mscorlibAssembly = new AssemblyReference(Assembly.GetAssembly(typeof(string)).GetName().Name, 
                                                                                 Assembly.GetAssembly(typeof(string)).GetName().Version.ToString());

        private static AssemblyReference _systemCoreAssembly = new AssemblyReference(Assembly.GetAssembly(typeof(IQueryable)).GetName().Name,
                                                                                 Assembly.GetAssembly(typeof(IQueryable)).GetName().Version.ToString());

        private static AssemblyReference _systemDataAssembly = new AssemblyReference(Assembly.GetAssembly(typeof(DataTable)).GetName().Name,
                                                                                Assembly.GetAssembly(typeof(DataTable)).GetName().Version.ToString());

        public static Dictionary<string, AssemblyReference> DefaultNamespaces = new Dictionary<string, AssemblyReference>()
        {
            //all default namespaces are part of mscorlib
            { "System", _mscorlibAssembly },
            { "System.Collections.Generic", _mscorlibAssembly },
            { "System.Data", _systemDataAssembly },
            { "System.Linq", _systemCoreAssembly },
            { "System.Text", _mscorlibAssembly },
            { "System.Threading.Tasks", _mscorlibAssembly }
        };
    }
}
