using Newtonsoft.Json;
using OpenBots.Commands.UIAutomation.Forms;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Properties;
using OpenBots.Core.UI.Controls;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenBots.Commands.Input
{
    [Serializable]
	[Category("Input Commands")]
	[Description("This command provides the user with a form to input and store a collection of data.")]
	public class InputCommand : ScriptCommand
	{

		[Required]
		[DisplayName("Header Name")]
		[Description("Define the header to be displayed on the input form.")]
		[SampleUsage("\"Please Provide Input\" || vHeader")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_InputHeader { get; set; }

		[Required]
		[DisplayName("Input Directions")]
		[Description("Define the directions to give to the user.")]
		[SampleUsage("\"Directions: Please fill in the following fields\" || vDirections")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_InputDirections { get; set; }

		[DisplayName("Input Parameters (Optional)")]
		[Description("Define the required input parameters.")]
		[SampleUsage("[TextBox | Name | 500,100 | \"John\" | vName]\n" +
					 "[CheckBox | Developer | 500,30 | \"True\" | vDeveloper]\n" +
					 "[ComboBox | Gender | 500,30 | \"Male,Female,Other\" | vGender]")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string), typeof(bool) })]
		public DataTable v_UserInputConfig { get; set; }

		[JsonIgnore]
		[Browsable(false)]
		private DataGridView _userInputGridViewHelper;

		[JsonIgnore]
		[Browsable(false)]
		private CommandItemControl _addRowControl;

		[JsonIgnore]
		[Browsable(false)]
		private int _indexOfItemUnderMouseToDrag = -1;

		[JsonIgnore]
		[Browsable(false)]
		private int _indexOfItemUnderMouseToDrop = -1;

		[JsonIgnore]
		[Browsable(false)]
		private Rectangle _dragBoxFromMouseDown = Rectangle.Empty;

		public InputCommand()
		{
			CommandName = "InputCommand";
			SelectionName = "Prompt for Input";
			CommandEnabled = true;
			CommandIcon = Resources.command_input;

			v_UserInputConfig = new DataTable();
			v_UserInputConfig.TableName = DateTime.Now.ToString("UserInputParamTable" + DateTime.Now.ToString("MMddyy.hhmmss"));
			v_UserInputConfig.Columns.Add("Type");
			v_UserInputConfig.Columns.Add("Label");
			v_UserInputConfig.Columns.Add("Size");
			v_UserInputConfig.Columns.Add("DefaultValue");
			v_UserInputConfig.Columns.Add("StoreInVariable");

			v_InputHeader = "\"Please Provide Input\"";
			v_InputDirections = "\"Directions: Please fill in the following fields\"";
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var header = (string)await v_InputHeader.EvaluateCode(engine);
			var directions = (string)await v_InputDirections.EvaluateCode(engine);
			
			//translate variables for each label
			foreach (DataRow rw in v_UserInputConfig.Rows)
			{
				rw["DefaultValue"] = (await rw["DefaultValue"].ToString().EvaluateCode(engine)).ToString();
				string targetVariable = rw["StoreInVariable"].ToString();

				if (string.IsNullOrEmpty(targetVariable))
				{
					var message = "User Input question '" + rw["Label"] + "' is missing variables to apply results to! " +
								  "Results for the item will not be tracked. To fix this, assign a variable in the designer!";

					MessageBox.Show(message, "Input Command", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}

			//get input from user
			List<object> userInputs = new List<object>();

			var inputForm = new frmUserInput(header, directions, v_UserInputConfig);

			var dialogResult = inputForm.ShowDialog();

			if (dialogResult == DialogResult.OK)
			{
				foreach (var ctrl in inputForm.InputControls)
				{
					if (ctrl is CheckBox)
					{
						var checkboxCtrl = (CheckBox)ctrl;
						userInputs.Add(checkboxCtrl.Checked);
					}
					else
						userInputs.Add(ctrl.Text);
				}
				inputForm.Dispose();

			}
			else
			{
				inputForm.Dispose();
				userInputs = null;
			}

			//check if user provided input
			if (userInputs != null)
				{
					//loop through each input and assign
					for (int i = 0; i < userInputs.Count; i++)
					{                       
						//get target variable
						string targetVariable = v_UserInputConfig.Rows[i]["StoreInVariable"].ToString();

						//store user data in variable
						if (!string.IsNullOrEmpty(targetVariable))
							userInputs[i].SetVariableValue(engine, targetVariable);
					}
				}
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			_userInputGridViewHelper = commandControls.CreateDefaultDataGridViewFor("v_UserInputConfig", this);
			_userInputGridViewHelper.KeyDown += UserInputDataGridView_KeyDown;

			var typefield = new DataGridViewComboBoxColumn();
			typefield.Items.Add("TextBox");
			typefield.Items.Add("CheckBox");
			typefield.Items.Add("ComboBox");
			typefield.HeaderText = "Input Type";
			typefield.DataPropertyName = "Type";
			_userInputGridViewHelper.Columns.Add(typefield);

			var field = new DataGridViewTextBoxColumn();
			field.HeaderText = "Input Label";
			field.DataPropertyName = "Label";
			_userInputGridViewHelper.Columns.Add(field);

			field = new DataGridViewTextBoxColumn();
			field.HeaderText = "Input Size (X,Y)";
			field.DataPropertyName = "Size";
			_userInputGridViewHelper.Columns.Add(field);

			field = new DataGridViewTextBoxColumn();
			field.HeaderText = "Default Value";
			field.DataPropertyName = "DefaultValue";
			_userInputGridViewHelper.Columns.Add(field);

			field = new DataGridViewTextBoxColumn();
			field.HeaderText = "Assigned Variable";
			field.DataPropertyName = "StoreInVariable";
			_userInputGridViewHelper.Columns.Add(field);

			_userInputGridViewHelper.AllowUserToAddRows = false;
			_userInputGridViewHelper.AllowDrop = true;
			_userInputGridViewHelper.MouseDown += UserInputGridViewHelper_MouseDown;
			_userInputGridViewHelper.MouseUp += UserInputGridViewHelper_MouseUp;
			_userInputGridViewHelper.MouseMove += UserInputGridViewHelper_MouseMove;
			_userInputGridViewHelper.DragOver += UserInputGridViewHelper_DragOver;
			_userInputGridViewHelper.DragDrop += UserInputGridViewHelper_DragDrop;

			_addRowControl = new CommandItemControl();
			_addRowControl.Padding = new Padding(10, 0, 0, 0);
			_addRowControl.ForeColor = Color.AliceBlue;
			_addRowControl.Font = new Font("Segoe UI Semilight", 10);
			_addRowControl.CommandImage = Resources.command_input;
			_addRowControl.CommandDisplay = "Add Input Parameter";
			_addRowControl.Click += (sender, e) => AddInputParameter(sender, e, editor);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InputHeader", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InputDirections", this, editor));

			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_UserInputConfig", this));
			RenderedControls.Add(_addRowControl);
			RenderedControls.AddRange(commandControls.CreateUIHelpersFor("v_UserInputConfig", this, new Control[] { _userInputGridViewHelper }, editor));
			RenderedControls.Add(_userInputGridViewHelper);

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Header '{v_InputHeader}']";
		}

		private void AddInputParameter(object sender, EventArgs e, IfrmCommandEditor editor)
		{
			var newRow = v_UserInputConfig.NewRow();
			newRow["Size"] = "500,30";
			v_UserInputConfig.Rows.Add(newRow);
		}

		private void UserInputDataGridView_KeyDown(object sender, KeyEventArgs e)
		{
			if (_userInputGridViewHelper.SelectedRows.Count > 0)
				_userInputGridViewHelper.Rows.RemoveAt(_userInputGridViewHelper.SelectedCells[0].RowIndex);
		}		

		private void UserInputGridViewHelper_MouseDown(object sender, MouseEventArgs e)
		{
			var hitTest = _userInputGridViewHelper.HitTest(e.X, e.Y);
			if (hitTest.Type != DataGridViewHitTestType.Cell)
				return;

			_indexOfItemUnderMouseToDrag = hitTest.RowIndex;
			if (_indexOfItemUnderMouseToDrag > -1)
			{
				Size dragSize = SystemInformation.DragSize;
				_dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);
			}
			else
				_dragBoxFromMouseDown = Rectangle.Empty;
		}

		private void UserInputGridViewHelper_MouseUp(object sender, MouseEventArgs e)
		{
			_dragBoxFromMouseDown = Rectangle.Empty;
		}

		private void UserInputGridViewHelper_MouseMove(object sender, MouseEventArgs e)
		{
			if ((e.Button & MouseButtons.Left) != MouseButtons.Left)
				return;
			if (_dragBoxFromMouseDown == Rectangle.Empty || _dragBoxFromMouseDown.Contains(e.X, e.Y))
				return;
			if (_indexOfItemUnderMouseToDrag < 0)
				return;

			var row = _userInputGridViewHelper.Rows[_indexOfItemUnderMouseToDrag];
			_userInputGridViewHelper.DoDragDrop(row, DragDropEffects.All);

			//Clear	
			_userInputGridViewHelper.ClearSelection();
			//Set	
			if (_indexOfItemUnderMouseToDrop > -1)
				_userInputGridViewHelper.Rows[_indexOfItemUnderMouseToDrop].Selected = true;
		}

		private void UserInputGridViewHelper_DragOver(object sender, DragEventArgs e)
		{
			Point p = _userInputGridViewHelper.PointToClient(new Point(e.X, e.Y));
			var hitTest = _userInputGridViewHelper.HitTest(p.X, p.Y);
			if (hitTest.Type != DataGridViewHitTestType.Cell || hitTest.RowIndex == _indexOfItemUnderMouseToDrag)
			{
				e.Effect = DragDropEffects.None;
				return;
			}
			e.Effect = DragDropEffects.Move;
		}

		private void UserInputGridViewHelper_DragDrop(object sender, DragEventArgs e)
		{
			Point p = _userInputGridViewHelper.PointToClient(new Point(e.X, e.Y));
			var hitTest = _userInputGridViewHelper.HitTest(p.X, p.Y);
			if (hitTest.Type != DataGridViewHitTestType.Cell || hitTest.RowIndex == _indexOfItemUnderMouseToDrag + 1)
				return;

			_indexOfItemUnderMouseToDrop = hitTest.RowIndex;

			var tempRow = v_UserInputConfig.NewRow();
			tempRow.ItemArray = v_UserInputConfig.Rows[_indexOfItemUnderMouseToDrag].ItemArray;
			v_UserInputConfig.Rows.RemoveAt(_indexOfItemUnderMouseToDrag);

			if (_indexOfItemUnderMouseToDrag < _indexOfItemUnderMouseToDrop)
				_indexOfItemUnderMouseToDrop--;

			v_UserInputConfig.Rows.InsertAt(tempRow, _indexOfItemUnderMouseToDrop);
		}
	}
}