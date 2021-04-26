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
	[Description("This command merges a source DataTable into a destination DataTable.")]

	public class MergeDataTableCommand : ScriptCommand
	{

		[Required]
		[DisplayName("Source DataTable")]
		[Description("Enter an existing DataTable to merge into another one.")]
		[SampleUsage("vSrcDataTable")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(OBDataTable) })]
		public string v_SourceDataTable { get; set; }

		[Required]
		[DisplayName("Destination DataTable")]
		[Description("Enter an existing DataTable to apply the merge operation to.")]
		[SampleUsage("vDestDataTable")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(OBDataTable) })]
		public string v_DestinationDataTable { get; set; }

		[Required]
		[DisplayName("Missing Schema Action")]
		[PropertyUISelectionOption("Add")]
		[PropertyUISelectionOption("AddWithKey")]
		[PropertyUISelectionOption("Error")]
		[PropertyUISelectionOption("Ignore")]
		[Description("Select any Missing Schema Action.")]
		[SampleUsage("")]
		[Remarks("Specifies the action to take when adding data to the DataSet and the required DataTable or DataColumn is missing.")]
		public string v_MissingSchemaAction { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output DataTable Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("vUserVariable")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(OBDataTable) })]
		public string v_OutputUserVariableName { get; set; }

		public MergeDataTableCommand()
		{
			CommandName = "MergeDataTableCommand";
			SelectionName = "Merge DataTable";
			CommandEnabled = true;
			CommandIcon = Resources.command_spreadsheet;

			v_MissingSchemaAction = "Add";
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;

			// Get Variable Objects
			var SourceDTVariable = (OBDataTable)await v_SourceDataTable.EvaluateCode(engine);
			var DestinationDTVariable = (OBDataTable)await v_DestinationDataTable.EvaluateCode(engine);

			// Same Variable Check
			if (v_SourceDataTable != v_DestinationDataTable)
			{
				var sourceDT = SourceDTVariable;
				var destinationDT = DestinationDTVariable;

				switch (v_MissingSchemaAction)
				{
					case "Add":
						destinationDT.Merge(sourceDT, false, MissingSchemaAction.Add);
						break;
					case "AddWithKey":
						destinationDT.Merge(sourceDT, false, MissingSchemaAction.AddWithKey);
						break;
					case "Error":
						destinationDT.Merge(sourceDT, false, MissingSchemaAction.Error);
						break;
					case "Ignore":
						destinationDT.Merge(sourceDT, false, MissingSchemaAction.Ignore);
						break;
					default:
						throw new NotImplementedException("Missing Schema Action '" + v_MissingSchemaAction + "' not implemented");
				}

				// Update Destination Variable Value
				destinationDT.SetVariableValue(engine, v_OutputUserVariableName);               
			}
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SourceDataTable", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_DestinationDataTable", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_MissingSchemaAction", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Merge Source '{v_SourceDataTable}' Into Destination '{v_DestinationDataTable}' - Store DataTable in '{v_OutputUserVariableName}']";
		}       
	}
}