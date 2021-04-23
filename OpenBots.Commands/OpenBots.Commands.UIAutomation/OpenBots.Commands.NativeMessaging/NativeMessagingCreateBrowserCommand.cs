using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Properties;
using OpenBots.Core.UI.Controls;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Core.ChromeNativeClient;
using OpenBots.Core.User32;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Data;
using System.Diagnostics;
using OpenBots.Core.IO;

namespace OpenBots.Commands.NativeMessaging
{
    [Serializable]
    [Category("Native Messaging Commands")]
    [Description("This command creates a new web browser session which enables automation for websites.")]
    public class NativeMessagingCreateBrowserCommand : ScriptCommand
    {
		[Required]
		[DisplayName("Browser Instance Name")]
		[Description("Enter a unique name that will represent the application instance.")]
		[SampleUsage("MyBrowserInstance")]
		[Remarks("This unique name allows you to refer to the instance by name in future commands, " +
		 "ensuring that the commands you specify run against the correct application.")]
		[CompatibleTypes(new Type[] { typeof(Process) })]
		public string v_InstanceName { get; set; }

		[DisplayName("URL (Optional)")]
        [Description("Enter the URL that you want the selenium instance to navigate to.")]
        [SampleUsage("https://mycompany.com/orders || {vURL}")]
        [Remarks("This input is optional.")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(null, true)]
        public string v_URL { get; set; }

		public NativeMessagingCreateBrowserCommand()
        {
            CommandName = "NativeMessagingCreateBrowserCommand";
            SelectionName = "Create Browser";
            CommandEnabled = true;
            CommandIcon = Resources.command_web;

			v_InstanceName = "DefaultChromeBrowser";
			v_URL = "https://";
        }

		public override void RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var vURL = v_URL.ConvertUserVariableToString(engine);

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
			//Delay 7 seconds
			System.Threading.Thread.Sleep(7000);

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
