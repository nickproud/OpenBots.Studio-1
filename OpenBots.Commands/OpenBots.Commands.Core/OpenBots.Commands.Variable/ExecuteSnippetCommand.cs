using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
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
	[Description("This command runs a snippet and assigns the output to a variable.")]
	public class ExecuteSnippetCommand : ScriptCommand
	{
		[Required]
		[DisplayName("Input Value")]
		[Description("Enter the input value for the variable.")]
		[SampleUsage("")]
		[Remarks("")]
		[CompatibleTypes(new Type[] { typeof(object) }, true)]
		public string v_Input { get; set; }

		[Required]
		[DisplayName("Output Variable")]
		[Description("Enter the variable to which the output will be assigned. Leave empty to execute code without assignment.")]
		[SampleUsage("")]
		[Remarks("")]
		[CompatibleTypes(new Type[] { typeof(object) }, true)]
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
			object value = null;
			if (v_OutputUserVariableName != "")
			{
				value = await VariableMethods.EvaluateCode(v_Input, engine);
				value.SetVariableValue(engine, v_OutputUserVariableName, VariableMethods.GetVariableType(v_OutputUserVariableName, engine));
			}
			else
				await v_Input.EvaluateCodeInPlace(engine);
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
				return base.GetDisplayValue() + $" ['{v_OutputUserVariableName}' = '{v_Input}']";
			else
				return base.GetDisplayValue() + $"['{v_Input}']";
		}
	}
}