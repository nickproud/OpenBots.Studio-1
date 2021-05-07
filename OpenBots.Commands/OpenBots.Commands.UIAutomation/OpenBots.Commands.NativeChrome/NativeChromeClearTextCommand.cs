using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Properties;
using OpenBots.Core.UI.Controls;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Core.ChromeNativeClient;
using OpenBots.Core.User32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;
using OpenBots.Commands.UIAutomation.Library;
using System.Linq;

namespace OpenBots.Commands.NativeChrome
{
	[Serializable]
	[Category("Native Chrome Commands")]
	[Description("This command clears text from input in chrome.")]
	public class NativeChromeClearTextCommand : ScriptCommand
	{
		[Required]
		[DisplayName("Chrome Browser Instance Name")]
		[Description("Enter the unique instance that was specified in the **Create Browser** command.")]
		[SampleUsage("MyChromeBrowserInstance")]
		[Remarks("Failure to enter the correct instance name or failure to first call the **Create Browser** command will cause an error.")]
		[CompatibleTypes(new Type[] { typeof(Process) })]
		public string v_InstanceName { get; set; }

		[Required]
		[DisplayName("Element Search Parameter")]
		[Description("Use the Element Recorder to generate a listing of potential search parameters." +
					"Select the specific search type(s) that you want to use to isolate the element on the web page.")]
		[SampleUsage("XPath : \"//*[@id='features']/div[2]/div/h2/div[\" + var1 + \"]/div\"" +
						 "\n\tRelative XPath : //*[@id=\"features\"]" +
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

		[JsonIgnore]
		[Browsable(false)]
		private DataGridView _searchParametersGridViewHelper;

		public NativeChromeClearTextCommand()
		{
			CommandName = "NativeChromeClearTextCommand";
			SelectionName = "Clear Text";
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
			var browserObject = v_InstanceName.GetAppInstance(engine);
			var chromeProcess = (Process)browserObject;

			WebElement webElement = await NativeHelper.DataTableToWebElement(v_NativeSearchParameters, engine);

			User32Functions.BringWindowToFront(chromeProcess.Handle);

			string responseText;
			NativeRequest.ProcessRequest("cleartext", JsonConvert.SerializeObject(webElement), out responseText);
			NativeResponse responseObject = JsonConvert.DeserializeObject<NativeResponse>(responseText);
			if (responseObject.Status == "Failed")
				throw new Exception(responseObject.Result);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			//create helper control
			CommandItemControl helperControl = new CommandItemControl();
			helperControl.Padding = new Padding(10, 0, 0, 0);
			helperControl.ForeColor = Color.AliceBlue;
			helperControl.Font = new Font("Segoe UI Semilight", 10);
			helperControl.CommandImage = Resources.command_camera;
			helperControl.CommandDisplay = "Element Recorder";
			helperControl.Click += new EventHandler((s, e) => ShowRecorder(s, e, editor, commandControls));

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
			//create search parameters   
			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_NativeSearchParameters", this));
			RenderedControls.Add(helperControl);

			//create search param grid
			_searchParametersGridViewHelper = commandControls.CreateDefaultDataGridViewFor("v_NativeSearchParameters", this);

			DataGridViewCheckBoxColumn enabled = new DataGridViewCheckBoxColumn();
			enabled.HeaderText = "Enabled";
			enabled.DataPropertyName = "Enabled";
			enabled.FillWeight = 30;
			_searchParametersGridViewHelper.Columns.Add(enabled);

			DataGridViewTextBoxColumn propertyName = new DataGridViewTextBoxColumn();
			propertyName.HeaderText = "Parameter Name";
			propertyName.DataPropertyName = "Parameter Name";
			propertyName.FillWeight = 40;
			_searchParametersGridViewHelper.Columns.Add(propertyName);

			DataGridViewTextBoxColumn propertyValue = new DataGridViewTextBoxColumn();
			propertyValue.HeaderText = "Parameter Value";
			propertyValue.DataPropertyName = "Parameter Value";
			_searchParametersGridViewHelper.Columns.Add(propertyValue);

			RenderedControls.AddRange(commandControls.CreateUIHelpersFor("v_NativeSearchParameters", this, new Control[] { _searchParametersGridViewHelper }, editor));
			RenderedControls.Add(_searchParametersGridViewHelper);

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

			return base.GetDisplayValue() + $" [Clear Text by {searchParameterName}" +
											$" '{searchParameterValue}' - Instance Name '{v_InstanceName}']";
		}
		public void ShowRecorder(object sender, EventArgs e, IfrmCommandEditor editor, ICommandControls commandControls)
		{
			try
			{
				User32Functions.BringChromeWindowToTop();

				string webElementStr;
				NativeRequest.ProcessRequest("getelement", "", out webElementStr);
				if (!string.IsNullOrEmpty(webElementStr))
				{
					WebElement webElement = JsonConvert.DeserializeObject<WebElement>(webElementStr);
					DataTable SearchParameters = NativeHelper.WebElementToDataTable(webElement);

					if (SearchParameters != null)
					{
						v_NativeSearchParameters.Rows.Clear();

						foreach (DataRow rw in SearchParameters.Rows)
							v_NativeSearchParameters.ImportRow(rw);

						_searchParametersGridViewHelper.DataSource = v_NativeSearchParameters;
						_searchParametersGridViewHelper.Refresh();
					}
				}
			}
			catch (Exception ex)
			{
				// Throw Error in Message Box
				var result = ((Form)editor).Invoke(new Action(() =>
				{
					editor.ShowMessage(ex.Message, "MessageBox", DialogType.OkOnly, 10);
				}
				));
			}
			finally
			{
				Process process = Process.GetCurrentProcess();
				User32Functions.ActivateWindow(process.MainWindowTitle);
			}
		}
	}
}
