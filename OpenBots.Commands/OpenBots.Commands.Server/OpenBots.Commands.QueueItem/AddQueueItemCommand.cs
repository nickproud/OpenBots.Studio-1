using Newtonsoft.Json;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Properties;
using OpenBots.Core.Server.HelperMethods;
using OpenBots.Core.Server.Models;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using QueueItemModel = OpenBots.Core.Server.Models.QueueItem;

namespace OpenBots.Commands.QueueItem
{
	[Serializable]
	[Category("QueueItem Commands")]
	[Description("This command adds a QueueItem to an existing Queue in OpenBots Server.")]
	public class AddQueueItemCommand : ScriptCommand
	{
		[Required]
		[DisplayName("Queue Name")]
		[Description("Enter the name of the existing Queue.")]
		[SampleUsage("\"Name\" || vQueueName")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_QueueName { get; set; }

		[Required]
		[DisplayName("QueueItem Name")]
		[Description("Enter the name of the new QueueItem.")]
		[SampleUsage("\"Name\" || vQueueItemName")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_QueueItemName { get; set; }

		[DisplayName("Source (Optional)")]
		[Description("If the item being enqueued is a business event, define the source of the event.\n" +
					 "This is typically the system name that caused the business event.")]
		[SampleUsage("\"Loan Origination System\" || \"Lead Generation System\" || vSource")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_Source { get; set; }

		[DisplayName("Event (Optional)")]
		[Description("If the item being enqueued is a business event, define the name of the event.\n" +
					 "This is typically what has occured.")]
		[SampleUsage("\"Payment Rejected\" || \"New Employee Onboarded\" || vEvent")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_Event { get; set; }

		[Required]
		[DisplayName("QueueItem Type")]
		[PropertyUISelectionOption("Text")]
		[PropertyUISelectionOption("Json")]
		[Description("Specify the type of the new QueueItem.")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_QueueItemType { get; set; }

		[Required]
		[DisplayName("Json Type")]
		[Description("Specify the type of the Json.")]
		[SampleUsage("\"Company\" || vJsonType")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_JsonType { get; set; }

		[Required]
		[DisplayName("QueueItem Value")]
		[Description("Enter the value of the new QueueItem.")]
		[SampleUsage("\"Value\" || vQueueItemValue")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_QueueItemTextValue { get; set; }

		[DisplayName("Priority (Optional)")]
		[Description("Enter a priority value between 0-100.")]
		[SampleUsage("100 || vPriority")]
		[Remarks("Priority determines the order in which QueueItems will be worked.\n" +
				 "If no priority is set, QueueItems will be ordered by time of creation.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(int) })]
		public string v_Priority { get; set; }

		[DisplayName("Attachment File Path(s) (Optional)")]
		[Description("Enter the file path(s) of the file(s) to attach.")]
		[SampleUsage("new List<string>() { \"C:\\temp\\myFile1.xlsx\", \"C:\\temp\\myFile2.xlsx\" } || vFileList")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[Editor("ShowFileSelectionHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(List<string>) })]
		public string v_Attachments { get; set; }

		[JsonIgnore]
		[Browsable(false)]
		private List<Control> _jsonTypeControls;

		[JsonIgnore]
		[Browsable(false)]
		private bool _hasRendered;

		public AddQueueItemCommand()
		{
			CommandName = "AddQueueItemCommand";
			SelectionName = "Add QueueItem";
			CommandEnabled = true;
			CommandIcon = Resources.command_queueitem;

			v_QueueItemType = "Text";
			v_Priority = "10";
			CommonMethods.InitializeDefaultWebProtocol();
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var vQueueName = (string)await v_QueueName.EvaluateCode(engine);
			var vQueueItemName = (string)await v_QueueItemName.EvaluateCode(engine);
			var vSource = (string)await v_Source.EvaluateCode(engine);
			var vEvent = (string)await v_Event.EvaluateCode(engine);
			var vJsonType = (string)await v_JsonType.EvaluateCode(engine);            
			int priority = (int)await v_Priority.EvaluateCode(engine);
			var vQueueItemTextValue = (string)await v_QueueItemTextValue.EvaluateCode(engine);

			var userInfo = AuthMethods.GetUserInfo();
            Queue queue = QueueMethods.GetQueue(userInfo.Token, userInfo.ServerUrl, userInfo.OrganizationId, $"Name eq '{vQueueName}'");

            if (queue == null)
                throw new DataException($"Queue with name '{vQueueName}' not found");

            QueueItemModel queueItem = new QueueItemModel()
            {
                IsLocked = false,
                QueueId = queue.Id,
                Type = v_QueueItemType,
                JsonType = vJsonType,
                DataJson = vQueueItemTextValue,
                Name = vQueueItemName,
                IsDeleted = false,
                Priority = priority,
                Source = vSource,
                Event = vEvent
            };

            QueueItemMethods.EnqueueQueueItem(userInfo.Token, userInfo.ServerUrl, userInfo.OrganizationId, queueItem);

			if (!string.IsNullOrEmpty(v_Attachments))
            {
				var vAttachments = (List<string>)await v_Attachments.EvaluateCode(engine);
				QueueItemMethods.AttachFiles(userInfo.Token, userInfo.ServerUrl, userInfo.OrganizationId, queueItem.Id, vAttachments);
			}
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_QueueName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_QueueItemName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Source", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Event", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_QueueItemType", this, editor));
			((ComboBox)RenderedControls[13]).SelectedIndexChanged += QueueItemTypeComboBox_SelectedIndexChanged;

			_jsonTypeControls = new List<Control>();
			_jsonTypeControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_JsonType", this, editor));
			
			RenderedControls.AddRange(_jsonTypeControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_QueueItemTextValue", this, editor, 100, 300));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Priority", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Attachments", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			var attachmentCount = v_Attachments?.Split(';').Length;
			if (attachmentCount == null)
				return base.GetDisplayValue() + $" ['{v_QueueItemName}' of Type '{v_QueueItemType}' to Queue '{v_QueueName}' With 0 File Attachment(s)]";
			else
				return base.GetDisplayValue() + $" ['{v_QueueItemName}' of Type '{v_QueueItemType}' to Queue '{v_QueueName}' With {attachmentCount} File Attachment(s)]";
		}

		public override void Shown()
		{
			base.Shown();
			_hasRendered = true;
			QueueItemTypeComboBox_SelectedIndexChanged(null, null);
		}

		private void QueueItemTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (((ComboBox)RenderedControls[13]).Text == "Json" && _hasRendered)
			{
				foreach (var ctrl in _jsonTypeControls)
					ctrl.Visible = true;
			}
			else if(_hasRendered)
			{
				foreach (var ctrl in _jsonTypeControls)
				{
					ctrl.Visible = false;
					if (ctrl is TextBox)
						((TextBox)ctrl).Clear();
				}
			}
		}
	}
}