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
    [Description("This command signifies the exit point of a Loop command and is required for all Loop commands.")]
    public class EndLoopCommand : ScriptCommand
    {
        public string LoopType { get; set; }

        public EndLoopCommand()
        {
            CommandName = "EndLoopCommand";
            SelectionName = "End Loop";
            CommandEnabled = true;
            CommandIcon = Resources.command_endloop;
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return $"End {LoopType}";
        }
    }
}