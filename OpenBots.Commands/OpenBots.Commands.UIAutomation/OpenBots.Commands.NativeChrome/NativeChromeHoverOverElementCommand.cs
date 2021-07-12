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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenBots.Commands.NativeChrome
{
	[Serializable]
	[Category("Native Chrome Commands")]
	[Description("This command hovers over an element in Chrome.")]
	public class NativeChromeHoverOverElementCommand : ScriptCommand
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
		[DisplayName("Hover Type")]
		[PropertyUISelectionOption("Simulate Hover")]
		[PropertyUISelectionOption("Cursor Hover")]
		[Description("Please specify how an element should be hovered.")]
		[SampleUsage("")]
		[Remarks("If selected, *Simulate Hover* will simulate a background hover using javascript. Otherwise it will be a regular mouse move.")]
		public string v_HoverType { get; set; }

		[Required]
		[DisplayName("X Adjustment")]
		[Description("Enter the value for adjusting the horizontal coordinate of the mouse.")]
		[SampleUsage("0 || -50 || 50 || vXAdjustment")]
		[Remarks("This number will be added to the horizontal axis pixel location of the provided element to adjust hover position.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(int) })]
		public string v_XAdjustment { get; set; }

		[Required]
		[DisplayName("Y Adjustment")]
		[Description("Enter the value for adjusting the vertical coordinate of the mouse.")]
		[SampleUsage("0 || -50 || 50 || vYAdjustment")]
		[Remarks("This number will be added to the vertical axis pixel location of the provided element to adjust hover position.")]
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
		private List<Control> _cursorAdjustmentControls;

		[JsonIgnore]
		[Browsable(false)]
		private bool _hasRendered;

		public NativeChromeHoverOverElementCommand()
		{
			CommandName = "NativeChromeHoverOverElementCommand";
			SelectionName = "Native Chrome Hover Over Element";
			CommandEnabled = true;
			CommandIcon = Resources.command_nativechrome;

			v_NativeSearchParameters = NativeHelper.CreateSearchParametersDT();
			v_HoverType = "Cursor Hover";
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

			User32Functions.BringWindowToFront(chromeProcess.MainWindowHandle);

			string responseText;
			if(v_HoverType == "Simulate Hover")
            {
				NativeRequest.ProcessRequest("hoveroverelement", JsonConvert.SerializeObject(webElement), vTimeout, out responseText);
				NativeResponse responseObject = JsonConvert.DeserializeObject<NativeResponse>(responseText);

				if (responseObject.Status == "Failed")
					throw new Exception(responseObject.Result);
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
			}
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			NativeHelper.AddDefaultSearchRows(v_NativeSearchParameters);
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultWebElementDataGridViewGroupFor("v_NativeSearchParameters", this, editor,
				new Control[] { NativeHelper.NativeChromeRecorderControl(v_NativeSearchParameters, editor) }));
			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_HoverType", this, editor));
			((ComboBox)RenderedControls[8]).SelectedIndexChanged += HoverTypeComboBox_SelectionChangeCommitted;

			_cursorAdjustmentControls = new List<Control>();
			_cursorAdjustmentControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_XAdjustment", this, editor));
			_cursorAdjustmentControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_YAdjustment", this, editor));

			RenderedControls.AddRange(_cursorAdjustmentControls);
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Timeout", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [{NativeHelper.GetSearchNameValue(v_NativeSearchParameters)} - Instance Name '{v_InstanceName}']";
		}

		public override void Shown()
		{
			base.Shown();
			_hasRendered = true;
			HoverTypeComboBox_SelectionChangeCommitted(null, null);
		}

		public void HoverTypeComboBox_SelectionChangeCommitted(object sender, EventArgs e)
		{
			if (((ComboBox)RenderedControls[8]).Text == "Cursor Hover" && _hasRendered)
			{
				foreach (var ctrl in _cursorAdjustmentControls)
					ctrl.Visible = true;
			}
			else if (_hasRendered)
			{
				foreach (var ctrl in _cursorAdjustmentControls)
				{
					ctrl.Visible = false;
					if (ctrl is TextBox)
						((TextBox)ctrl).Clear();
				}
			}
		}
	}
}
