using Microsoft.Office.Interop.Outlook;
using Newtonsoft.Json;
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

namespace OpenBots.Commands.List
{
    [Serializable]
	[Category("List Commands")]
	[Description("This command returns the count of items contained in a List.")]
	public class GetListCountCommand : ScriptCommand
	{
		[Required]
		[DisplayName("List")]
		[Description("Provide a List variable.")]
		[SampleUsage("vList || new List<string>() { \"hello\", \"world\" }")]
		[Remarks("Providing any type of variable other than a List will result in an error.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(List<>) })]
		public string v_ListName { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output Count Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("vUserVariable")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(int) })]
		public string v_OutputUserVariableName { get; set; }

		public GetListCountCommand()
		{
			CommandName = "GetListCountCommand";
			SelectionName = "Get List Count";
			CommandEnabled = true;
			CommandIcon = Resources.command_function;

		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;

			dynamic dynamicList = await v_ListName.EvaluateCode(engine);

			int count = dynamicList.Count;
			count.SetVariableValue(engine, v_OutputUserVariableName);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_ListName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [From '{v_ListName}' - Store Count in '{v_OutputUserVariableName}']";
		}       
	}
}