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
	[Description("Creates an action in the NexBotix platform to be attended to by an assigned user.")]
	public class CreatePlatformTaskCommand : ScriptCommand
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
		[DisplayName("Document Url")]
		[Description("URL of a document to show in action detail.")]
		[SampleUsage("")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		//can be changed to accept a specific data type
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_docUrl { get; set; } //holds value added to field

		[Required]
		[DisplayName("Extracted Data")]
		[Description("Base64 encoded string containing extraction result json")]
		[SampleUsage("")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		//can be changed to accept a specific data type
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_extractedData { get; set; } //holds value added to field

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

		[Required]
		[DisplayName("Unique Id")]
		[Description("Unique Id of the action")]
		[SampleUsage("")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		//can be changed to accept a specific data type
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_uniqueId { get; set; } //holds value added to field

		[Required]
		[DisplayName("Secondary Id")]
		[Description("Secondary Id of the action")]
		[SampleUsage("")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		//can be changed to accept a specific data type
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_secondaryId { get; set; } //holds value added to field

		[Required]
		[DisplayName("Assigned User")]
		[Description("User to whom the action is assigned")]
		[SampleUsage("")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		//can be changed to accept a specific data type
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_assignedUser { get; set; } //holds value added to field


		[Required]
		[DisplayName("Message from Bot")]
		[Description("Reason for raising the action according to the bot")]
		[SampleUsage("")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		//can be changed to accept a specific data type
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_messageFromBot { get; set; } //holds value added to field


		[Required]
		[DisplayName("Task Type")]
		[PropertyUISelectionOption("Business Exception")]
		[PropertyUISelectionOption("Extraction Issue")]
		[PropertyUISelectionOption("Approval")]
		[Description("Type of action required")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_taskType { get; set; }

		[Required]
		[DisplayName("Job Id")]
		[Description("Id of related job")]
		[SampleUsage("")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		//can be changed to accept a specific data type
		[CompatibleTypes(new Type[] { typeof(int) })]
		public string v_jobId { get; set; } //holds value added to field



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

		public CreatePlatformTaskCommand()
		{
			CommandName = "CreatePlatformTaskCommand";
			SelectionName = "Create a Platform Task";
			CommandEnabled = true;
			CommandIcon = Resources.command_run_code; //change this as needed
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			using (var client = new HttpClient())
            {
				var platformTask = new PlatFormTask
				{
					DocumentUrl = v_docUrl.GetVariableValue(engine),
					ExtractedData = v_extractedData.GetVariableValue(engine),
					AutomationId = int.Parse(v_processId.GetVariableValue(engine)),
					Phase = int.Parse(v_phase.GetVariableValue(engine)),
					UniqueIdentifier = v_uniqueId.GetVariableValue(engine),
					SecondaryIdentifier = v_secondaryId.GetVariableValue(engine),
					Status = 0,
					AssignedUser = v_assignedUser.GetVariableValue(engine),
					Detail = v_messageFromBot.GetVariableValue(engine),
					TaskType = TaskTypeNumber(v_taskType.GetVariableValue(engine)),
					JobId = int.Parse(v_jobId.GetVariableValue(engine))
				};

				var platformUrl = v_platformUrl.GetVariableValue(engine);
				var endpointSection = platformUrl.EndsWith("/") ? "api/app/platform-task" : "/api/app/platform-task";
				platformUrl += endpointSection;
				var platformTaskJson = JsonConvert.SerializeObject(platformTask);
				var platformTaskPayload = new StringContent(platformTaskJson);
				platformTaskPayload.Headers.ContentType = new MediaTypeHeaderValue("application/json");
				string result = client.PostAsync(platformUrl, platformTaskPayload).Result.Content.ReadAsStringAsync().Result;
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
			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_docUrl", this));
			RenderedControls.Add(commandControls.CreateDefaultInputFor("v_docUrl", this));
			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_extractedData", this));
			RenderedControls.Add(commandControls.CreateDefaultInputFor("v_extractedData", this));
			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_processId", this));
			RenderedControls.Add(commandControls.CreateDefaultInputFor("v_processId", this));
			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_phase", this));
			RenderedControls.Add(commandControls.CreateDefaultInputFor("v_phase", this));
			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_uniqueId", this));
			RenderedControls.Add(commandControls.CreateDefaultInputFor("v_uniqueId", this));
			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_secondaryId", this));
			RenderedControls.Add(commandControls.CreateDefaultInputFor("v_secondaryId", this));
			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_assignedUser", this));
			RenderedControls.Add(commandControls.CreateDefaultInputFor("v_assignedUser", this));
			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_messageFromBot", this));
			RenderedControls.Add(commandControls.CreateDefaultInputFor("v_messageFromBot", this));
			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_taskType", this, editor));
			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_jobId", this));
			RenderedControls.Add(commandControls.CreateDefaultInputFor("v_jobId", this));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));


			//RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			//shows in command sequence once saved - used field variables
			return base.GetDisplayValue() + $" at {v_platformUrl}";
		}

		
		private int TaskTypeNumber(string taskTypeString)
        {
			switch (taskTypeString)
            {
				case "Business Exception":
						return 0;
				case "Extraction Issue":
					return 1;
				case "Approval":
					return 2;
				default:
					throw new InvalidOperationException("Unknown task type");
            }
        }
	}

	public class PlatFormTask
	{
		public int AutomationId { get; set; }
		public int Phase { get; set; }
		public int Status { get; set; }
		public string ExtractedData { get; set; }
		public string Detail { get; set; }
		public int TaskType { get; set; }
		public string AssignedUser { get; set; }
		public string DocumentUrl { get; set; }
		public string UniqueIdentifier { get; set; }
		public string SecondaryIdentifier { get; set; }
		public int JobId { get; set; }


	}
}

