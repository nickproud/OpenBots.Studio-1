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
        [Remarks("Variables not pre-defined in the Variable Manager will be automatically generated at runtime.")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        public string v_OutputUserVariableName { get; set; }

        public GetOutlookEmailPropertyCommand()
        {
            CommandName = "GetOutlookEmailProperty";
            SelectionName = "Get Outlook Email Property";
            CommandEnabled = true;
            CommandIcon = Resources.command_smtp;

            v_Property = "";
        }

        public override void RunCommand(object sender)
        {
            var engine = (IAutomationEngineInstance)sender;
            MailItem item = (MailItem)v_MailItem.ConvertUserVariableToObject(engine);

            string output = "";
            switch (v_Property)
            {
                case "AlternateRecipientAllowed":
                    item.AlternateRecipientAllowed.ToString().StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "Attachments":
                    foreach(Attachment attachment in item.Attachments)
                    {
                        output = output + attachment.FileName + "\n";
                    }
                    output.StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "AutoForwarded":
                    item.AutoForwarded.ToString().StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "BCC":
                    item.BCC.StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "BillingInformation":
                    item.BillingInformation.StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "Body":
                    item.Body.StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "BodyFormat":
                    item.BodyFormat.ToString().StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "Categories":
                    item.Categories.StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "CC":
                    item.CC.StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "Companies":
                    item.Companies.StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "ConversationID":
                    item.ConversationID.StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "ConversationIndex":
                    item.ConversationIndex.StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "ConversationTopic":
                    item.ConversationTopic.StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "CreationTime":
                    item.CreationTime.StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "DeferredDeliveryTime":
                    item.DeferredDeliveryTime.StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "DeleteAfterSubmit":
                    item.DeleteAfterSubmit.ToString().StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "DownloadState":
                    item.DownloadState.ToString().StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "EntryID":
                    item.EntryID.StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "ExpiryTime":
                    item.ExpiryTime.StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "HTMLBody":
                    item.HTMLBody.StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "Importance":
                    item.Importance.ToString().StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "InternetCodepage":
                    item.InternetCodepage.ToString().Length.StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "IsConflict":
                    item.IsConflict.ToString().StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "IsMarkedAsTask":
                    item.IsMarkedAsTask.ToString().StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "LastModificationTime":
                    item.LastModificationTime.StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "MarkForDownload":
                    item.MarkForDownload.ToString().StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "MessageClass":
                    item.MessageClass.StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "Mileage":
                    item.Mileage.StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "NoAging":
                    item.NoAging.ToString().StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "OriginatorDeliveryReportRequested":
                    item.OriginatorDeliveryReportRequested.ToString().StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "Permission":
                    item.Permission.ToString().StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "PermissionService":
                    item.PermissionService.ToString().StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "PermissionTemplateGuid":
                    item.PermissionTemplateGuid.StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "ReadReceiptRequested":
                    item.ReadReceiptRequested.ToString().StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "ReceivedByEntryID":
                    item.ReceivedByEntryID.StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "ReceivedByName":
                    item.ReceivedByName.StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "ReceivedOnBehalfOfEntryID":
                    item.ReceivedOnBehalfOfEntryID.StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "ReceivedOnBehalfOfName":
                    item.ReceivedOnBehalfOfName.StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "ReceivedTime":
                    item.ReceivedTime.StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "RecipientReassignmentProhibited":
                    item.RecipientReassignmentProhibited.ToString().StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "Recipients":
                    foreach(Recipient rec in item.Recipients)
                    {
                        output = output + rec.Address + ";";
                    }
                    output.Substring(0, output.Length - 1).StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "ReminderSet":
                    item.ReminderSet.ToString().StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "ReminderTime":
                    item.ReminderTime.StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "RemoteStatus":
                    item.RemoteStatus.ToString().StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "ReplyRecipientNames":
                    item.ReplyRecipientNames.StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "ReplyRecipients":
                    foreach(Recipient rec in item.ReplyRecipients)
                    {
                        output = output + rec.Address + ";";
                    }
                    output.Substring(0, output.Length - 1).StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "RetentionExpirationDate":
                    item.RetentionExpirationDate.StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "RetentionPolicyName":
                    item.RetentionPolicyName.StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "Saved":
                    item.Saved.ToString().StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "SenderEmailAddress":
                    item.SenderEmailAddress.StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "SenderEmailType":
                    item.SenderEmailType.StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "SenderName":
                    item.SenderName.StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "Sent":
                    item.Sent.ToString().StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "SentOn":
                    item.SentOn.StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "SentOnBehalfOfName":
                    item.SentOnBehalfOfName.StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "Size":
                    item.Size.ToString().StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "Subject":
                    item.Subject.StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "Submitted":
                    item.Submitted.ToString().StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "TaskCompletedDate":
                    item.TaskCompletedDate.StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "TaskDueDate":
                    item.TaskDueDate.StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "TaskStartDate":
                    item.TaskStartDate.StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "TaskSubject":
                    item.TaskSubject.StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "To":
                    item.To.StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                case "UnRead":
                    item.UnRead.ToString().StoreInUserVariable(engine, v_OutputUserVariableName);
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
