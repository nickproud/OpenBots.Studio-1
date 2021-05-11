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
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenBots.Commands.NativeChrome
{
    [Serializable]
	[Category("Native Chrome Commands")]
	[Description("This command sets option on dropdown list in chrome.")]
	public class NativeChromeSetOptionCommand : ScriptCommand
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
		[SampleUsage("XPath : \"//*[@id='features']/div[2]/div/h2/div[\" + var1 + \"]/div\"" +
						 "\n\tRelative XPath : //*[@id='features']" +
						 "\n\tID: \"1\"" +
						 "\n\tName: \"my\" + var2 + \"Name\"" +
						 "\n\tTag Name: \"h1\"" +
						 "\n\tClass Name: \"myClass\"" +
						 "\n\tCSS Selector: \"[attribute=value]\"" +
						 "\n\tLink Text: \"https://www.mylink.com/\"")]
		[Remarks("If multiple parameters are enabled, an attempt will be made to find the element(s) that match(es) all the selected parameters.")]
		[Editor("ShowElementHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public DataTable v_NativeSearchParameters { get; set; }

		[Required]
		[DisplayName("Selection Rule")]
		[PropertyUISelectionOption("Select By Index")]
		[PropertyUISelectionOption("Select By Value")]
		[PropertyUISelectionOption("Select By Text")]
		[Description("Select whether the option should be selected by which of the follwoing.")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_Option { get; set; }

		[Required]
		[DisplayName("Selection Value")]
		[Description("Enter the value that will be set in the dropdown.")]
		[SampleUsage("\"Hello World\" || vText")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_SelectionValue { get; set; }

		public NativeChromeSetOptionCommand()
		{
			CommandName = "NativeChromeSetOptionCommand";
			SelectionName = "Set Option";
			CommandEnabled = true;
			CommandIcon = Resources.command_web;

			v_InstanceName = "DefaultChromeBrowser";
			v_Option = "Select By Index";
			//set up search parameter table
			v_NativeSearchParameters = new DataTable();
			v_NativeSearchParameters.Columns.Add("Enabled");
			v_NativeSearchParameters.Columns.Add("Parameter Name");
			v_NativeSearchParameters.Columns.Add("Parameter Value");
			v_NativeSearchParameters.TableName = DateTime.Now.ToString("v_NativeSearchParameters" + DateTime.Now.ToString("MMddyy.hhmmss"));
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var browserObject = ((OBAppInstance)await v_InstanceName.EvaluateCode(engine)).Value;
			var chromeProcess = (Process)browserObject;
			var vTargetText = (string)await v_SelectionValue.EvaluateCode(engine);

			WebElement webElement = await NativeHelper.DataTableToWebElement(v_NativeSearchParameters, engine);

			webElement.Value = vTargetText;
			webElement.SelectionRules = v_Option;
			User32Functions.BringWindowToFront(chromeProcess.Handle);

			string responseText;
			NativeRequest.ProcessRequest("setoption", JsonConvert.SerializeObject(webElement), out responseText);
			NativeResponse responseObject = JsonConvert.DeserializeObject<NativeResponse>(responseText);
			if (responseObject.Status == "Failed")
				throw new Exception(responseObject.Result);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));

			if (v_NativeSearchParameters.Rows.Count == 0)
			{
				v_NativeSearchParameters.Rows.Add(false, "\"XPath\"", "");
				v_NativeSearchParameters.Rows.Add(false, "\"Relative XPath\"", "");
				v_NativeSearchParameters.Rows.Add(false, "\"ID\"", "");
				v_NativeSearchParameters.Rows.Add(false, "\"Name\"", "");
				v_NativeSearchParameters.Rows.Add(false, "\"Tag Name\"", "");
				v_NativeSearchParameters.Rows.Add(false, "\"Class Name\"", "");
				v_NativeSearchParameters.Rows.Add(false, "\"Link Text\"", "");
				v_NativeSearchParameters.Rows.Add(true, "\"CSS Selector\"", "");
			}

			RenderedControls.AddRange(commandControls.CreateDefaultWebElementDataGridViewGroupFor("v_NativeSearchParameters", this, editor, new Control[] { NativeHelper.NativeChromeRecorderControl(v_NativeSearchParameters, editor) }));
			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_Option", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SelectionValue", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			string searchParameterName = (from rw in v_NativeSearchParameters.AsEnumerable()
										  where rw.Field<string>("Enabled") == "True"
										  select rw.Field<string>("Parameter Name")).FirstOrDefault();

			string searchParameterValue = (from rw in v_NativeSearchParameters.AsEnumerable()
										   where rw.Field<string>("Enabled") == "True"
										   select rw.Field<string>("Parameter Value")).FirstOrDefault();

			return base.GetDisplayValue() + $" [Set Options for {searchParameterName}" +
											$" '{searchParameterValue}' - Instance Name '{v_InstanceName}']";
		}
	}
}
