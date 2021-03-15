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
using System.IO;
using System.Windows.Forms;

namespace OpenBots.Commands.Data
{
	[Serializable]
	[Category("Data Commands")]
	[Description("This command converts a date to a specified format and saves the result in a variable.")]
	public class FormatDateCommand : ScriptCommand
	{        
		[Required]
		[DisplayName("Input Date")]
		[Description("Specify either text or a variable that contains a date requiring formatting.")]
		[SampleUsage("1/1/2000 || {vDate} || {DateTime.Now}")]
		[Remarks("Utilize the *Create DateTime* command to provide a DateTime variable")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(DateTime), typeof(string) }, true)]
		public string v_InputData { get; set; }

		[Required]
		[DisplayName("Date Format")]
		[Description("Specify the output data format.")]
		[SampleUsage("MM/dd/yy, hh:mm:ss || {vDateFormat}")]
		[Remarks("You should specify a valid input data format; invalid formats will result in an error.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(null, true)]
		public string v_ToStringFormat { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output Text Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("{vUserVariable}")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_OutputUserVariableName { get; set; }

		public FormatDateCommand()
		{
			CommandName = "FormatDateCommand";
			SelectionName = "Format Date";
			CommandEnabled = true;
			CommandIcon = Resources.command_stopwatch;

			v_InputData = "{DateTime.Now}";
			v_ToStringFormat = "MM/dd/yyyy";
		}

		public override void RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var formatting = v_ToStringFormat.ConvertUserVariableToString(engine);

			dynamic input = v_InputData.ConvertUserVariableToString(engine);

			if (input == v_InputData && input.StartsWith("{") && input.EndsWith("}"))
				input = v_InputData.ConvertUserVariableToObject(engine, nameof(v_InputData), this);

			DateTime variableDate;

			if (input is DateTime)
				variableDate = (DateTime)input;
			else if (input is string)
				variableDate = DateTime.Parse((string)input);
			else
				throw new InvalidDataException($"{v_InputData} is not a valid DateTime");

			string formattedString  = variableDate.ToString(formatting);
				
			formattedString.StoreInUserVariable(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			//create standard group controls
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InputData", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_ToStringFormat", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" ['{v_InputData}' as '{v_ToStringFormat}' - Store Text in '{v_OutputUserVariableName}']";
		}
	}
}