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

namespace OpenBots.Commands.NativeMessaging
{
	[Serializable]
	[Category("Native Messaging Commands")]
	[Description("This command sets option on dropdown list in chrome.")]
	public class NativeMessagingSetOptionCommand : ScriptCommand
	{
		[Required]
		[DisplayName("Browser Instance Name")]
		[Description("Enter the unique instance that was specified in the **Create Browser** command.")]
		[SampleUsage("MyBrowserInstance")]
		[Remarks("Failure to enter the correct instance name or failure to first call the **Create Browser** command will cause an error.")]
		[CompatibleTypes(new Type[] { typeof(Process) })]
		public string v_InstanceName { get; set; }

		[Required]
		[DisplayName("Element Search Parameter")]
		[Description("Use the Element Recorder to generate a listing of potential search parameters." +
					"Select the specific search type(s) that you want to use to isolate the element on the web page.")]
		[SampleUsage("XPath : //*[@id=\"features\"]/div[2]/div/h2/div[{var1}]/div" +
						 "\n\tRelative XPath : //*[@id=\"features\"]" +
						 "\n\tID: 1" +
						 "\n\tName: my{var2}Name" +
						 "\n\tTag Name: h1" +
						 "\n\tClass Name: myClass" +
						 "\n\tCSS Selector: [attribute=value]" +
						 "\n\tLink Text: https://www.mylink.com/")]
		[Remarks("If multiple parameters are enabled, an attempt will be made to find the element(s) that match(es) all the selected parameters.")]
		[Editor("ShowElementHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(null, true)]
		public DataTable v_SeleniumSearchParameters { get; set; }

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
		[SampleUsage("Hello World || {vText}")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(null, true)]
		public string v_SelectionValue { get; set; }

		[JsonIgnore]
		[Browsable(false)]
		private DataGridView _searchParametersGridViewHelper;

		public NativeMessagingSetOptionCommand()
		{
			CommandName = "NativeMessagingSetOptionCommand";
			SelectionName = "Set Option";
			CommandEnabled = true;
			CommandIcon = Resources.command_web;

			v_InstanceName = "DefaultChromeBrowser";
			v_Option = "Select By Index";
			//set up search parameter table
			v_SeleniumSearchParameters = new DataTable();
			v_SeleniumSearchParameters.Columns.Add("Enabled");
			v_SeleniumSearchParameters.Columns.Add("Parameter Name");
			v_SeleniumSearchParameters.Columns.Add("Parameter Value");
			v_SeleniumSearchParameters.TableName = DateTime.Now.ToString("v_SeleniumSearchParameters" + DateTime.Now.ToString("MMddyy.hhmmss"));
		}

		public override void RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var browserObject = v_InstanceName.GetAppInstance(engine);
			var chromeProcess = (Process)browserObject;
			var vTargetText = v_SelectionValue.ConvertUserVariableToString(engine);

			DataRow elementRow = v_SeleniumSearchParameters.Rows[0];
			WebElement webElement = NativeHelper.DataTableToWebElement(v_SeleniumSearchParameters);

			webElement.Value = vTargetText;
			webElement.SelectionRules = v_Option;
			User32Functions.BringWindowToFront(chromeProcess.Handle);

			string elementText;
			NativeRequest.ProcessRequest("setoption", JsonConvert.SerializeObject(webElement), out elementText);
			if (elementText == "Failed")
			{
				//Failed
			}
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

			if (v_SeleniumSearchParameters.Rows.Count == 0)
			{
				v_SeleniumSearchParameters.Rows.Add(false, "XPath", "");
				v_SeleniumSearchParameters.Rows.Add(false, "Relative XPath", "");
				v_SeleniumSearchParameters.Rows.Add(false, "ID", "");
				v_SeleniumSearchParameters.Rows.Add(false, "Name", "");
				v_SeleniumSearchParameters.Rows.Add(false, "Tag Name", "");
				v_SeleniumSearchParameters.Rows.Add(false, "Class Name", "");
				v_SeleniumSearchParameters.Rows.Add(false, "Link Text", "");
				v_SeleniumSearchParameters.Rows.Add(true, "CSS Selector", "");
			}
			//create search parameters   
			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_SeleniumSearchParameters", this));
			RenderedControls.Add(helperControl);

			//create search param grid
			_searchParametersGridViewHelper = commandControls.CreateDefaultDataGridViewFor("v_SeleniumSearchParameters", this);
			_searchParametersGridViewHelper.MouseEnter += ActionParametersGridViewHelper_MouseEnter;

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

			RenderedControls.AddRange(commandControls.CreateUIHelpersFor("v_SeleniumSearchParameters", this, new Control[] { _searchParametersGridViewHelper }, editor));
			RenderedControls.Add(_searchParametersGridViewHelper);

			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_Option", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SelectionValue", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Instance Name '{v_InstanceName}']";
		}
		private void ActionParametersGridViewHelper_MouseEnter(object sender, EventArgs e)
		{
			//SeleniumAction_SelectionChangeCommitted(null, null);
		}
		public void ShowRecorder(object sender, EventArgs e, IfrmCommandEditor editor, ICommandControls commandControls)
		{
			User32Functions.BringChromeWindowToTop();

			string webElementStr;
			NativeRequest.ProcessRequest("getelement", "", out webElementStr);
			WebElement webElement = JsonConvert.DeserializeObject<WebElement>(webElementStr);
			Process process = Process.GetCurrentProcess();
			User32Functions.ActivateWindow(process.MainWindowTitle);
			DataTable SearchParameters = NativeHelper.WebElementToDataTable(webElement);

			try
			{
				if (SearchParameters != null)
				{
					v_SeleniumSearchParameters.Rows.Clear();

					foreach (DataRow rw in SearchParameters.Rows)
						v_SeleniumSearchParameters.ImportRow(rw);

					_searchParametersGridViewHelper.DataSource = v_SeleniumSearchParameters;
					_searchParametersGridViewHelper.Refresh();
				}
			}
			catch (Exception)
			{
				//Search parameter not found
			}
		}
	}
}
