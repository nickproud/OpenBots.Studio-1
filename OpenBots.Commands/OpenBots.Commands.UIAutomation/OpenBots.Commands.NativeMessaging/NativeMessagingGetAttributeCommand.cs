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
	[Description("This command gets attribute from specified element in chrome.")]
	public class NativeMessagingGetAttributeCommand : ScriptCommand
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
		public DataTable v_NativeSearchParameters { get; set; }

		[Required]
		[DisplayName("Attribute Name")]
		[Description("Enter the attribute name to be set.")]
		[SampleUsage("Hello World || {vAttribute}")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(null, true)]
		public string v_Attribute { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output Text Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("{vUserVariable}")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_OutputUserVariableName { get; set; }

		[JsonIgnore]
		[Browsable(false)]
		private DataGridView _searchParametersGridViewHelper;

		public NativeMessagingGetAttributeCommand()
		{
			CommandName = "NativeMessagingGetAttributeCommand";
			SelectionName = "Get Attribute";
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

		public override void RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var browserObject = v_InstanceName.GetAppInstance(engine);
			var chromeProcess = (Process)browserObject;
			var vTargetText = v_Attribute.ConvertUserVariableToString(engine);

			DataRow elementRow = v_NativeSearchParameters.Rows[0];
			WebElement webElement = NativeHelper.DataTableToWebElement(v_NativeSearchParameters);

			User32Functions.BringWindowToFront(chromeProcess.Handle);

			string elementText;
			NativeRequest.ProcessRequest("getattribute", JsonConvert.SerializeObject(webElement), out elementText);
			elementText.StoreInUserVariable(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
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
				v_NativeSearchParameters.Rows.Add(false, "XPath", "");
				v_NativeSearchParameters.Rows.Add(false, "Relative XPath", "");
				v_NativeSearchParameters.Rows.Add(false, "ID", "");
				v_NativeSearchParameters.Rows.Add(false, "Name", "");
				v_NativeSearchParameters.Rows.Add(false, "Tag Name", "");
				v_NativeSearchParameters.Rows.Add(false, "Class Name", "");
				v_NativeSearchParameters.Rows.Add(false, "Link Text", "");
				v_NativeSearchParameters.Rows.Add(true, "CSS Selector", "");
			}
			//create search parameters   
			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_NativeSearchParameters", this));
			RenderedControls.Add(helperControl);

			//create search param grid
			_searchParametersGridViewHelper = commandControls.CreateDefaultDataGridViewFor("v_NativeSearchParameters", this);
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

			RenderedControls.AddRange(commandControls.CreateUIHelpersFor("v_NativeSearchParameters", this, new Control[] { _searchParametersGridViewHelper }, editor));
			RenderedControls.Add(_searchParametersGridViewHelper);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Attribute", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

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
					v_NativeSearchParameters.Rows.Clear();

					foreach (DataRow rw in SearchParameters.Rows)
						v_NativeSearchParameters.ImportRow(rw);

					_searchParametersGridViewHelper.DataSource = v_NativeSearchParameters;
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
