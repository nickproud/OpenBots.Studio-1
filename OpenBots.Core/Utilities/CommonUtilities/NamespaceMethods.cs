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
        public static Dictionary<string, List<AssemblyReference>> GetNamespacesDict(IAutomationEngineInstance engine)
        {
            return engine.AutomationEngineContext.ImportedNamespaces;
        }

        public static List<string> GetNamespacesList(IAutomationEngineInstance engine)
        {
            return engine.AutomationEngineContext.ImportedNamespaces.Keys.ToList();
        }

        public static List<string> GetNamespacesList(Dictionary<string, List<AssemblyReference>> importedNamespaces)
        {
            return importedNamespaces.Keys.ToList();
        }

        public static List<Assembly> GetAssemblies(this string namespaceKey, IAutomationEngineInstance engine)
        {
            try
            {
                List<Assembly> assemblies = new List<Assembly>();
                var importedNamespaces = engine.AutomationEngineContext.ImportedNamespaces;
                if (importedNamespaces.TryGetValue(namespaceKey, out List<AssemblyReference> assemblyReferences))
                {
                    assemblyReferences.ForEach(y => assemblies
                                      .Add(AppDomain.CurrentDomain.GetAssemblies()
                                      .Where(x => x.GetName().Name == y.AssemblyName && x.GetName().Version == Version.Parse(y.AssemblyVersion))
                                      .FirstOrDefault()));
                   
                    return assemblies;
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
                List<Assembly> assemblyList = new List<Assembly>();
                var importedNamespaces = engine.AutomationEngineContext.ImportedNamespaces;
                if (importedNamespaces != null && importedNamespaces.Count > 0)
                {
                    importedNamespaces.ToList()
                                      .ForEach(z => assemblyList
                                      .AddRange(z.Value
                                      .Select(x => AppDomain.CurrentDomain.GetAssemblies()
                                      .Where(y => y.GetName().Name == x.AssemblyName && y.GetName().Version == Version.Parse(x.AssemblyVersion))
                                      .FirstOrDefault())
                                      .Distinct()
                                      .ToList()));

                    return assemblyList;
                }

                throw new Exception($"ImportedNamespaces is null or empty!");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Assembly> GetAssemblies(Dictionary<string, List<AssemblyReference>> importedNamespaces)
        {
            try
            {
                List<Assembly> assemblyList = new List<Assembly>();
                if (importedNamespaces != null && importedNamespaces.Count > 0)
                {
                    importedNamespaces.ToList()
                                     .ForEach(z => assemblyList
                                     .AddRange(z.Value
                                     .Select(x => AppDomain.CurrentDomain.GetAssemblies()
                                     .Where(y => y.GetName().Name == x.AssemblyName && y.GetName().Version == Version.Parse(x.AssemblyVersion))
                                     .FirstOrDefault())
                                     .Distinct()
                                     .ToList()));

                    return assemblyList;
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
