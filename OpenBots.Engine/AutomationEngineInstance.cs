using Autofac;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Newtonsoft.Json;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Interfaces;
using OpenBots.Core.IO;
using OpenBots.Core.Model.EngineModel;
using OpenBots.Core.Script;
using OpenBots.Core.Settings;
using OpenBots.Core.Utilities.CommonUtilities;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OBScript = OpenBots.Core.Script.Script;
using OBScriptVariable = OpenBots.Core.Script.ScriptVariable;

namespace OpenBots.Engine
{
    public class AutomationEngineInstance : IAutomationEngineInstance
    {
        //engine variables
        public EngineContext EngineContext { get; set; } = new EngineContext();  
        public List<ScriptError> ErrorsOccured { get; set; }
        public string ErrorHandlingAction { get; set; }
        public bool ChildScriptFailed { get; set; }
        public bool ChildScriptErrorCaught { get; set; }
        public ScriptCommand LastExecutedCommand { get; set; }
        public bool IsCancellationPending { get; set; }
        public bool CurrentLoopCancelled { get; set; }
        public bool CurrentLoopContinuing { get; set; }
        public bool _isScriptPaused { get; private set; }
        private bool _isScriptSteppedOver { get; set; }
        private bool _isScriptSteppedInto { get; set; }
        private bool _isScriptSteppedOverBeforeException { get; set; }
        private bool _isScriptSteppedIntoBeforeException { get; set; }
        [JsonIgnore]
        private Stopwatch _stopWatch { get; set; }
        public string PrivateCommandLog { get; set; }
        //events
        public event EventHandler<ReportProgressEventArgs> ReportProgressEvent;
        public event EventHandler<ScriptFinishedEventArgs> ScriptFinishedEvent;
        public event EventHandler<LineNumberChangedEventArgs> LineNumberChangedEvent;

        public AutomationEngineInstance(EngineContext engineContext)
        {
            if (engineContext != null)
                EngineContext = engineContext;

            //initialize logger
            if (EngineContext.EngineLogger != null)
            {
                Log.Logger = EngineContext.EngineLogger;
                Log.Information("Engine Class has been initialized");
            }
            
            PrivateCommandLog = "Can't log display value as the command contains sensitive data";

            //initialize error tracking list
            ErrorsOccured = new List<ScriptError>();

            //set to initialized
            EngineContext.CurrentEngineStatus = EngineStatus.Loaded;

            //get engine settings
            EngineContext.EngineSettings = new ApplicationSettings().GetOrCreateApplicationSettings().EngineSettings;

            if (EngineContext.Variables == null)
                EngineContext.Variables = new List<OBScriptVariable>();

            if (EngineContext.Arguments == null)
                EngineContext.Arguments = new List<ScriptArgument>();

            if (EngineContext.Elements == null)
                EngineContext.Elements = new List<ScriptElement>();

            if (EngineContext.ImportedNamespaces == null)
                EngineContext.ImportedNamespaces = new Dictionary<string, List<AssemblyReference>>(ScriptDefaultNamespaces.DefaultNamespaces);

            if (EngineContext.SessionVariables == null)
                EngineContext.SessionVariables = new Dictionary<string, object>();

            ErrorHandlingAction = string.Empty;          
        }

        public IAutomationEngineInstance CreateAutomationEngineInstance(EngineContext engineContext)
        {
            return new AutomationEngineInstance(engineContext);
        }

        public void ExecuteScriptSync()
        {
            Log.Information("Client requesting to execute script independently");
            EngineContext.IsServerExecution = true;
            ExecuteScript();
        }

        public void ExecuteScriptAsync()
        {
            Log.Information("Client requesting to execute script using frmEngine");

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                ExecuteScript();
            }).Start();
        }

        private async void ExecuteScript()
        {
            try
            {
                EngineContext.CurrentEngineStatus = EngineStatus.Running;

                //create stopwatch for metrics tracking
                _stopWatch = new Stopwatch();
                _stopWatch.Start();

                //log starting
                ReportProgress("Bot Engine Started: " + DateTime.Now.ToString());

                //get automation script
                OBScript automationScript; 

                ReportProgress("Deserializing File");
                Log.Information("Script Path: " + EngineContext.FilePath);
                automationScript = OBScript.DeserializeFile(EngineContext);

                //initialize roslyn instance
                EngineContext.ImportedNamespaces = automationScript.ImportedNamespaces;
                EngineContext.InitializeEngineInstance();

                ReportProgress("Creating Variable List");

                //set variables if they were passed in
                if (EngineContext.Variables != null)
                {
                    foreach (var var in EngineContext.Variables)
                    {
                        var variableFound = automationScript.Variables.Where(f => f.VariableName == var.VariableName).FirstOrDefault();

                        if (variableFound != null)
                            variableFound.VariableValue = var.VariableValue;
                    }
                }

                EngineContext.Variables = automationScript.Variables;
                
                //update ProjectPath variable
                var projectPathVariable = EngineContext.Variables.Where(v => v.VariableName == "ProjectPath").SingleOrDefault();

                if (projectPathVariable != null)
                    projectPathVariable.VariableValue = "@\"" + EngineContext.ProjectPath + '"';
                else
                {
                    projectPathVariable = new OBScriptVariable
                    {
                        VariableName = "ProjectPath",
                        VariableType = typeof(string),
                        VariableValue = "@\"" + EngineContext.ProjectPath + '"'
                    };
                    EngineContext.Variables.Add(projectPathVariable);
                }

                foreach (OBScriptVariable var in EngineContext.Variables)
                {
                    dynamic evaluatedValue = await VariableMethods.InstantiateVariable(var.VariableName, (string)var.VariableValue, var.VariableType, EngineContext);
                    VariableMethods.SetVariableValue(evaluatedValue, EngineContext, var.VariableName);
                }           

                ReportProgress("Creating Argument List");

                //set arguments if they were passed in
                if (EngineContext.Arguments != null)
                {
                    foreach (var arg in EngineContext.Arguments)
                    {
                        var argumentFound = automationScript.Arguments.Where(f => f.ArgumentName == arg.ArgumentName).FirstOrDefault();

                        if (argumentFound != null)
                            argumentFound.ArgumentValue = arg.ArgumentValue;
                    }
                }

                EngineContext.Arguments = automationScript.Arguments;
                
                //used by RunTaskCommand to assign parent values to child arguments 
                if(EngineContext.IsChildEngine)
                {
                    foreach (ScriptArgument arg in EngineContext.Arguments)
                    {
                        await VariableMethods.InstantiateVariable(arg.ArgumentName, "", arg.ArgumentType, EngineContext);
                        VariableMethods.SetVariableValue(arg.ArgumentValue, EngineContext, arg.ArgumentName);
                    }
                }
                else
                {
                    foreach (ScriptArgument arg in EngineContext.Arguments)
                    {
                        dynamic evaluatedValue = await VariableMethods.InstantiateVariable(arg.ArgumentName, (string)arg.ArgumentValue, arg.ArgumentType, EngineContext);
                        VariableMethods.SetVariableValue(evaluatedValue, EngineContext, arg.ArgumentName);
                    }
                }

                ReportProgress("Creating Element List");

                //set elements if they were passed in
                if (EngineContext.Elements != null)
                {
                    foreach (var elem in EngineContext.Elements)
                    {
                        var elementFound = automationScript.Elements.Where(f => f.ElementName == elem.ElementName).FirstOrDefault();

                        if (elementFound != null)
                            elementFound.ElementValue = elem.ElementValue;
                    }
                }

                EngineContext.Elements = automationScript.Elements;

                //execute commands
                ScriptAction startCommand = automationScript.Commands.Where(x => x.ScriptCommand.LineNumber <= EngineContext.StartFromLineNumber)
                                                                     .Last();

                int startCommandIndex = automationScript.Commands.FindIndex(x => x.ScriptCommand.LineNumber == startCommand.ScriptCommand.LineNumber);

                while (startCommandIndex < automationScript.Commands.Count)
                {
                    if (IsCancellationPending)
                    {
                        ReportProgress("Cancelling Script");
                        ScriptFinished(ScriptFinishedResult.Cancelled);
                        return;
                    }

                    await ExecuteCommand(automationScript.Commands[startCommandIndex]);
                    startCommandIndex++;
                }

                if (IsCancellationPending)
                    ScriptFinished(ScriptFinishedResult.Cancelled);
                else
                    ScriptFinished(ScriptFinishedResult.Successful);
            }
            catch (Exception ex)
            {
                ScriptFinished(ScriptFinishedResult.Error, ex.ToString());
            }

            if(!EngineContext.IsChildEngine || (EngineContext.IsServerExecution && !EngineContext.IsServerChildExecution))
                EngineContext.EngineLogger.Dispose();
        }

        public async Task ExecuteCommand(ScriptAction command)
        {
            //get command
            ScriptCommand parentCommand = command.ScriptCommand;

            if (parentCommand == null)
                return;

            //in RunFromThisCommand exection, determine if/while logic. If logic returns true, skip until reaching the selected command
            if (!parentCommand.ScopeStartCommand && parentCommand.LineNumber < EngineContext.StartFromLineNumber)
                return;
            //if the selected command is within a while/retry, reset starting line number so that previous commands within the scope run in the following iteration
            else if (!parentCommand.ScopeStartCommand && parentCommand.LineNumber == EngineContext.StartFromLineNumber)
                EngineContext.StartFromLineNumber = 1;

            if (EngineContext.ScriptEngine != null && (parentCommand.CommandName == "RunTaskCommand" || parentCommand.CommandName == "ShowMessageCommand"))
                parentCommand.CurrentScriptBuilder = EngineContext.ScriptEngine.EngineContext.ScriptBuilder;

            //set LastCommandExecuted
            LastExecutedCommand = command.ScriptCommand;

            //update execution line numbers
            LineNumberChanged(parentCommand.LineNumber);

            //handle pause request
            if (EngineContext.ScriptEngine != null && parentCommand.PauseBeforeExecution && EngineContext.IsDebugMode && !ChildScriptFailed)
            {
                ReportProgress("Pausing Before Execution");
                _isScriptPaused = true;
                EngineContext.ScriptEngine.IsHiddenTaskEngine = false;
            }

            //handle pause
            bool isFirstWait = true;
            while (_isScriptPaused)
            {
                //only show pause first loop
                if (isFirstWait)
                {
                    EngineContext.CurrentEngineStatus = EngineStatus.Paused;
                    ReportProgress("Paused on Line " + parentCommand.LineNumber + ": "
                        + (parentCommand.v_IsPrivate ? PrivateCommandLog : parentCommand.GetDisplayValue()));

                    ReportProgress("[Please select 'Resume' when ready]");
                    isFirstWait = false;
                }

                if (_isScriptSteppedInto && parentCommand.CommandName == "RunTaskCommand")
                {
                    parentCommand.IsSteppedInto = true;
                    parentCommand.CurrentScriptBuilder = EngineContext.ScriptEngine.EngineContext.ScriptBuilder;
                    _isScriptSteppedInto = false;
                    EngineContext.ScriptEngine.IsHiddenTaskEngine = true;
                    
                    break;
                }
                else if (_isScriptSteppedOver || _isScriptSteppedInto)
                {
                    _isScriptSteppedOver = false;
                    _isScriptSteppedInto = false;
                    break;
                }

                if (((Form)EngineContext.ScriptEngine).IsDisposed)
                {
                    IsCancellationPending = true;
                    break;
                }
                                  
                //wait
                Thread.Sleep(1000);
            }

            EngineContext.CurrentEngineStatus = EngineStatus.Running;

            //handle if cancellation was requested
            if (IsCancellationPending)
                return;

            //If Child Script Failed and Child Script Error not Caught, next command should not be executed
            if (ChildScriptFailed && !ChildScriptErrorCaught)
                throw new Exception("Child Script Failed");

            //bypass comments
            if (parentCommand.CommandName == "AddCodeCommentCommand" || parentCommand.CommandName == "BrokenCodeCommentCommand" || parentCommand.IsCommented)             
                return;

            //report intended execution
            if (parentCommand.CommandName != "LogMessageCommand")
                ReportProgress($"Running Line {parentCommand.LineNumber}: {(parentCommand.v_IsPrivate ? PrivateCommandLog : parentCommand.GetDisplayValue())}");

            //handle any errors
            try
            {
                //determine type of command
                if ((parentCommand.CommandName == "LoopNumberOfTimesCommand") || (parentCommand.CommandName == "LoopContinuouslyCommand") ||
                    (parentCommand.CommandName == "BeginForEachCommand") || (parentCommand.CommandName == "BeginIfCommand") ||
                    (parentCommand.CommandName == "BeginMultiIfCommand") || (parentCommand.CommandName == "BeginTryCommand") ||
                    (parentCommand.CommandName == "BeginWhileCommand") || (parentCommand.CommandName == "BeginMultiWhileCommand") ||
                    (parentCommand.CommandName == "BeginDoWhileCommand") || (parentCommand.CommandName == "BeginRetryCommand") || 
                    (parentCommand.CommandName == "BeginSwitchCommand"))
                {
                    //run the command and pass bgw/command as this command will recursively call this method for sub commands
                    //TODO: Make sure that removing these lines doesn't create any other issues
                    //command.IsExceptionIgnored = true;
                    await parentCommand.RunCommand(this, command);
                }
                else if (parentCommand.CommandName == "SequenceCommand")
                {
                    //command.IsExceptionIgnored = true;
                    await parentCommand.RunCommand(this, command);
                }
                else if (parentCommand.CommandName == "StopCurrentTaskCommand")
                {
                    if (EngineContext.ScriptEngine != null && EngineContext.ScriptEngine.EngineContext.ScriptBuilder != null)
                        EngineContext.ScriptEngine.EngineContext.ScriptBuilder.IsScriptRunning = false;

                    IsCancellationPending = true;
                    return;
                }
                else if (parentCommand.CommandName == "BreakCommand")
                    CurrentLoopCancelled = true;
                else if (parentCommand.CommandName == "ContinueCommand")
                    CurrentLoopContinuing = true;
                else
                {
                    //sleep required time
                    Thread.Sleep(EngineContext.EngineSettings.DelayBetweenCommands);

                    if (!parentCommand.v_ErrorHandling.Equals("None"))
                        ErrorHandlingAction = parentCommand.v_ErrorHandling;
                    else
                        ErrorHandlingAction = string.Empty;

                    //run the command
                    try
                    {
                        await parentCommand.RunCommand(this);
                    }
                    catch (Exception ex)
                    {
                        switch (ErrorHandlingAction)
                        {
                            case "Ignore Error":
                                ReportProgress("Error Occured at Line " + parentCommand.LineNumber + ":" + ex.ToString(), 
                                    Enum.GetName(typeof(LogEventLevel), LogEventLevel.Error));
                                ReportProgress("Ignoring Per Error Handling");
                                break;
                            case "Report Error":
                                ReportProgress("Error Occured at Line " + parentCommand.LineNumber + ":" + ex.ToString(), 
                                    Enum.GetName(typeof(LogEventLevel), LogEventLevel.Error));
                                ReportProgress("Handling Error and Attempting to Continue");
                                throw ex;
                            default:
                                throw ex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (!(LastExecutedCommand.CommandName == "RethrowCommand"))
                {
                    if (ChildScriptFailed)
                    {
                        ChildScriptFailed = false;
                        ErrorsOccured.Clear();
                    }

                    ErrorsOccured.Add(new ScriptError()
                    {
                        SourceFile = EngineContext.FilePath,
                        LineNumber = parentCommand.LineNumber,
                        StackTrace = ex.ToString(),
                        ErrorType = ex.GetType().Name,
                        ErrorMessage = ex.Message
                    });
                }

                var error = ErrorsOccured.OrderByDescending(x => x.LineNumber).FirstOrDefault();
                string errorMessage = $"Source: {error.SourceFile}, Line: {error.LineNumber} {parentCommand.GetDisplayValue()}, " +
                        $"Exception Type: {error.ErrorType}, Exception Message: {error.ErrorMessage}";

                if (EngineContext.ScriptEngine != null && !command.IsExceptionIgnored && EngineContext.IsDebugMode)
                {
                    //load error form if exception is not handled
                    EngineContext.ScriptEngine.EngineContext.ScriptBuilder.IsUnhandledException = true;
                    EngineContext.ScriptEngine.AddStatus("Pausing Before Exception");

                    DialogResult result = DialogResult.OK;

                    if (ErrorHandlingAction != "Ignore Error")
                        result = EngineContext.ScriptEngine.EngineContext.ScriptBuilder.LoadErrorForm(errorMessage);

                    ReportProgress("Error Occured at Line " + parentCommand.LineNumber + ":" + ex.ToString(), Enum.GetName(typeof(LogEventLevel), LogEventLevel.Debug));
                    EngineContext.ScriptEngine.EngineContext.ScriptBuilder.IsUnhandledException = false;

                    if (result == DialogResult.OK)
                    {                           
                        ReportProgress("Ignoring Per User Choice");
                        ErrorsOccured.Clear();

                        if (_isScriptSteppedIntoBeforeException)
                        {
                            EngineContext.ScriptEngine.EngineContext.ScriptBuilder.IsScriptSteppedInto = true;
                            _isScriptSteppedIntoBeforeException = false;
                        }
                        else if (_isScriptSteppedOverBeforeException)
                        {
                            EngineContext.ScriptEngine.EngineContext.ScriptBuilder.IsScriptSteppedOver = true;
                            _isScriptSteppedOverBeforeException = false;
                        }

                        EngineContext.ScriptEngine.uiBtnPause_Click(null, null);
                    }
                    else if (result == DialogResult.Abort || result == DialogResult.Cancel)
                    {
                        ReportProgress("Continuing Per User Choice");
                        EngineContext.ScriptEngine.EngineContext.ScriptBuilder.RemoveDebugTab();
                        EngineContext.ScriptEngine.uiBtnPause_Click(null, null);                           
                        throw ex;
                    }
                }
                else
                    throw ex;            
            }
        }     

        public void CancelScript()
        {
            IsCancellationPending = true;
        }

        public void PauseScript()
        {
            _isScriptPaused = true;
        }

        public void ResumeScript()
        {
            _isScriptPaused = false;
        }

        public void StepOverScript()
        {
            _isScriptSteppedOver = true;
            _isScriptSteppedOverBeforeException = true;
        }

        public void StepIntoScript()
        {
            _isScriptSteppedInto = true;
            _isScriptSteppedIntoBeforeException = true;
        }

        public virtual void ReportProgress(string progress, string eventLevel = "Information")
        {
            ReportProgressEventArgs args = new ReportProgressEventArgs();
            LogEventLevel logEventLevel = (LogEventLevel)Enum.Parse(typeof(LogEventLevel), eventLevel);

            switch (logEventLevel)
            {
                case LogEventLevel.Verbose:
                    Log.Verbose(progress);
                    args.LoggerColor = Color.Purple;
                    break;
                case LogEventLevel.Debug:
                    Log.Debug(progress);
                    args.LoggerColor = Color.Green;
                    break;
                case LogEventLevel.Information:
                    Log.Information(progress);
                    args.LoggerColor = SystemColors.Highlight;
                    break;
                case LogEventLevel.Warning:
                    Log.Warning(progress);
                    args.LoggerColor = Color.Goldenrod;
                    break;
                case LogEventLevel.Error:
                    Log.Error(progress);
                    args.LoggerColor = Color.Red;
                    break;
                case LogEventLevel.Fatal:
                    Log.Fatal(progress);
                    args.LoggerColor = Color.Black;
                    break;
            }

            if (progress.StartsWith("Skipping"))
                args.LoggerColor = Color.Green;
             
            args.ProgressUpdate = progress;

            //invoke event
            ReportProgressEvent?.Invoke(this, args);
        }

        public virtual void ScriptFinished(ScriptFinishedResult result, string error = null)
        {
            if (ChildScriptFailed && !ChildScriptErrorCaught)
            {
                error = "Terminate with failure";
                result = ScriptFinishedResult.Error;
                Log.Fatal("Result Code: " + result.ToString());
            }
            else
                Log.Information("Result Code: " + result.ToString());

            //add result variable if missing
            var resultVar = EngineContext.Variables.Where(f => f.VariableName == "OpenBots.Result").FirstOrDefault();

            //handle if variable is missing
            if (resultVar == null)
                resultVar = new OBScriptVariable() { VariableName = "OpenBots.Result", VariableValue = "" };

            //check value
            var resultValue = resultVar.VariableValue.ToString();

            if (error == null)
            {
                Log.Information("Error: None");

                if (string.IsNullOrEmpty(resultValue))
                    EngineContext.TaskResult = "Successfully Completed Script";
                else
                    EngineContext.TaskResult = resultValue;
            }

            else
            {
                if (ErrorsOccured.Count > 0)
                    error = ErrorsOccured.OrderByDescending(x => x.LineNumber).FirstOrDefault().StackTrace;

                Log.Error("Error: " + error);
                EngineContext.TaskResult = error;
            }

            if ((EngineContext.ScriptEngine != null && !EngineContext.IsChildEngine) ||
                (EngineContext.IsServerExecution && !EngineContext.IsServerChildExecution))
                Log.CloseAndFlush();

            EngineContext.CurrentEngineStatus = EngineStatus.Finished;
            ScriptFinishedEventArgs args = new ScriptFinishedEventArgs
            {
                LoggedOn = DateTime.Now,
                Result = result,
                Error = error,
                ExecutionTime = _stopWatch.Elapsed,
                FileName = EngineContext.FilePath
            };

            //convert to json
            var serializedArguments = JsonConvert.SerializeObject(args);

            //write execution metrics
            if (EngineContext.EngineSettings.TrackExecutionMetrics && (EngineContext.FilePath != null))
            {
                string summaryLoggerFilePath = Path.Combine(Folders.GetFolder(FolderType.LogFolder), "OpenBots Execution Summary Logs.txt");
                Logger summaryLogger = new LoggingMethods().CreateJsonFileLogger(summaryLoggerFilePath, RollingInterval.Infinite);
                summaryLogger.Information(serializedArguments);

                if (!EngineContext.IsChildEngine)
                    summaryLogger.Dispose();
            }

            ScriptFinishedEvent?.Invoke(this, args);
        }

        public virtual void LineNumberChanged(int lineNumber)
        {
            LineNumberChangedEventArgs args = new LineNumberChangedEventArgs
            {
                CurrentLineNumber = lineNumber
            };

            LineNumberChangedEvent?.Invoke(this, args);
        }

        public string GetEngineContext()
        {
            //set json settings
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                Error = (serializer, err) =>
                {
                    err.ErrorContext.Handled = true;                    
                },
                Formatting = Formatting.Indented
            };

            var strippedContext = new Dictionary<string, object>();
            
            foreach (PropertyInfo pi in EngineContext.GetType().GetProperties())
            {
                if (pi.Name != "ScriptEngine" && pi.Name != "EngineScript" && pi.Name != "EngineScriptState" && pi.Name != "ScriptBuilder")
                    strippedContext.Add(pi.Name, pi.GetValue(EngineContext));
            }

            return JsonConvert.SerializeObject(strippedContext, settings);
        }
    }
}
