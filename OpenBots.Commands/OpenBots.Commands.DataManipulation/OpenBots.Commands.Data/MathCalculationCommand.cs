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
using OBDataTable = System.Data.DataTable;

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
		[SampleUsage("(2 + 5) * 3 || ({vNumber1} + {vNumber2}) * {vNumber3}")]
		[Remarks("You can use known numbers or variables.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(null, true)]
		public string v_MathExpression { get; set; }

		[DisplayName("Thousand Separator (Optional)")]
		[Description("Specify the seperator used to identify decimal places.")]
		[SampleUsage(", || . || {vThousandSeparator}")]
		[Remarks("Typically a comma or a decimal point (period), like in 100,000, ',' is a thousand separator.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(null, true)]
		public string v_ThousandSeparator { get; set; }

		[DisplayName("Decimal Separator (Optional)")]
		[Description("Specify the seperator used to identify decimal places.")]
		[SampleUsage(". || , || {vDecimalSeparator}")]
		[Remarks("Typically a comma or a decimal point (period), like in 60.99, '.' is a decimal separator.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(null, true)]
		public string v_DecimalSeparator { get; set; }

		[Required]
		[DisplayName("Accept Undefined")]
		[PropertyUISelectionOption("Yes")]
		[PropertyUISelectionOption("No")]
		[Description("Specify whether the equation can return undefined as a valid result. Otherwise throw a 'DivideByZeroException'.")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_AcceptUndefined { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output Result Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("{vUserVariable}")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_OutputUserVariableName { get; set; }

		public MathCalculationCommand()
		{
			CommandName = "MathCalculationCommand";
			SelectionName = "Math Calculation";
			CommandEnabled = true;
			CommandIcon = Resources.command_function;

			v_MathExpression = "(2 + 5) * 3";
			v_DecimalSeparator = ".";
			v_ThousandSeparator = ",";
			v_AcceptUndefined = "No";
		}

		public override void RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;

			//get variablized string
			var variableMath = v_MathExpression.ConvertUserVariableToString(engine);
			var thousandSeparator = v_ThousandSeparator.ConvertUserVariableToString(engine);
			var decimalSeparator = v_DecimalSeparator.ConvertUserVariableToString(engine);

			//Check if expression calculation is infinity
			if (variableMath == "∞")
			{
				if (v_AcceptUndefined == "No")
					throw new DivideByZeroException("Expression calculation resulted in inifinty.");
				else
					typeof(Nullable).StoreInUserVariable(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
			}
			else
			{
				//remove thousandths markers
				if (!string.IsNullOrEmpty(thousandSeparator))
					variableMath = variableMath.Replace(thousandSeparator, "");

				//check decimal seperator
				if (decimalSeparator != "." && !string.IsNullOrEmpty(decimalSeparator))
					variableMath = variableMath.Replace(decimalSeparator, ".");

				//perform compute
				OBDataTable dt = new OBDataTable();
				string result = dt.Compute(variableMath, "").ToString();

				//restore thousandths markers
				if (!string.IsNullOrEmpty(thousandSeparator))
				{
					result = double.Parse(result).ToString("N");
					result = result.Replace(",", thousandSeparator);
				}

				//restore decimal seperator
				if (decimalSeparator != "." && !string.IsNullOrEmpty(decimalSeparator))
				{
					var lastIndex = result.LastIndexOf(".");

					if (lastIndex != -1)
						result = result.Remove(lastIndex, 1).Insert(lastIndex, decimalSeparator);
				}

				//store string in variable
				result.StoreInUserVariable(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
			}

		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			//create standard group controls
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_MathExpression", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_ThousandSeparator", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_DecimalSeparator", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_AcceptUndefined", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Compute '{v_MathExpression}' - Store Result in '{v_OutputUserVariableName}']";
		}
	}
}
