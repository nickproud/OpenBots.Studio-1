using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Interfaces;
using OpenBots.Core.Properties;
using OpenBots.Core.Server_Documents.Interfaces;
using OpenBots.Core.Server_Documents.User;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Server.SDK.HelperMethods;
using OpenBots.Server.SDK.Model;
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
    [Description("This command submits a file for processing by creating a new Task.")]
    public class SubmitDocumentCommand : ScriptCommand, ISubmitFileRequest
    {
        [Required]
        [DisplayName("File Path")]
        [Description("Path of the file to be submitted.")]
        [SampleUsage("@\"C:\\temp\\myfile.pdf\" || ProjectPath + @\"\\myfile.pdf\" || vFilePath")]
        [Remarks("ProjectPath} is the directory path of the current project.")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [Editor("ShowFileSelectionHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(string) })]

        public string v_FilePath { get; set; }

        [DisplayName("Task Queue Name (Optional)")]
        [Description("Name of the Queue that this task would be created in. If unspecified, it will be defauted to the 'Default' queue.")]
        [SampleUsage("\"My Queue Name\" || vQueueName")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(string) })]
        public string v_QueueName { get; set; }

        [Required]
        [DisplayName("Task Name/Title")]
        [Description("Name or Title of the task to be created.")]
        [SampleUsage("\"My OB Document\" || vTaskName")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(string) })]
        public string v_Name { get; set; }

        [DisplayName("Description (Optional)")]
        [Description("Description of the task for Reviewers or other downstream processing.")]
        [SampleUsage("\"Hello World\" || vDescription")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(string) })]
        public string v_Description { get; set; }

        [DisplayName("Case Number (Optional)")]
        [Description("A case number for reference.")]
        [SampleUsage("\"123\" || vCaseNumber")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(string) })]
        public string v_CaseNumber { get; set; }

        [DisplayName("Case Type (Optional)")]
        [Description("A case type for reference.")]
        [SampleUsage("\"Test\" || vCaseType")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(string) })]
        public string v_CaseType { get; set; }

        [DisplayName("Assign To User (Optional)")]
        [Description("Name of the user to assign the task.")]
        [SampleUsage("\"Test User\" || vUser")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(string) })]
        public string v_AssignedTo { get; set; }

        [DisplayName("Task Due Date (Optional)")]
        [Description("Due Date for the Task.")]
        [SampleUsage("new DateTime(2020, 2, 20) || vDate || DateTime.Now")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(DateTime) })]
        public string v_DueDate { get; set; }

        [Required]
        [Editable(false)]
        [DisplayName("Output TaskId Guid Variable")]
        [Description("Create a new variable or select a variable from the list.")]
        [SampleUsage("vUserVariable")]
        [Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
        [CompatibleTypes(new Type[] { typeof(Guid) })]
        public string v_OutputUserVariableName { get; set; } //"An identification number for the task created that can be used for subsequent calls / queries."

        [Required]
        [Editable(false)]
        [DisplayName("Output Document Status Variable")]
        [Description("Create a new variable or select a variable from the list.")]
        [SampleUsage("vUserVariable")]
        [Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
        [CompatibleTypes(new Type[] { typeof(string) })]
        public string v_OutputUserVariableName2 { get; set; } //"Status of the task just after submitting the document. Expect 'Created' or 'InProgress'"

        public SubmitDocumentCommand()
        {
            CommandName = "SubmitDocumentCommand";
            SelectionName = "Submit Document";
            CommandEnabled = true;
            CommandIcon = Resources.command_documents;
        }

        public override async Task RunCommand(object sender)
        {
            var engine = (IAutomationEngineInstance)sender;

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

            string vFileToProcess = (string)await v_FilePath.EvaluateCode(engine);
            string vTaskQueueName = (string)await v_QueueName.EvaluateCode(engine);
            string vName = (string)await v_Name.EvaluateCode(engine);
            string vDescription = (string)await v_Description.EvaluateCode(engine);
            string vCaseNumber = (string)await v_CaseNumber.EvaluateCode(engine);
            string vCaseType = (string)await v_CaseType.EvaluateCode(engine);
            string vAssignedTo = (string)await v_AssignedTo.EvaluateCode(engine);

            dynamic vDueOn = v_DueDate.EvaluateCode(engine);
            if (!string.IsNullOrEmpty(v_DueDate))
                vDueOn = (DateTime)await v_DueDate.EvaluateCode(engine);

            UserInfo userInfo = authMethods.GetDocumentsAuthToken(vUsername, vPassword);
            var docInfo = DocumentMethods.SubmitDocument(userInfo, vFileToProcess, vTaskQueueName, vName, vDescription, vCaseType, vCaseNumber,
                vAssignedTo, vDueOn);

            Guid taskId = Guid.Parse(docInfo["TaskID"]);
            string status = docInfo["Status"];
            taskId.SetVariableValue(engine, v_OutputUserVariableName);
            status.SetVariableValue(engine, v_OutputUserVariableName2);
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

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_FilePath", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_QueueName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Name", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Description", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_CaseNumber", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_CaseType", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_AssignedTo", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_DueDate", this, editor));

            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName2", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Store TaskId in '{v_OutputUserVariableName}' - Store Document Status in '{v_OutputUserVariableName2}']";
        }
    }
}
