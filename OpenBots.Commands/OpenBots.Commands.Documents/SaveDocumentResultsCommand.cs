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
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenBots.Commands.Documents
{
    [Serializable]
    [Category("Documents Commands")]
    [Description("This command saves the processing results in a file system folder.")]
    public class SaveDocumentResultsCommand : ScriptCommand, ISaveRequest
    {
        [Required]
        [DisplayName("TaskId")]
        [Description("Task Identifier that was provided while submitting the document.")]
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
        [DisplayName("Save Page Images")]
        [Description("Allows the service to download Images of each page.")]
        [SampleUsage("true || vSavePageImages")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(bool) })]
        public string v_SavePageImages { get; set; }

        [Required]
        [DisplayName("Save Page Text")]
        [Description("Allows the service to download Text of each page.")]
        [SampleUsage("true || vSavePageText")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(bool) })]
        public string v_SavePageText { get; set; }

        [Required]
        [DisplayName("Timeout (Seconds)")]
        [Description("Specify how many seconds to wait before throwing an exception.")]
        [SampleUsage("30 || vSeconds")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(int) })]
        public string v_Timeout { get; set; }

        [Required]
        [DisplayName("Output Folder")]
        [Description("Folder in which the resulting text and documents are saved.")]
        [SampleUsage("@\"C:\\temp\" || ProjectPath + \"\\temp\" || vFolderPath")]
        [Remarks("ProjectPath is the directory path of the current project.")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [Editor("ShowFolderSelectionHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(string) })]
        public string v_OutputFolder { get; set; }

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
        [DisplayName("Output HasFailed Bool Variable")]
        [Description("Create a new variable or select a variable from the list.")]
        [SampleUsage("vUserVariable")]
        [Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
        [CompatibleTypes(new Type[] { typeof(bool) })]
        public string v_OutputUserVariableName2 { get; set; }
        //"Returns if the document processing has errors or has failed."

        [Required]
        [Editable(false)]
        [DisplayName("Output DocumentInfo JSON Variable")]
        [Description("Create a new variable or select a variable from the list.")]
        [SampleUsage("vUserVariable")]
        [Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
        [CompatibleTypes(new Type[] { typeof(string) })]
        public string v_OutputUserVariableName3 { get; set; }
        //"Returns the documents extracted as an output for this task as a JSON String"

        [Required]
        [Editable(false)]
        [DisplayName("Output DocumentInfo DataTable Variable")]
        [Description("Create a new variable or select a variable from the list.")]
        [SampleUsage("vUserVariable")]
        [Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
        [CompatibleTypes(new Type[] { typeof(DataTable) })]
        public string v_OutputUserVariableName4 { get; set; }
        //"Returns the documents extracted as an output for this task as a DataTable. Columns are DocumentNumber " +
        //"(int), Schema (string), PageNumbers (string), Folder (string), DocumentId (string), Confidence (double)"

        [Required]
        [Editable(false)]
        [DisplayName("Output Data DataTable Variable")]
        [Description("Create a new variable or select a variable from the list.")]
        [SampleUsage("vUserVariable")]
        [Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
        [CompatibleTypes(new Type[] { typeof(DataTable) })]
        public string v_OutputUserVariableName5 { get; set; }
        //"Appends the data extracted as an output for this task in a DataTable. Columns are TaskId (string), " +
        //"DocumentId (string), DocumentNumber (int), Schema (string), PageNumbers (string), <fields of from all " +
        //"schemas found>. Simply use 'Write CSV' to save these results."

        public SaveDocumentResultsCommand()
        {
            CommandName = "SaveDocumentResultsCommand";
            SelectionName = "Save Document Results";
            CommandEnabled = true;
            CommandIcon = Resources.command_documents;

            v_AwaitCompletion = "false";
            v_SavePageImages = "false";
            v_SavePageText = "false";
            v_Timeout = "120";
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

            Guid vHumanTaskId = (Guid)await v_TaskId.EvaluateCode(engine);
            bool vSavePageText = (bool)await v_SavePageText.EvaluateCode(engine);
            bool vSavePageImages = (bool)await v_SavePageImages.EvaluateCode(engine);
            string vOutputFolder = (string)await v_OutputFolder.EvaluateCode(engine);
            bool vAwaitCompletion = (bool)await v_AwaitCompletion.EvaluateCode(engine);
            int vTimeout = (int)await v_Timeout.EvaluateCode(engine);

            var vData = await v_OutputUserVariableName5.EvaluateCode(engine);
            DataTable dataDt = vData == null ? null : (DataTable)vData;

            var userInfo = authMethods.GetDocumentsAuthToken(vUsername, vPassword);
            DocumentResult docInfo = DocumentMethods.SaveDocumentResults(userInfo, vHumanTaskId, vAwaitCompletion, vSavePageImages, vSavePageText, vTimeout,
                vOutputFolder, dataDt);
            string docInfoAsJSON = docInfo.OutputAsJSON;
            DataTable docInfoAsDataTable = docInfo.DataAsTable;
            dataDt = docInfo.OutputAsTable;

            docInfo.Status.SetVariableValue(engine, v_OutputUserVariableName);
            docInfo.IsCompleted.SetVariableValue(engine, v_OutputUserVariableName1);
            docInfo.HasFailedOrError.SetVariableValue(engine, v_OutputUserVariableName2);
            docInfoAsJSON.SetVariableValue(engine, v_OutputUserVariableName3);
            docInfoAsDataTable.SetVariableValue(engine, v_OutputUserVariableName4);
            dataDt.SetVariableValue(engine, v_OutputUserVariableName5);
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
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SavePageImages", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SavePageText", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Timeout", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_OutputFolder", this, editor));
            
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName1", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName2", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName3", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName4", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName5", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Store Document Status in '{v_OutputUserVariableName}']";
        }
    }
}
