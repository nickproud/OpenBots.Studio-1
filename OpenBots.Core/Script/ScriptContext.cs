using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using OBScriptVariable = OpenBots.Core.Script.ScriptVariable;
using RSScript = Microsoft.CodeAnalysis.Scripting.Script;

namespace OpenBots.Core.Script
{
    public class ScriptContext
    {
        public List<OBScriptVariable> Variables { get; set; }
        public List<ScriptArgument> Arguments { get; set; }
        public List<ScriptElement> Elements { get; set; }
        public Dictionary<string, AssemblyReference> ImportedNamespaces { get; set; }
        public List<Assembly> AssembliesList { get; set; }
        public List<string> NamespacesList { get; set; }
        public RSScript EngineScript { get; set; }
        public ScriptState EngineScriptState { get; set; }
        public string GuidPlaceholder { get; set; }

        public ScriptContext()
        {
            Variables = new List<OBScriptVariable>();
            Arguments = new List<ScriptArgument>();
            Elements = new List<ScriptElement>();
            ImportedNamespaces = new Dictionary<string, AssemblyReference>(ScriptDefaultNamespaces.DefaultNamespaces);

            AssembliesList = NamespaceMethods.GetAssemblies(ImportedNamespaces);
            NamespacesList = NamespaceMethods.GetNamespacesList(ImportedNamespaces);

            EngineScript = CSharpScript.Create("", ScriptOptions.Default.WithReferences(AssembliesList)
                                                                    .WithImports(NamespacesList));
     
            GuidPlaceholder = $"v{Guid.NewGuid()}".Replace("-", "");
        }

        public async Task ReinitializeEngineScript()
        {

            EngineScript = CSharpScript.Create("", ScriptOptions.Default.WithReferences(AssembliesList)
                                                                    .WithImports(NamespacesList));

            EngineScriptState = await EngineScript.RunAsync();
        }

        public async Task AddVariable(string varName, Type varType, string code)
        {
            if (string.IsNullOrEmpty(code))
                code = "null";

            if (EngineScriptState == null)
                EngineScriptState = await EngineScript.RunAsync();

            string script = $"{varType.GetRealTypeName()}? {varName} = {code};";

            EngineScriptState = await EngineScriptState
                .ContinueWithAsync(script, ScriptOptions.Default
                .WithReferences(AssembliesList)
                .WithImports(NamespacesList));
        }

        public async Task UpdateVariable(string varName, Type varType, string code)
        {
            if (string.IsNullOrEmpty(code))
                code = "null";

            var existingVariable = EngineScriptState.Variables.Where(x => x.Name == varName).LastOrDefault();

            if (existingVariable != null && (existingVariable.Type.GetRealTypeName() == varType.GetRealTypeName() ||
                                             existingVariable.Type.GetRealTypeName() == $"Nullable<{varType.GetRealTypeName()}>"))
            {
                if (EngineScriptState == null)
                    EngineScriptState = await EngineScript.RunAsync();

                string script = $"{varName} = {code};";

                EngineScriptState = await EngineScriptState
                   .ContinueWithAsync(script, ScriptOptions.Default
                   .WithReferences(AssembliesList)
                   .WithImports(NamespacesList));
            }
            else if (existingVariable != null)
            {
                var errors = await ResetEngineVariables();
                if (errors.Count > 0)
                    throw errors.Last();
            }
            else
                await AddVariable(varName, varType, code);
        }

        public async Task<List<Exception>> ResetEngineVariables()
        {
            List<Exception> errors = new List<Exception>();

            await ReinitializeEngineScript();

            foreach (var variable in Variables)
            {
                try
                {
                    await AddVariable(variable.VariableName, variable.VariableType, variable.VariableValue?.ToString());
                }
                catch (Exception ex)
                {
                    errors.Add(ex);
                }
            }

            foreach (var argument in Arguments)
            {
                try
                {
                    await AddVariable(argument.ArgumentName, argument.ArgumentType, argument.ArgumentValue.ToString());
                }
                catch (Exception ex)
                {
                    errors.Add(ex);
                }
            }

            return errors;
        }

        public async Task EvaluateInput(Type varType, string code)
        {
            if (string.IsNullOrEmpty(code))
                code = "null";

            if (EngineScriptState == null)
                EngineScriptState = await EngineScript.RunAsync();

            string type;
            if (varType.IsGenericType)
                type = "object";
            else
                type = varType.GetRealTypeName();

            string script = $"{type} {GuidPlaceholder} = {code};";

            EngineScriptState = await EngineScriptState
                .ContinueWithAsync(script, ScriptOptions.Default
                .WithReferences(AssembliesList)
                .WithImports(NamespacesList));

            var value = EngineScriptState.GetVariable($"{GuidPlaceholder}").Value;

            if (varType.IsGenericType && !value.GetType().FullName.StartsWith(varType.FullName))
                throw new Exception("Input value is an invalid Type.");
        }

        public async Task<object> InstantiateVariable(string varName, string code, Type varType)
        {
            string type = varType.GetRealTypeName();

            if (string.IsNullOrEmpty(code))
                code = "null";

            if (EngineScriptState == null)
                EngineScriptState = await EngineScript.RunAsync();

            string script = $"{type}? {varName} = {code};";

            EngineScriptState = await EngineScriptState
                .ContinueWithAsync(script, ScriptOptions.Default
                .WithReferences(AssembliesList)
                .WithImports(NamespacesList));

            return EngineScriptState.GetVariable(varName).Value;
        }

        public async Task<object> EvaluateCode(string code)
        {
            if (string.IsNullOrEmpty(code))
                return null;

            if (EngineScriptState == null)
                EngineScriptState = await EngineScript.RunAsync();

            string script = $"object {GuidPlaceholder} = {code};";

            EngineScriptState = await EngineScriptState
                .ContinueWithAsync(script, ScriptOptions.Default
                .WithReferences(AssembliesList)
                .WithImports(NamespacesList));

            return EngineScriptState.GetVariable($"{GuidPlaceholder}").Value;
        }
    }
}
