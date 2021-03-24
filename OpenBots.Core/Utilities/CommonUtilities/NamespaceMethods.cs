using OpenBots.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace OpenBots.Core.Utilities.CommonUtilities
{
    public static class NamespaceMethods
    {
        public static Dictionary<string, Assembly> GetNamespaces(IAutomationEngineInstance engine)
        {
            return engine.AutomationEngineContext.ImportedNamespaces;
        }

        public static Assembly GetAssembly(this string namespaceKey, IAutomationEngineInstance engine)
        {
            try
            {
                if (engine.AutomationEngineContext.ImportedNamespaces.TryGetValue(namespaceKey, out Assembly assembly))
                    return assembly;

                throw new Exception($"Assembly for Namespace '{namespaceKey}' not found!");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
