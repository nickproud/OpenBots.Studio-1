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

namespace NexBots.Commands.Platform
{
	[Serializable]
	[Category("NexBotix Platform Commands")]
	[Description("Gets outcomes for jobs ran for a process on a particular date")]
	public class GetOutcomesByDateCommand : ScriptCommand
    {

		[Required] //remove if not required
		[DisplayName("Platform Url")]
		[Description("URL of the platform instance containing outcomes.")]
		[SampleUsage("")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		//can be changed to accept a specific data type
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_platformUrl { get; set; } //holds value added to field

		[Required] //remove if not required
		[DisplayName("Date")]
		[Description("Date to get outcomes for")]
		[SampleUsage("")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		//can be changed to accept a specific data type
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_date { get; set; } //holds value added to field

		[Required] //remove if not required
		[DisplayName("Process Id")]
		[Description("Id of process to get outcomes for")]
		[SampleUsage("")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		//can be changed to accept a specific data type
		[CompatibleTypes(new Type[] { typeof(int) })]
		public string v_processId { get; set; } //holds value added to field


		[Required]
		[Editable(false)]
		[DisplayName("Output Platform Response Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("vUserVariable")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_OutputUserVariableName { get; set; }

		public GetOutcomesByDateCommand()
		{
			CommandName = "GetOutcomesByDateCommand";
			SelectionName = "Get Outcomes By Date";
			CommandEnabled = true;
			CommandIcon = Resources.command_run_code; //change this as needed
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			DateTime outcomesDate = new DateTime();
			if (!DateTime.TryParse(v_date.GetVariableValue(engine), out outcomesDate))
			{
				throw new FormatException("Not a valid DateTime");
            }
			string outcomesDateString = outcomesDate.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffK");
			var platformUrl = v_platformUrl.GetVariableValue(engine);
			var endpointSection = platformUrl.EndsWith("/") ? $"api/app/job/job-outcomes-for-date/{v_processId.GetVariableValue(engine)}?date={outcomesDateString}" : $"/api/app/job/job-outcomes-for-date/{v_processId.GetVariableValue(engine)}?date={outcomesDateString}";
			platformUrl += endpointSection;
			using (var client = new HttpClient())
			{
				string result = client.GetAsync(platformUrl).Result.Content.ReadAsStringAsync().Result;
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
			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_date", this));
			RenderedControls.Add(commandControls.CreateDefaultInputFor("v_date", this));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));
			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			//shows in command sequence once saved - used field variables
			return base.GetDisplayValue() + $" Get outcomes for process Id {v_processId} on {v_date} from platform - {v_platformUrl}";
		}
	}
}
