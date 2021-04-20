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
	[Description("This command creates a new List variable.")]
	public class CreateListCommand : ScriptCommand
	{
		[DisplayName("List Item(s) (Optional)")]
		[Description("Enter the item(s) to write to the List.")]
		[SampleUsage("Hello || {vItem} || Hello,World || {vItem1},{vItem2}")]
		[Remarks("Multiple items should be delimited by a comma(,). This input is optional.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(object) })]
		public string v_ListItems { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output List Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("{vUserVariable}")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(List<>) })]
		public string v_OutputUserVariableName { get; set; }

		public CreateListCommand()
		{
			CommandName = "CreateListCommand";
			SelectionName = "Create List";
			CommandEnabled = true;
			CommandIcon = Resources.command_function;
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			dynamic dynamicNewList;
			string[] splitListItems = null;

			if (!string.IsNullOrEmpty(v_ListItems))
				splitListItems = v_ListItems.Split(',');

			string listTypeName = v_OutputUserVariableName.GetVarArgType(engine).GetRealTypeName();
			dynamicNewList = await $"new {listTypeName}()".EvaluateCode(engine);

			foreach (string item in splitListItems)
            {
				dynamic dynamicItem = await item.EvaluateCode(engine);
				dynamicNewList.Add(dynamicItem);
			}
				
			((object)dynamicNewList).SetVariableValue(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_ListItems", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Create New List With Item(s) '{v_ListItems}' - Store List in '{v_OutputUserVariableName}']";
		}
	}
}