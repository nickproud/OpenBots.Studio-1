using OpenBots.Core.Interfaces;
using OpenBots.Core.Model.EngineModel;
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
        public async static Task<object> InstantiateVariable(string varName, string code, Type varType, EngineContext engineContext)
        {
            string type = varType.GetRealTypeFullName();

            if (string.IsNullOrEmpty(code))
                code = "null";

            if (engineContext.EngineScriptState == null)
                engineContext.EngineScriptState = await engineContext.EngineScript.RunAsync();

            string script = $"{type}? {varName} = {code};";

            engineContext.EngineScriptState = await engineContext.EngineScriptState
                .ContinueWithAsync(script);

            return engineContext.EngineScriptState.GetVariable(varName).Value;
        }

        public async static Task<bool> EvaluateSnippet(this string code, IAutomationEngineInstance engine)
        {
            if (engine.EngineContext.EngineScriptState == null)
                engine.EngineContext.EngineScriptState = await engine.EngineContext.EngineScript.RunAsync();

            string script = $"{code};";

            engine.EngineContext.EngineScriptState = await engine.EngineContext.EngineScriptState
                .ContinueWithAsync(script);

            return true;
        }

        public async static Task<object> EvaluateCode(this string code, IAutomationEngineInstance engine)
        {
            if (string.IsNullOrEmpty(code))
                return null;

            if (engine.EngineContext.EngineScriptState == null)
                engine.EngineContext.EngineScriptState = await engine.EngineContext.EngineScript.RunAsync();

            string script = $"object {engine.EngineContext.GuidPlaceholder} = {code};";

            engine.EngineContext.EngineScriptState = await engine.EngineContext.EngineScriptState
                .ContinueWithAsync(script);

            return engine.EngineContext.EngineScriptState.GetVariable($"{engine.EngineContext.GuidPlaceholder}").Value;
        }

        public async static Task<object> EvaluateCode(this string code, EngineContext engineContext)
        {
            if (string.IsNullOrEmpty(code))
                return null;

            if (engineContext.EngineScriptState == null)
                engineContext.EngineScriptState = await engineContext.EngineScript.RunAsync();

            string script = $"object {engineContext.GuidPlaceholder} = {code};";

            engineContext.EngineScriptState = await engineContext.EngineScriptState
                .ContinueWithAsync(script);

            return engineContext.EngineScriptState.GetVariable($"{engineContext.GuidPlaceholder}").Value;
        }

        public static void SetVariableValue(this object newVal, IAutomationEngineInstance engine, string varName)
        {
            engine.EngineContext.EngineScriptState.Variables.Where(x => x.Name == varName).FirstOrDefault().Value = newVal;

            var existingVar = engine.EngineContext.Variables.Where(x => x.VariableName == varName).FirstOrDefault();
            if (existingVar != null)
                existingVar.VariableValue = newVal;
            var existingArg = engine.EngineContext.Arguments.Where(x => x.ArgumentName == varName).FirstOrDefault();
            if (existingArg != null)
                existingArg.ArgumentValue = newVal;
        }

        public static void SetVariableValue(this object newVal, EngineContext engineContext, string varName)
        {
            engineContext.EngineScriptState.Variables.Where(x => x.Name == varName).FirstOrDefault().Value = newVal;

            var existingVar = engineContext.Variables.Where(x => x.VariableName == varName).FirstOrDefault();
            if (existingVar != null)
                existingVar.VariableValue = newVal;
            var existingArg = engineContext.Arguments.Where(x => x.ArgumentName == varName).FirstOrDefault();
            if (existingArg != null)
                existingArg.ArgumentValue = newVal;
        }

        public static dynamic GetVariableValue(this string varName, IAutomationEngineInstance engine)
        {
            return engine.EngineContext.EngineScriptState.GetVariable(varName).Value;
        }

        public static void SyncVariableValues(IAutomationEngineInstance engine)
        {
            engine.EngineContext.Variables.ForEach(v => v.VariableValue = v.VariableName.GetVariableValue(engine));
            engine.EngineContext.Arguments.ForEach(a => a.ArgumentValue = a.ArgumentName.GetVariableValue(engine));
        }

        public static Type GetVarArgType(this string varArgName, IAutomationEngineInstance engine)
        {
            OBScriptVariable requiredVariable;

            var variableList = engine.EngineContext.Variables;
            var argumentsAsVariablesList = engine.EngineContext.Arguments
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
            engine.EngineContext.Variables.Add(newVar);
        }
    }
}
