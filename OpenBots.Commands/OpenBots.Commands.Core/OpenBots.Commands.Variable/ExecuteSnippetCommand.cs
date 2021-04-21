using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Properties;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;
using Tasks = System.Threading.Tasks;

namespace OpenBots.Commands.Variable
{
    [Serializable]
	[Category("Variable Commands")]
	[Description("This command runs a snippet and updates a variable if it is modified.")]
	public class ExecuteSnippetCommand : ScriptCommand
	{
		[Required]
		[DisplayName("Input Value")]
		[Description("Enter the input value for the variable.")]
		[SampleUsage("")]
		[Remarks("")]
		[CompatibleTypes(new Type[] { typeof(object) })]
		public string v_Input { get; set; }

		[Required]
		[DisplayName("Output Modified Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("{vUserVariable}")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(object) })]
		public string v_OutputUserVariableName { get; set; }

		public ExecuteSnippetCommand()
		{
			CommandName = "ExecuteSnippetCommand";
			SelectionName = "Execute Snippet";
			CommandEnabled = true;
			CommandIcon = Resources.command_parse;
		}

		public async override Tasks.Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;

			if (!string.IsNullOrEmpty(v_OutputUserVariableName))
			{
				await v_Input.EvaluateUnassignedCode(engine, nameof(v_OutputUserVariableName), this);

				//grabs the modified variable from the Roslyn engine and stores the value into the Studio engine
				object variableValue = v_OutputUserVariableName.GetVariableValue(engine);
				variableValue.SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
			}
			else
				await v_Input.EvaluateUnassignedCode(engine, nameof(v_OutputUserVariableName), this);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Input", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			if (v_OutputUserVariableName != null)
				return base.GetDisplayValue() + $" ['{v_Input}' - Update Variable '{v_OutputUserVariableName}']";
			else
				return base.GetDisplayValue() + $" ['{v_Input}']";
		}
	}
}