using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Properties;
using OpenBots.Core.User32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;
using OBProcess = System.Diagnostics.Process;
using Tasks = System.Threading.Tasks;

namespace OpenBots.Commands.System
{
    [Serializable]
	[Category("System Commands")]
	[Description("This command performs a system action.")]
	public class SystemActionCommand : ScriptCommand
	{

		[Required]
		[DisplayName("System Action")]
		[PropertyUISelectionOption("Shutdown")]
		[PropertyUISelectionOption("Restart")]
		[PropertyUISelectionOption("Logoff")]
		[PropertyUISelectionOption("Lock Screen")]
		[Description("Select a system action from one of the options.")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_ActionName { get; set; }

		public SystemActionCommand()
		{
			CommandName = "SystemActionCommand";
			SelectionName = "System Action";
			CommandEnabled = true;
			CommandIcon = Resources.command_system;

			v_ActionName = "Shutdown";
		}

		public async override Tasks.Task RunCommand(object sender)
		{
			switch (v_ActionName)
			{
				case "Shutdown":
					OBProcess.Start("shutdown", "/s /t 0");
					break;
				case "Restart":
					OBProcess.Start("shutdown", "/r /t 0");
					break;
				case "Logoff":
					User32Functions.WindowsLogOff();
					break;
				case "Lock Screen":
					User32Functions.LockWorkStation();
					break;
				default:
					break;
			}
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_ActionName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [{v_ActionName}]";
		}
	}
}