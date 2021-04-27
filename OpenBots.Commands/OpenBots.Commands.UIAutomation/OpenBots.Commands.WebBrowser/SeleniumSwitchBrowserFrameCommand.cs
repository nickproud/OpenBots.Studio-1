using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Properties;
using OpenBots.Core.Utilities.CommonUtilities;

using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenBots.Commands.WebBrowser
{
	[Serializable]
	[Category("Web Browser Commands")]
	[Description("This command switches between browser frames provided a valid search parameter.")]  
	public class SeleniumSwitchBrowserFrameCommand : ScriptCommand
	{
		[Required]
		[DisplayName("Browser Instance Name")]
		[Description("Enter the unique instance that was specified in the **Create Browser** command.")]
		[SampleUsage("MyBrowserInstance")]
		[Remarks("Failure to enter the correct instance name or failure to first call the **Create Browser** command will cause an error.")]
		[CompatibleTypes(new Type[] { typeof(IWebDriver) })]
		public string v_InstanceName { get; set; }

		[Required]
		[DisplayName("Frame Search Type")]
		[PropertyUISelectionOption("Index")]
		[PropertyUISelectionOption("Name or ID")]
		[PropertyUISelectionOption("Parent Frame")]
		[PropertyUISelectionOption("Default Content")]
		[PropertyUISelectionOption("Alert")]
		[Description("Select an option which best fits the search type you would like to use.")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_SelectionType { get; set; }

		[Required]
		[DisplayName("Frame Search Parameter")]
		[Description("Provide the parameter to match (ex. Index, Name or ID).")]
		[SampleUsage("1 || \"name\" || vSearchData")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string), typeof(int) })]
		public string v_FrameParameter { get; set; }

		public SeleniumSwitchBrowserFrameCommand()
		{
			CommandName = "SeleniumSwitchBrowserFrameCommand";
			SelectionName = "Switch Browser Frame";
			CommandEnabled = true;
			CommandIcon = Resources.command_web;

			v_InstanceName = "DefaultBrowser";
			v_SelectionType = "Index";
			v_FrameParameter = "0";
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var browserObject = v_InstanceName.GetAppInstance(engine);
			var seleniumInstance = (IWebDriver)browserObject;
			dynamic frameIndex = await v_FrameParameter.EvaluateCode(engine);

			switch (v_SelectionType)
			{
				case "Index":
					var intFrameIndex = (int)frameIndex;
					seleniumInstance.SwitchTo().Frame(intFrameIndex);
					break;
				case "Name or ID":
					seleniumInstance.SwitchTo().Frame((string)frameIndex);
					break;
				case "Parent Frame":
					seleniumInstance.SwitchTo().ParentFrame();
					break;
				case "Default Content":
					seleniumInstance.SwitchTo().DefaultContent();
					break;
				case "Alert":
					seleniumInstance.SwitchTo().Alert();
					break;
				default:
					throw new NotImplementedException($"Logic to Select Frame '{v_SelectionType}' Not Implemented");
			}
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_SelectionType", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_FrameParameter", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [To {v_SelectionType} '{v_FrameParameter}' - Instance Name '{v_InstanceName}']";
		}
	}
}