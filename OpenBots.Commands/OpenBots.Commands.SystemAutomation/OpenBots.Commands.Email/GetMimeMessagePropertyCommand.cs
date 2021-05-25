using MimeKit;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;
using System.IO;
using OpenBots.Core.Properties;
using System.Linq;
using System.Threading.Tasks;

namespace OpenBots.Commands.Email
{
    [Serializable]
    [Category("Email Commands")]
    [Description("This command gets a property from an email.")]
    public class GetMimeMessagePropertyCommand : ScriptCommand
    {
        [Required]
        [DisplayName("MimeMessage")]
        [Description("Enter the MimeMessage from which to retrieve the property.")]
        [SampleUsage("vMimeMessage")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(MimeMessage) })]
        public string v_MimeMessage { get; set; }

        [Required]
        [DisplayName("Property")]
        [PropertyUISelectionOption("Attachments")]
        [PropertyUISelectionOption("Bcc")]
        [PropertyUISelectionOption("Body")]
        [PropertyUISelectionOption("BodyParts")]
        [PropertyUISelectionOption("Cc")]
        [PropertyUISelectionOption("Date")]
        [PropertyUISelectionOption("From")]
        [PropertyUISelectionOption("Headers")]
        [PropertyUISelectionOption("HtmlBody")]
        [PropertyUISelectionOption("Importance")]
        [PropertyUISelectionOption("InReplyTo")]
        [PropertyUISelectionOption("MessageId")]
        [PropertyUISelectionOption("MimeVersion")]
        [PropertyUISelectionOption("Priority")]
        [PropertyUISelectionOption("References")]
        [PropertyUISelectionOption("ReplyTo")]
        [PropertyUISelectionOption("ResentBcc")]
        [PropertyUISelectionOption("ResentCc")]
        [PropertyUISelectionOption("ResentDate")]
        [PropertyUISelectionOption("ResentFrom")]
        [PropertyUISelectionOption("ResentMessageId")]
        [PropertyUISelectionOption("ResentReplyTo")]
        [PropertyUISelectionOption("ResentSender")]
        [PropertyUISelectionOption("ResentTo")]
        [PropertyUISelectionOption("Sender")]
        [PropertyUISelectionOption("Subject")]
        [PropertyUISelectionOption("TextBody")]
        [PropertyUISelectionOption("To")]
        [PropertyUISelectionOption("XPriority")]
        [Description("Specify which property to retrieve.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_Property { get; set; }

        [Required]
        [DisplayName("Output MimeMessage Property Variable")]
        [Description("Create a new variable or select a variable from the list.")]
        [SampleUsage("vUserVariable")]
        [Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
        [CompatibleTypes(new Type[] { typeof(string), typeof(DateTime), typeof(List<MimeEntity>) })]
        public string v_OutputUserVariableName { get; set; }

        public GetMimeMessagePropertyCommand()
        {
            CommandName = "GetMimeMessagePropertyCommand";
            SelectionName = "Get MimeMessage Property";
            CommandEnabled = true;
            CommandIcon = Resources.command_smtp;

            v_Property = "Sender";
        }

        public async override Task RunCommand(object sender)
        {
            var engine = (IAutomationEngineInstance)sender;

            MimeMessage email = (MimeMessage)await v_MimeMessage.EvaluateCode(engine);

            Stream itemStream = new MemoryStream();
            StreamReader reader = new StreamReader(itemStream);
            string output = "";
            
            switch (v_Property)
            {
                case "Attachments":
                    email.Attachments.ToList().SetVariableValue(engine, v_OutputUserVariableName);
                    break;
                case "Bcc":
                    output = "";
                    foreach (InternetAddress item in email.Bcc)
                        output = output + item.ToString() + ";";

                    if (output != "")
                        output.Substring(0, output.Length - 1).SetVariableValue(engine, v_OutputUserVariableName);
                    else
                        output.SetVariableValue(engine, v_OutputUserVariableName);
                        break;
                case "Body":
                    await email.Body.WriteToAsync(itemStream);
                    reader.ReadToEnd().SetVariableValue(engine, v_OutputUserVariableName);
                    break;
                case "BodyParts":
                    output = "";
                    foreach (MimeEntity item in email.BodyParts)
                    {
                        await item.WriteToAsync(itemStream);
                        output = output + reader.ReadToEnd() + "\n";
                    }
                    output.SetVariableValue(engine, v_OutputUserVariableName);
                    break;
                case "Cc":
                    output = "";
                    foreach (InternetAddress item in email.Cc)
                        output = output + item.ToString() + ";";

                    if (output != "")
                        output.Substring(0, output.Length - 1).SetVariableValue(engine, v_OutputUserVariableName);
                    else
                        output.SetVariableValue(engine, v_OutputUserVariableName);
                    break;
                case "Date":
                    email.Date.UtcDateTime.SetVariableValue(engine, v_OutputUserVariableName);
                    break;
                case "From":
                    output = "";
                    foreach (InternetAddress item in email.From)
                        output = output + item.ToString() + ";";

                    if (output != "")
                        output.Substring(0, output.Length - 1).SetVariableValue(engine, v_OutputUserVariableName);
                    else
                        output.SetVariableValue(engine, v_OutputUserVariableName);
                    break;
                case "Headers":
                    output = "";
                    foreach (Header item in email.Headers)
                        output = output + item.ToString() + "\n";

                    output.SetVariableValue(engine, v_OutputUserVariableName);
                    break;
                case "HtmlBody":
                    email.HtmlBody.SetVariableValue(engine, v_OutputUserVariableName);
                    break;
                case "Importance":
                    email.Importance.ToString().SetVariableValue(engine, v_OutputUserVariableName);
                    break;
                case "InReplyTo":
                    email.InReplyTo.SetVariableValue(engine, v_OutputUserVariableName);
                    break;
                case "MessageId":
                    email.MessageId.SetVariableValue(engine, v_OutputUserVariableName);
                    break;
                case "MimeVersion":
                    email.MimeVersion.ToString().SetVariableValue(engine, v_OutputUserVariableName);
                    break;
                case "Priority":
                    email.Priority.ToString().SetVariableValue(engine, v_OutputUserVariableName);
                    break;
                case "References":
                    output = "";
                    foreach (var item in email.References)
                        output = output + item.ToString() + ",";

                    if (output != "")
                        output.Substring(0, output.Length - 1).SetVariableValue(engine, v_OutputUserVariableName);
                    else
                        output.SetVariableValue(engine, v_OutputUserVariableName);
                    break;
                case "ReplyTo":
                    output = "";
                    foreach (InternetAddress item in email.ReplyTo)
                        output = output + item.ToString() + ";";

                    if (output != "")
                        output.Substring(0, output.Length - 1).SetVariableValue(engine, v_OutputUserVariableName);
                    else
                        output.SetVariableValue(engine, v_OutputUserVariableName);
                    break;
                case "ResentBcc":
                    output = "";
                    foreach (InternetAddress item in email.ResentBcc)
                        output = output + item.ToString() + ";";

                    if (output != "")
                        output.Substring(0, output.Length - 1).SetVariableValue(engine, v_OutputUserVariableName);
                    else
                        output.SetVariableValue(engine, v_OutputUserVariableName);
                    break;
                case "ResentCc":
                    output = "";
                    foreach (InternetAddress item in email.ResentCc)
                        output = output + item.ToString() + ";";

                    if (output != "")
                        output.Substring(0, output.Length - 1).SetVariableValue(engine, v_OutputUserVariableName);
                    else
                        output.SetVariableValue(engine, v_OutputUserVariableName);
                    break;
                case "ResentDate":
                    email.ResentDate.UtcDateTime.SetVariableValue(engine, v_OutputUserVariableName);
                    break;
                case "ResentFrom":
                    output = "";
                    foreach (InternetAddress item in email.ResentFrom)
                        output = output + item.ToString() + ";";

                    if (output != "")
                        output.Substring(0, output.Length - 1).SetVariableValue(engine, v_OutputUserVariableName);
                    else
                        output.SetVariableValue(engine, v_OutputUserVariableName);
                    break;
                case "ResentMessageId":
                    email.ResentMessageId.SetVariableValue(engine, v_OutputUserVariableName);
                    break;
                case "ResentReplyTo":
                    output = "";
                    foreach (InternetAddress item in email.ResentReplyTo)
                        output = output + item.ToString() + ";";

                    if (output != "")
                        output.Substring(0, output.Length - 1).SetVariableValue(engine, v_OutputUserVariableName);
                    else
                        output.SetVariableValue(engine, v_OutputUserVariableName);
                    break;
                case "ResentSender":
                    if (email.ResentSender == null)
                        "null".SetVariableValue(engine, v_OutputUserVariableName);
                    else
                        email.ResentSender.Address.SetVariableValue(engine, v_OutputUserVariableName);
                    break;
                case "ResentTo":
                    output = "";
                    foreach (InternetAddress item in email.ResentTo)
                        output = output + item.ToString() + ";";

                    if (output != "")
                        output.Substring(0, output.Length - 1).SetVariableValue(engine, v_OutputUserVariableName);
                    else
                        output.SetVariableValue(engine, v_OutputUserVariableName);
                    break;
                case "Sender":
                    if (email.Sender == null)
                        "null".SetVariableValue(engine, v_OutputUserVariableName);
                    else
                        email.Sender.Address.SetVariableValue(engine, v_OutputUserVariableName);
                    break;
                case "Subject":
                    email.Subject.SetVariableValue(engine, v_OutputUserVariableName);
                    break;
                case "TextBody":
                    email.TextBody.SetVariableValue(engine, v_OutputUserVariableName);
                    break;
                case "To":
                    output = "";
                    foreach (InternetAddress item in email.To)
                        output = output + item.ToString() + ";";

                    if (output != "")
                        output.Substring(0, output.Length - 1).SetVariableValue(engine, v_OutputUserVariableName);
                    else
                        output.SetVariableValue(engine, v_OutputUserVariableName);
                    break;
                case "XPriority":
                    email.XPriority.ToString().SetVariableValue(engine, v_OutputUserVariableName);
                    break;
                default:
                    throw new NotImplementedException($"Property '{v_Property}' has not been implemented.");
            }
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_MimeMessage", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_Property", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [From '{v_MimeMessage}' - Get '{v_Property}' Value - Store Property in '{v_OutputUserVariableName}']";
        }
    }
}
