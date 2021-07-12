using Microsoft.Office.Interop.Outlook;
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
using System.Windows.Forms;
using System.IO;
using System.Threading.Tasks;

namespace OpenBots.Commands.Outlook
{
    [Serializable]
    [Category("Outlook Commands")]
    [Description("This command saves an Outlook MailItem as as .msg file.")]
    public class SaveOutlookEmailCommand : ScriptCommand
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
        [DisplayName("MailItem Location")]
        [Description("Enter or Select the path to the directory to store the MailItem in.")]
        [SampleUsage("@\"C:\\temp\" || ProjectPath + @\"\\temp\" || vDirectoryPath || ProjectPath")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [Editor("ShowFolderSelectionHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(string) })]
        public string v_MailItemDirectory { get; set; }

        [Required]
        [DisplayName("MailItem File Name")]
        [Description("Enter or Select the name of the email file.")]
        [SampleUsage("\"myEmail\" || vEmailName")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(string) })]
        public string v_MailItemFileName { get; set; }

        public SaveOutlookEmailCommand()
        {
            CommandName = "SaveOutlookEmailCommand";
            SelectionName = "Save Outlook Email";
            CommandEnabled = true;
            CommandIcon = Resources.command_outlook;
        }

        public async override Task RunCommand(object sender)
        {
            var engine = (IAutomationEngineInstance)sender;
            MailItem message = (MailItem)await v_MailItem.EvaluateCode(engine);
            string msgDirectory = (string)await v_MailItemDirectory.EvaluateCode(engine);
            string msgFileName = (string)await v_MailItemFileName.EvaluateCode(engine);

            if (!Directory.Exists(msgDirectory))
                Directory.CreateDirectory(msgDirectory);

            message.SaveAs(Path.Combine(msgDirectory, msgFileName + ".msg"));
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_MailItem", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_MailItemDirectory", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_MailItemFileName", this, editor));

            return RenderedControls;
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [MailItem '{v_MailItem}' as File Name '{v_MailItemFileName}']";
        }
    }
}
