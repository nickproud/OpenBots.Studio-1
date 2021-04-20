using OpenBots.Core.Command;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Properties;
using OpenBots.Core.Script;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Tasks = System.Threading.Tasks;

namespace OpenBots.Commands.Misc
{
    [Serializable]
    [Category("Misc Commands")]
    [Description("This command groups multiple actions together.")]
    public class SequenceCommand : ScriptCommand, ISequenceCommand
    {
        [Browsable(false)]
        public List<ScriptCommand> ScriptActions { get; set; } = new List<ScriptCommand>();

        public SequenceCommand()
        {
            CommandName = "SequenceCommand";
            SelectionName = "Sequence Command";
            CommandEnabled = true;
            CommandIcon = Resources.command_sequence;

        }

        public async override Tasks.Task RunCommand(object sender, ScriptAction parentCommand)
        {
            var engine = (IAutomationEngineInstance)sender;

            foreach (var item in ScriptActions)
            {
                //exit if cancellation pending
                if (engine.IsCancellationPending)
                    return;

                //only run if not commented
                if (!item.IsCommented)
                    item.RunCommand(engine);
            }
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [{ScriptActions.Count()} Embedded Commands]";
        }
    }
}