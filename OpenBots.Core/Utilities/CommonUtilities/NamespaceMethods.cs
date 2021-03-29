using OpenBots.Core.Infrastructure;
using OpenBots.Core.Script;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OpenBots.Core.Utilities.CommonUtilities
{
    public static class NamespaceMethods
    {
        public static Dictionary<string, AssemblyReference> GetNamespaces(IAutomationEngineInstance engine)
        {
            return engine.AutomationEngineContext.ImportedNamespaces;
        }

        public static Assembly GetAssembly(this string namespaceKey, IAutomationEngineInstance engine)
        {
            try
            {
                if (engine.AutomationEngineContext.ImportedNamespaces.TryGetValue(namespaceKey, out AssemblyReference assemblyReference))
                {
                    return AppDomain.CurrentDomain.GetAssemblies().Where(x => x.GetName().Name == assemblyReference.AssemblyName && 
                                                                              x.GetName().Version == Version.Parse(assemblyReference.AssemblyVersion))
                                                                  .FirstOrDefault();
                }
                
                throw new Exception($"Assembly for Namespace '{namespaceKey}' not found!");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
