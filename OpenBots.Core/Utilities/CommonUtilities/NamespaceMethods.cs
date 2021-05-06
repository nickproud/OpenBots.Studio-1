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
        public static Dictionary<string, AssemblyReference> GetNamespacesDict(IAutomationEngineInstance engine)
        {
            return engine.AutomationEngineContext.ImportedNamespaces;
        }

        public static List<string> GetNamespacesList(IAutomationEngineInstance engine)
        {
            return engine.AutomationEngineContext.ImportedNamespaces.Keys.ToList();
        }

        public static List<string> GetNamespacesList(Dictionary<string, AssemblyReference> importedNamespaces)
        {
            return importedNamespaces.Keys.ToList();
        }

        public static Assembly GetAssembly(this string namespaceKey, IAutomationEngineInstance engine)
        {
            try
            {
                var importedNamespaces = engine.AutomationEngineContext.ImportedNamespaces;
                if (importedNamespaces.TryGetValue(namespaceKey, out AssemblyReference assemblyReference))
                {
                    return AppDomain.CurrentDomain.GetAssemblies()
                                                  .Where(x => x.GetName().Name == assemblyReference.AssemblyName && 
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

        public static List<Assembly> GetAssemblies(IAutomationEngineInstance engine)
        {
            try
            {
                var importedNamespaces = engine.AutomationEngineContext.ImportedNamespaces;
                if (importedNamespaces != null && importedNamespaces.Count > 0)
                {
                    return importedNamespaces.Select(x => AppDomain.CurrentDomain.GetAssemblies()
                                                                                 .Where(y => y.GetName().Name == x.Value.AssemblyName &&
                                                                                             y.GetName().Version == Version.Parse(x.Value.AssemblyVersion))
                                                                                 .FirstOrDefault())
                                             .Distinct()
                                             .ToList();
                }

                throw new Exception($"ImportedNamespaces is null or empty!");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Assembly> GetAssemblies(Dictionary<string, AssemblyReference> importedNamespaces)
        {
            try
            {
                if (importedNamespaces != null && importedNamespaces.Count > 0)
                {
                    return importedNamespaces.Select(x => AppDomain.CurrentDomain.GetAssemblies()
                                                                                 .Where(y => y.GetName().Name == x.Value.AssemblyName &&
                                                                                             y.GetName().Version == Version.Parse(x.Value.AssemblyVersion))
                                                                                 .FirstOrDefault())
                                             .Distinct()
                                             .ToList();
                }

                throw new Exception($"ImportedNamespaces is null or empty!");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
