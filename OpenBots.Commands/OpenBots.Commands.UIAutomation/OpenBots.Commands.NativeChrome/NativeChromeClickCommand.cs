using Newtonsoft.Json;
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenBots.Commands.NativeChrome
{
    [Serializable]
	[Category("Native Chrome Commands")]
	[Description("This command performs a click on a specified element in Chrome.")]
	public class NativeChromeClickCommand : ScriptCommand
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
		[DisplayName("Click Button")]
		[PropertyUISelectionOption("Left")]
		[PropertyUISelectionOption("Middle")]
		[PropertyUISelectionOption("Right")]
		[Description("Please specify the mouse button used for the click action.")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_ClickButton { get; set; }

		[Required]
		[DisplayName("Click Type")]
		[PropertyUISelectionOption("Simulate Click")]
		[PropertyUISelectionOption("Mouse Click")]
		[Description("Please specify how an element should be clicked.")]
		[SampleUsage("")]
		[Remarks("If selected, *Simulate Click* will simulate a background click using javascript. Otherwise it will be a regular mouse click.")]
		public string v_ClickType { get; set; }

		[Required]
		[DisplayName("Open Link Option")]
		[PropertyUISelectionOption("Same Tab")]
		[PropertyUISelectionOption("New Tab")]
		[PropertyUISelectionOption("New Window")]
		[Description("Please specify where to open a link on a webpage.")]
		[SampleUsage("")]
		[Remarks("This will only affect elements that contain a link.")]
		public string v_OpenLinkOptions { get; set; }

		[Required]
		[DisplayName("New Chrome Browser Instance Name")]
		[Description("Enter a unique name that will represent the application instance.")]
		[SampleUsage("MyChromeBrowserInstance")]
		[Remarks("This unique name allows you to refer to the instance by name in future commands, " +
				 "ensuring that the commands you specify run against the correct application.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(OBAppInstance) })]
		public string v_NewInstanceName { get; set; }

		[Required]
		[DisplayName("X Adjustment")]
		[Description("Enter the value for adjusting the horizontal coordinate of the mouse.")]
		[SampleUsage("0 || -50 || 50 || vXAdjustment")]
		[Remarks("This number will be added to the horizontal axis pixel location of the provided element to adjust click position.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(int) })]
		public string v_XAdjustment { get; set; }

		[Required]
		[DisplayName("Y Adjustment")]
		[Description("Enter the value for adjusting the vertical coordinate of the mouse.")]
		[SampleUsage("0 || -50 || 50 || vYAdjustment")]
		[Remarks("This number will be added to the vertical axis pixel location of the provided element to adjust click position.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(int) })]
		public string v_YAdjustment { get; set; }

		[Required]
		[DisplayName("Timeout (Seconds)")]
		[Description("Specify how many seconds to wait before throwing an exception.")]
		[SampleUsage("30 || vSeconds")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(int) })]
		public string v_Timeout { get; set; }

		[JsonIgnore]
		[Browsable(false)]
		private List<Control> _clickTypeControls;

		[JsonIgnore]
		[Browsable(false)]
		private List<Control> _cursorAdjustmentControls;

		[JsonIgnore]
		[Browsable(false)]
		private List<Control> _openLinkOptions;

		[JsonIgnore]
		[Browsable(false)]
		private List<Control> _newInstanceName;

		[JsonIgnore]
		[Browsable(false)]
		private bool _hasRendered;

		public NativeChromeClickCommand()
		{
			CommandName = "NativeChromeClickCommand";
			SelectionName = "Native Chrome Click";
			CommandEnabled = true;
			CommandIcon = Resources.command_nativechrome;

			v_NativeSearchParameters = NativeHelper.CreateSearchParametersDT();
			v_ClickButton = "Left";
			v_OpenLinkOptions = "Same Tab";
			v_ClickType = "Mouse Click";
			v_XAdjustment = "0";
			v_YAdjustment = "0";
			v_Timeout = "30";
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var browserObject = ((OBAppInstance)await v_InstanceName.EvaluateCode(engine)).Value;
			var chromeProcess = (Process)browserObject;
			var vTimeout = (int)await v_Timeout.EvaluateCode(engine);
			var vXAdjustment = (int)await v_XAdjustment.EvaluateCode(engine);
			var vYAdjustment = (int)await v_YAdjustment.EvaluateCode(engine);

			User32Functions.GetWindowRect(chromeProcess.MainWindowHandle, out Rect chromeRect);

			WebElement webElement = await NativeHelper.DataTableToWebElement(v_NativeSearchParameters, engine);
			webElement.Value = v_OpenLinkOptions;

			User32Functions.BringWindowToFront(chromeProcess.MainWindowHandle);

			string responseText;
			if ((v_ClickButton == "Left" || v_ClickButton == "Middle") && v_ClickType == "Simulate Click")
            {
				string clickButton = v_ClickButton.ToLower() + "click";
				NativeRequest.ProcessRequest(clickButton, JsonConvert.SerializeObject(webElement), vTimeout, out responseText);
				NativeResponse responseObject = JsonConvert.DeserializeObject<NativeResponse>(responseText);

				if (responseObject.Status == "Failed")
					throw new Exception(responseObject.Result);

				if (v_OpenLinkOptions == "New Window" && v_ClickButton == "Left")
				{
					//Create new browser
					int chromeProcessCount = Process.GetProcessesByName("chrome").Length;
					var process = Process.Start("chrome.exe", responseObject.Result + " --new-window");

					if (chromeProcessCount > 0)
					{
						while (process.HasExited == false)
						{
							Thread.Sleep(100);
							process.Refresh();
						}
					}
					//Delay 7 seconds
					Thread.Sleep(7000);

					Process[] procsChrome = Process.GetProcessesByName("chrome");

					foreach (Process chrome in procsChrome)
					{
						// the chrome process must have a window
						if (chrome.MainWindowHandle == IntPtr.Zero)
							continue;

						process = chrome;
						break;
					}

					new OBAppInstance(v_NewInstanceName, process).SetVariableValue(engine, v_NewInstanceName);
				}
			}
            else
            {
				NativeRequest.ProcessRequest("getcoordinates", JsonConvert.SerializeObject(webElement), vTimeout, out responseText);
				NativeResponse responseObject = JsonConvert.DeserializeObject<NativeResponse>(responseText);

				if (responseObject.Status == "Failed")
					throw new Exception(responseObject.Result);

				var screenPosition = responseObject.Result.Split(',');

				/* Following are the values returned from extension
				screenPosition[0] = element.left, screenPosition[1] = element.top, 
				screenPosition[2] = element.width, screenPosition[3] = element.height, 
				screenPosition[4] = chrome toolbar height */
				int xPosition = chromeRect.left + Convert.ToInt32(Convert.ToDouble(screenPosition[0])) + (Convert.ToInt32(Convert.ToDouble(screenPosition[2])) / 2) + vXAdjustment;
				int yPosition = chromeRect.top + Convert.ToInt32(Convert.ToDouble(screenPosition[1])) + Convert.ToInt32(Convert.ToDouble(screenPosition[4])) + (Convert.ToInt32(Convert.ToDouble(screenPosition[3])) / 2) + vYAdjustment;

				User32Functions.SetCursorPosition(xPosition, yPosition);

				if (v_ClickButton == "Left")
					User32Functions.SendMouseClick("Left Click", xPosition, yPosition);
				else if (v_ClickButton == "Right")
					User32Functions.SendMouseClick("Right Click", xPosition, yPosition);
                else
					User32Functions.SendMouseClick("Middle Click", xPosition, yPosition);
			}
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			NativeHelper.AddDefaultSearchRows(v_NativeSearchParameters);
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultWebElementDataGridViewGroupFor("v_NativeSearchParameters", this, editor, 
				new Control[] { NativeHelper.NativeChromeRecorderControl(v_NativeSearchParameters, editor) }));

			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_ClickButton", this, editor));
			((ComboBox)RenderedControls[8]).SelectedIndexChanged += ClickButtonComboBox_SelectionChangeCommitted;

			_clickTypeControls = new List<Control>();
			_clickTypeControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_ClickType", this, editor));

			RenderedControls.AddRange(_clickTypeControls);

			((ComboBox)RenderedControls[10]).SelectedIndexChanged += HoverTypeComboBox_SelectionChangeCommitted;

			_openLinkOptions = new List<Control>();
			_openLinkOptions.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_OpenLinkOptions", this, editor));

			RenderedControls.AddRange(_openLinkOptions);

			((ComboBox)RenderedControls[12]).SelectedIndexChanged += OpenLinkOptionsComboBox_SelectionChangeCommitted;

			_newInstanceName = new List<Control>();
			_newInstanceName.AddRange(commandControls.CreateDefaultInputGroupFor("v_NewInstanceName", this, editor));

			RenderedControls.AddRange(_newInstanceName);

			_cursorAdjustmentControls = new List<Control>();
			_cursorAdjustmentControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_XAdjustment", this, editor));
			_cursorAdjustmentControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_YAdjustment", this, editor));

			RenderedControls.AddRange(_cursorAdjustmentControls);
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Timeout", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [{v_ClickButton} Click {NativeHelper.GetSearchNameValue(v_NativeSearchParameters)} - Instance Name '{v_InstanceName}']";
		}

		public override void Shown()
		{
			base.Shown();
			_hasRendered = true;
			ClickButtonComboBox_SelectionChangeCommitted(null, null);
			OpenLinkOptionsComboBox_SelectionChangeCommitted(null, null);
			HoverTypeComboBox_SelectionChangeCommitted(null, null);
		}

		public void ClickButtonComboBox_SelectionChangeCommitted(object sender, EventArgs e)
		{
			if (((ComboBox)RenderedControls[8]).Text == "Left" && _hasRendered )
			{
				if(((ComboBox)RenderedControls[10]).Text != "Mouse Click")
				{
					foreach (var ctrl in _openLinkOptions)
						ctrl.Visible = true;
				}
                else
                {
					foreach (var ctrl in _clickTypeControls)
						ctrl.Visible = true;
				}

			}
			else if (((ComboBox)RenderedControls[8]).Text == "Right" && _hasRendered)
			{
				foreach (var ctrl in _openLinkOptions)
				{
					ctrl.Visible = false;
					if (ctrl is TextBox)
						((TextBox)ctrl).Clear();
				}
				foreach (var ctrl in _newInstanceName)
				{
					ctrl.Visible = false;
					if (ctrl is TextBox)
						((TextBox)ctrl).Clear();
				}
				foreach (var ctrl in _clickTypeControls)
				{
					ctrl.Visible = false;
				}
			}
			else if (_hasRendered)
			{
				foreach (var ctrl in _clickTypeControls)
					ctrl.Visible = true;
				foreach (var ctrl in _openLinkOptions)
				{
					ctrl.Visible = false;
					if (ctrl is TextBox)
						((TextBox)ctrl).Clear();
				}
				foreach (var ctrl in _newInstanceName)
				{
					ctrl.Visible = false;
					if (ctrl is TextBox)
						((TextBox)ctrl).Clear();
				}
			}
		}

		public void OpenLinkOptionsComboBox_SelectionChangeCommitted(object sender, EventArgs e)
		{
			if (((ComboBox)RenderedControls[12]).Text == "New Window" && _hasRendered && ((ComboBox)RenderedControls[8]).Text == "Left")
			{
				foreach (var ctrl in _newInstanceName)
					ctrl.Visible = true;
			}
			else if (_hasRendered)
			{
				foreach (var ctrl in _newInstanceName)
				{
					ctrl.Visible = false;
					if (ctrl is TextBox)
						((TextBox)ctrl).Clear();
				}
			}
		}

		public void HoverTypeComboBox_SelectionChangeCommitted(object sender, EventArgs e)
		{
			if (((ComboBox)RenderedControls[10]).Text == "Mouse Click" && _hasRendered)
			{
				foreach (var ctrl in _cursorAdjustmentControls)
					ctrl.Visible = true;

				if (((ComboBox)RenderedControls[8]).Text == "Left")
				{
					foreach (var ctrl in _openLinkOptions)
					{
						ctrl.Visible = false;
						if (ctrl is TextBox)
							((TextBox)ctrl).Clear();
					}
					foreach (var ctrl in _newInstanceName)
					{
						ctrl.Visible = false;
						if (ctrl is TextBox)
							((TextBox)ctrl).Clear();
					}
				}
			}
			else if (_hasRendered)
			{
				if (((ComboBox)RenderedControls[8]).Text == "Left")
				{
					foreach (var ctrl in _openLinkOptions)
						ctrl.Visible = true;
				}
				foreach (var ctrl in _cursorAdjustmentControls)
				{
					ctrl.Visible = false;
					if (ctrl is TextBox)
						((TextBox)ctrl).Text = "0";
				}
			}
		}
	}
}
