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
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using OBDataTable = System.Data.DataTable;

namespace OpenBots.Commands.Dictionary
{
    [Serializable]
	[Category("Dictionary Commands")]
	[Description("This command creates a new Dictionary.")]
	public class CreateDictionaryCommand : ScriptCommand
	{
		[DisplayName("Keys and Values (Optional)")]
		[Description("Enter the Keys and Values required for the new dictionary.")]
		[SampleUsage("[ \"FirstName\" | \"John\" ] || [ vKey | vValue ]")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(object) })]
		public OBDataTable v_ColumnNameDataTable { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output Dictionary Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("vUserVariable")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(Dictionary<,>) })]
		public string v_OutputUserVariableName { get; set; }

		public CreateDictionaryCommand()
		{
			CommandName = "CreateDictionaryCommand";
			SelectionName = "Create Dictionary";
			CommandEnabled = true;
			CommandIcon = Resources.command_dictionary;

			//initialize Datatable
			v_ColumnNameDataTable = new OBDataTable
			{
				TableName = "ColumnNamesDataTable" + DateTime.Now.ToString("MMddyy.hhmmss")
			};

			v_ColumnNameDataTable.Columns.Add("Keys");
			v_ColumnNameDataTable.Columns.Add("Values");
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			string dictTypeName = v_OutputUserVariableName.GetVarArgType(engine).GetRealTypeName();
			dynamic dynamicDict = await $"new {dictTypeName}()".EvaluateCode(engine);

			foreach (DataRow rwColumnName in v_ColumnNameDataTable.Rows)
			{
				dynamic dynamicKey = await rwColumnName.Field<string>("Keys").EvaluateCode(engine);
				dynamic dynamicValue = await rwColumnName.Field<string>("Values").EvaluateCode(engine);

				dynamicDict.Add(dynamicKey, dynamicValue);
			}

			((object)dynamicDict).SetVariableValue(engine, v_OutputUserVariableName);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultDataGridViewGroupFor("v_ColumnNameDataTable", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [With {v_ColumnNameDataTable.Rows.Count} Entries - Store Dictionary in '{v_OutputUserVariableName}']";
		}
	}
}