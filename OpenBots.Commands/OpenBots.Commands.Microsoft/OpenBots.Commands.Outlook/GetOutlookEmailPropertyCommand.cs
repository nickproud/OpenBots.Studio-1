using Microsoft.Office.Interop.Outlook;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Properties;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;

namespace OpenBots.Commands.Outlook
{

    [Serializable]
    [Category("Outlook Commands")]
    [Description("This command gets a property from an outlook email.")]

    public class GetOutlookEmailPropertyCommand : ScriptCommand
    {

        [Required]
        [DisplayName("MailItem")]
        [Description("Enter the MailItem from which to retrieve the property.")]
        [SampleUsage("{vMailItem}")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(MailItem) })]
        public string v_MailItem { get; set; }

        [Required]
        [DisplayName("Property")]
        [PropertyUISelectionOption("AlternateRecipientAllowed")]
        [PropertyUISelectionOption("Attachments")]
        [PropertyUISelectionOption("AutoForwarded")]
        [PropertyUISelectionOption("BCC")]
        [PropertyUISelectionOption("BillingInformation")]
        [PropertyUISelectionOption("Body")]
        [PropertyUISelectionOption("BodyFormat")]
        [PropertyUISelectionOption("Categories")]
        [PropertyUISelectionOption("CC")]
        [PropertyUISelectionOption("Companies")]
        [PropertyUISelectionOption("ConversationID")]
        [PropertyUISelectionOption("ConversationIndex")]
        [PropertyUISelectionOption("ConversationTopic")]
        [PropertyUISelectionOption("CreationTime")]
        [PropertyUISelectionOption("DeferredDeliveryTime")]
        [PropertyUISelectionOption("DeleteAfterSubmit")]
        [PropertyUISelectionOption("DownloadState")]
        [PropertyUISelectionOption("EntryID")]
        [PropertyUISelectionOption("ExpiryTime")]
        [PropertyUISelectionOption("HTMLBody")]
        [PropertyUISelectionOption("Importance")]
        [PropertyUISelectionOption("InternetCodepage")]
        [PropertyUISelectionOption("IsConflict")]
        [PropertyUISelectionOption("IsMarkedAsTask")]
        [PropertyUISelectionOption("LastModificationTime")]
        [PropertyUISelectionOption("MarkForDownload")]
        [PropertyUISelectionOption("MessageClass")]
        [PropertyUISelectionOption("Mileage")]
        [PropertyUISelectionOption("NoAging")]
        [PropertyUISelectionOption("OriginatorDeliveryReportRequested")]
        [PropertyUISelectionOption("Permission")]
        [PropertyUISelectionOption("PermissionService")]
        [PropertyUISelectionOption("PermissionTemplateGuid")]
        [PropertyUISelectionOption("ReadReceiptRequested")]
        [PropertyUISelectionOption("ReceivedByEntryID")]
        [PropertyUISelectionOption("ReceivedByName")]
        [PropertyUISelectionOption("ReceivedOnBehalfOfEntryID")]
        [PropertyUISelectionOption("ReceivedOnBehalfOfName")]
        [PropertyUISelectionOption("RecipientReassignmentProhibited")]
        [PropertyUISelectionOption("Recipients")]
        [PropertyUISelectionOption("ReminderSet")]
        [PropertyUISelectionOption("ReminderTime")]
        [PropertyUISelectionOption("RemoteStatus")]
        [PropertyUISelectionOption("ReplyRecipientNames")]
        [PropertyUISelectionOption("ReplyRecipients")]
        [PropertyUISelectionOption("RetentionExpirationDate")]
        [PropertyUISelectionOption("RetentionPolicyName")]
        [PropertyUISelectionOption("Saved")]
        [PropertyUISelectionOption("SenderEmailAddress")]
        [PropertyUISelectionOption("SenderEmailType")]
        [PropertyUISelectionOption("SenderName")]
        [PropertyUISelectionOption("Sent")]
        [PropertyUISelectionOption("SentOn")]
        [PropertyUISelectionOption("SentOnBehalfOfName")]
        [PropertyUISelectionOption("Size")]
        [PropertyUISelectionOption("Subject")]
        [PropertyUISelectionOption("Submitted")]
        [PropertyUISelectionOption("TaskCompletedDate")]
        [PropertyUISelectionOption("TaskDueDate")]
        [PropertyUISelectionOption("TaskStartDate")]
        [PropertyUISelectionOption("TaskSubject")]
        [PropertyUISelectionOption("To")]
        [PropertyUISelectionOption("UnRead")]
        [Description("Select the property to retrieve.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_Property { get; set; }

        [Required]
        [DisplayName("Output MailItem Property Variable")]
        [Description("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(string), typeof(DateTime), typeof(bool), typeof(int) })]
        public string v_OutputUserVariableName { get; set; }

        public GetOutlookEmailPropertyCommand()
        {
            CommandName = "GetOutlookEmailPropertyCommand";
            SelectionName = "Get Outlook Email Property";
            CommandEnabled = true;
            CommandIcon = Resources.command_smtp;

            v_Property = "";
        }

        public async override void RunCommand(object sender)
        {
            var engine = (IAutomationEngineInstance)sender;
            MailItem item = (MailItem)await v_MailItem.EvaluateCode(engine, nameof(v_MailItem), this);

            string output = "";
            switch (v_Property)
            {
                case "AlternateRecipientAllowed":
                    item.AlternateRecipientAllowed.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "Attachments":
                    foreach(Attachment attachment in item.Attachments)
                    {
                        output = output + attachment.FileName + "\n";
                    }
                    output.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "AutoForwarded":
                    item.AutoForwarded.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "BCC":
                    item.BCC.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "BillingInformation":
                    item.BillingInformation.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "Body":
                    item.Body.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "BodyFormat":
                    item.BodyFormat.ToString().SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "Categories":
                    item.Categories.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "CC":
                    item.CC.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "Companies":
                    item.Companies.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "ConversationID":
                    item.ConversationID.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "ConversationIndex":
                    item.ConversationIndex.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "ConversationTopic":
                    item.ConversationTopic.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "CreationTime":
                    item.CreationTime.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "DeferredDeliveryTime":
                    item.DeferredDeliveryTime.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "DeleteAfterSubmit":
                    item.DeleteAfterSubmit.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "DownloadState":
                    item.DownloadState.ToString().SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "EntryID":
                    item.EntryID.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "ExpiryTime":
                    item.ExpiryTime.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "HTMLBody":
                    item.HTMLBody.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "Importance":
                    item.Importance.ToString().SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "InternetCodepage":
                    item.InternetCodepage.ToString().Length.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "IsConflict":
                    item.IsConflict.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "IsMarkedAsTask":
                    item.IsMarkedAsTask.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "LastModificationTime":
                    item.LastModificationTime.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "MarkForDownload":
                    item.MarkForDownload.ToString().SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "MessageClass":
                    item.MessageClass.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "Mileage":
                    item.Mileage.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "NoAging":
                    item.NoAging.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "OriginatorDeliveryReportRequested":
                    item.OriginatorDeliveryReportRequested.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "Permission":
                    item.Permission.ToString().SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "PermissionService":
                    item.PermissionService.ToString().SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "PermissionTemplateGuid":
                    item.PermissionTemplateGuid.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "ReadReceiptRequested":
                    item.ReadReceiptRequested.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "ReceivedByEntryID":
                    item.ReceivedByEntryID.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "ReceivedByName":
                    item.ReceivedByName.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "ReceivedOnBehalfOfEntryID":
                    item.ReceivedOnBehalfOfEntryID.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "ReceivedOnBehalfOfName":
                    item.ReceivedOnBehalfOfName.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "ReceivedTime":
                    item.ReceivedTime.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "RecipientReassignmentProhibited":
                    item.RecipientReassignmentProhibited.ToString().SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "Recipients":
                    foreach(Recipient rec in item.Recipients)
                    {
                        output = output + rec.Address + ";";
                    }
                    output.Substring(0, output.Length - 1).SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "ReminderSet":
                    item.ReminderSet.ToString().SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "ReminderTime":
                    item.ReminderTime.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "RemoteStatus":
                    item.RemoteStatus.ToString().SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "ReplyRecipientNames":
                    item.ReplyRecipientNames.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "ReplyRecipients":
                    foreach(Recipient rec in item.ReplyRecipients)
                    {
                        output = output + rec.Address + ";";
                    }
                    output.Substring(0, output.Length - 1).SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "RetentionExpirationDate":
                    item.RetentionExpirationDate.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "RetentionPolicyName":
                    item.RetentionPolicyName.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "Saved":
                    item.Saved.ToString().SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "SenderEmailAddress":
                    item.SenderEmailAddress.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "SenderEmailType":
                    item.SenderEmailType.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "SenderName":
                    item.SenderName.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "Sent":
                    item.Sent.ToString().SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "SentOn":
                    item.SentOn.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "SentOnBehalfOfName":
                    item.SentOnBehalfOfName.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "Size":
                    item.Size.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "Subject":
                    item.Subject.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "Submitted":
                    item.Submitted.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "TaskCompletedDate":
                    item.TaskCompletedDate.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "TaskDueDate":
                    item.TaskDueDate.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "TaskStartDate":
                    item.TaskStartDate.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "TaskSubject":
                    item.TaskSubject.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "To":
                    item.To.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                case "UnRead":
                    item.UnRead.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
                    break;
                default:
                    throw new NotImplementedException($"Property '{v_Property}' has not been implemented.");
            }
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_MailItem", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_Property", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [From '{v_MailItem}' - Get '{v_Property}' Value - Store Property in '{v_OutputUserVariableName}']";
        }
    }
}
