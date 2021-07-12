using OpenBots.Core.Command;
using OpenBots.Core.Interfaces;
using OpenBots.Core.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace OpenBots.Commands.Loop
{
    [Serializable]
    [Category("Loop Commands")]
    [Description("This command ends the current loop iteration and moves on to the next one.")]
    public class ContinueCommand : ScriptCommand
    {
        public ContinueCommand()
        {
            CommandName = "ContinueCommand";
            SelectionName = "Continue";
            CommandEnabled = true;
            CommandIcon = Resources.command_nextloop;
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