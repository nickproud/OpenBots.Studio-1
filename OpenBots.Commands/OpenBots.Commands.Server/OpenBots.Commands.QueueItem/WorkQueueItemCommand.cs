using Newtonsoft.Json;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Properties;
using OpenBots.Core.Server.API_Methods;
using OpenBots.Core.Server.Models;
using OpenBots.Core.Server.User;
using OpenBots.Core.Utilities.CommonUtilities;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace OpenBots.Commands.QueueItem
{
	[Serializable]
	[Category("QueueItem Commands")]
	[Description("This command gets and locks a QueueItem from an existing Queue in OpenBots Server.")]
	public class WorkQueueItemCommand : ScriptCommand
	{
		[Required]
		[DisplayName("Queue Name")]
		[Description("Enter the name of the Queue.")]
		[SampleUsage("Name || {vQueueName}")]
		[Remarks("QueueItem Text/Json values are store in the 'DataJson' key of a QueueItem Dictionary.\n" +
				 "If a Queue has no workable items, the output value will be set to null.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(null, true)]
		public string v_QueueName { get; set; }

		[Required]
		[DisplayName("Save Attachments")]
		[PropertyUISelectionOption("Yes")]
		[PropertyUISelectionOption("No")]
		[Description("Specify whether to save the queue item attachments to a local directory.")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_SaveAttachments { get; set; }

		[Required]
		[DisplayName("Output Attachment Directory")]
		[Description("Enter or Select the path to the directory to store the attachments in.")]
		[SampleUsage(@"C:\temp\myfolder\attachments || {vFolderPath} || {ProjectPath}\myFolder\attachments")]
		[Remarks("This input is optional and will only be used if *Save Attachments* is set to **Yes**.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[Editor("ShowFolderSelectionHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(null, true)]
		public string v_AttachmentDirectory { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output QueueItem Dictionary Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("{vUserVariable}")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(Dictionary<,>) })]
		public string v_OutputUserVariableName { get; set; }

		[JsonIgnore]
		[Browsable(false)]
		private List<Control> _savingControls;

		public WorkQueueItemCommand()
		{
			CommandName = "WorkQueueItemCommand";
			SelectionName = "Work QueueItem";
			CommandEnabled = true;
			CommandIcon = Resources.command_queueitem;

			v_SaveAttachments = "No";
			CommonMethods.InitializeDefaultWebProtocol();
		}

		public override void RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var vQueueName = v_QueueName.ConvertUserVariableToString(engine);
			var vAttachmentDirectory = v_AttachmentDirectory.ConvertUserVariableToString(engine);
			Dictionary<string, object> queueItemDict = new Dictionary<string, object>();

			var client = AuthMethods.GetAuthToken();

			var settings = EnvironmentSettings.GetAgentSettings();
			string agentId = settings["AgentId"];

			if (string.IsNullOrEmpty(agentId))
				throw new NullReferenceException("Agent is not connected");

			Queue queue = QueueMethods.GetQueue(client, $"name eq '{vQueueName}'");

			if (queue == null)
				throw new DataException($"Queue with name '{vQueueName}' not found");

			var queueItem = QueueItemMethods.DequeueQueueItem(client, Guid.Parse(agentId), queue.Id);

			if (queueItem == null)
			{
				queueItemDict = null;
				queueItemDict.StoreInUserVariable(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
				return;
			}

			queueItemDict = queueItem.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
											   .ToDictionary(prop => prop.Name, prop => prop.GetValue(queueItem, null));

			queueItemDict = queueItemDict.Where(kvp => kvp.Key == "LockTransactionKey" ||
													   kvp.Key == "Name" ||
													   kvp.Key == "Source" ||
													   kvp.Key == "Event" ||
													   kvp.Key == "Type" ||
													   kvp.Key == "JsonType" ||
													   kvp.Key == "DataJson" ||
													   kvp.Key == "Priority" ||
													   kvp.Key == "LockedUntilUTC")
										 .ToDictionary(i => i.Key, i => i.Value);

			queueItemDict.StoreInUserVariable(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);

			if (v_SaveAttachments == "Yes")
			{
				
				if (Directory.Exists(vAttachmentDirectory))
				{
					//get all queue item attachments
					var attachments = QueueItemMethods.GetAttachments(client, queueItem.Id);
					//save each attachment in the directory
					foreach (var attachment in attachments)
					{
						//export (save) in appropriate directory
						QueueItemMethods.DownloadFile(client, attachment, vAttachmentDirectory);
					}
				}
			}
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_QueueName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_SaveAttachments", this, editor));
			((ComboBox)RenderedControls[4]).SelectedIndexChanged += SaveQueueItemFilesComboBox_SelectedValueChanged;

			_savingControls = new List<Control>();
			_savingControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_AttachmentDirectory", this, editor));

			foreach (var ctrl in _savingControls)
				ctrl.Visible = false;

			RenderedControls.AddRange(_savingControls);
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));


			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			if (v_SaveAttachments == "Yes")
				return base.GetDisplayValue() + $" [From Queue '{v_QueueName}' - Store QueueItem Dictionary in '{v_OutputUserVariableName}' and File(s) in '{v_AttachmentDirectory}']";
			else
				return base.GetDisplayValue() + $" [From Queue '{v_QueueName}' - Store QueueItem Dictionary in '{v_OutputUserVariableName}']";
		}

		private void SaveQueueItemFilesComboBox_SelectedValueChanged(object sender, EventArgs e)
		{
			if (((ComboBox)RenderedControls[4]).Text == "Yes")
			{
				foreach (var ctrl in _savingControls)
					ctrl.Visible = true;
			}
			else
			{
				foreach (var ctrl in _savingControls)
				{
					ctrl.Visible = false;
					if (ctrl is TextBox)
						((TextBox)ctrl).Clear();
				}
			}
		}
	}
}