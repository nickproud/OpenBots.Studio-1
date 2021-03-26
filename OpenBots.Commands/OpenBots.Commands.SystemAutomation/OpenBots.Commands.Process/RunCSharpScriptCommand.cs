using CSScriptLibrary;
using Newtonsoft.Json;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Properties;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Windows.Forms;
using OBFile = System.IO.File;

namespace OpenBots.Commands.Process
{
    [Serializable]
	[Category("Programs/Process Commands")]
	[Description("This command runs a C# script and waits for it to exit before proceeding.")]

	public class RunCSharpScriptCommand : ScriptCommand
	{

		[Required]
		[DisplayName("Script Path")]
		[Description("Enter a fully qualified path to the script, including the script extension.")]
		[SampleUsage(@"C:\temp\myscript.ps1 || {vScriptPath} || {ProjectPath}\myscript.ps1")]
		[Remarks("This command differs from *Start Process* because this command blocks execution until the script has completed. " +
				 "If you do not want to stop while the script executes, consider using *Start Process* instead.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[Editor("ShowFileSelectionHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(null, true)]
		public string v_ScriptPath { get; set; }

		[Required]
		[DisplayName("Argument Style")]
		[PropertyUISelectionOption("In-Studio Variables")]
		[PropertyUISelectionOption("Command Line")]
		[Description("Whether to pass a string[] (command line) or object[] (variables) to your script.")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_ArgumentType { get; set; }

		[DisplayName("Argument Values (Optional)")]
		[Description("Enter the values to pass into the script.")]
		[SampleUsage("hello || {vValue}")]
		[Remarks("This input is passed to your script as a object[].")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(object) }, true)]
		public DataTable v_VariableArgumentsDataTable { get; set; }

		[DisplayName("Command Line Arguments (Optional)")]
		[Description("Enter any arguments as a single string.")]
		[SampleUsage("-message Hello -t 2 || {vArguments} || -message {vMessage} -t 2")]
		[Remarks("This input is passed to your script as a string[].")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(null, true)]
		public string v_ScriptArgs { get; set; }

		[Required]
		[DisplayName("Script Has Output")]
		[PropertyUISelectionOption("Yes")]
		[PropertyUISelectionOption("No")]
		[Description("Whether the Main function of the script returns a value.")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_HasOutput { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output Script Result Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("vUserVariable")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(object) })]
		public string v_OutputUserVariableName { get; set; }

		[JsonIgnore]
		[Browsable(false)]
		private List<Control> _variableInputControls;

		[JsonIgnore]
		[Browsable(false)]
		private List<Control> _commandLineInputControls;

		[JsonIgnore]
		[Browsable(false)]
		private List<Control> _outputControls;

		[JsonIgnore]
		[Browsable(false)]
		private bool _hasRendered;

		public RunCSharpScriptCommand()
		{
			CommandName = "RunCSharpScriptCommand";
			SelectionName = "Run CSharp Script";
			CommandEnabled = true;
			CommandIcon = Resources.command_script;

			//initialize data table
			v_VariableArgumentsDataTable = new DataTable
			{
				TableName = "VariableArgumentsDataTable" + DateTime.Now.ToString("MMddyy.hhmmss")
			};

			v_HasOutput = "No";
			v_VariableArgumentsDataTable.Columns.Add("Argument Values");
		}

		public override void RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;

			string scriptPath = v_ScriptPath.ConvertUserVariableToString(engine);

			string code = OBFile.ReadAllText(scriptPath);
			var mainMethod = CSScript.LoadCode(code).CreateObject("*").GetType().GetMethod("Main");

			if (v_ArgumentType == "In-Studio Variables")
			{
				object[] args = new object[v_VariableArgumentsDataTable.Rows.Count];
				int i = 0;
				foreach (DataRow varColumn in v_VariableArgumentsDataTable.Rows)
                {
					string var = varColumn.Field<string>("Argument Values").Trim();
					dynamic input = var.ConvertUserVariableToString(engine);

					if (input == var && input.StartsWith("{") && input.EndsWith("}"))
					{
						if (var.ConvertUserVariableToObject(engine, nameof(v_VariableArgumentsDataTable), this) != null)
							input = var.ConvertUserVariableToObject(engine, nameof(v_VariableArgumentsDataTable), this);
					}
					args[i] = input;
					i++;
				}

				if (v_HasOutput == "No") 
				{
					if (mainMethod.GetParameters().Length == 0)
						mainMethod.Invoke(null, null);
					else
						mainMethod.Invoke(null, new object[] { args });
				}
				else
				{
					object result;
					if (mainMethod.GetParameters().Length == 0)
						result = mainMethod.Invoke(null, null);
					else
						result = mainMethod.Invoke(null, new object[] { args });

					result.StoreInUserVariable(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
				}
			}
			else if (v_ArgumentType == "Command Line")
			{
				string scriptArgs = v_ScriptArgs.ConvertUserVariableToString(engine);
				string[] argStrings = scriptArgs.Trim().Split(' ');
				if (v_HasOutput == "No")
                {
					if (mainMethod.GetParameters().Length == 0)
						mainMethod.Invoke(null, null);
					else
						mainMethod.Invoke(null, new object[] { argStrings });
				}
				else
				{
					object result;
					if (mainMethod.GetParameters().Length == 0)
						result = mainMethod.Invoke(null, null);
					else
						result = mainMethod.Invoke(null, new object[] { argStrings });

					result.StoreInUserVariable(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
				}
			}
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_ScriptPath", this, editor));

			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_ArgumentType", this, editor));
			((ComboBox)RenderedControls[5]).SelectedIndexChanged += ArgumentTypeComboBox_SelectedIndexChanged;

			_commandLineInputControls = commandControls.CreateDefaultInputGroupFor("v_ScriptArgs", this, editor);
			RenderedControls.AddRange(_commandLineInputControls);

			_variableInputControls = commandControls.CreateDefaultDataGridViewGroupFor("v_VariableArgumentsDataTable", this, editor);
			RenderedControls.AddRange(_variableInputControls);

			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_HasOutput", this, editor));
			((ComboBox)RenderedControls[13]).SelectedIndexChanged += HasOutputComboBox_SelectedIndexChanged;

			_outputControls = commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor);
			RenderedControls.AddRange(_outputControls);

			return RenderedControls;
		}

        public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [C# Script Path '{v_ScriptPath}']";
		}

		public override void Shown()
		{
			base.Shown();
			_hasRendered = true;
			if (v_ArgumentType == null)
			{
				v_ArgumentType = "In-Studio Variables";
				((ComboBox)RenderedControls[5]).Text = v_ArgumentType;
			}
			ArgumentTypeComboBox_SelectedIndexChanged(this, null);
			HasOutputComboBox_SelectedIndexChanged(this, null);
		}

		private void ArgumentTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (((ComboBox)RenderedControls[5]).Text == "In-Studio Variables" && _hasRendered)
			{

				foreach (var ctrl in _variableInputControls)
					ctrl.Visible = true;
				foreach (var ctrl in _commandLineInputControls)
				{
					ctrl.Visible = false;
					if (ctrl is TextBox)
						((TextBox)ctrl).Clear();
				}
			}
			else if (_hasRendered)
			{
				foreach (var ctrl in _variableInputControls)
				{
					ctrl.Visible = false;
					if (ctrl is DataGridView)
					{
						v_VariableArgumentsDataTable.Rows.Clear();
					}
				}
				foreach (var ctrl in _commandLineInputControls)
					ctrl.Visible = true;
			}
		}

		private void HasOutputComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (((ComboBox)RenderedControls[13]).Text == "Yes" && _hasRendered)
			{
				foreach (var ctrl in _outputControls)
					ctrl.Visible = true;
			}
			else if(_hasRendered)
			{
				foreach (var ctrl in _outputControls)
				{
					ctrl.Visible = false;
					if (ctrl is ComboBox)
						((ComboBox)ctrl).SelectedIndex = -1;
				}
			}
		}
	}
}
