using Autofac;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Newtonsoft.Json;
using OpenBots.Core.Enums;
using OpenBots.Core.Interfaces;
using OpenBots.Core.Script;
using OpenBots.Core.Settings;
using OpenBots.Core.Utilities.CommonUtilities;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using OBScriptVariable = OpenBots.Core.Script.ScriptVariable;
using RSScript = Microsoft.CodeAnalysis.Scripting.Script;

namespace OpenBots.Core.Model.EngineModel
{
    public class EngineContext
    {
        public string FilePath { get; set; }
        public string ProjectPath { get; set; }
        public IContainer Container { get; set; }
        public IfrmScriptBuilder ScriptBuilder { get; set; }
        public Logger EngineLogger { get; set; }
        public List<OBScriptVariable> Variables { get; set; }
        public List<ScriptArgument> Arguments { get; set; }
        public List<ScriptElement> Elements { get; set; }
        public Dictionary<string, List<AssemblyReference>> ImportedNamespaces { get; set; }
        public List<Assembly> AssembliesList { get; set; }
        public List<string> NamespacesList { get; set; }
        public EngineSettings EngineSettings { get; set; }
        public IfrmScriptEngine ScriptEngine { get; set; }
        public EngineStatus CurrentEngineStatus { get; set; }
        public Dictionary<string, object> SessionVariables { get; set; }
        public bool IsTest { get; set; } = false;
        public int StartFromLineNumber { get; set; } = 1;       
        public bool IsDebugMode { get; set; }
        public bool IsChildEngine { get; set; }
        public bool IsServerExecution{ get; set; }
        public bool IsServerChildExecution { get; set; }
        public bool IsScheduledOrAttendedTask { get; set; }
        public string TaskResult { get; set; } = "";
        public string GuidPlaceholder { get; set; }
        public RSScript EngineScript { get; set; }
        public ScriptState EngineScriptState { get; set; }

        public EngineContext()
        {
            GuidPlaceholder = GenerateGuidPlaceHolder();
        }

        public EngineContext(string filePath, string projectPath, IContainer container, IfrmScriptBuilder scriptBuilder, Logger engineLogger,
            List<OBScriptVariable> variables, List<ScriptArgument> arguments, List<ScriptElement> elements, Dictionary<string, List<AssemblyReference>> importedNamespaces,
            Dictionary<string, object> sessionVariables, IfrmScriptEngine scriptEngine, int startFromLineNumber, bool isDebugMode, bool isChildEngine)
        {
            FilePath = filePath;
            ProjectPath = projectPath;
            Container = container;
            ScriptBuilder = scriptBuilder;
            EngineLogger = engineLogger;
            Variables = variables;
            Arguments = arguments;
            Elements = elements;
            ImportedNamespaces = importedNamespaces;
            SessionVariables = sessionVariables;
            ScriptEngine = scriptEngine;
            StartFromLineNumber = startFromLineNumber;
            IsDebugMode = isDebugMode;
            IsChildEngine = isChildEngine;
            GuidPlaceholder = GenerateGuidPlaceHolder();
        }

        public EngineContext(IScriptContext scriptContext, string projectPath)
        {
            Variables = new List<OBScriptVariable>((List<OBScriptVariable>)CommonMethods.Clone(scriptContext.Variables));
            Variables.Where(x => x.VariableName == "ProjectPath").FirstOrDefault().VariableValue = "@\"" + projectPath + '"';
            Arguments = new List<ScriptArgument>(scriptContext.Arguments);
            AssembliesList = new List<Assembly>(scriptContext.AssembliesList);
            NamespacesList = new List<string>(scriptContext.NamespacesList);
            EngineScript = CSharpScript.Create("", ScriptOptions.Default.WithReferences(AssembliesList).WithImports(NamespacesList));
            GuidPlaceholder = GenerateGuidPlaceHolder();
        }

        public void InitializeEngineInstance()
        {
            AssembliesList = NamespaceMethods.GetAssemblies(ImportedNamespaces);
            NamespacesList = NamespaceMethods.GetNamespacesList(ImportedNamespaces);
            EngineScript = CSharpScript.Create("", ScriptOptions.Default.WithReferences(AssembliesList).WithImports(NamespacesList));
            EngineScriptState = null;       
        }

        public async Task InitializeEngineInstanceTest(string scriptFilePath)
        {
            //different fix for type deserialization. Makes opening files slow but can be used if the other fix fails
            string scriptFileText = File.ReadAllText(scriptFilePath);
            dynamic scriptFileObj = JsonConvert.DeserializeObject(scriptFileText);

            ImportedNamespaces = JsonConvert.DeserializeObject<Dictionary<string, List<AssemblyReference>>>(JsonConvert.SerializeObject(scriptFileObj["ImportedNamespaces"]));
            AssembliesList = NamespaceMethods.GetAssemblies(ImportedNamespaces);
            NamespacesList = NamespaceMethods.GetNamespacesList(ImportedNamespaces);
            EngineScript = CSharpScript.Create("", ScriptOptions.Default.WithReferences(AssembliesList).WithImports(NamespacesList));
            EngineScriptState = await EngineScript.RunAsync();
            EngineScriptState = await EngineScriptState.ContinueWithAsync($"string {GenerateGuidPlaceHolder()} = \"\";");
        }

        public string GenerateGuidPlaceHolder()
        {
            return $"v{Guid.NewGuid()}".Replace("-", "");
        }
    }
}
