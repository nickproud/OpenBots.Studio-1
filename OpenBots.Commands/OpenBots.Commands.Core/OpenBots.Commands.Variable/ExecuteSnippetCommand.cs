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
		[Description("")]
		[SampleUsage("")]
		[Remarks("")]
		[CompatibleTypes(new Type[] { typeof(object) }, true)]
		public string v_VariableName { get; set; }

		public ExecuteSnippetCommand()
		{
			CommandName = "ExecuteSnippetCommand";
			SelectionName = "Execute Snippet";
			CommandEnabled = true;
			CommandIcon = Resources.command_parse;
		}

		public async override void RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;

			await VariableMethods.EvaluateCode(v_VariableName, v_Input, null, engine);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Input", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_VariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" ['{v_VariableName}' = '{v_Input}']";
		}
	}
}