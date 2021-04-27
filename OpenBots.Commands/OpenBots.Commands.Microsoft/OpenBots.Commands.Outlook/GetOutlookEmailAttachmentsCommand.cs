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
using System.IO;
using System.Threading.Tasks;

namespace OpenBots.Commands.Outlook
{
    [Serializable]
    [Category("Outlook Commands")]
    [Description("This command gets attachments from a selected email in Outlook.")]

    public class GetOutlookEmailAttachmentsCommand : ScriptCommand
    {
        [Required]
        [DisplayName("MailItem")]
        [Description("Enter the MailItem to retrieve attachments from.")]
        [SampleUsage("vMailItem")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(MailItem) })]
        public string v_MailItem { get; set; }

        [Required]
        [DisplayName("Output Attachment Directory")]
        [Description("Enter or Select the path to the directory to store the attachments in.")]
        [SampleUsage("@\"C:\\temp\" || ProjectPath + @\"\\temp\" || vDirectoryPath")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [Editor("ShowFolderSelectionHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(string) })]
        public string v_AttachmentDirectory { get; set; }

        [Required]
        [DisplayName("Include Embedded Images")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [Description("Specify whether to consider images in body as attachments.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_IncludeEmbeddedImagesAsAttachments { get; set; }

        [Required]
        [Editable(false)]
        [DisplayName("Output Attachment List Variable")]
        [Description("Create a new variable or select a variable from the list.")]
        [SampleUsage("vUserVariable")]
        [Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
        [CompatibleTypes(new Type[] { typeof(List<string>) })]
        public string v_OutputUserVariableName { get; set; }
        
        public GetOutlookEmailAttachmentsCommand()
        {
            CommandName = "GetOutlookEmailAttachmentsCommand";
            SelectionName = "Get Outlook Email Attachments";
            CommandEnabled = true;
            CommandIcon = Resources.command_smtp;
        }

        public async override Task RunCommand(object sender)
        {
            var engine = (IAutomationEngineInstance)sender;
            MailItem email = (MailItem)await v_MailItem.EvaluateCode(engine);
            bool includeEmbeds = v_IncludeEmbeddedImagesAsAttachments.Equals("Yes");
            string attDirectory = (string)await v_AttachmentDirectory.EvaluateCode(engine);

            List<string> attachmentList = new List<string>();

            foreach (Attachment attachment in email.Attachments)
            {
                bool isEmbed = email.HTMLBody.Contains(attachment.FileName);
                if (isEmbed && includeEmbeds)
                {
                    attachmentList.Add(Path.Combine(attDirectory, attachment.FileName));
                    attachment.SaveAsFile(Path.Combine(attDirectory, attachment.FileName));
                }
                else if (!isEmbed)
                {
                    attachmentList.Add(Path.Combine(attDirectory, attachment.FileName));
                    attachment.SaveAsFile(Path.Combine(attDirectory, attachment.FileName));
                }
            }

            attachmentList.SetVariableValue(engine, v_OutputUserVariableName);
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_MailItem", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_AttachmentDirectory", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_IncludeEmbeddedImagesAsAttachments", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [From '{v_MailItem}' - Store Attachment List in '{v_OutputUserVariableName}']";
        }
    }
}
