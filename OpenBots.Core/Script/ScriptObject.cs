using System.Collections.Generic;

namespace OpenBots.Core.Script
{
    public class ScriptObject
    {
        public List<ScriptVariable> ScriptVariables { get; set; }
        public List<ScriptArgument> ScriptArguments { get; set; }
        public List<ScriptElement> ScriptElements { get; set; }
        public Dictionary<string, AssemblyReference> ImportedNamespaces { get; set; }

        public ScriptObject()
        {
            ScriptVariables = new List<ScriptVariable>();
            ScriptArguments = new List<ScriptArgument>();
            ScriptElements = new List<ScriptElement>();
            ImportedNamespaces = ScriptDefaultNamespaces.DefaultNamespaces;
        }

        public ScriptObject(List<ScriptVariable> scriptVariables, List<ScriptArgument> scriptArguments, List<ScriptElement> scriptElements,
            Dictionary<string, AssemblyReference> importedNamespaces)
        {
            ScriptVariables = scriptVariables;
            ScriptArguments = scriptArguments;
            ScriptElements = scriptElements;
            ImportedNamespaces = importedNamespaces;
        }
    }
}
