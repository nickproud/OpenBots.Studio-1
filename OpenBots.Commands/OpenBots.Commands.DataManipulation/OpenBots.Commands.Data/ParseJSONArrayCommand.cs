using Newtonsoft.Json.Linq;
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
	[Description("This command parses a JSON array into a list.")]
	public class ParseJSONArrayCommand : ScriptCommand
	{
		[Required]
		[DisplayName("JSON")]
		[Description("Provide a variable or JSON array value.")]
		[SampleUsage("@\"['Small','Medium','Large']\" || vArrayVariable")]
		[Remarks("Providing data of a type other than a 'JSON Array' will result in an error.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_JsonArrayName { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output List Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("vUserVariable")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(List<string>) })]
		public string v_OutputUserVariableName { get; set; }

		public ParseJSONArrayCommand()
		{
			CommandName = "ParseJSONArrayCommand";
			SelectionName = "Parse JSON Array";
			CommandEnabled = true;
			CommandIcon = Resources.command_parse;

		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var variableInput = (string)await v_JsonArrayName.EvaluateCode(engine);

			//create objects
			JArray arr;
			List<string> resultList = new List<string>();

			//parse json
			try
			{
				arr = JArray.Parse(variableInput);
			}
			catch (Exception ex)
			{
				throw new Exception("Error Occured Selecting Tokens: " + ex.ToString());
			}
 
			//add results to result list since list<string> is supported
			foreach (var result in arr)
				resultList.Add(result.ToString());

			resultList.SetVariableValue(engine, v_OutputUserVariableName);           
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_JsonArrayName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Parse '{v_JsonArrayName}' - Store List in '{v_OutputUserVariableName}']";
		}
	}
}