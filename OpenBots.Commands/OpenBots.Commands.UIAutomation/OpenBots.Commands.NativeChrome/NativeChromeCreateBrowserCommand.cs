using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Properties;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Threading.Tasks;

namespace OpenBots.Commands.NativeChrome
{
	[Serializable]
	[Category("Native Chrome Commands")]
	[Description("This command creates a new web browser session which enables automation for websites.")]
    public class NativeChromeCreateBrowserCommand : ScriptCommand
    {
		[Required]
		[DisplayName("Chrome Browser Instance Name")]
		[Description("Enter the unique instance that was specified in the **Create Browser** command.")]
		[SampleUsage("MyChromeBrowserInstance")]
		[Remarks("Failure to enter the correct instance name or failure to first call the **Create Browser** command will cause an error.")]
		[CompatibleTypes(new Type[] { typeof(Process) })]
		public string v_InstanceName { get; set; }

		[DisplayName("URL (Optional)")]
        [Description("Enter the URL that you want the selenium instance to navigate to.")]
        [SampleUsage("\"https://mycompany.com/orders\" || vURL")]
        [Remarks("This input is optional.")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_URL { get; set; }

		public NativeChromeCreateBrowserCommand()
        {
            CommandName = "NativeChromeCreateBrowserCommand";
            SelectionName = "Create Browser";
            CommandEnabled = true;
            CommandIcon = Resources.command_web;

			v_InstanceName = "DefaultChromeBrowser";
			v_URL = "\"https://\"";
        }

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var vURL = (string)await v_URL.EvaluateCode(engine);

			int chromeProcessCount = Process.GetProcessesByName("chrome").Length;
			var process = Process.Start("chrome.exe", vURL + " --new-window");

			if(chromeProcessCount > 0)
            {
				while (process.HasExited == false)
				{
					System.Threading.Thread.Sleep(100);
					process.Refresh();
				}
			}
			//Delay 3 seconds
			System.Threading.Thread.Sleep(3000);

			Process[] procsChrome = Process.GetProcessesByName("chrome");

			foreach (Process chrome in procsChrome)
			{
				// the chrome process must have a window
				if (chrome.MainWindowHandle == IntPtr.Zero)
					continue;
				process = chrome;
				break;
			}

			process.AddAppInstance(engine, v_InstanceName);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_URL", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return $"Create Browser [Navigate To URL '{v_URL}']";
		}
	}
}
