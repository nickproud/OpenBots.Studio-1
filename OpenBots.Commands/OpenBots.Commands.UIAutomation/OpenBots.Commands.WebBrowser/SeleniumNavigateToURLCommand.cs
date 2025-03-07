﻿using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Interfaces;
using OpenBots.Core.Model.ApplicationModel;
using OpenBots.Core.Properties;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenQA.Selenium;
using SHDocVw;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenBots.Commands.WebBrowser
{
	[Serializable]
	[Category("Web Browser Commands")]
	[Description("This command navigates a Selenium web browser session to a provided URL.")]
	public class SeleniumNavigateToURLCommand : ScriptCommand
	{
		[Required]
		[DisplayName("Browser Instance Name")]
		[Description("Enter the unique instance that was specified in the **Create Browser** command.")]
		[SampleUsage("MyBrowserInstance")]
		[Remarks("Failure to enter the correct instance name or failure to first call the **Create Browser** command will cause an error.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(OBAppInstance) })]
		public string v_InstanceName { get; set; }

		[Required]
		[DisplayName("URL")]
		[Description("Enter the URL that you want the selenium instance to navigate to.")]
		[SampleUsage("\"https://mycompany.com/orders\" || vURL")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_URL { get; set; }

		public SeleniumNavigateToURLCommand()
		{
			CommandName = "SeleniumNavigateToURLCommand";
			SelectionName = "Selenium Navigate to URL";
			CommandEnabled = true;
			CommandIcon = Resources.command_web;

			v_URL = "\"https://\"";
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var browserObject = ((OBAppInstance)await v_InstanceName.EvaluateCode(engine)).Value;
			var vURL = (string)await v_URL.EvaluateCode(engine);
			var seleniumInstance = (IWebDriver)browserObject;

			try
			{
				seleniumInstance.Navigate().GoToUrl(vURL);
			}
			catch (Exception ex)
			{
				if (!vURL.StartsWith("https://"))
					seleniumInstance.Navigate().GoToUrl("https://" + vURL);
				else
					throw ex;
			}           
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
			return base.GetDisplayValue() + $" [URL '{v_URL}' - Instance Name '{v_InstanceName}']";
		}

		private void WaitForReadyState(InternetExplorer ieInstance)
		{
			DateTime waitExpires = DateTime.Now.AddSeconds(15);
			do
			{
				Thread.Sleep(500);
			}
			while ((ieInstance.ReadyState != tagREADYSTATE.READYSTATE_COMPLETE) && (waitExpires > DateTime.Now));
		}
	}
}