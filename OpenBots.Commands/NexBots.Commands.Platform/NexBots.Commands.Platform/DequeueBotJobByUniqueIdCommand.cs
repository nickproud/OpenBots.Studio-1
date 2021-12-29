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
	[Description("Dequeues a pending job from the NexBotix platform for processing")]
	public class DequeueBotJobCommandByUniqueId : ScriptCommand
	{
		[Required] //remove if not required
		[DisplayName("Platform Url")]
		[Description("URL of the platform instance you are dequeuing jobs from.")]
		[SampleUsage("")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		//can be changed to accept a specific data type
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_platformUrl { get; set; } //holds value added to field

		[Required] //remove if not required
		[DisplayName("Unique Id")]
		[Description("Unique Id of the job you are dequeueing")]
		[SampleUsage("")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		//can be changed to accept a specific data type
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_uniqueId { get; set; } //holds value added to field

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
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_OutputUserVariableName { get; set; }

		public DequeueBotJobCommandByUniqueId()
		{
			CommandName = "DequeueBotJobByUniqueIdCommand";
			SelectionName = "Dequeue BotJob by Unique Id";
			CommandEnabled = true;
			CommandIcon = Resources.command_job; //change this as needed
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var platformUrl = v_platformUrl.GetVariableValue(engine);
			var endpointSection = platformUrl.EndsWith("/") ? $"api/app/job/next-queued-job/{v_processId.GetVariableValue(engine)}?phase={v_phase.GetVariableValue(engine)}&uniqueId={v_uniqueId.GetVariableValue(engine)}" : $"/api/app/job/next-queued-job/{v_processId.GetVariableValue(engine)}?phase={v_phase.GetVariableValue(engine)}&uniqueId={v_uniqueId.GetVariableValue(engine)}";
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
			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_phase", this));
			RenderedControls.Add(commandControls.CreateDefaultInputFor("v_phase", this));
			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_uniqueId", this));
			RenderedControls.Add(commandControls.CreateDefaultInputFor("v_uniqueId", this));
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
