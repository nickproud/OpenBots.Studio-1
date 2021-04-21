using Autofac;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Script;
using Serilog.Core;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.Scripting;
using OBScriptVariable = OpenBots.Core.Script.ScriptVariable;
using RSScript = Microsoft.CodeAnalysis.Scripting.Script;
using System;
using System.Reflection;

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
        public Dictionary<string, object> AppInstances { get; set; }
        public Dictionary<string, AssemblyReference> ImportedNamespaces { get; set; }
        public List<Assembly> AssembliesList { get; set; }
        public List<string> NamespacesList { get; set; }
        public IfrmScriptEngine ScriptEngine { get; set; }
        public bool IsTest { get; set; } = false;
        public int StartFromLineNumber { get; set; } = 1;       
        public bool IsDebugMode { get; set; }
        public bool IsChildEngine { get; set; }
        public string GuidPlaceholder { get; set; }
        public RSScript EngineScript { get; set; }
        public ScriptState EngineScriptState { get; set; }

        public EngineContext()
        {
            GuidPlaceholder = $"v{Guid.NewGuid()}".Replace("-", "");
        }

        public EngineContext(string filePath, string projectPath, IContainer container, IfrmScriptBuilder scriptBuilder, Logger engineLogger,
            List<OBScriptVariable> variables, List<ScriptArgument> arguments, List<ScriptElement> elements, Dictionary<string, object> appInstances, 
            Dictionary<string, AssemblyReference> importedNamespaces, IfrmScriptEngine scriptEngine, int startFromLineNumber, bool isDebugMode, bool isChildEngine)
        {
            FilePath = filePath;
            ProjectPath = projectPath;
            Container = container;
            ScriptBuilder = scriptBuilder;
            EngineLogger = engineLogger;
            Variables = variables;
            Arguments = arguments;
            Elements = elements;
            AppInstances = appInstances;
            ImportedNamespaces = importedNamespaces;
            ScriptEngine = scriptEngine;
            StartFromLineNumber = startFromLineNumber;
            IsDebugMode = isDebugMode;
            IsChildEngine = isChildEngine;
            GuidPlaceholder = $"v{Guid.NewGuid()}".Replace("-", "");
        }
    }
}
