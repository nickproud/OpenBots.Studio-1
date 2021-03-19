using Autofac;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Script;
using Serilog.Core;
using System.Collections.Generic;

namespace OpenBots.Core.Model.EngineModel
{
    public class EngineContext
    {
        public string FilePath { get; set; }
        public string ProjectPath { get; set; }
        public IContainer Container { get; set; }
        public IfrmScriptBuilder ScriptBuilder { get; set; }
        public Logger EngineLogger { get; set; }
        public List<ScriptVariable> Variables { get; set; }
        public List<ScriptArgument> Arguments { get; set; }
        public List<ScriptElement> Elements { get; set; }
        public Dictionary<string, object> AppInstances { get; set; }
        public IfrmScriptEngine ScriptEngine { get; set; }
        public bool IsTest { get; set; } = false;
        public int StartFromLineNumber { get; set; } = 1;

        public EngineContext()
        {

        }

        public EngineContext(string filePath, string projectPath, IContainer container, IfrmScriptBuilder scriptBuilder, Logger engineLogger,
            List<ScriptVariable> variables, List<ScriptArgument> arguments, List<ScriptElement> elements, Dictionary<string, object> appInstances, 
            IfrmScriptEngine scriptEngine, int startFromLineNumber)
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
            ScriptEngine = scriptEngine;
            StartFromLineNumber = startFromLineNumber;
        }
    }
}
