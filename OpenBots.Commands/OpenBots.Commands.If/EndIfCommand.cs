using OpenBots.Core.Command;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace OpenBots.Commands.If
{
    [Serializable]
    [Category("If Commands")]
    [Description("This command signifies the exit point of If action(s) and is required for all the Begin If commands.")]
    public class EndIfCommand : ScriptCommand
    {
        public EndIfCommand()
        {
            CommandName = "EndIfCommand";
            SelectionName = "End If";
            CommandEnabled = true;
            CommandIcon = Resources.command_end_if;

        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return "End If";
        }
    }
}