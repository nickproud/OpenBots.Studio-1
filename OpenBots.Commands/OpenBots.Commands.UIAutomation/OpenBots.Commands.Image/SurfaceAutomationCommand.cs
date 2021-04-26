using Newtonsoft.Json;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Properties;
using OpenBots.Core.User32;
using OpenBots.Core.Utilities.CommandUtilities;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Core.Utilities.FormsUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsInput;

namespace OpenBots.Commands.Image
{
    [Serializable]
	[Category("Image Commands")]
	[Description("This command attempts to find and perform an action on an existing image on screen.")]
	public class SurfaceAutomationCommand : ScriptCommand, IImageCommands
	{
		[Required]
		[DisplayName("Window Name")]
		[Description("Select the name of the window to activate and bring forward.")]
		[SampleUsage("\"Untitled - Notepad\" || vWindow")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[Editor("CaptureWindowHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_WindowName { get; set; }

		[Required]
		[DisplayName("Capture Search Image")]
		[Description("Use the tool to capture an image that will be located on screen during execution.")]
		[SampleUsage("")]
		[Remarks("Images with larger color variance will be found more quickly than those with a lot of white space. \n" +
				 "For images that are primarily white space, tagging color to the top-left corner of the image and setting \n" +
				 "the relative click position will produce faster results.")]
		[Editor("ShowImageCaptureHelper", typeof(UIAdditionalHelperType))]
		[Editor("ShowImageRecognitionTestHelper", typeof(UIAdditionalHelperType))]
		public string v_ImageCapture { get; set; }

		[Required]
		[DisplayName("Element Action")]
		[PropertyUISelectionOption("Click Image")]
		[PropertyUISelectionOption("Set Text")]
		[PropertyUISelectionOption("Set Secure Text")]
		[PropertyUISelectionOption("Image Exists")]
		[PropertyUISelectionOption("Wait For Image To Exist")]
		[Description("Select the appropriate corresponding action to take once the image has been located.")]
		[SampleUsage("")]
		[Remarks("Selecting this field changes the parameters required in the following step.")]
		public string v_ImageAction { get; set; }

		[Required]
		[DisplayName("Action Parameters")]
		[Description("Action Parameters will be required based on the action settings selected.")]
		[SampleUsage("\"data\" || vData")]
		[Remarks("Action Parameters range from adding offset coordinates to specifying a variable to apply element text to.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string), typeof(int), typeof(SecureString), typeof(bool) })]
		public DataTable v_ImageActionParameterTable { get; set; }

		[Required]
		[DisplayName("Accuracy (0-1)")]
		[Description("Enter a value between 0 and 1 to set the match Accuracy. Set to 1 for a perfect match.")]
		[SampleUsage("0.8 || 1 || vAccuracy")]
		[Remarks("Accuracy must be a value between 0 and 1.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(double) })]
		public string v_MatchAccuracy { get; set; }

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
		public bool TestMode { get; set; } = false;

		[JsonIgnore]
		[Browsable(false)]
		private DataGridView _imageGridViewHelper;

		[JsonIgnore]
		[Browsable(false)]
		private ComboBox _imageActionDropdown;

		[JsonIgnore]
		[Browsable(false)]
		private List<Control> _imageParameterControls;

		public SurfaceAutomationCommand()
		{
			CommandName = "SurfaceAutomationCommand";
			SelectionName = "Surface Automation";
			CommandEnabled = true;
			CommandIcon = Resources.command_camera;

			v_WindowName = "\"None\"";
			v_MatchAccuracy = "0.8";
			v_Timeout = "30";

			v_ImageActionParameterTable = new DataTable
			{
				TableName = "ImageActionParamTable" + DateTime.Now.ToString("MMddyy.hhmmss")
			};
			v_ImageActionParameterTable.Columns.Add("Parameter Name");
			v_ImageActionParameterTable.Columns.Add("Parameter Value");			
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			string windowName = (string)await v_WindowName.EvaluateCode(engine);
			int timeout = (int)await v_Timeout.EvaluateCode(engine);
			var timeToEnd = DateTime.Now.AddSeconds(timeout);

			bool testMode = TestMode;
			//user image to bitmap
			Bitmap userImage = new Bitmap(CommonMethods.Base64ToImage(v_ImageCapture));
			double accuracy;
			try
			{
				accuracy = Convert.ToDouble(await v_MatchAccuracy.EvaluateCode(engine));
				if (accuracy > 1 || accuracy < 0)
					throw new ArgumentOutOfRangeException("Accuracy value is out of range (0-1)");
			}
			catch (Exception)
			{
				throw new InvalidDataException("Accuracy value is invalid");
			}

			//Activate window if specified
			if (windowName != "None")
            {
				while (timeToEnd >= DateTime.Now)
				{
					try
					{
						if (engine.IsCancellationPending)
							break;

						User32Functions.ActivateWindow(windowName);

						if (!User32Functions.GetActiveWindowTitle().Equals(windowName))
							throw new Exception($"Window '{windowName}' Not Yet Found... ");

						break;
					}
					catch (Exception)
					{
						engine.ReportProgress($"Window '{windowName}' Not Yet Found... {(timeToEnd - DateTime.Now).Minutes}m, {(timeToEnd - DateTime.Now).Seconds}s remain");
						Thread.Sleep(500);
					}
				}

				if (!User32Functions.GetActiveWindowTitle().Equals(windowName))
					throw new Exception($"Window '{windowName}' Not Found");
				else
					Thread.Sleep(500);
			}

			dynamic element = null;

			while (timeToEnd >= DateTime.Now)
			{
				try
				{
					if (engine.IsCancellationPending)
						break;

					element = CommandsHelper.FindImageElement(userImage, accuracy, engine, timeToEnd);

					if (element == null)
						throw new Exception("Specified image was not found in window!");
					else
						break;
				}
				catch (Exception)
				{
					engine.ReportProgress("Element Not Yet Found... " + (timeToEnd - DateTime.Now).Seconds + "s remain");
					Thread.Sleep(1000);
				}
			}

			if (element == null)
            {
				FormsHelper.ShowAllForms(engine.AutomationEngineContext.IsDebugMode);
				throw new Exception("Specified image was not found in window!");
			}

			PerformImageElementAction(engine, element);
		}	

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultWindowControlGroupFor("v_WindowName", this, editor));

			var imageCapture = commandControls.CreateDefaultPictureBoxFor("v_ImageCapture", this);
			imageCapture.MouseEnter += ImageGridViewHelper_MouseEnter;
			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_ImageCapture", this));
			RenderedControls.AddRange(commandControls.CreateUIHelpersFor("v_ImageCapture", this, new Control[] { imageCapture }, editor));
			RenderedControls.Add(imageCapture);

			_imageActionDropdown = commandControls.CreateDropdownFor("v_ImageAction", this);
			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_ImageAction", this));
			RenderedControls.AddRange(commandControls.CreateUIHelpersFor("v_ImageAction", this, new Control[] { _imageActionDropdown }, editor));
			_imageActionDropdown.SelectionChangeCommitted += ImageAction_SelectionChangeCommitted;
			RenderedControls.Add(_imageActionDropdown);

			_imageParameterControls = new List<Control>();
			_imageParameterControls.Add(commandControls.CreateDefaultLabelFor("v_ImageActionParameterTable", this));

			_imageGridViewHelper = commandControls.CreateDefaultDataGridViewFor("v_ImageActionParameterTable", this);
			_imageGridViewHelper.AllowUserToAddRows = false;
			_imageGridViewHelper.AllowUserToDeleteRows = false;
			//_imageGridViewHelper.AllowUserToResizeRows = false;
			_imageGridViewHelper.MouseEnter += ImageGridViewHelper_MouseEnter;

			_imageParameterControls.AddRange(commandControls.CreateUIHelpersFor("v_ImageActionParameterTable", this, new Control[] { _imageGridViewHelper }, editor));
			_imageParameterControls.Add(_imageGridViewHelper);
			RenderedControls.AddRange(_imageParameterControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_MatchAccuracy", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Timeout", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [{v_ImageAction} on Screen - Accuracy '{v_MatchAccuracy}']";
		}

		public async void PerformImageElementAction(IAutomationEngineInstance engine, ImageElement element)
		{
			try
			{
				string clickPosition;
				string xAdjustString;
				string yAdjustString;
				int xAdjust;
				int yAdjust;
				switch (v_ImageAction)
				{
					case "Click Image":
						string clickType = (from rw in v_ImageActionParameterTable.AsEnumerable()
											where rw.Field<string>("Parameter Name") == "Click Type"
											select rw.Field<string>("Parameter Value")).FirstOrDefault();
						clickPosition = (from rw in v_ImageActionParameterTable.AsEnumerable()
										 where rw.Field<string>("Parameter Name") == "Click Position"
										 select rw.Field<string>("Parameter Value")).FirstOrDefault();
						xAdjustString = (from rw in v_ImageActionParameterTable.AsEnumerable()
												   where rw.Field<string>("Parameter Name") == "X Adjustment"
												   select rw.Field<string>("Parameter Value")).FirstOrDefault();
						xAdjust = (int)await xAdjustString.EvaluateCode(engine);
						yAdjustString = (from rw in v_ImageActionParameterTable.AsEnumerable()
												   where rw.Field<string>("Parameter Name") == "Y Adjustment"
												   select rw.Field<string>("Parameter Value")).FirstOrDefault();
						yAdjust = (int)await yAdjustString.EvaluateCode(engine);

						Point clickPositionPoint = GetClickPosition(clickPosition, element);

						//move mouse to position
						var mouseX = clickPositionPoint.X + xAdjust;
						var mouseY = clickPositionPoint.Y + yAdjust;
						User32Functions.SendMouseMove(mouseX, mouseY, clickType);
						break;

					case "Set Text":
						string textToSet = (from rw in v_ImageActionParameterTable.AsEnumerable()
											where rw.Field<string>("Parameter Name") == "Text To Set"
											select rw.Field<string>("Parameter Value")).FirstOrDefault();
						textToSet = (string)await textToSet.EvaluateCode(engine);
						clickPosition = (from rw in v_ImageActionParameterTable.AsEnumerable()
										 where rw.Field<string>("Parameter Name") == "Click Position"
										 select rw.Field<string>("Parameter Value")).FirstOrDefault();
						xAdjustString = (from rw in v_ImageActionParameterTable.AsEnumerable()
												   where rw.Field<string>("Parameter Name") == "X Adjustment"
												   select rw.Field<string>("Parameter Value")).FirstOrDefault();
						xAdjust = (int)await xAdjustString.EvaluateCode(engine);
						yAdjustString = (from rw in v_ImageActionParameterTable.AsEnumerable()
												   where rw.Field<string>("Parameter Name") == "Y Adjustment"
												   select rw.Field<string>("Parameter Value")).FirstOrDefault();
						yAdjust = (int)await yAdjustString.EvaluateCode(engine);
						string encryptedData = (from rw in v_ImageActionParameterTable.AsEnumerable()
												where rw.Field<string>("Parameter Name") == "Encrypted Text"
												select rw.Field<string>("Parameter Value")).FirstOrDefault();

						if (encryptedData == "Encrypted")
							textToSet = EncryptionServices.DecryptString(textToSet, "OPENBOTS");

						Point setTextPositionPoint = GetClickPosition(clickPosition, element);

						//move mouse to position and set text
						var xPos = setTextPositionPoint.X + xAdjust;
						var yPos = setTextPositionPoint.Y + yAdjust;
						User32Functions.SendMouseMove(xPos, yPos, "Left Click");

						var simulator = new InputSimulator();
						simulator.Keyboard.TextEntry(textToSet);
						Thread.Sleep(100);
						break;

					case "Set Secure Text":
						var secureString = (from rw in v_ImageActionParameterTable.AsEnumerable()
											where rw.Field<string>("Parameter Name") == "Secure String Variable"
											select rw.Field<string>("Parameter Value")).FirstOrDefault();
						clickPosition = (from rw in v_ImageActionParameterTable.AsEnumerable()
										 where rw.Field<string>("Parameter Name") == "Click Position"
										 select rw.Field<string>("Parameter Value")).FirstOrDefault();
						xAdjustString = (from rw in v_ImageActionParameterTable.AsEnumerable()
												   where rw.Field<string>("Parameter Name") == "X Adjustment"
												   select rw.Field<string>("Parameter Value")).FirstOrDefault();
						xAdjust = (int)await xAdjustString.EvaluateCode(engine);
						yAdjustString = (from rw in v_ImageActionParameterTable.AsEnumerable()
												   where rw.Field<string>("Parameter Name") == "Y Adjustment"
												   select rw.Field<string>("Parameter Value")).FirstOrDefault();
						yAdjust = (int)await yAdjustString.EvaluateCode(engine);

						var secureStrVariable = (SecureString)await secureString.EvaluateCode(engine);

						secureString = secureStrVariable.ConvertSecureStringToString();

						Point setSecureTextPositionPoint = GetClickPosition(clickPosition, element);

						//move mouse to position and set text
						var xPosition = setSecureTextPositionPoint.X + xAdjust;
						var yPosition = setSecureTextPositionPoint.Y + yAdjust;
						User32Functions.SendMouseMove(xPosition, yPosition, "Left Click");

						var simulator2 = new InputSimulator();
						simulator2.Keyboard.TextEntry(secureString);
						Thread.Sleep(100);
						break;

					case "Image Exists":
						var outputVariable = (from rw in v_ImageActionParameterTable.AsEnumerable()
											  where rw.Field<string>("Parameter Name") == "Output Bool Variable Name"
											  select rw.Field<string>("Parameter Value")).FirstOrDefault();

						if (element != null)
							true.SetVariableValue(engine, outputVariable);
						else
							false.SetVariableValue(engine, outputVariable);
						break;
					default:
						break;
				}
				FormsHelper.ShowAllForms(engine.AutomationEngineContext.IsDebugMode);
			}
			catch (Exception ex)
			{
				FormsHelper.ShowAllForms(engine.AutomationEngineContext.IsDebugMode);
				throw ex;
			}
		}

		private void ImageAction_SelectionChangeCommitted(object sender, EventArgs e)
		{
			SurfaceAutomationCommand cmd = this;
			DataTable actionParameters = cmd.v_ImageActionParameterTable;

			if (sender != null)
				actionParameters.Rows.Clear();

			DataGridViewComboBoxCell mouseClickPositionBox = new DataGridViewComboBoxCell();
			mouseClickPositionBox.Items.Add("Center");
			mouseClickPositionBox.Items.Add("Top Left");
			mouseClickPositionBox.Items.Add("Top Middle");
			mouseClickPositionBox.Items.Add("Top Right");
			mouseClickPositionBox.Items.Add("Bottom Left");
			mouseClickPositionBox.Items.Add("Bottom Middle");
			mouseClickPositionBox.Items.Add("Bottom Right");
			mouseClickPositionBox.Items.Add("Middle Left");
			mouseClickPositionBox.Items.Add("Middle Right");

			switch (_imageActionDropdown.SelectedItem)
			{
				case "Click Image":
					foreach (var ctrl in _imageParameterControls)
						ctrl.Show();

					DataGridViewComboBoxCell mouseClickTypeBox = new DataGridViewComboBoxCell();
					mouseClickTypeBox.Items.Add("Left Click");
					mouseClickTypeBox.Items.Add("Middle Click");
					mouseClickTypeBox.Items.Add("Right Click");
					mouseClickTypeBox.Items.Add("Left Down");
					mouseClickTypeBox.Items.Add("Middle Down");
					mouseClickTypeBox.Items.Add("Right Down");
					mouseClickTypeBox.Items.Add("Left Up");
					mouseClickTypeBox.Items.Add("Middle Up");
					mouseClickTypeBox.Items.Add("Right Up");
					mouseClickTypeBox.Items.Add("Double Left Click");

					if (sender != null)
					{
						actionParameters.Rows.Add("Click Type", "Left Click");
						actionParameters.Rows.Add("Click Position", "Center");
						actionParameters.Rows.Add("X Adjustment", 0);
						actionParameters.Rows.Add("Y Adjustment", 0);
					}

					_imageGridViewHelper.Rows[0].Cells[1] = mouseClickTypeBox;
					_imageGridViewHelper.Rows[1].Cells[1] = mouseClickPositionBox;

					break;

				case "Set Text":
					foreach (var ctrl in _imageParameterControls)
						ctrl.Show();

					DataGridViewComboBoxCell encryptedBox = new DataGridViewComboBoxCell();
					encryptedBox.Items.Add("Not Encrypted");
					encryptedBox.Items.Add("Encrypted");

					if (sender != null)
					{
						actionParameters.Rows.Add("Text To Set");
						actionParameters.Rows.Add("Click Position", "Center");
						actionParameters.Rows.Add("X Adjustment", 0);
						actionParameters.Rows.Add("Y Adjustment", 0);
						actionParameters.Rows.Add("Encrypted Text", "Not Encrypted");
						actionParameters.Rows.Add("Optional - Click to Encrypt 'Text To Set'");

						var buttonCell = new DataGridViewButtonCell();
						_imageGridViewHelper.Rows[5].Cells[1] = buttonCell;
						_imageGridViewHelper.Rows[5].Cells[1].Value = "Encrypt Text";
						_imageGridViewHelper.CellContentClick += ImageGridViewHelper_CellContentClick;
					}

					_imageGridViewHelper.Rows[1].Cells[1] = mouseClickPositionBox;
					_imageGridViewHelper.Rows[4].Cells[1] = encryptedBox;

					break;

				case "Set Secure Text":
					foreach (var ctrl in _imageParameterControls)
						ctrl.Show();

					if (sender != null)
					{
						actionParameters.Rows.Add("Secure String Variable");
						actionParameters.Rows.Add("Click Position", "Center");
						actionParameters.Rows.Add("X Adjustment", 0);
						actionParameters.Rows.Add("Y Adjustment", 0);
					}

					_imageGridViewHelper.Rows[1].Cells[1] = mouseClickPositionBox;

					break;

				case "Image Exists":
					foreach (var ctrl in _imageParameterControls)
						ctrl.Show();

					if (sender != null)
						actionParameters.Rows.Add("Output Bool Variable Name", "");
					break;

				case "Wait For Image To Exist":
					foreach (var ctrl in _imageParameterControls)
						ctrl.Hide();
					break;

				default:
					break;
			}
			_imageGridViewHelper.Columns[0].ReadOnly = true;
		}

		private void ImageGridViewHelper_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			var targetCell = _imageGridViewHelper.Rows[e.RowIndex].Cells[e.ColumnIndex];

			if (targetCell is DataGridViewButtonCell && targetCell.Value.ToString() == "Encrypt Text")
			{
				var targetElement = _imageGridViewHelper.Rows[0].Cells[1];

				if (targetElement.Value == null)
					return;

				var warning = MessageBox.Show("Warning! Text should only be encrypted one time and is not reversible in the builder. " +
											   $"Would you like to proceed and convert '{targetElement.Value}' to an encrypted value?",
											   "Encryption Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

				if (warning == DialogResult.Yes)
				{
					targetElement.Value = $"\"{EncryptionServices.EncryptString(targetElement.Value.ToString().TrimStart('\"').TrimEnd('\"'), "OPENBOTS")}\"";
					_imageGridViewHelper.Rows[4].Cells[1].Value = "Encrypted";
				}
			}
		}

		private Point GetClickPosition(string clickPosition, ImageElement element)
		{
			int clickPositionX = 0;
			int clickPositionY = 0;
			switch (clickPosition)
			{
				case "Center":
					clickPositionX = element.MiddleX;
					clickPositionY = element.MiddleY;
					break;
				case "Top Left":
					clickPositionX = element.LeftX;
					clickPositionY = element.TopY;
					break;
				case "Top Middle":
					clickPositionX = element.MiddleX;
					clickPositionY = element.TopY;
					break;
				case "Top Right":
					clickPositionX = element.RightX;
					clickPositionY = element.TopY;
					break;
				case "Bottom Left":
					clickPositionX = element.LeftX;
					clickPositionY = element.BottomY;
					break;
				case "Bottom Middle":
					clickPositionX = element.MiddleX;
					clickPositionY = element.BottomY;
					break;
				case "Bottom Right":
					clickPositionX = element.RightX;
					clickPositionY = element.BottomY;
					break;
				case "Middle Left":
					clickPositionX = element.LeftX;
					clickPositionY = element.MiddleX;
					break;
				case "Middle Right":
					clickPositionX = element.RightX;
					clickPositionY = element.MiddleY;
					break;
			}
			return new Point(clickPositionX, clickPositionY);
		}
		private void ImageGridViewHelper_MouseEnter(object sender, EventArgs e)
		{
			ImageAction_SelectionChangeCommitted(null, null);
		}
	}
}