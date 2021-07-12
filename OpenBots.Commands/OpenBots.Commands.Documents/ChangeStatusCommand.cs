using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Interfaces;
using OpenBots.Core.Server_Documents.Models;
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
    [Description("This command changes the status of the document/task. Eg. Change Status to AwaitVerification for Human Review.")]
    public class ChangeStatusCommand : ScriptCommand
    {
        [Required]
        [DisplayName("TaskId")]
        [Description("Task Identifier that was provided while submiting the document.")]
        [SampleUsage("new Guid(\"13db91cf-1f65-4a14-a1cc-bf7aff751b83\") || vTaskId")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(Guid) })]
        public string v_TaskId { get; set; }

        [Required]
        [DisplayName("Task Status")]
        [Description("Status to change to.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_Status { get; set; }

        public ChangeStatusCommand()
        {
            CommandName = "ChangeStatusCommand";
            SelectionName = "Change Status";
            CommandEnabled = true;

            v_Status = "Created";
        }

        public override async Task RunCommand(object sender)
        {
            var engine = (IAutomationEngineInstance)sender;
            var vTaskID = (Guid)await v_TaskId.EvaluateCode(engine);
            var vStatus = v_Status.ToString();

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
            DocumentMethods.ChangeStatus(userInfo, vTaskID, vStatus);
            
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

            var terminalKeyNameLabel = commandControls.CreateDefaultLabelFor("v_Status", this);
            var terminalKeyNameComboBox = commandControls.CreateDropdownFor("v_Status", this);
            terminalKeyNameComboBox.DataSource = Enum.GetValues(typeof(TaskStatusTypes));

            RenderedControls.Add(terminalKeyNameLabel);
            RenderedControls.Add(terminalKeyNameComboBox);

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [TaskId '{v_TaskId}' - Task Status '{v_Status}']";
        }
    }
}
