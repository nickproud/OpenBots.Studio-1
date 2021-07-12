using Newtonsoft.Json;
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
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenBots.Commands.NativeChrome
{
	[Serializable]
	[Category("Native Chrome Commands")]
	[Description("This command executes JavaScript code on the active tab of a selected Chrome instance.")]
	public class NativeChromeInjectJsScriptCommand : ScriptCommand
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
		[DisplayName("Script Source")]
		[PropertyUISelectionOption("File Path")]
		[PropertyUISelectionOption("Script Code")]
		[Description("Please specify the script source you want to use.")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_ScriptSource { get; set; }

		[Required]
		[DisplayName("File Path")]
		[Description("Enter the file path of javascript script file.")]
		[SampleUsage("\"C:\\inject.js\" || vFilePath")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_FilePath { get; set; }

		[Required]
		[DisplayName("Script Code")]
		[Description("Enter JavaScript code you want to run.")]
		[SampleUsage("")]
		[Remarks("Should not be enclosed in quotation marks since the JavaScript is not evaluated as C# code.")]
		public string v_InputJS { get; set; }

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
		private List<Control> _filePath;

		[JsonIgnore]
		[Browsable(false)]
		private List<Control> _inputJS;

		[JsonIgnore]
		[Browsable(false)]
		private bool _hasRendered;

		public NativeChromeInjectJsScriptCommand()
		{
			CommandName = "NativeChromeInjectJsScriptCommand";
			SelectionName = "Native Chrome Inject Js Script";
			CommandEnabled = true;
			CommandIcon = Resources.command_nativechrome;

			v_ScriptSource = "File Path";
			v_Timeout = "30";
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var browserObject = ((OBAppInstance)await v_InstanceName.EvaluateCode(engine)).Value;
			var chromeProcess = (Process)browserObject;
			var vScript = (string)await v_FilePath.EvaluateCode(engine);
			var vTimeout = (int)await v_Timeout.EvaluateCode(engine);

			if (v_ScriptSource == "File Path")
				vScript = File.ReadAllText(vScript);
			else
				vScript = v_InputJS;

			WebElement webElement = new WebElement();

			webElement.Value = vScript;
			User32Functions.BringWindowToFront(chromeProcess.MainWindowHandle);

			string responseText;
			NativeRequest.ProcessRequest("injectjsscript", JsonConvert.SerializeObject(webElement), vTimeout, out responseText);
			NativeResponse responseObject = JsonConvert.DeserializeObject<NativeResponse>(responseText);
			if (responseObject.Status == "Failed")
				throw new Exception(responseObject.Result);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));

			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_ScriptSource", this, editor));
			((ComboBox)RenderedControls[4]).SelectedIndexChanged += ScriptSourceComboBox_SelectionChangeCommitted;

			_filePath = new List<Control>();
			_filePath.AddRange(commandControls.CreateDefaultInputGroupFor("v_FilePath", this, editor));

			RenderedControls.AddRange(_filePath);

			_inputJS = new List<Control>();
			_inputJS.AddRange(commandControls.CreateDefaultInputGroupFor("v_InputJS", this, editor, 200, 300, false));

			RenderedControls.AddRange(_inputJS);
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Timeout", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Inject by {v_ScriptSource} - Instance Name '{v_InstanceName}']";
		}

		public override void Shown()
		{
			base.Shown();
			_hasRendered = true;
			ScriptSourceComboBox_SelectionChangeCommitted(null, null);
		}

		public void ScriptSourceComboBox_SelectionChangeCommitted(object sender, EventArgs e)
		{
			if (((ComboBox)RenderedControls[4]).Text == "File Path" && _hasRendered)
			{
				foreach (var ctrl in _filePath)
					ctrl.Visible = true;
				foreach (var ctrl in _inputJS)
				{
					ctrl.Visible = false;
					if (ctrl is TextBox)
						((TextBox)ctrl).Clear();
				}
			}
			else if (_hasRendered)
			{
				foreach (var ctrl in _inputJS)
					ctrl.Visible = true;
				foreach (var ctrl in _filePath)
				{
					ctrl.Visible = false;
					if (ctrl is TextBox)
						((TextBox)ctrl).Clear();
				}
			}
		}
	}
}
