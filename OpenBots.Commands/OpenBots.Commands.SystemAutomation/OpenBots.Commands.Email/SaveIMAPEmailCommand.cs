using MimeKit;
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
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenBots.Commands.Email
{
    [Serializable]
    [Category("Email Commands")]
    [Description("This command saves an IMAP MimeMessage as an .eml file.")]
    public class SaveIMAPEmailCommand : ScriptCommand
    {
        [Required]
        [DisplayName("MimeMessage")]
        [Description("Enter the MimeMessage to retrieve attachments from.")]
        [SampleUsage("vMimeMessage")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(MimeMessage) })]
        public string v_MimeMessage { get; set; }

        [Required]
        [DisplayName("MimeMessage Location")]
        [Description("Enter or Select the path to the directory to store the MimeMessage in.")]
        [SampleUsage("@\"C:\\temp\" || ProjectPath + @\"\\temp\" || vDirectoryPath || ProjectPath")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [Editor("ShowFolderSelectionHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(string) })]
        public string v_MimeMessageDirectory { get; set; }

        [Required]
        [DisplayName("MimeMessage File Name")]
        [Description("Enter or Select the name of the email file.")]
        [SampleUsage("\"myEmail\" || vEmailName")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(string) })]
        public string v_MimeMessageFileName { get; set; }

        public SaveIMAPEmailCommand()
        {
            CommandName = "SaveIMAPEmailCommand";
            SelectionName = "Save IMAP Email";
            CommandEnabled = true;
            CommandIcon = Resources.command_smtp;
        }

        public async override Task RunCommand(object sender)
        {
            var engine = (IAutomationEngineInstance)sender;
            MimeMessage message = (MimeMessage)await v_MimeMessage.EvaluateCode(engine);
            string msgDirectory = (string)await v_MimeMessageDirectory.EvaluateCode(engine);
            string msgFileName = (string)await v_MimeMessageFileName.EvaluateCode(engine);

            if (!Directory.Exists(msgDirectory))
                Directory.CreateDirectory(msgDirectory);

            message.WriteTo(Path.Combine(msgDirectory, msgFileName + ".eml"));
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_MimeMessage", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_MimeMessageDirectory", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_MimeMessageFileName", this, editor));

            return RenderedControls;
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [MimeMessage '{v_MimeMessage}' as File Name '{v_MimeMessageFileName}']";
        }
    }
}
