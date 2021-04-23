using Microsoft.CodeAnalysis.Scripting;
using OpenBots.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Security;
using System.Threading.Tasks;
using OBScriptVariable = OpenBots.Core.Script.ScriptVariable;

namespace OpenBots.Core.Utilities.CommonUtilities
{
    public static class VariableMethods
    {
        public async static Task<object> InstantiateVariable(string varName, string code, Type varType, IAutomationEngineInstance engine)
        {
            string type = varType.GetRealTypeName();

            if (string.IsNullOrEmpty(code))
                code = "null";

            if (engine.AutomationEngineContext.EngineScriptState == null)
                engine.AutomationEngineContext.EngineScriptState = await engine.AutomationEngineContext.EngineScript.RunAsync();

            string script = $"{type}? {varName} = {code};";

            engine.AutomationEngineContext.EngineScriptState = await engine.AutomationEngineContext.EngineScriptState
                .ContinueWithAsync(script, ScriptOptions.Default
                .WithReferences(engine.AutomationEngineContext.AssembliesList)
                .WithImports(engine.AutomationEngineContext.NamespacesList));

            return engine.AutomationEngineContext.EngineScriptState.GetVariable(varName).Value;
        }

        public async static Task<bool> EvaluateSnippet(this string code, IAutomationEngineInstance engine)
        {
            if (engine.AutomationEngineContext.EngineScriptState == null)
                engine.AutomationEngineContext.EngineScriptState = await engine.AutomationEngineContext.EngineScript.RunAsync();

            string script = $"{code};";

            engine.AutomationEngineContext.EngineScriptState = await engine.AutomationEngineContext.EngineScriptState
                .ContinueWithAsync(script, ScriptOptions.Default
                .WithReferences(engine.AutomationEngineContext.AssembliesList)
                .WithImports(engine.AutomationEngineContext.NamespacesList));

            return true;
        }

        public async static Task<object> EvaluateCode(this string code, IAutomationEngineInstance engine)
        {
            if (string.IsNullOrEmpty(code))
                return null;

            if (engine.AutomationEngineContext.EngineScriptState == null)
                engine.AutomationEngineContext.EngineScriptState = await engine.AutomationEngineContext.EngineScript.RunAsync();

            string script = $"object {engine.AutomationEngineContext.GuidPlaceholder} = {code};";

            engine.AutomationEngineContext.EngineScriptState = await engine.AutomationEngineContext.EngineScriptState
                .ContinueWithAsync(script, ScriptOptions.Default
                .WithReferences(engine.AutomationEngineContext.AssembliesList)
                .WithImports(engine.AutomationEngineContext.NamespacesList));

            return engine.AutomationEngineContext.EngineScriptState.GetVariable($"{engine.AutomationEngineContext.GuidPlaceholder}").Value;
        }

        public static void SetVariableValue(this object newVal, IAutomationEngineInstance engine, string varName)
        {
            engine.AutomationEngineContext.EngineScriptState.Variables.Where(x => x.Name == varName).FirstOrDefault().Value = newVal;

            var existingVar = engine.AutomationEngineContext.Variables.Where(x => x.VariableName == varName).FirstOrDefault();
            if (existingVar != null)
                existingVar.VariableValue = newVal;
            var existingArg = engine.AutomationEngineContext.Arguments.Where(x => x.ArgumentName == varName).FirstOrDefault();
            if (existingArg != null)
                existingArg.ArgumentValue = newVal;
        }

        public static dynamic GetVariableValue(this string varName, IAutomationEngineInstance engine)
        {
            return engine.AutomationEngineContext.EngineScriptState.GetVariable(varName).Value;
        }

        public static Type GetVariableType(this string varName, IAutomationEngineInstance engine)
        {
            return engine.AutomationEngineContext.EngineScriptState.GetVariable(varName).Type;
        } 

        public static void SyncVariableValues(IAutomationEngineInstance engine)
        {
            engine.AutomationEngineContext.Variables.ForEach(v => v.VariableValue = v.VariableName.GetVariableValue(engine));
            engine.AutomationEngineContext.Arguments.ForEach(a => a.ArgumentValue = a.ArgumentName.GetVariableValue(engine));
        }

        public static Type GetVarArgType(this string varArgName, IAutomationEngineInstance engine)
        {
            OBScriptVariable requiredVariable;

            var variableList = engine.AutomationEngineContext.Variables;
            var argumentsAsVariablesList = engine.AutomationEngineContext.Arguments
                .Select(arg => new OBScriptVariable
                {
                    VariableName = arg.ArgumentName,
                    VariableType = arg.ArgumentType,
                    VariableValue = arg.ArgumentValue
                })
                .ToList();

            var variableSearchList = new List<OBScriptVariable>();
            variableSearchList.AddRange(variableList);
            variableSearchList.AddRange(argumentsAsVariablesList);

            requiredVariable = variableSearchList.Where(var => var.VariableName == varArgName).FirstOrDefault();

            if (requiredVariable != null)
                return requiredVariable.VariableType;
            else
                return null;
        }

        public static SecureString ConvertStringToSecureString(this string value)
        {
            SecureString secureString = new NetworkCredential(string.Empty, value).SecurePassword;
            return secureString;
        }

        public static string ConvertSecureStringToString(this SecureString secureString)
        {
            string strValue = new NetworkCredential(string.Empty, secureString).Password;
            return strValue;
        }

        public static void CreateTestVariable(object variableValue, IAutomationEngineInstance engine, string variableName, Type variableType)
        {
            OBScriptVariable newVar = new OBScriptVariable();
            newVar.VariableName = variableName;
            newVar.VariableValue = variableValue;
            newVar.VariableType = variableType;
            engine.AutomationEngineContext.Variables.Add(newVar);
        }
    }
}
