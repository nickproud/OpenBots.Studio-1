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

namespace OpenBots.Commands.DataTable
{
	[Serializable]
	[Category("DataTable Commands")]
	[Description("This command gets a DataRow from a DataTable.")]
	public class GetDataRowCommand : ScriptCommand
	{
		[Required]
		[DisplayName("DataTable")]
		[Description("Enter an existing DataTable to get rows from.")]
		[SampleUsage("vDataTable")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(OBDataTable) })]
		public string v_DataTable { get; set; }

		[Required]
		[DisplayName("DataRow Index")]
		[Description("Enter a valid DataRow index value.")]
		[SampleUsage("0 || vIndex")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(int) })]
		public string v_DataRowIndex { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output DataRow Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("vUserVariable")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(DataRow) })]
		public string v_OutputUserVariableName { get; set; }

		public GetDataRowCommand()
		{
			CommandName = "GetDataRowCommand";
			SelectionName = "Get DataRow";
			CommandEnabled = true;
			CommandIcon = Resources.command_spreadsheet;
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			OBDataTable dataTable = (OBDataTable)await v_DataTable.EvaluateCode(engine);
			int index = (int)await v_DataRowIndex.EvaluateCode(engine);

			DataRow row = dataTable.Rows[index];

			row.SetVariableValue(engine, v_OutputUserVariableName);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_DataTable", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_DataRowIndex", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Get Row '{v_DataRowIndex}' From '{v_DataTable}' - Store DataRow in '{v_OutputUserVariableName}']";
		}        
	}
}