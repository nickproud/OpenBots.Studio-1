using Newtonsoft.Json;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Interfaces;
using OpenBots.Core.Properties;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace NexBots.Commands.Platform
{
    [Serializable]
    [Category("NexBotix Platform Commands")]
    [Description("Updates the platform process history")]
    public class UpdateProcessHistoryCommand : ScriptCommand
    {
		[Required] //remove if not required
		[DisplayName("Platform Url")]
		[Description("URL of the platform instance where the job resides.")]
		[SampleUsage("")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		//can be changed to accept a specific data type
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_platformUrl { get; set; } //holds value added to field

		[Required] //remove if not required
		[DisplayName("Process Id")]
		[Description("Id that identifies the process for which history is to be updated")]
		[SampleUsage("")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		//can be changed to accept a specific data type
		[CompatibleTypes(new Type[] { typeof(int) })]
		public string v_processId { get; set; } //holds value added to field

		[Required] //remove if not required
		[DisplayName("Phase")]
		[Description("The relevant phase for which history should be written")]
		[SampleUsage("")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		//can be changed to accept a specific data type
		[CompatibleTypes(new Type[] { typeof(int) })]
		public string v_phase { get; set; } //holds value added to field

		[Required] //remove if not required
		[DisplayName("Output Title")]
		[Description("Title for the process history entry")]
		[SampleUsage("")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		//can be changed to accept a specific data type
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_outputTitle{ get; set; } //holds value added to field

		[Required] //remove if not required
		[DisplayName("Output Body")]
		[Description("Body for the process history entry")]
		[SampleUsage("")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		//can be changed to accept a specific data type
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_outputBody { get; set; } //holds value added to field

		[Required]
		[DisplayName("Output Group")]
		[Description("Unique group identifier for the current transaction. Eg: Document URL")]
		[SampleUsage("")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		//can be changed to accept a specific data type
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_outputGroup { get; set; } //holds value added to field


		[Required]
		[DisplayName("Run Duration (Seconds)")]
		[Description("Time in seconds recorded in the run up to posting of the history record")]
		[SampleUsage("")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		//can be changed to accept a specific data type
		[CompatibleTypes(new Type[] { typeof(int) })]
		public string v_runDuration { get; set; } //holds value added to field

		[Required]
		[DisplayName("BotJob Id")]
		[Description("Id of the current BotJob")]
		[SampleUsage("")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		//can be changed to accept a specific data type
		[CompatibleTypes(new Type[] { typeof(int) })]
		public string v_jobId { get; set; } //holds value added to field


		[Required]
		[DisplayName("Event Type")]
		[PropertyUISelectionOption("Process Started")]
		[PropertyUISelectionOption("Process Fully Completed")]
		[PropertyUISelectionOption("Business Exception")]
		[PropertyUISelectionOption("Ended With Fault")]
		[PropertyUISelectionOption("Phase Completed")]
		[Description("Type of process history record to be created")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_eventType { get; set; }



		public UpdateProcessHistoryCommand()
        {
			CommandName = "UpdateProcessHistoryCommand";
			SelectionName = "Update Process History";
			CommandEnabled = true;
			CommandIcon = Resources.command_run_code; //change this as needed
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var platformUrl = v_platformUrl.GetVariableValue(engine);
			var endpointSection = platformUrl.EndsWith("/") ? $"api/app/process-history" : $"/api/app/process-history";
			platformUrl += endpointSection;
			using (var client = new HttpClient())
			{
				var processHistory = new ProcessHistory()
				{
					ProcessId = int.Parse(v_processId.GetVariableValue(engine)),
					ProcessPhase = int.Parse(v_phase.GetVariableValue(engine)),
					ProcessSubPhase = 1,
					OutputTitle = v_outputTitle.GetVariableValue(engine),
					Output = v_outputBody.GetVariableValue(engine),
					OutputType = GetOutputTypeId(v_eventType),
					OutputGroup = v_outputGroup.GetVariableValue(engine),
					CompletionTimeInSeconds = int.Parse(v_runDuration.GetVariableValue(engine)),
					JobId = int.Parse(v_jobId.GetVariableValue(engine))
				};

				var processHistoryJson = JsonConvert.SerializeObject(processHistory);
				var processHistoryPayload = new StringContent(processHistoryJson);
				processHistoryPayload.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
				client.PostAsync(platformUrl, processHistoryPayload).Wait();
			}
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			//need this
			base.Render(editor, commandControls);

			//render controls for your field variables - access methods on commandsControls to do this

			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_platformUrl", this));
			RenderedControls.Add(commandControls.CreateDefaultInputFor("v_platformUrl", this));
			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_processId", this));
			RenderedControls.Add(commandControls.CreateDefaultInputFor("v_processId", this));
			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_phase", this));
			RenderedControls.Add(commandControls.CreateDefaultInputFor("v_phase", this));
			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_outputTitle", this));
			RenderedControls.Add(commandControls.CreateDefaultInputFor("v_outputTitle", this));
			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_outputBody", this));
			RenderedControls.Add(commandControls.CreateDefaultInputFor("v_outputBody", this));
			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_eventType", this, editor));
			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_outputGroup", this));
			RenderedControls.Add(commandControls.CreateDefaultInputFor("v_outputGroup", this));
			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_runDuration", this));
			RenderedControls.Add(commandControls.CreateDefaultInputFor("v_runDuration", this));
			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_jobId", this));
			RenderedControls.Add(commandControls.CreateDefaultInputFor("v_jobId", this));
			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			//shows in command sequence once saved - used field variables
			return base.GetDisplayValue() + $" - Update process history with event type - {v_eventType} on platform - {v_platformUrl}";
		}

		private int GetOutputTypeId(string outputType)
        {
			switch (outputType)
            {
				case "Process Started":
					return 0;
				case "Process Fully Completed":
					return 1;
				case "Business Exception":
					return 2;
				case "Ended With Fault":
					return 3;
				case "Phase Completed":
					return 4;
				default:
					throw new InvalidOperationException($"Unknown process history output type: {outputType}");
            }
        }

	}
}
