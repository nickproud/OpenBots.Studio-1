using Microsoft.Office.Interop.Outlook;
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
	[Description("This command adds an item to an existing List variable.")]
	public class AddListItemCommand : ScriptCommand
	{
		[Required]
		[DisplayName("List")]
		[Description("Provide a List variable.")]
		[SampleUsage("{vList}")]
		[Remarks("Any type of variable other than List will cause error.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(List<>) })]
		public string v_ListName { get; set; }

		[Required]
		[DisplayName("List Item")]
		[Description("Enter the item to add to the List.")]
		[SampleUsage("Hello || {vItem}")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(object) })]
		public string v_ListItem { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output List Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("{vUserVariable}")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(List<>) })]
		public string v_OutputUserVariableName { get; set; }

		public AddListItemCommand()
		{
			CommandName = "AddListItemCommand";
			SelectionName = "Add List Item";
			CommandEnabled = true;
			CommandIcon = Resources.command_function;
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;

			var vListVariable = await v_ListName.EvaluateCode(engine, nameof(v_ListName), this);
			var vListItem = await v_ListItem.EvaluateCode(engine);

			dynamic dynamicList = vListVariable;
			dynamic dynamicItem = vListItem;
			dynamicList.Add(dynamicItem);

			((object)dynamicList).SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_ListName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_ListItem", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Add Item '{v_ListItem}' to List '{v_ListName}' - Store List in '{v_OutputUserVariableName}']";
		}
	}
}
