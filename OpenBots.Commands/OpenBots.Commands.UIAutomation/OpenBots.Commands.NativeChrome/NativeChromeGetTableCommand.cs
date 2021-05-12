using HtmlAgilityPack;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace OpenBots.Commands.NativeChrome
{
    [Serializable]
	[Category("Native Chrome Commands")]
	[Description("This command gets table from web element in chrome.")]
	public class NativeChromeGetTableCommand : ScriptCommand
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
		[Editable(false)]
		[DisplayName("Output DataTable Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("{vUserVariable}")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(DataTable) })]
		public string v_OutputUserVariableName { get; set; }

		public NativeChromeGetTableCommand()
		{
			CommandName = "NativeChromeGetTableCommand";
			SelectionName = "Get Table";
			CommandEnabled = true;
			CommandIcon = Resources.command_web;

			v_InstanceName = "DefaultChromeBrowser";
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

			WebElement webElement = await NativeHelper.DataTableToWebElement(v_NativeSearchParameters, engine);

			User32Functions.BringWindowToFront(chromeProcess.Handle);

			string responseText;
			NativeRequest.ProcessRequest("gettable", JsonConvert.SerializeObject(webElement), out responseText);
			NativeResponse responseObject = JsonConvert.DeserializeObject<NativeResponse>(responseText);
			if (responseObject.Status == "Failed")
				throw new Exception(responseObject.Result);

			HtmlDocument doc = new HtmlDocument();

			//Load Source (String) as HTML Document
			doc.LoadHtml(responseObject.Result);

			//Get Header Tags
			var headers = doc.DocumentNode.SelectNodes("//tr/th");
			DataTable DT = new DataTable();

			//If headers found
			if (headers != null && headers.Count != 0)
			{
				// add columns from th (headers)
				foreach (HtmlNode header in headers)
					DT.Columns.Add(Regex.Replace(header.InnerText, @"\t|\n|\r", "").Trim());
			}
			else
			{
				var columnsCount = doc.DocumentNode.SelectSingleNode("//tr[1]").ChildNodes.Where(node => node.Name == "td").Count();
				DT.Columns.AddRange((Enumerable.Range(1, columnsCount).Select(dc => new DataColumn())).ToArray());
			}

			// select rows with td elements and load each row (containing <td> tags) into DataTable
			foreach (var row in doc.DocumentNode.SelectNodes("//tr[td]"))
				DT.Rows.Add(row.SelectNodes("td").Select(td => Regex.Replace(td.InnerText, @"\t|\n|\r", "").Trim()).ToArray());

			DT.SetVariableValue(engine, v_OutputUserVariableName);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			NativeHelper.AddDefaultSearchRows(v_NativeSearchParameters);
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultWebElementDataGridViewGroupFor("v_NativeSearchParameters", this, editor, new Control[] { NativeHelper.NativeChromeRecorderControl(v_NativeSearchParameters, editor) }));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

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

			return base.GetDisplayValue() + $" [Get Table by {searchParameterName}" +
											$" '{searchParameterValue}' - Store Table in '{v_OutputUserVariableName}' - Instance Name '{v_InstanceName}']";
		}
	}
}
