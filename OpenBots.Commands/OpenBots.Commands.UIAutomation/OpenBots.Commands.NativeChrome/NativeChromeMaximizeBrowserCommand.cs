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
using OpenBots.Core.Model.ApplicationModel;

namespace OpenBots.Commands.NativeChrome
{
	[Serializable]
	[Category("Native Chrome Commands")]
	[Description("This command maximizes a Chrome browser.")]
	public class NativeChromeMaximizeBrowserCommand : ScriptCommand
	{
		[Required]
		[DisplayName("Chrome Browser Instance Name")]
		[Description("Enter the unique instance that was specified in the **Create Browser** command.")]
		[SampleUsage("MyChromeBrowserInstance")]
		[Remarks("Failure to enter the correct instance name or failure to first call the **Create Browser** command will cause an error.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(OBAppInstance) })]
		public string v_InstanceName { get; set; }

		public NativeChromeMaximizeBrowserCommand()
		{
			CommandName = "NativeChromeMaximizeBrowserCommand";
			SelectionName = "Native Chrome Maximize Browser";
			v_InstanceName = "DefaultChromeBrowser";
			CommandEnabled = true;
			CommandIcon = Resources.command_nativechrome;
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var browserObject = ((OBAppInstance)await v_InstanceName.EvaluateCode(engine)).Value;
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