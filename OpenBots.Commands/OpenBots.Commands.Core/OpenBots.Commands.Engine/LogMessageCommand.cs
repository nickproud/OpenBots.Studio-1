using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Properties;
using OpenBots.Core.Utilities.CommonUtilities;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;
using Tasks = System.Threading.Tasks;

namespace OpenBots.Commands.Engine
{
	[Serializable]
	[Category("Engine Commands")]
	[Description("This command logs text data to either an engine file or a custom file.")]
	public class LogMessageCommand : ScriptCommand
	{
		[Required]
		[DisplayName("Write Log To")]
		[Description("Specify the corresponding logging option to save logs to Engine Logs or to a custom File.")]
		[SampleUsage("\"Engine Logs\" || @\"C:\\temp\\myfile.txt\" || ProjectPath + @\"\\myfile.txt\" || vFilePath")]
		[Remarks("Selecting 'Engine Logs' will result in writing execution logs in the 'Engine Logs'. " +
			"The current Date and Time will be automatically appended to a local file if a custom file name is provided. " +
			"Logs are all saved in the OpenBots Studio Root Folder in the 'Logs' folder.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[Editor("ShowFileSelectionHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_LogFile { get; set; }

		[Required]
		[DisplayName("Log Text")]
		[Description("Specify the log text.")]
		[SampleUsage("\"Third Step is Complete\" || vLogText")]
		[Remarks("Provide only text data.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_LogText { get; set; }

		[Required]
		[DisplayName("Log Type")]
		[PropertyUISelectionOption("Verbose")]
		[PropertyUISelectionOption("Debug")]
		[PropertyUISelectionOption("Information")]
		[PropertyUISelectionOption("Warning")]
		[PropertyUISelectionOption("Error")]
		[PropertyUISelectionOption("Fatal")]
		[Description("Specify the log type.")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_LogType { get; set; }

		public LogMessageCommand()
		{
			CommandName = "LogMessageCommand";
			SelectionName = "Log Message";
			CommandEnabled = true;
			CommandIcon = Resources.command_files;

			v_LogFile = "\"Engine Logs\"";
			v_LogType = "Information";
		}

		public async override Tasks.Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;

			//get text to log and log file name       
			var textToLog = (string)await v_LogText.EvaluateCode(engine);
			var loggerFilePath = (string)await v_LogFile.EvaluateCode(engine);

			LogEventLevel logLevel = LogEventLevel.Information;

			//determine log file
			switch (v_LogType)
			{
				case "Verbose":
					logLevel = LogEventLevel.Verbose;
					break;
				case "Debug":
					logLevel = LogEventLevel.Debug;
					break;
				case "Information":
					logLevel = LogEventLevel.Information;
					break;
				case "Warning":
					logLevel = LogEventLevel.Warning;
					break;
				case "Error":
					logLevel = LogEventLevel.Error;
					break;
				case "Fatal":
					logLevel = LogEventLevel.Fatal;
					break;
			}

			if (loggerFilePath != "Engine Logs")
			{
				//create new logger and log to custom file
				using (var logger = new Logging().CreateFileLogger(loggerFilePath, RollingInterval.Infinite))
				{
					switch (v_LogType)
					{
						case "Verbose":
							logger.Verbose(textToLog);
							break;
						case "Debug":
							logger.Debug(textToLog);
							break;
						case "Information":
							logger.Information(textToLog);
							break;
						case "Warning":
							logger.Warning(textToLog);
							break;
						case "Error":
							logger.Error(textToLog);
							break;
						case "Fatal":
							logger.Fatal(textToLog);
							break;
					}                  
				}
			}

			string logMessage = $"{v_LogType} - {textToLog}";
			engine.ReportProgress($"Logging Line {LineNumber}: {(v_IsPrivate ? engine.PrivateCommandLog : logMessage)}", logLevel);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			//create standard group controls
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_LogFile", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_LogText", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_LogType", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [{v_LogType} - '{v_LogText}' to '{v_LogFile}']";
		}
	}
}
