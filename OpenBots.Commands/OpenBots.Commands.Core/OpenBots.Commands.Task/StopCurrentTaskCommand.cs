using OpenBots.Core.Command;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Tasks = System.Threading.Tasks;

namespace OpenBots.Commands.Task
{
    [Serializable]
    [Category("Task Commands")]
    [Description("This command stops the currently running task.")]
    public class StopCurrentTaskCommand : ScriptCommand
    {
        public StopCurrentTaskCommand()
        {
            CommandName = "StopCurrentTaskCommand";
            SelectionName = "Stop Current Task";
            CommandEnabled = true;
            CommandIcon = Resources.command_stop_process;

        }

        public async override Tasks.Task RunCommand(object sender)
        {
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue();
        }
    }
}