using Newtonsoft.Json;
using NexBots.Commands.Platform.NexBots.Commands.Platform;
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

namespace NexBots.Commands.Template
{
	[Serializable]
	[Category("NexBotix Platform Commands")]
	[Description("This command does something really cool")]
	public class EnqueueBotJobCommand : ScriptCommand
	{
		[Required] //remove if not required
		[DisplayName("Platform Url")]
		[Description("URL of the platform instance you are posting to.")]
		[SampleUsage("")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		//can be changed to accept a specific data type
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_platformUrl { get; set; } //holds value added to field

		[Required]
		[DisplayName("Process Id")]
		[Description("Id of the relevant automation process")]
		[SampleUsage("")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		//can be changed to accept a specific data type
		[CompatibleTypes(new Type[] { typeof(int) })]
		public string v_processId { get; set; } //holds value added to field

		[Required]
		[DisplayName("Phase")]
		[Description("The phase relating to the action")]
		[SampleUsage("")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		//can be changed to accept a specific data type
		[CompatibleTypes(new Type[] { typeof(int) })]
		public string v_phase { get; set; } //holds value added to field

		[Required] //remove if not required
		[DisplayName("Job Data")]
		[Description("Primary payload for the job encoded as a Base64 string. Eg: JSON returned from OCR")]
		[SampleUsage("")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		//can be changed to accept a specific data type
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_jobData { get; set; } //holds value added to field

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
		[DisplayName("Unique Id")]
		[Description("Unique identifier for the job.")]
		[SampleUsage("")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		//can be changed to accept a specific data type
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_uniqueId { get; set; } //holds value added to field
		[Required]
		[DisplayName("Secondary Id")]
		[Description("Secondary id for the job.")]
		[SampleUsage("")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		//can be changed to accept a specific data type
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_secondaryId { get; set; } //holds value added to field

		[Required]
		[Editable(false)]
		[DisplayName("Output Platform Response Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("vUserVariable")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_OutputUserVariableName { get; set; }

		//examples of grid view that can be added
		//[JsonIgnore]
		//[Browsable(false)]
		//private DataGridView _parametersGridViewHelper;

		//[JsonIgnore]
		//[Browsable(false)]
		//private DataGridView _advancedParametersGridViewHelper;

		public EnqueueBotJobCommand()
		{
			CommandName = "EnqueueBotJob";
			SelectionName = "Enqueue a Bot Job";
			CommandEnabled = true;
			CommandIcon = Resources.command_run_code; //change this as needed
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			using (var client = new HttpClient())
            {
				var job = new BotJob()
				{
					OutputGroup = v_outputGroup.GetVariableValue(engine),
					Data = v_jobData.GetVariableValue(engine),
					ProcessId = int.Parse(v_processId.GetVariableValue(engine)),
					Phase = int.Parse(v_phase.GetVariableValue(engine)),
					UniqueId = v_uniqueId.GetVariableValue(engine),
					SecondaryId = v_secondaryId.GetVariableValue(engine),
					Status = 0
				};

				var platformUrl = v_platformUrl.GetVariableValue(engine);
				var endpointSection = platformUrl.EndsWith("/") ? "api/app/job" : "/api/app/job";
				platformUrl += endpointSection;
				var jobJson = JsonConvert.SerializeObject(job);
				var jobPayload = new StringContent(jobJson);
				jobPayload.Headers.ContentType = new MediaTypeHeaderValue("application/json");
				HttpResponseMessage result = client.PostAsync(platformUrl, jobPayload).Result;
				string resultJson = result.Content.ReadAsStringAsync().Result;
				resultJson.SetVariableValue(engine, v_OutputUserVariableName);
			}
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			//need this
			base.Render(editor, commandControls);

			//render controls for your field variables - access methods on commandsControls to do this
			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_platformUrl", this));
			RenderedControls.Add(commandControls.CreateDefaultInputFor("v_platformUrl", this));
			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_outputGroup", this));
			RenderedControls.Add(commandControls.CreateDefaultInputFor("v_outputGroup", this));
			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_processId", this));
			RenderedControls.Add(commandControls.CreateDefaultInputFor("v_processId", this));
			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_phase", this));
			RenderedControls.Add(commandControls.CreateDefaultInputFor("v_phase", this));
			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_jobData", this));
			RenderedControls.Add(commandControls.CreateDefaultInputFor("v_jobData", this));
			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_uniqueId", this));
			RenderedControls.Add(commandControls.CreateDefaultInputFor("v_uniqueId", this));
			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_secondaryId", this));
			RenderedControls.Add(commandControls.CreateDefaultInputFor("v_secondaryId", this));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			//shows in command sequence once saved - used field variables
			return base.GetDisplayValue() + $"Queue bot job to {v_platformUrl}, save result in {v_OutputUserVariableName}";
		}
	}
}

