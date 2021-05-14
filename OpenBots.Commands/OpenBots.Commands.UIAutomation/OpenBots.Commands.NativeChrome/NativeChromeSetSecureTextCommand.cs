using Newtonsoft.Json;
using OpenBots.Commands.UIAutomation.Library;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.ChromeNativeClient;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
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
using System.Security;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenBots.Commands.NativeChrome
{
    [Serializable]
	[Category("Native Chrome Commands")]
	[Description("This command sets secure text to a specified input in Chrome.")]
	public class NativeChromeSetSecureTextCommand : ScriptCommand
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
		[DisplayName("Clear Text")]
		[PropertyUISelectionOption("Yes")]
		[PropertyUISelectionOption("No")]
		[Description("Select whether the element should be cleared before setting text.")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_Option { get; set; }

		[Required]
		[DisplayName("SecureString To Set")]
		[Description("Enter the SecureString variable to set in the input.")]
		[SampleUsage("vSecureText")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(SecureString) })]
		public string v_SecureString { get; set; }

		public NativeChromeSetSecureTextCommand()
		{
			CommandName = "NativeChromeSetSecureTextCommand";
			SelectionName = "Native Chrome Set Secure Text";
			CommandEnabled = false;
			CommandIcon = Resources.command_nativechrome;

			v_InstanceName = "DefaultChromeBrowser";
			v_Option = "Yes";
			v_NativeSearchParameters = NativeHelper.CreateSearchParametersDT();
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var browserObject = ((OBAppInstance)await v_InstanceName.EvaluateCode(engine)).Value;
			var chromeProcess = (Process)browserObject;
			var secureStrVariable = (SecureString)await v_SecureString.EvaluateCode(engine);

			string secureString = secureStrVariable.ConvertSecureStringToString();

			WebElement webElement = await NativeHelper.DataTableToWebElement(v_NativeSearchParameters, engine);

			webElement.Value = secureString;
			webElement.SelectionRules = v_Option;
			User32Functions.BringWindowToFront(chromeProcess.Handle);

			string responseText;
			NativeRequest.ProcessRequest("settext", JsonConvert.SerializeObject(webElement), out responseText);
			NativeResponse responseObject = JsonConvert.DeserializeObject<NativeResponse>(responseText);
			if (responseObject.Status == "Failed")
				throw new Exception(responseObject.Result);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			NativeHelper.AddDefaultSearchRows(v_NativeSearchParameters);
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultWebElementDataGridViewGroupFor("v_NativeSearchParameters", this, editor, 
				new Control[] { NativeHelper.NativeChromeRecorderControl(v_NativeSearchParameters, editor) }));
			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_Option", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SecureString", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [{NativeHelper.GetSearchNameValue(v_NativeSearchParameters)} - Instance Name '{v_InstanceName}']";
		}
	}
}
