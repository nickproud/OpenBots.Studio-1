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
	[Description("Gets the total jobs waiting to be processed for a specific phase")]
	public class GetTotalPendingBotJobsCommand : ScriptCommand
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
		[Description("The phase relating to the job")]
		[SampleUsage("")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		//can be changed to accept a specific data type
		[CompatibleTypes(new Type[] { typeof(int) })]
		public string v_phase { get; set; } //holds value added to field

		[Required]
		[Editable(false)]
		[DisplayName("Output Platform Response Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("vUserVariable")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(int) })]
		public string v_OutputUserVariableName { get; set; }

		//examples of grid view that can be added
		//[JsonIgnore]
		//[Browsable(false)]
		//private DataGridView _parametersGridViewHelper;

		//[JsonIgnore]
		//[Browsable(false)]
		//private DataGridView _advancedParametersGridViewHelper;

		public GetTotalPendingBotJobsCommand()
		{
			CommandName = "GetTotalPendingBotJobsCommand";
			SelectionName = "Get Total Pending BotJobs";
			CommandEnabled = true;
			CommandIcon = Resources.command_run_code; //change this as needed
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var platformUrl = v_platformUrl.GetVariableValue(engine);
			var endpointSection = platformUrl.EndsWith("/") ? $"api/app/job/pending-total/{v_processId.GetVariableValue(engine)}?phase={v_phase.GetVariableValue(engine)}" : $"/api/app/job/pending-total/{v_processId.GetVariableValue(engine)}?phase={v_phase.GetVariableValue(engine)}";
			platformUrl += endpointSection;

			using(var client = new HttpClient())
            {
				int result = int.Parse(client.GetAsync(platformUrl).Result.Content.ReadAsStringAsync().Result);
				result.SetVariableValue(engine, v_OutputUserVariableName);
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
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			//shows in command sequence once saved - used field variables
			return base.GetDisplayValue() + $" from {v_platformUrl}";
		}
	}
}
