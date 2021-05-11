using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Properties;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Windows.Forms;
using OpenBots.Core.User32;
using System.Threading.Tasks;
using OpenBots.Core.Enums;

namespace OpenBots.Commands.NativeChrome
{
	[Serializable]
	[Category("Native Chrome Commands")]
	[Description("This command maximize a native web browser.")]
	public class NativeChromeMaximizeBrowserCommand : ScriptCommand
	{
		[Required]
		[DisplayName("Chrome Browser Instance Name")]
		[Description("Enter the unique instance that was specified in the **Create Browser** command.")]
		[SampleUsage("MyChromeBrowserInstance")]
		[Remarks("Failure to enter the correct instance name or failure to first call the **Create Browser** command will cause an error.")]
		[CompatibleTypes(new Type[] { typeof(Process) })]
		public string v_InstanceName { get; set; }

		public NativeChromeMaximizeBrowserCommand()
		{
			CommandName = "NativeChromeMaximizeBrowserCommand";
			SelectionName = "Maximize Browser";
			v_InstanceName = "DefaultChromeBrowser";
			CommandEnabled = true;
			CommandIcon = Resources.command_web;
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var browserObject = v_InstanceName.GetAppInstance(engine);
			var chromeProcess = (Process)browserObject;

			User32Functions.SetWindowState(chromeProcess.MainWindowHandle, WindowState.SwMaximize);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Instance Name '{v_InstanceName}']";
		}
	}
}