using OpenBots.Core.Infrastructure;
using System.Collections.Generic;

namespace OpenBots.Core.Utilities.CommonUtilities
{
    public class NamespaceMethods
    {
        public static List<string> GetNamespaces(IAutomationEngineInstance engine)
        {
            return engine.AutomationEngineContext.ImportedNamespaces;
        }
    }
}
