using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Text;
using OpenBots.Core.Enums;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Folders = OpenBots.Core.IO.Folders;
using OBScriptVariable = OpenBots.Core.Script.ScriptVariable;

namespace OpenBots.Core.Script
{
    public class ScriptContext
    {
        public List<OBScriptVariable> Variables { get; set; }
        public List<ScriptArgument> Arguments { get; set; }
        public List<ScriptElement> Elements { get; set; }
        public Dictionary<string, List<AssemblyReference>> ImportedNamespaces { get; set; }
        public List<Assembly> AssembliesList { get; set; }
        public List<string> NamespacesList { get; set; }
        public CSharpCompilationOptions DefaultCompilationOptions { get; set; }
        public List<MetadataReference> DefaultReferences { get; set; }
        public string GuidPlaceholder { get; set; }
        public string CSharpPath { get; set; }

        public ScriptContext()
        {
            Variables = new List<OBScriptVariable>();
            Arguments = new List<ScriptArgument>();
            Elements = new List<ScriptElement>();
            ImportedNamespaces = new Dictionary<string, List<AssemblyReference>>(ScriptDefaultNamespaces.DefaultNamespaces);

            AssembliesList = NamespaceMethods.GetAssemblies(ImportedNamespaces);
            NamespacesList = NamespaceMethods.GetNamespacesList(ImportedNamespaces);

            DefaultCompilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary).WithOverflowChecks(true)
                                                                                                         .WithOptimizationLevel(OptimizationLevel.Release)
                                                                                                         .WithUsings(NamespacesList);

            DefaultReferences = AssembliesList.Select(x => (MetadataReference)MetadataReference.CreateFromFile(x.Location)).ToList();
            CSharpPath = Path.Combine(Folders.GetFolder(FolderType.StudioFolder), "CSharp");
            GenerateGuidPlaceHolder();
        }

        public void ReloadCompilerObjects()
        {
            AssembliesList = NamespaceMethods.GetAssemblies(ImportedNamespaces);
            NamespacesList = NamespaceMethods.GetNamespacesList(ImportedNamespaces);

            DefaultCompilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary).WithOverflowChecks(true)
                                                                                                         .WithOptimizationLevel(OptimizationLevel.Release)
                                                                                                         .WithUsings(NamespacesList);

            DefaultReferences = AssembliesList.Select(x => (MetadataReference)MetadataReference.CreateFromFile(x.Location)).ToList();
        }

        public void GenerateGuidPlaceHolder()
        {
            GuidPlaceholder = $"v{Guid.NewGuid()}".Replace("-", "");
        }

        public EmitResult EvaluateVariable(string varName, Type varType, string code)
        {
            if (string.IsNullOrEmpty(code))
                code = "null";

            string script = $"{varType.GetRealTypeFullName()}? {varName} = {code};";

            var parsedSyntaxTree = SyntaxFactory.ParseSyntaxTree(SourceText.From(script, Encoding.UTF8), new CSharpParseOptions(languageVersion: LanguageVersion.CSharp8, kind: SourceCodeKind.Script), "");
            var compilation = CSharpCompilation.Create("CSharp", new SyntaxTree[] { parsedSyntaxTree }, DefaultReferences, DefaultCompilationOptions);
            var result = compilation.Emit(CSharpPath);

            return result;
        }

        public EmitResult EvaluateInput(Type varType, string code)
        {
            if (string.IsNullOrEmpty(code))
                code = "null";

            var script = "";
            Variables.ForEach(v => script += $"{v.VariableType.GetRealTypeFullName()}? {v.VariableName} = " +
                $"{(v.VariableValue == null || (v.VariableValue is string && string.IsNullOrEmpty(v.VariableValue.ToString())) ? "null" : v.VariableValue)};");
            Arguments.ForEach(a => script += $"{a.ArgumentType.GetRealTypeFullName()}? {a.ArgumentName} = " +
                $"{(a.ArgumentValue == null || (a.ArgumentValue is string && string.IsNullOrEmpty(a.ArgumentValue.ToString())) ? "null" : a.ArgumentValue)};");

            string type;
            var test = varType.GetGenericArguments();
            if (varType.IsGenericType && varType.GetGenericArguments()[0].Name == "T")
                type = "object";
            else
                type = varType.GetRealTypeFullName();

            GenerateGuidPlaceHolder();
            script += $"{type}? {GuidPlaceholder} = {code};";

            var parsedSyntaxTree = SyntaxFactory.ParseSyntaxTree(SourceText.From(script, Encoding.UTF8), new CSharpParseOptions(languageVersion: LanguageVersion.CSharp8, kind: SourceCodeKind.Script), "");
            var compilation = CSharpCompilation.Create("CSharp", new SyntaxTree[] { parsedSyntaxTree }, DefaultReferences, DefaultCompilationOptions);
            var result = compilation.Emit(CSharpPath);

            return result;
        }

        public EmitResult EvaluateSnippet(string code)
        {
            if (string.IsNullOrEmpty(code))
                code = "null";

            var script = "";
            Variables.ForEach(v => script += $"{v.VariableType.GetRealTypeFullName()}? {v.VariableName} = " +
               $"{(v.VariableValue == null || (v.VariableValue is string && string.IsNullOrEmpty(v.VariableValue.ToString())) ? "null" : v.VariableValue)};");
            Arguments.ForEach(a => script += $"{a.ArgumentType.GetRealTypeFullName()}? {a.ArgumentName} = " +
                $"{(a.ArgumentValue == null || (a.ArgumentValue is string && string.IsNullOrEmpty(a.ArgumentValue.ToString())) ? "null" : a.ArgumentValue)};");

            script += $"{code};";

            var parsedSyntaxTree = SyntaxFactory.ParseSyntaxTree(SourceText.From(script, Encoding.UTF8), new CSharpParseOptions(languageVersion: LanguageVersion.CSharp8, kind: SourceCodeKind.Script), "");
            var compilation = CSharpCompilation.Create("CSharp", new SyntaxTree[] { parsedSyntaxTree }, DefaultReferences, DefaultCompilationOptions);
            var result = compilation.Emit(CSharpPath);

            return result;
        }
    }
}
