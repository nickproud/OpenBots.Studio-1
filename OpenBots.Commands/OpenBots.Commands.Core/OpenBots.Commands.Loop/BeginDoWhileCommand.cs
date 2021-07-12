using Newtonsoft.Json;
using OpenBots.Commands.Core.Library;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Interfaces;
using OpenBots.Core.Model.ApplicationModel;
using OpenBots.Core.Properties;
using OpenBots.Core.Script;
using OpenBots.Core.Utilities.CommandUtilities;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Tasks = System.Threading.Tasks;

namespace OpenBots.Commands.Loop
{
    [Serializable]
	[Category("Loop Commands")]
	[Description("This command evaluates a specified logical statement and executes the contained commands repeatedly (in loop) " +
		"until that logical statement becomes false. Execution of the commands occurs at least once.")]
	public class BeginDoWhileCommand : ScriptCommand, IConditionCommand
	{
		[Required]
		[DisplayName("Condition Option")]
		[PropertyUISelectionOption("Inline")]
		[PropertyUISelectionOption("Builder")]
		[Description("Select whether to create the condition using C# or with the condition builder")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_Option { get; set; }

		[Required]
		[DisplayName("Do While Condition")]
		[Description("Enter the condition to evaluate in C#.")]
		[SampleUsage("")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(bool) })]
		public string v_Condition { get; set; }

		[Required]
		[DisplayName("Do While Condition")]
		[PropertyUISelectionOption("Number Compare")]
		[PropertyUISelectionOption("Date Compare")]
		[PropertyUISelectionOption("Text Compare")]
		[PropertyUISelectionOption("Has Value")]
		[PropertyUISelectionOption("Is Numeric")]
		[PropertyUISelectionOption("Window Name Exists")]
		[PropertyUISelectionOption("Active Window Name Is")]
		[PropertyUISelectionOption("File Exists")]
		[PropertyUISelectionOption("Folder Exists")]
		[PropertyUISelectionOption("Selenium Web Element Exists")]
		[PropertyUISelectionOption("GUI Element Exists")]
		[PropertyUISelectionOption("Image Element Exists")]
		[PropertyUISelectionOption("App Instance Exists")]
		[PropertyUISelectionOption("Error Occured")]
		[PropertyUISelectionOption("Error Did Not Occur")]
		[Description("Select the necessary condition type.")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_ActionType { get; set; }

		[Required]
		[DisplayName("Additional Parameters")]
		[Description("Supply or Select the required comparison parameters.")]
		[SampleUsage("Param Value || vParamValue")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(Bitmap), typeof(DateTime), typeof(string), typeof(double), typeof(int), typeof(bool), typeof(OBAppInstance) })]
		public DataTable v_ActionParameterTable { get; set; }

		[JsonIgnore]
		[Browsable(false)]
		private List<Control> _noCSharpControls;

		[JsonIgnore]
		[Browsable(false)]
		private List<Control> _cSharpControls;

		[JsonIgnore]
		[Browsable(false)]
		private bool _hasRendered;
		
		[JsonIgnore]
		[Browsable(false)]
		private ConditionCommandHelper _commandHelper;

		public BeginDoWhileCommand()
		{
			CommandName = "BeginDoWhileCommand";
			SelectionName = "Do While";
			CommandEnabled = true;
			CommandIcon = Resources.command_startloop;
			ScopeStartCommand = true;

			//define parameter table
			v_ActionParameterTable = new DataTable
			{
				TableName = DateTime.Now.ToString("ActionParamTable" + DateTime.Now.ToString("MMddyy.hhmmss"))
			};
			v_ActionParameterTable.Columns.Add("Parameter Name");
			v_ActionParameterTable.Columns.Add("Parameter Value");

			_commandHelper = new ConditionCommandHelper(this);
		}

		public async override Tasks.Task RunCommand(object sender, ScriptAction parentCommand)
		{
			var engine = (IAutomationEngineInstance)sender;

			bool whileResult = true;
			
			engine.ReportProgress("Starting While"); 

			while (whileResult)
			{
				foreach (var cmd in parentCommand.AdditionalScriptCommands)
				{
					if (engine.IsCancellationPending)
						return;

					await engine.ExecuteCommand(cmd);

					if (engine.CurrentLoopCancelled)
					{
						engine.ReportProgress("Exiting While"); 
						engine.CurrentLoopCancelled = false;
						return;
					}

					if (engine.CurrentLoopContinuing)
					{
						engine.ReportProgress("Continuing Next While"); 
						engine.CurrentLoopContinuing = false;
						break;
					}
				}
				if (v_Option == "Inline")
					whileResult = (bool)await v_Condition.EvaluateCode(engine);
				else
					whileResult = await CommandsHelper.DetermineStatementTruth(engine, v_ActionType, v_ActionParameterTable);
			}
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_Option", this, editor));
			((ComboBox)RenderedControls[1]).SelectedIndexChanged += OptionComboBox_SelectedIndexChanged;

			//CSharp Controls
			_cSharpControls = new List<Control>();
			_cSharpControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Condition", this, editor));
			RenderedControls.AddRange(_cSharpControls);

			//no CSharp Controls
			_noCSharpControls = _commandHelper.CreateConditionActionParameterTable(editor, commandControls);
			RenderedControls.AddRange(_noCSharpControls);

			return RenderedControls;
		}
		
		public override string GetDisplayValue()
		{
			if (v_Option == "Inline")
				return $"Do While ({v_Condition})";
			else
				return _commandHelper.GetConditionDisplayValue(v_ActionType, v_ActionParameterTable, "Do While");
		}

		public override void Shown()
		{
			base.Shown();
			_hasRendered = true;
			if (v_Option == null)
			{
				v_Option = "Inline";
				((ComboBox)RenderedControls[1]).Text = v_Option;
			}
			OptionComboBox_SelectedIndexChanged(null, null);
		}

		private void OptionComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (((ComboBox)RenderedControls[1]).Text == "Inline" && _hasRendered)
			{
				foreach (var ctrl in _cSharpControls)
					ctrl.Visible = true;

				foreach (var ctrl in _noCSharpControls)
				{
					ctrl.Visible = false;
					if (ctrl is DataGridView)
						v_ActionParameterTable.Rows.Clear();
					else if (ctrl is ComboBox)
						((ComboBox)ctrl).SelectedIndex = -1;
				}
			}
			else if (_hasRendered)
			{
				foreach (var ctrl in _cSharpControls)
				{
					ctrl.Visible = false;
					if (ctrl is TextBox)
						((TextBox)ctrl).Clear();
				}

				foreach (var ctrl in _noCSharpControls)
					ctrl.Visible = true;
			}
		}
	}
}
