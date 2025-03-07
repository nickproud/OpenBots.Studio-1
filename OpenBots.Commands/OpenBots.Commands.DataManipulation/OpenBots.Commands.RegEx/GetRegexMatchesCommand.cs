﻿using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Interfaces;
using OpenBots.Core.Properties;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenBots.Commands.RegEx
{
	[Serializable]
	[Category("Regex Commands")]
	[Description("This command gets all matches in a given text based on a Regex pattern.")]
	public class GetRegexMatchesCommand : ScriptCommand
	{
		[Required]
		[DisplayName("Text")]
		[Description("Select or provide text to apply Regex on.")]
		[SampleUsage("\"Hello\" || vText")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_InputText { get; set; }

		[Required]
		[DisplayName("Regex Pattern")]
		[Description("Enter a Regex Pattern to apply to the input Text.")]
		[SampleUsage("@\"^([\\w\\-]+)\" || vPattern")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_Regex { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output Match List Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("vUserVariable")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(List<string>) })]
		public string v_OutputUserVariableName { get; set; }

		public GetRegexMatchesCommand()
		{
			CommandName = "GetRegexMatchesCommand";
			SelectionName = "Get Regex Matches";
			CommandEnabled = true;
			CommandIcon = Resources.command_regex;
			v_Regex = "@\"\"";
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var vInputData = (string)await v_InputText.EvaluateCode(engine);
			string vRegex = (string)await v_Regex.EvaluateCode(engine);

			var matches = (from match in Regex.Matches(vInputData, vRegex).Cast<Match>() 
						   select match.Groups[0].Value).ToList();

			matches.SetVariableValue(engine, v_OutputUserVariableName);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InputText", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Regex", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Text '{v_InputText}' - Regex Pattern '{v_Regex}' - Store Match List in '{v_OutputUserVariableName}']";
		}
	}
}