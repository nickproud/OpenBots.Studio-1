﻿using Newtonsoft.Json;
using OpenBots.Commands.UIAutomation.Library;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.ChromeNativeClient;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Interfaces;
using OpenBots.Core.Model.ApplicationModel;
using OpenBots.Core.Properties;
using OpenBots.Core.User32;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenBots.Commands.NativeChrome
{
    [Serializable]
	[Category("Native Chrome Commands")]
	[Description("This command gets an attribute from a specified element in Chrome.")]
	public class NativeChromeGetAttributeCommand : ScriptCommand
	{
		[Required]
		[DisplayName("Chrome Browser Instance Name")]
		[Description("Enter the unique instance that was specified in the **Create Browser** command.")]
		[SampleUsage("MyChromeBrowserInstance")]
		[Remarks("Failure to enter the correct instance name or failure to first call the **Create Browser** command will cause an error.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(OBAppInstance) })]
		public string v_InstanceName { get; set; }

		[Required]
		[DisplayName("Element Search Parameter")]
		[Description("Use the Element Recorder to generate a listing of potential search parameters." +
					"Select the specific search type(s) that you want to use to isolate the element on the web page.")]
		[SampleUsage(NativeHelper.SearchParameterSample)]
		[Remarks("If multiple parameters are enabled, an attempt will be made to find the element(s) that match(es) all the selected parameters.")]
		[Editor("ShowElementHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public DataTable v_NativeSearchParameters { get; set; }

		[Required]
		[DisplayName("Attribute Name")]
		[Description("Enter the name of the attribute to retrieve.")]
		[SampleUsage("\"ID\" || vAttribute")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_Attribute { get; set; }

		[Required]
		[DisplayName("Timeout (Seconds)")]
		[Description("Specify how many seconds to wait before throwing an exception.")]
		[SampleUsage("30 || vSeconds")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(int) })]
		public string v_Timeout { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output Attribute Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("vUserVariable")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_OutputUserVariableName { get; set; }

		public NativeChromeGetAttributeCommand()
		{
			CommandName = "NativeChromeGetAttributeCommand";
			SelectionName = "Native Chrome Get Attribute";
			CommandEnabled = true;
			CommandIcon = Resources.command_nativechrome;

			v_NativeSearchParameters = NativeHelper.CreateSearchParametersDT();
			v_Timeout = "30";
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var browserObject = ((OBAppInstance)await v_InstanceName.EvaluateCode(engine)).Value;
			var chromeProcess = (Process)browserObject;
			var vTargetText = (string)await v_Attribute.EvaluateCode(engine);
			var vTimeout = (int)await v_Timeout.EvaluateCode(engine);

			WebElement webElement = await NativeHelper.DataTableToWebElement(v_NativeSearchParameters, engine);

			webElement.SelectionRules = vTargetText;
			User32Functions.BringWindowToFront(chromeProcess.MainWindowHandle);

			string responseText;
			NativeRequest.ProcessRequest("getattribute", JsonConvert.SerializeObject(webElement), vTimeout, out responseText);
			NativeResponse responseObject = JsonConvert.DeserializeObject<NativeResponse>(responseText);
			if (responseObject.Status == "Failed")
				throw new Exception(responseObject.Result);
			responseObject.Result.SetVariableValue(engine, v_OutputUserVariableName);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			NativeHelper.AddDefaultSearchRows(v_NativeSearchParameters);
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultWebElementDataGridViewGroupFor("v_NativeSearchParameters", this, editor, 
				new Control[] { NativeHelper.NativeChromeRecorderControl(v_NativeSearchParameters, editor) }));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Attribute", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Timeout", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [{NativeHelper.GetSearchNameValue(v_NativeSearchParameters)} - Store Attribute in '{v_OutputUserVariableName}'" +
				$" - Instance Name '{v_InstanceName}']";
		}
	}
}
