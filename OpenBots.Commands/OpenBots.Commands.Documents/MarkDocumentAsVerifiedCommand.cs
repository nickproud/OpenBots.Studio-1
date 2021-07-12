using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Interfaces;
using OpenBots.Core.Properties;
using OpenBots.Core.Server_Documents.User;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Server.SDK.HelperMethods;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenBots.Commands.Documents
{
    [Serializable]
    [Category("Documents Commands")]
    [Description("This command marks a specific document in a bundle as 'Verfied'. The remaining documents typically go for 'Human Review'.")]
    public class MarkDocumentAsVerifiedCommand : ScriptCommand
    {
        [Required]
        [DisplayName("TaskId")]
        [Description("Task Identifier that was provided while submitting the document.")]
        [SampleUsage("new Guid(\"13db91cf-1f65-4a14-a1cc-bf7aff751b83\") || vTaskID")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(Guid) })]
        public string v_TaskId { get; set; }

        [Required]
        [DisplayName("DocumentId")]
        [Description("Document Identifier that was provided while retrieving the processing results.")]
        [SampleUsage("new Guid(\"13db91cf-1f65-4a14-a1cc-bf7aff751b83\") || vDocumentId")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(Guid) })]
        public string v_DocumentId { get; set; }

        public MarkDocumentAsVerifiedCommand()
        {
            CommandName = "MarkDocumentAsVerifiedCommand";
            SelectionName = "Mark Document As Verified";
            CommandEnabled = true;
            CommandIcon = Resources.command_documents;
        }

        public override async Task RunCommand(object sender)
        {
            var engine = (IAutomationEngineInstance)sender;

            var vHumanTaskId = (Guid)await v_TaskId.EvaluateCode(engine);
            var vDocId = (Guid)await v_DocumentId.EvaluateCode(engine);

            string vUsername;
            string vPassword;

            var environmentSettings = new EnvironmentSettings();
            environmentSettings.Load();
            AuthMethods authMethods = new AuthMethods();
            authMethods.Initialize(environmentSettings.ServerType, environmentSettings.OrganizationName, environmentSettings.ServerUrl, environmentSettings.Username, environmentSettings.Password);

            if (environmentSettings.ServerType == "Local")
            {
                throw new Exception("Documents commands cannot be used with local Server");
            }
            else
            {
                vUsername = environmentSettings.Username;
                vPassword = environmentSettings.Password;
            }

            var userInfo = authMethods.GetDocumentsAuthToken(vUsername, vPassword);
            DocumentMethods.MarkDocumentAsVerified(userInfo, vHumanTaskId, vDocId);
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            //var environmentSettings = new EnvironmentSettings();
            //environmentSettings.Load();

            //if (environmentSettings.ServerType == "Local")
            //{
            //    throw new Exception("Documents commands cannot be used with local Server");
            //}

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_TaskId", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_DocumentId", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [TaskId '{v_TaskId}' - DocumentId '{v_DocumentId}']";
        }
    }
}
