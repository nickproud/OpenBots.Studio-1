using Newtonsoft.Json;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Properties;
using OpenBots.Core.Server.API_Methods;
using OpenBots.Core.Utilities.CommonUtilities;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;

namespace OpenBots.Commands.QueueItem
{
	[Serializable]
	[Category("QueueItem Commands")]
	[Description("This command updates the status of a QueueItem in an existing Queue in OpenBots Server.")]
	public class SetQueueItemStatusCommand : ScriptCommand
	{
		[Required]
		[DisplayName("QueueItem")]
		[Description("Enter a QueueItem Dictionary variable.")]
		[SampleUsage("{vQueueItem}")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(Dictionary<,>) })]
		public string v_QueueItem { get; set; }

		[Required]
		[DisplayName("QueueItem Status Type")]
		[PropertyUISelectionOption("Successful")]
		[PropertyUISelectionOption("Failed - Should Retry")]
		[PropertyUISelectionOption("Failed - Fatal")]
		[Description("Specify the QueueItem status type.")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_QueueItemStatusType { get; set; }

		[DisplayName("QueueItem Error Code (Optional)")]
		[Description("Enter the QueueItem code.")]
		[SampleUsage("400 || {vStatusCode}")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(null, true)]
		public string v_QueueItemErrorCode { get; set; }

		[DisplayName("QueueItem Error Message (Optional)")]
		[Description("Enter the QueueItem error message.")]
		[SampleUsage("File not found || {vStatusMessage}")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(null, true)]
		public string v_QueueItemErrorMessage { get; set; }

		[JsonIgnore]
		[Browsable(false)]
		private List<Control> _errorMessageControls;

		[JsonIgnore]
		[Browsable(false)]
		private bool _hasRendered;

		public SetQueueItemStatusCommand()
		{
			CommandName = "SetQueueItemStatusCommand";
			SelectionName = "Set QueueItem Status";
			CommandEnabled = true;
			CommandIcon = Resources.command_queueitem;

			v_QueueItemStatusType = "Successful";
			CommonMethods.InitializeDefaultWebProtocol();
		}

		public override void RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var vQueueItem = (Dictionary<string, object>)v_QueueItem.ConvertUserVariableToObject(engine, nameof(v_QueueItem), this);
			var vQueueItemErrorMessage = v_QueueItemErrorMessage.ConvertUserVariableToString(engine);
			var vQueueItemErrorCode = v_QueueItemErrorCode.ConvertUserVariableToString(engine);

			var client = AuthMethods.GetAuthToken();

			Guid transactionKey = (Guid)vQueueItem["LockTransactionKey"];

			if (transactionKey == null || transactionKey == Guid.Empty)
				throw new NullReferenceException($"Transaction key {transactionKey} is invalid or not found");

			switch (v_QueueItemStatusType)
			{
				case "Successful":
					QueueItemMethods.CommitQueueItem(client, transactionKey);
					break;
				case "Failed - Should Retry":
					QueueItemMethods.RollbackQueueItem(client, transactionKey, vQueueItemErrorCode, vQueueItemErrorMessage, false);
					break;
				case "Failed - Fatal":
					QueueItemMethods.RollbackQueueItem(client, transactionKey, vQueueItemErrorCode, vQueueItemErrorMessage, true);
					break;
			}
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_QueueItem", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_QueueItemStatusType", this, editor));
			((ComboBox)RenderedControls[4]).SelectedIndexChanged += QueueItemStatusTypeComboBox_SelectedIndexChanged;

			_errorMessageControls = new List<Control>();
			_errorMessageControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_QueueItemErrorCode", this, editor));
			_errorMessageControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_QueueItemErrorMessage", this, editor));

			RenderedControls.AddRange(_errorMessageControls);

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			if (v_QueueItemStatusType != "Successful")
				return base.GetDisplayValue() + $" [Set '{v_QueueItem}' Status to '{v_QueueItemStatusType}' With Message '{v_QueueItemErrorMessage}']";
			else
				return base.GetDisplayValue() + $" [Set '{v_QueueItem}' Status to '{v_QueueItemStatusType}']";
		}

		public override void Shown()
		{
			base.Shown();
			_hasRendered = true;
			QueueItemStatusTypeComboBox_SelectedIndexChanged(this, null);
		}

		private void QueueItemStatusTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (((ComboBox)RenderedControls[4]).Text != "Successful" && _hasRendered)
			{
				foreach (var ctrl in _errorMessageControls)
					ctrl.Visible = true;
			}
			else if(_hasRendered)
			{
				foreach (var ctrl in _errorMessageControls)
				{
					ctrl.Visible = false;
					if (ctrl is TextBox)
						((TextBox)ctrl).Clear();
				}
			}
		}
	}
}