using OpenBots.Core.Interfaces;
using OpenBots.Core.Script;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OpenBots.Core.Utilities.CommonUtilities
{
    public static class NamespaceMethods
    {
        public static List<string> GetNamespacesList(Dictionary<string, List<AssemblyReference>> importedNamespaces)
        {
            return importedNamespaces.Keys.ToList();
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
                                     .Where(y => y.GetName().Name == x.AssemblyName && y.GetName().Version >= Version.Parse(x.AssemblyVersion))
                                     .FirstOrDefault())
                                     .Distinct()
                                     .ToList()));

                    return assemblyList.Where(x => x != null).ToList();
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
