using OpenBots.Commands.Server.Library;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Interfaces;
using OpenBots.Core.Properties;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Server.SDK.HelperMethods;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenBots.Commands.Job
{
    [Serializable]
    [Category("Job Commands")]
    [Description("This command checks the current job's status from OpenBots Server. " +
        "Returns true if job status is \"Stopping.\" " +
        "Returns false if job cannot be found or has another status.")]
    public class StopJobCommand : ScriptCommand
    {
        [Required]
        [Editable(false)]
        [DisplayName("Output Result Variable")]
        [Description("Create a new variable or select a variable from the list.")]
        [SampleUsage("vUserVariable")]
        [Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
        [CompatibleTypes(new Type[] { typeof(bool) })]
        public string v_OutputUserVariableName { get; set; }

        public StopJobCommand()
        {
            CommandName = "StopJobCommand";
            SelectionName = "Stop Job";
            CommandEnabled = true;
            CommandIcon = Resources.command_job;

            CommonMethods.InitializeDefaultWebProtocol();
        }

        public async override Task RunCommand(object sender)
        {
            var engine = (IAutomationEngineInstance)sender;

            var jobId = SessionVariableMethods.GetJobId(engine);
            string jobStatus = string.Empty;
            bool stopJob;

            if (!string.IsNullOrEmpty(jobId))
            {
                var userInfo = ServerSessionVariableMethods.GetUserInfo(engine);
                jobStatus = JobMethods.GetJobStatus(userInfo, jobId);
            }

            if (jobStatus == "Stopping")
                stopJob = true;
            else stopJob = false;

            stopJob.SetVariableValue(engine, v_OutputUserVariableName);
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" ['Store Result in '{v_OutputUserVariableName}']";
        }
    }
}
