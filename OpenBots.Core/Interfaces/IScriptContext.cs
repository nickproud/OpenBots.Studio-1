using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using OpenBots.Core.Script;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.Control;

namespace OpenBots.Core.Interfaces
{
    public interface IScriptContext
    {
        List<ScriptVariable> Variables { get; set; }
        List<ScriptArgument> Arguments { get; set; }
        List<ScriptElement> Elements { get; set; }
        Dictionary<string, List<AssemblyReference>> ImportedNamespaces { get; set; }
        List<Assembly> AssembliesList { get; set; }
        List<string> NamespacesList { get; set; }
        CSharpCompilationOptions DefaultCompilationOptions { get; set; }
        List<MetadataReference> DefaultReferences { get; set; }
        string GuidPlaceholder { get; set; }
        string CSharpPath { get; set; }
        EmitResult EvaluateInput(Type varType, string code);
        EmitResult EvaluateSnippet(string code);
        void CodeInput_KeyDown(object sender, KeyEventArgs e);
        void CodeDGVInput_TextChanged(object sender, EventArgs e);
        void AddIntellisenseControls(ControlCollection controls);
        void RemoveIntellisenseControls(ControlCollection controls);
    }
}
