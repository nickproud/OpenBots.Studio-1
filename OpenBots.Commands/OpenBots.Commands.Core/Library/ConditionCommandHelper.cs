using OpenBots.Core.Command;
using OpenBots.Core.Interfaces;
using OpenBots.Core.Properties;
using OpenBots.Core.UI.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace OpenBots.Commands.Core.Library
{
    public class ConditionCommandHelper
    {
		public ScriptCommand ConditionCommand { get; set; }
		public CommandItemControl RecorderControl { get; set; }
		public DataGridView ActionGridViewHelper { get; set; }
		public ComboBox ActionDropdown { get; set; }

		public ConditionCommandHelper(ScriptCommand command)
        {
			ConditionCommand = command;
        }

        public List<Control> CreateConditionActionParameterTable(IfrmCommandEditor editor, ICommandControls commandControls)
        {
			var noCSharpControls = new List<Control>();
			RecorderControl = new CommandItemControl("guirecorder_helper", Resources.command_camera, "Element Recorder");
			RecorderControl.Hide();

			noCSharpControls.Add(commandControls.CreateDefaultLabelFor("v_ActionType", ConditionCommand));
			ActionDropdown = commandControls.CreateDropdownFor("v_ActionType", ConditionCommand);
			noCSharpControls.AddRange(commandControls.CreateUIHelpersFor("v_ActionType", ConditionCommand, new Control[] { ActionDropdown }, editor));
			ActionDropdown.SelectionChangeCommitted += Action_SelectionChangeCommitted;
			noCSharpControls.Add(ActionDropdown);

			noCSharpControls.Add(commandControls.CreateDefaultLabelFor("v_ActionParameterTable", ConditionCommand));

			RecorderControl.Click += (sender, e) => ShowElementRecorder(sender, e, editor, commandControls);
			noCSharpControls.Add(RecorderControl);

			ActionGridViewHelper = commandControls.CreateDefaultDataGridViewFor("v_ActionParameterTable", ConditionCommand);
			ActionGridViewHelper.AllowUserToAddRows = false;
			ActionGridViewHelper.AllowUserToDeleteRows = false;
			ActionGridViewHelper.MouseEnter += ActionGridViewHelper_MouseEnter;

			noCSharpControls.AddRange(commandControls.CreateUIHelpersFor("v_ActionParameterTable", ConditionCommand, new Control[] { ActionGridViewHelper }, editor));
			noCSharpControls.Add(ActionGridViewHelper);
			return noCSharpControls;
		}

		private void Action_SelectionChangeCommitted(object sender, EventArgs e)
		{
			DataGridView actionParameterBox = ActionGridViewHelper;

			IConditionCommand cmd = (IConditionCommand)ConditionCommand;
			DataTable actionParameters = cmd.v_ActionParameterTable;

			//sender is null when command is updating
			if (sender != null)
				actionParameters.Rows.Clear();

			DataGridViewComboBoxCell comparisonComboBox;

			//remove if exists            
			if (RecorderControl.Visible)
			{
				RecorderControl.Hide();
			}

			switch (ActionDropdown.SelectedItem)
			{
				case "Number Compare":
					actionParameterBox.Visible = true;

					if (sender != null)
					{
						actionParameters.Rows.Add("Number1", "");
						actionParameters.Rows.Add("Operand", "");
						actionParameters.Rows.Add("Number2", "");
						actionParameterBox.DataSource = actionParameters;
					}

					//combobox cell for Variable Name
					comparisonComboBox = new DataGridViewComboBoxCell();
					comparisonComboBox.Items.Add("is equal to");
					comparisonComboBox.Items.Add("is greater than");
					comparisonComboBox.Items.Add("is greater than or equal to");
					comparisonComboBox.Items.Add("is less than");
					comparisonComboBox.Items.Add("is less than or equal to");
					comparisonComboBox.Items.Add("is not equal to");

					//assign cell as a combobox
					actionParameterBox.Rows[1].Cells[1] = comparisonComboBox;

					break;
				case "Date Compare":
					actionParameterBox.Visible = true;

					if (sender != null)
					{
						actionParameters.Rows.Add("Date1", "");
						actionParameters.Rows.Add("Operand", "");
						actionParameters.Rows.Add("Date2", "");
						actionParameterBox.DataSource = actionParameters;
					}

					//combobox cell for Variable Name
					comparisonComboBox = new DataGridViewComboBoxCell();
					comparisonComboBox.Items.Add("is equal to");
					comparisonComboBox.Items.Add("is greater than");
					comparisonComboBox.Items.Add("is greater than or equal to");
					comparisonComboBox.Items.Add("is less than");
					comparisonComboBox.Items.Add("is less than or equal to");
					comparisonComboBox.Items.Add("is not equal to");

					//assign cell as a combobox
					actionParameterBox.Rows[1].Cells[1] = comparisonComboBox;

					break;
				case "Text Compare":
					actionParameterBox.Visible = true;

					if (sender != null)
					{
						actionParameters.Rows.Add("Text1", "");
						actionParameters.Rows.Add("Operand", "");
						actionParameters.Rows.Add("Text2", "");
						actionParameters.Rows.Add("Case Sensitive", "No");
						actionParameterBox.DataSource = actionParameters;
					}

					//combobox cell for Variable Name
					comparisonComboBox = new DataGridViewComboBoxCell();
					comparisonComboBox.Items.Add("contains");
					comparisonComboBox.Items.Add("does not contain");
					comparisonComboBox.Items.Add("is equal to");
					comparisonComboBox.Items.Add("is not equal to");

					//assign cell as a combobox
					actionParameterBox.Rows[1].Cells[1] = comparisonComboBox;

					comparisonComboBox = new DataGridViewComboBoxCell();
					comparisonComboBox.Items.Add("Yes");
					comparisonComboBox.Items.Add("No");

					//assign cell as a combobox
					actionParameterBox.Rows[3].Cells[1] = comparisonComboBox;

					break;
				case "Has Value":
					actionParameterBox.Visible = true;
					if (sender != null)
					{
						actionParameters.Rows.Add("Variable Name", "");
						actionParameterBox.DataSource = actionParameters;
					}

					break;
				case "Is Numeric":
					actionParameterBox.Visible = true;
					if (sender != null)
					{
						actionParameters.Rows.Add("Variable Name", "");
						actionParameterBox.DataSource = actionParameters;
					}

					break;
				case "Error Occured":
					actionParameterBox.Visible = true;
					if (sender != null)
					{
						actionParameters.Rows.Add("Line Number", "");
						actionParameterBox.DataSource = actionParameters;
					}

					break;
				case "Error Did Not Occur":
					actionParameterBox.Visible = true;

					if (sender != null)
					{
						actionParameters.Rows.Add("Line Number", "");
						actionParameterBox.DataSource = actionParameters;
					}

					break;
				case "Window Name Exists":
				case "Active Window Name Is":
					actionParameterBox.Visible = true;
					if (sender != null)
					{
						actionParameters.Rows.Add("Window Name", "");
						actionParameterBox.DataSource = actionParameters;
					}

					break;
				case "File Exists":
					actionParameterBox.Visible = true;
					if (sender != null)
					{
						actionParameters.Rows.Add("File Path", "");
						actionParameters.Rows.Add("True When", "It Does Exist");
						actionParameterBox.DataSource = actionParameters;
					}

					//combobox cell for Variable Name
					comparisonComboBox = new DataGridViewComboBoxCell();
					comparisonComboBox.Items.Add("It Does Exist");
					comparisonComboBox.Items.Add("It Does Not Exist");

					//assign cell as a combobox
					actionParameterBox.Rows[1].Cells[1] = comparisonComboBox;

					break;
				case "Folder Exists":
					actionParameterBox.Visible = true;

					if (sender != null)
					{
						actionParameters.Rows.Add("Folder Path", "");
						actionParameters.Rows.Add("True When", "It Does Exist");
						actionParameterBox.DataSource = actionParameters;
					}

					//combobox cell for Variable Name
					comparisonComboBox = new DataGridViewComboBoxCell();
					comparisonComboBox.Items.Add("It Does Exist");
					comparisonComboBox.Items.Add("It Does Not Exist");

					//assign cell as a combobox
					actionParameterBox.Rows[1].Cells[1] = comparisonComboBox;
					break;
				case "Selenium Web Element Exists":
					actionParameterBox.Visible = true;

					if (sender != null)
					{
						actionParameters.Rows.Add("Selenium Instance Name", "DefaultBrowser");
						actionParameters.Rows.Add("Element Search Method", "");
						actionParameters.Rows.Add("Element Search Parameter", "");
						actionParameters.Rows.Add("Timeout (Seconds)", "30");
						actionParameters.Rows.Add("True When", "It Does Exist");
						actionParameterBox.DataSource = actionParameters;
					}

					comparisonComboBox = new DataGridViewComboBoxCell();
					comparisonComboBox.Items.Add("It Does Exist");
					comparisonComboBox.Items.Add("It Does Not Exist");

					//assign cell as a combobox
					actionParameterBox.Rows[4].Cells[1] = comparisonComboBox;

					comparisonComboBox = new DataGridViewComboBoxCell();
					comparisonComboBox.Items.Add("XPath");
					comparisonComboBox.Items.Add("ID");
					comparisonComboBox.Items.Add("Name");
					comparisonComboBox.Items.Add("Tag Name");
					comparisonComboBox.Items.Add("Class Name");
					comparisonComboBox.Items.Add("CSS Selector");

					//assign cell as a combobox
					actionParameterBox.Rows[1].Cells[1] = comparisonComboBox;

					break;
				case "GUI Element Exists":
					actionParameterBox.Visible = true;
					if (sender != null)
					{
						actionParameters.Rows.Add("Window Name", "Current Window");
						actionParameters.Rows.Add("Element Search Method", "AutomationId");
						actionParameters.Rows.Add("Element Search Parameter", "");
						actionParameters.Rows.Add("Timeout (Seconds)", "30");
						actionParameters.Rows.Add("True When", "It Does Exist");
						actionParameterBox.DataSource = actionParameters;
					}

					comparisonComboBox = new DataGridViewComboBoxCell();
					comparisonComboBox.Items.Add("It Does Exist");
					comparisonComboBox.Items.Add("It Does Not Exist");

					//assign cell as a combobox
					actionParameterBox.Rows[4].Cells[1] = comparisonComboBox;

					var parameterName = new DataGridViewComboBoxCell();
					parameterName.Items.Add("AcceleratorKey");
					parameterName.Items.Add("AccessKey");
					parameterName.Items.Add("AutomationId");
					parameterName.Items.Add("ClassName");
					parameterName.Items.Add("FrameworkId");
					parameterName.Items.Add("HasKeyboardFocus");
					parameterName.Items.Add("HelpText");
					parameterName.Items.Add("IsContentElement");
					parameterName.Items.Add("IsControlElement");
					parameterName.Items.Add("IsEnabled");
					parameterName.Items.Add("IsKeyboardFocusable");
					parameterName.Items.Add("IsOffscreen");
					parameterName.Items.Add("IsPassword");
					parameterName.Items.Add("IsRequiredForForm");
					parameterName.Items.Add("ItemStatus");
					parameterName.Items.Add("ItemType");
					parameterName.Items.Add("LocalizedControlType");
					parameterName.Items.Add("Name");
					parameterName.Items.Add("NativeWindowHandle");
					parameterName.Items.Add("ProcessID");

					//assign cell as a combobox
					actionParameterBox.Rows[1].Cells[1] = parameterName;

					RecorderControl.Show();
					break;
				case "Image Element Exists":
					actionParameterBox.Visible = true;

					if (sender != null)
					{
						actionParameters.Rows.Add("Captured Image Variable", "");
						actionParameters.Rows.Add("Accuracy (0-1)", "0.8");
						actionParameters.Rows.Add("True When", "It Does Exist");
						actionParameters.Rows.Add("Timeout (Seconds)", "30");
						actionParameterBox.DataSource = actionParameters;
					}

					comparisonComboBox = new DataGridViewComboBoxCell();
					comparisonComboBox.Items.Add("It Does Exist");
					comparisonComboBox.Items.Add("It Does Not Exist");

					//assign cell as a combobox
					actionParameterBox.Rows[2].Cells[1] = comparisonComboBox;
					break;
				case "App Instance Exists":
					actionParameterBox.Visible = true;

					if (sender != null)
					{
						actionParameters.Rows.Add("Instance Name", "");
						actionParameters.Rows.Add("True When", "It Does Exist");
						actionParameterBox.DataSource = actionParameters;
					}

					comparisonComboBox = new DataGridViewComboBoxCell();
					comparisonComboBox.Items.Add("It Does Exist");
					comparisonComboBox.Items.Add("It Does Not Exist");

					//assign cell as a combobox
					actionParameterBox.Rows[1].Cells[1] = comparisonComboBox;
					break;
				default:
					break;
			}
			actionParameterBox.Columns[0].ReadOnly = true;
		}

		private void ShowElementRecorder(object sender, EventArgs e, IfrmCommandEditor editor, ICommandControls commandControls)
		{
			var result = commandControls.ShowConditionElementRecorder(sender, e, editor);

			ActionGridViewHelper.Rows[0].Cells[1].Value = result.Item1;
			ActionGridViewHelper.Rows[2].Cells[1].Value = result.Item2;
		}

		private void ActionGridViewHelper_MouseEnter(object sender, EventArgs e)
		{
			try
			{
				Action_SelectionChangeCommitted(null, null);
			}
			catch (Exception)
			{
				Action_SelectionChangeCommitted(sender, e);
			}
		}

		public string GetConditionDisplayValue(string actionType, DataTable actionParameterTable, string conditionType)
        {
			switch (actionType)
			{
				case "Number Compare":
					string number1 = ((from rw in actionParameterTable.AsEnumerable()
									   where rw.Field<string>("Parameter Name") == "Number1"
									   select rw.Field<string>("Parameter Value")).FirstOrDefault());
					string operand = ((from rw in actionParameterTable.AsEnumerable()
									   where rw.Field<string>("Parameter Name") == "Operand"
									   select rw.Field<string>("Parameter Value")).FirstOrDefault());
					string number2 = ((from rw in actionParameterTable.AsEnumerable()
									   where rw.Field<string>("Parameter Name") == "Number2"
									   select rw.Field<string>("Parameter Value")).FirstOrDefault());

					return $"{conditionType} ('{number1}' {operand} '{number2}')";
				case "Date Compare":
					string date1 = ((from rw in actionParameterTable.AsEnumerable()
									 where rw.Field<string>("Parameter Name") == "Date1"
									 select rw.Field<string>("Parameter Value")).FirstOrDefault());
					string operand2 = ((from rw in actionParameterTable.AsEnumerable()
										where rw.Field<string>("Parameter Name") == "Operand"
										select rw.Field<string>("Parameter Value")).FirstOrDefault());
					string date2 = ((from rw in actionParameterTable.AsEnumerable()
									 where rw.Field<string>("Parameter Name") == "Date2"
									 select rw.Field<string>("Parameter Value")).FirstOrDefault());

					return $"{conditionType} ('{date1}' {operand2} '{date2}')";
				case "Text Compare":
					string text1 = ((from rw in actionParameterTable.AsEnumerable()
									 where rw.Field<string>("Parameter Name") == "Text1"
									 select rw.Field<string>("Parameter Value")).FirstOrDefault());
					string operand3 = ((from rw in actionParameterTable.AsEnumerable()
										where rw.Field<string>("Parameter Name") == "Operand"
										select rw.Field<string>("Parameter Value")).FirstOrDefault());
					string text2 = ((from rw in actionParameterTable.AsEnumerable()
									 where rw.Field<string>("Parameter Name") == "Text2"
									 select rw.Field<string>("Parameter Value")).FirstOrDefault());

					return $"{conditionType} ('{text1}' {operand3} '{text2}')";

				case "Has Value":
					string variableName = ((from rw in actionParameterTable.AsEnumerable()
											where rw.Field<string>("Parameter Name") == "Variable Name"
											select rw.Field<string>("Parameter Value")).FirstOrDefault());

					return $"{conditionType} (Variable '{variableName}' Has Value)";

				case "Is Numeric":
					string varName = ((from rw in actionParameterTable.AsEnumerable()
									   where rw.Field<string>("Parameter Name") == "Variable Name"
									   select rw.Field<string>("Parameter Value")).FirstOrDefault());

					return $"{conditionType} (Variable '{varName}' Is Numeric)";

				case "Error Occured":
					string lineNumber = ((from rw in actionParameterTable.AsEnumerable()
										  where rw.Field<string>("Parameter Name") == "Line Number"
										  select rw.Field<string>("Parameter Value")).FirstOrDefault());

					return $"{conditionType} (Error Occured on Line Number '{lineNumber}')";

				case "Error Did Not Occur":
					string lineNum = ((from rw in actionParameterTable.AsEnumerable()
									   where rw.Field<string>("Parameter Name") == "Line Number"
									   select rw.Field<string>("Parameter Value")).FirstOrDefault());

					return $"{conditionType} (Error Did Not Occur on Line Number '{lineNum}')";

				case "Window Name Exists":
				case "Active Window Name Is":

					string windowName = ((from rw in actionParameterTable.AsEnumerable()
										  where rw.Field<string>("Parameter Name") == "Window Name"
										  select rw.Field<string>("Parameter Value")).FirstOrDefault());

					return $"{conditionType} {actionType} [Window Name '{windowName}']";

				case "File Exists":
					string filePath = ((from rw in actionParameterTable.AsEnumerable()
										where rw.Field<string>("Parameter Name") == "File Path"
										select rw.Field<string>("Parameter Value")).FirstOrDefault());

					string fileCompareType = ((from rw in actionParameterTable.AsEnumerable()
											   where rw.Field<string>("Parameter Name") == "True When"
											   select rw.Field<string>("Parameter Value")).FirstOrDefault());

					if (fileCompareType == "It Does Not Exist")
						return $"{conditionType} File Does Not Exist [File '{filePath}']";
					else
						return $"{conditionType} File Exists [File '{filePath}']";

				case "Folder Exists":
					string folderPath = ((from rw in actionParameterTable.AsEnumerable()
										  where rw.Field<string>("Parameter Name") == "Folder Path"
										  select rw.Field<string>("Parameter Value")).FirstOrDefault());

					string folderCompareType = ((from rw in actionParameterTable.AsEnumerable()
												 where rw.Field<string>("Parameter Name") == "True When"
												 select rw.Field<string>("Parameter Value")).FirstOrDefault());

					if (folderCompareType == "It Does Not Exist")
						return $"{conditionType} Folder Does Not Exist [Folder '{folderPath}']";
					else
						return $"{conditionType} Folder Exists [Folder '{folderPath}']";

				case "Selenium Web Element Exists":
					string parameterName = ((from rw in actionParameterTable.AsEnumerable()
											 where rw.Field<string>("Parameter Name") == "Element Search Parameter"
											 select rw.Field<string>("Parameter Value")).FirstOrDefault());

					string searchMethod = ((from rw in actionParameterTable.AsEnumerable()
											where rw.Field<string>("Parameter Name") == "Element Search Method"
											select rw.Field<string>("Parameter Value")).FirstOrDefault());

					string webElementCompareType = ((from rw in actionParameterTable.AsEnumerable()
													 where rw.Field<string>("Parameter Name") == "True When"
													 select rw.Field<string>("Parameter Value")).FirstOrDefault());

					if (webElementCompareType == "It Does Not Exist")
						return $"{conditionType} Web Element Does Not Exist [{searchMethod} '{parameterName}']";
					else
						return $"{conditionType} Selenium Web Element Exists [{searchMethod} '{parameterName}']";

				case "GUI Element Exists":
					string guiWindowName = ((from rw in actionParameterTable.AsEnumerable()
											 where rw.Field<string>("Parameter Name") == "Window Name"
											 select rw.Field<string>("Parameter Value")).FirstOrDefault());

					string guiSearch = ((from rw in actionParameterTable.AsEnumerable()
										 where rw.Field<string>("Parameter Name") == "Element Search Parameter"
										 select rw.Field<string>("Parameter Value")).FirstOrDefault());

					string guiElementCompareType = ((from rw in actionParameterTable.AsEnumerable()
													 where rw.Field<string>("Parameter Name") == "True When"
													 select rw.Field<string>("Parameter Value")).FirstOrDefault());

					if (guiElementCompareType == "It Does Not Exist")
						return $"{conditionType} GUI Element Does Not Exist [Find '{guiSearch}' Element In '{guiWindowName}']";
					else
						return $"{conditionType} GUI Element Exists [Find '{guiSearch}' Element In '{guiWindowName}']";

				case "Image Element Exists":
					string imageCompareType = (from rw in actionParameterTable.AsEnumerable()
											   where rw.Field<string>("Parameter Name") == "True When"
											   select rw.Field<string>("Parameter Value")).FirstOrDefault();

					if (imageCompareType == "It Does Not Exist")
						return $"{conditionType} Image Does Not Exist on Screen";
					else
						return $"{conditionType} Image Exists on Screen";
				case "App Instance Exists":
					string instanceName = ((from rw in actionParameterTable.AsEnumerable()
											where rw.Field<string>("Parameter Name") == "Instance Name"
											select rw.Field<string>("Parameter Value")).FirstOrDefault());

					string instanceCompareType = (from rw in actionParameterTable.AsEnumerable()
												  where rw.Field<string>("Parameter Name") == "True When"
												  select rw.Field<string>("Parameter Value")).FirstOrDefault();

					if (instanceCompareType == "It Does Not Exist")
						return $"{conditionType} App Instance Does Not Exist [Instance Name '{instanceName}']";
					else
						return $"{conditionType} App Instance Exists [Instance Name '{instanceName}']";
				default:
					return $"{conditionType}...";
			}
		}
	}
}
