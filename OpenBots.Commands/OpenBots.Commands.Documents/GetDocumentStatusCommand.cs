using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Enums;
using OpenBots.Core.Interfaces;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenBots.Core.Server_Documents.Interfaces;
using OpenBots.Core.Server_Documents.User;
using OpenBots.Server.SDK.HelperMethods;
using OpenBots.Server.SDK.Model;
using OpenBots.Core.Command;
using OpenBots.Core.Properties;

namespace OpenBots.Commands.Documents
{
    [Serializable]
    [Category("Documents Commands")]
    [Description("This command retrieves the current status of the document being processed. It can also wait for the document's completion.")]
    public class GetDocumentStatusCommand : ScriptCommand, IGetStatusRequest
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
        [DisplayName("Await Completion")]
        [Description("Define if the activity should wait until the document processing is completed. Defaults to False. " +
                     "Awaiting queries the service for status every 10 seconds until completed.")]
        [SampleUsage("true || vAwaitCompletion")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(bool) })]
        public string v_AwaitCompletion { get; set; }

        [Required]
        [DisplayName("Timeout (Seconds)")]
        [Description("Specify how many seconds to wait before throwing an exception.")]
        [SampleUsage("30 || vSeconds")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(int) })]
        public string v_Timeout { get; set; }

        [Required]
        [Editable(false)]
        [DisplayName("Output Document Status Variable")]
        [Description("Create a new variable or select a variable from the list.")]
        [SampleUsage("vUserVariable")]
        [Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
        [CompatibleTypes(new Type[] { typeof(string) })]
        public string v_OutputUserVariableName { get; set; }
        //"Returns the status of the processing."

        [Required]
        [Editable(false)]
        [DisplayName("Output IsCompleted Bool Variable")]
        [Description("Create a new variable or select a variable from the list.")]
        [SampleUsage("vUserVariable")]
        [Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
        [CompatibleTypes(new Type[] { typeof(bool) })]
        public string v_OutputUserVariableName1 { get; set; }
        //"Returns if the document processing was completed."

        [Required]
        [Editable(false)]
        [DisplayName("Output HasError Bool Variable")]
        [Description("Create a new variable or select a variable from the list.")]
        [SampleUsage("vUserVariable")]
        [Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
        [CompatibleTypes(new Type[] { typeof(bool) })]
        public string v_OutputUserVariableName2 { get; set; }
        //"Document Processing has errors and couldnt complete."

        [Required]
        [Editable(false)]
        [DisplayName("Output IsCurrentlyProcessing Bool Variable")]
        [Description("Create a new variable or select a variable from the list.")]
        [SampleUsage("vUserVariable")]
        [Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
        [CompatibleTypes(new Type[] { typeof(bool) })]
        public string v_OutputUserVariableName3 { get; set; }
        //"Document is currently being processed."

        [Required]
        [Editable(false)]
        [DisplayName("Output IsSuccessful Bool Variable")]
        [Description("Create a new variable or select a variable from the list.")]
        [SampleUsage("vUserVariable")]
        [Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
        [CompatibleTypes(new Type[] { typeof(bool) })]
        public string v_OutputUserVariableName4 { get; set; }
        //"Is Document Processing Completed Successfully and read for results data to be read."

        public GetDocumentStatusCommand()
        {
            CommandName = "GetDocumentStatusCommand";
            SelectionName = "Get Document Status";
            CommandEnabled = true;
            CommandIcon = Resources.command_documents;

            v_AwaitCompletion = "false";
            v_Timeout = "120";
        }

        public override async Task RunCommand(object sender)
        {
            var engine = (IAutomationEngineInstance)sender;
            var vTaskId = (Guid)await v_TaskId.EvaluateCode(engine);
            var vAwaitCompletion = (bool)await v_AwaitCompletion.EvaluateCode(engine);

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
            DocumentStatus docStatus = DocumentMethods.GetDocumentStatus(userInfo, vTaskId);

            if(vAwaitCompletion)
            {
                int vTimeout = (int)await v_Timeout.EvaluateCode(engine);
                docStatus = DocumentMethods.AwaitProcessing(userInfo, vTaskId, vTimeout);
            }

            docStatus.Status.SetVariableValue(engine, v_OutputUserVariableName);
            docStatus.IsDocumentCompleted.SetVariableValue(engine, v_OutputUserVariableName1);
            docStatus.HasError.SetVariableValue(engine, v_OutputUserVariableName2);
            docStatus.IsCurrentlyProcessing.SetVariableValue(engine, v_OutputUserVariableName3);
            docStatus.IsSuccessful.SetVariableValue(engine, v_OutputUserVariableName4);
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
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_AwaitCompletion", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Timeout", this, editor));
            
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName1", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName2", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName3", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName4", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Store Document Status in '{v_OutputUserVariableName}']";
        }
    }
}
