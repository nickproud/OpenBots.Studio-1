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

namespace OpenBots.Commands.If
{
    [Serializable]
	[Category("If Commands")]
	[Description("This command evaluates a logical statement to determine if the statement is 'true' or 'false' and subsequently performs action(s) based on either condition.")]
	public class BeginIfCommand : ScriptCommand, IConditionCommand
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
		[DisplayName("If Condition")]
		[Description("Enter the condition to evaluate in C#.")]
		[SampleUsage("")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(bool) })]
		public string v_Condition { get; set; }

		[Required]
		[DisplayName("If Condition")]
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

		public BeginIfCommand()
		{
			CommandName = "BeginIfCommand";
			SelectionName = "If";
			CommandEnabled = true;
			CommandIcon = Resources.command_begin_if;
			ScopeStartCommand = true;

			//define parameter table
			v_ActionParameterTable = new DataTable
			{
				TableName = DateTime.Now.ToString("IfActionParamTable" + DateTime.Now.ToString("MMddyy.hhmmss"))
			};
			v_ActionParameterTable.Columns.Add("Parameter Name");
			v_ActionParameterTable.Columns.Add("Parameter Value");

			_commandHelper = new ConditionCommandHelper(this);
		}

		public async override Tasks.Task RunCommand(object sender, ScriptAction parentCommand)
		{
			var engine = (IAutomationEngineInstance)sender;

			bool ifResult;
			if (v_Option == "Inline")
				ifResult = (bool)await v_Condition.EvaluateCode(engine);
			else
				ifResult = await CommandsHelper.DetermineStatementTruth(engine, v_ActionType, v_ActionParameterTable);

			int startIndex = 0, endIndex = 0, elseIndex = 0;
			elseIndex = parentCommand.AdditionalScriptCommands.FindIndex(a => a.ScriptCommand is ElseCommand && a.ScriptCommand.IsCommented == false);

			var elseIfIndices = FindAllElseIfIndices(parentCommand.AdditionalScriptCommands);
			ScriptAction elseIfCommandItem;
			BeginElseIfCommand targetElseIfCommand;

			if (ifResult)
			{
				startIndex = 0;
				int nextIndex = FindNextElseIndex(elseIfIndices, -1, elseIndex);
				if (nextIndex != -1)
					endIndex = nextIndex;
				else
					endIndex = parentCommand.AdditionalScriptCommands.Count;
			}
			else if (elseIfIndices != null)
			{
				foreach (var elseIfIndex in elseIfIndices)
				{
					elseIfCommandItem = parentCommand.AdditionalScriptCommands[elseIfIndex];
					targetElseIfCommand = (BeginElseIfCommand)elseIfCommandItem.ScriptCommand;
					ifResult = await CommandsHelper.DetermineStatementTruth(engine, targetElseIfCommand.v_ActionType,
						targetElseIfCommand.v_ActionParameterTable, targetElseIfCommand.v_Condition);
					if (ifResult)
					{
						startIndex = elseIfIndex + 1;
						int nextIndex = FindNextElseIndex(elseIfIndices, elseIfIndex, elseIndex);
						if (nextIndex != elseIfIndex)
							endIndex = nextIndex;
						else
							endIndex = parentCommand.AdditionalScriptCommands.Count;
						break;
					}
				}
				if (!ifResult && elseIndex != -1)
				{
					startIndex = elseIndex + 1;
					endIndex = parentCommand.AdditionalScriptCommands.Count;
				}
				else if (!ifResult && elseIndex == -1)
					return;
			}
			else if (elseIndex != -1)
			{
				startIndex = elseIndex + 1;
				endIndex = parentCommand.AdditionalScriptCommands.Count;
			}
			else
				return;

			for (int i = startIndex; i < endIndex; i++)
			{
				if (engine.IsCancellationPending || engine.CurrentLoopCancelled || engine.CurrentLoopContinuing)
					return;

				await engine.ExecuteCommand(parentCommand.AdditionalScriptCommands[i]);
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
				return $"If ({v_Condition})";
			else
				return _commandHelper.GetConditionDisplayValue(v_ActionType, v_ActionParameterTable, "If");
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

		private List<int> FindAllElseIfIndices(List<ScriptAction> additionalCommands)
		{
			// get the count of all (Enabled) Case Commands
			int totalElseIfCommands = additionalCommands.FindAll(
				action => action.ScriptCommand is BeginElseIfCommand &&
				action.ScriptCommand.IsCommented == false
				).Count;

			if (totalElseIfCommands == 0)
				return null;
			else
			{
				List<int> elseIfIndices = new List<int>();
				int startIndex = 0;

				// get the indices of all (Enabled) Case Commands
				while (startIndex < additionalCommands.Count && (startIndex = additionalCommands.FindIndex(
					startIndex, (a => a.ScriptCommand is BeginElseIfCommand && a.ScriptCommand.IsCommented == false))) != -1)
				{
					elseIfIndices.Add(startIndex++);
				}

				return elseIfIndices;
			}
		}

		private int FindNextElseIndex(List<int> elseIfIndices, int currIndex, int elseIndex)
		{
			int nextIndex;

			try
			{
				var currentElseIfIndex = elseIfIndices.IndexOf(currIndex);
				nextIndex = elseIfIndices[currentElseIfIndex + 1];
			}
			catch (Exception)
			{
				if (elseIndex != -1)
					nextIndex = elseIndex;
				else
					nextIndex = currIndex;
			}

			return nextIndex;
		}
	}

	
}
