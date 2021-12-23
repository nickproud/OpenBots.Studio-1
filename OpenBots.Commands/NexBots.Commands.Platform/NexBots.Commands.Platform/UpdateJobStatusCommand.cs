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
    [Description("Updates the status of specific BotJob within the platform")]
    public class UpdateJobStatusCommand : ScriptCommand
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
		[DisplayName("New Status")]
		[Description("Status to be set")]
		[SampleUsage("")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		//can be changed to accept a specific data type
		[CompatibleTypes(new Type[] { typeof(int) })]
		public string v_newStatus { get; set; } //holds value added to field

		public UpdateJobStatusCommand()
		{
			CommandName = "UpdateJobStatusCommandCommand";
			SelectionName = "Update BotJob Status";
			CommandEnabled = true;
			CommandIcon = Resources.command_run_code; //change this as needed
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var platformUrl = v_platformUrl.GetVariableValue(engine);
			var endpointSection = platformUrl.EndsWith("/") ? $"api/app/job/job-status/{v_jobId.GetVariableValue(engine)}?status={v_newStatus.GetVariableValue(engine)}" : $"/api/app/job/job-status/{v_jobId.GetVariableValue(engine)}?status={v_newStatus.GetVariableValue(engine)}";
			platformUrl += endpointSection;
			using (var client = new HttpClient())
			{
				client.PutAsync(platformUrl, null).Wait();
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
			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_newStatus", this)); 
			RenderedControls.Add(commandControls.CreateDefaultInputFor("v_newStatus", this));
			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			//shows in command sequence once saved - used field variables
			return base.GetDisplayValue() + $" Set JobId {v_jobId} to status {v_newStatus} on platform - {v_platformUrl}";
		}
	}
}
