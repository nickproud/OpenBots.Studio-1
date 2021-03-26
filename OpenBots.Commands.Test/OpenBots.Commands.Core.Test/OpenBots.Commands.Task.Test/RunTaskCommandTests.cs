using Newtonsoft.Json;
using OpenBots.Core.Script;
using OpenBots.Core.Enums;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using Xunit;
using Xunit.Abstractions;
using OBIO = System.IO;

namespace OpenBots.Commands.Task.Test
{
    public class RunTaskCommandTests
    {
        private AutomationEngineInstance _engine;
        private RunTaskCommand _runTask;
        private Variable.SetVariableCommand _setVariable;
        private TextFile.WriteCreateTextFileCommand _textFile;
        private Script _taskScript;
        private readonly ITestOutputHelper output;

        public RunTaskCommandTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void RunsTask()
        {
            _engine = new AutomationEngineInstance(null);
            _runTask = new RunTaskCommand();
            _textFile = new TextFile.WriteCreateTextFileCommand();
            _setVariable = new Variable.SetVariableCommand();
            _taskScript = new Script();

            List<ScriptVariable> variables = new List<ScriptVariable>();
            ScriptVariable var1 = new ScriptVariable();
            var1.VariableName = "output";
            var1.VariableValue = "outputValue";
            var1.VariableType = typeof(string);
            variables.Add(var1);
            
            _taskScript.Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            // set script variables
            _taskScript.Variables = variables;

            List<ScriptArgument> arguments = new List<ScriptArgument>();
            ScriptArgument arg1 = new ScriptArgument();
            arg1.ArgumentName = "inputArg";
            arg1.Direction = ScriptArgumentDirection.In;
            arg1.ArgumentValue = "default";
            arg1.ArgumentType = typeof(string);
            arguments.Add(arg1);
            ScriptArgument arg2 = new ScriptArgument();
            arg2.ArgumentName = "outputArg";
            arg2.Direction = ScriptArgumentDirection.Out;
            arg2.ArgumentValue = "default";
            arg2.AssignedVariable = "{outputVar}";
            arg2.ArgumentType = typeof(string);
            arguments.Add(arg2);

            // set script arguments
            _taskScript.Arguments = arguments;

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string filePath = Path.Combine(projectDirectory, @"Resources");
            _textFile.v_FilePath = Path.Combine(filePath, @"test.txt");
            _textFile.v_TextToWrite = "{inputArg}";
            _textFile.v_Overwrite = "Overwrite";
            _engine.AutomationEngineContext.FilePath = Path.Combine(filePath, "task.obscript");
            _engine.AutomationEngineContext.IsTest = true;
            List<ScriptAction> commands = new List<ScriptAction>();
            ScriptAction com1 = new ScriptAction();
            com1.ScriptCommand = _textFile;
            commands.Add(com1);

            _setVariable.v_Input = "outputValue";
            _setVariable.v_OutputUserVariableName = "{outputArg}";
            ScriptAction com2 = new ScriptAction();
            com2.ScriptCommand = _setVariable;
            commands.Add(com2);

            _taskScript.Commands = commands;

            //write to file
            var serializerSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Objects,
            };
            JsonSerializer serializer = JsonSerializer.Create(serializerSettings);
            using (StreamWriter sw = new StreamWriter(_engine.AutomationEngineContext.FilePath))
            using (JsonWriter writer = new JsonTextWriter(sw) { Formatting = Formatting.Indented })
            {
                serializer.Serialize(writer, _taskScript, typeof(Script));
            }

            DataTable argumentTable = new DataTable();
            argumentTable.Columns.Add("ArgumentName");
            argumentTable.Columns.Add("ArgumentType");
            argumentTable.Columns.Add("ArgumentValue");
            argumentTable.Columns.Add("ArgumentDirection");
            argumentTable.Columns[1].DataType = typeof(Type);
            DataRow arg1row = argumentTable.NewRow();
            arg1row["ArgumentName"] = "inputArg";
            arg1row["ArgumentType"] = typeof(string);
            arg1row["ArgumentValue"] = "{taskInput}";
            arg1row["ArgumentDirection"] = "In";
            DataRow arg2row = argumentTable.NewRow();
            arg2row["ArgumentName"] = "outputArg";
            arg2row["ArgumentType"] = typeof(string);
            arg2row["ArgumentValue"] = "{outputVar}";
            arg2row["ArgumentDirection"] = "Out";
            argumentTable.Rows.Add(arg1row);
            argumentTable.Rows.Add(arg2row);

            _runTask.v_TaskPath = _engine.AutomationEngineContext.FilePath;
            _runTask.v_AssignArguments = true;
            _runTask.v_ArgumentAssignments = argumentTable;

            List<ScriptAction> mainCommands = new List<ScriptAction>();
            ScriptAction runTaskAction = new ScriptAction();
            runTaskAction.ScriptCommand = _runTask;

            VariableMethods.CreateTestVariable("inputValue", _engine, "taskInput", typeof(string));
            VariableMethods.CreateTestVariable("default", _engine, "taskOutput", typeof(string));
            VariableMethods.CreateTestVariable(null, _engine, "outputVar", typeof(string));

            _engine.ExecuteCommand(runTaskAction);

            Assert.Equal("outputValue", "{outputVar}".ConvertUserVariableToString(_engine));
            Assert.True(OBIO.File.Exists(Path.Combine(filePath, @"test.txt")));
            Assert.Equal("inputValue", OBIO.File.ReadAllText(Path.Combine(filePath, @"test.txt")));

            OBIO.File.Delete(Path.Combine(filePath, @"test.txt"));
            OBIO.File.Delete(Path.Combine(filePath, @"task.obscript"));
        }
    }
}
