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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenBots.Commands.Data
{
    [Serializable]
	[Category("Data Commands")]
	[Description("This command performs a math calculation and saves the result in a variable.")]
	public class MathCalculationCommand : ScriptCommand
	{
		[Required]
		[DisplayName("Math Expression")]
		[Description("Specify either text or a variable that contains a valid math expression.")]
		[SampleUsage("(2 + 5) * 3 || (vNumber1 + vNumber2) * vNumber3")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(double) })]
		public string v_MathExpression { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output Result Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("vUserVariable")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(double) })]
		public string v_OutputUserVariableName { get; set; }

		public MathCalculationCommand()
		{
			CommandName = "MathCalculationCommand";
			SelectionName = "Math Calculation";
			CommandEnabled = true;
			CommandIcon = Resources.command_function;

			v_MathExpression = "(2 + 5) * 3";
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;

			var result = Convert.ToDouble(await v_MathExpression.EvaluateCode(engine));

			result.SetVariableValue(engine, v_OutputUserVariableName);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_MathExpression", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Compute '{v_MathExpression}' - Store Result in '{v_OutputUserVariableName}']";
		}
	}
}
