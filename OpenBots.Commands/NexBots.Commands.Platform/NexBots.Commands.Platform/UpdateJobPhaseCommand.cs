using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    [Description("Updates the phases of specific BotJob within the platform")]
    public class UpdateJobPhaseCommand : ScriptCommand
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

		[Required]
		[DisplayName("BotJob Id")]
		[Description("Id of the BotJob where status should be changed")]
		[SampleUsage("")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		//can be changed to accept a specific data type
		[CompatibleTypes(new Type[] { typeof(int) })]
		public string v_jobId { get; set; } //holds value added to field

		[Required]
		[DisplayName("New Phase")]
		[Description("Status to be set")]
		[SampleUsage("")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		//can be changed to accept a specific data type
		[CompatibleTypes(new Type[] { typeof(int) })]
		public string v_newPhase { get; set; } //holds value added to field

		public UpdateJobPhaseCommand()
		{
			CommandName = "UpdateJobStatusCommand";
			SelectionName = "Update BotJob Status";
			CommandEnabled = true;
			CommandIcon = Resources.command_run_code; //change this as needed
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var platformUrl = v_platformUrl.GetVariableValue(engine);
			var endpointSection = platformUrl.EndsWith("/") ? $"api/app/job/{v_jobId.GetVariableValue(engine)}" : $"/api/app/job/{v_jobId.GetVariableValue(engine)}";
			platformUrl += endpointSection;
			using (var client = new HttpClient())
			{
				var jobJson = client.GetAsync(platformUrl).Result.Content.ReadAsStringAsync().Result;
				var jobObject = JObject.Parse(jobJson);
				jobObject["phase"] = v_newPhase.GetVariableValue(engine);

				var updateJobPayload = new StringContent(JsonConvert.SerializeObject(jobObject));
				updateJobPayload.Headers.ContentType = new MediaTypeHeaderValue("application/json");
				client.PutAsync(platformUrl, updateJobPayload).Result.Content.ReadAsStringAsync().Wait();
			}
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			//need this
			base.Render(editor, commandControls);

			//render controls for your field variables - access methods on commandsControls to do this

			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_platformUrl", this));
			RenderedControls.Add(commandControls.CreateDefaultInputFor("v_platformUrl", this));
			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_jobId", this));
			RenderedControls.Add(commandControls.CreateDefaultInputFor("v_jobId", this));
			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_newPhase", this));
			RenderedControls.Add(commandControls.CreateDefaultInputFor("v_newPhase", this));
			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			//shows in command sequence once saved - used field variables
			return base.GetDisplayValue() + $" Set JobId {v_jobId} to phase {v_newPhase} on platform - {v_platformUrl}";
		}
	}
}
