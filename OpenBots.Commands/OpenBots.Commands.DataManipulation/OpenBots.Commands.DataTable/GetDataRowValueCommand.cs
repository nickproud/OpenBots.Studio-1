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
using System.Windows.Forms;

namespace OpenBots.Commands.DataTable
{
	[Serializable]
	[Category("DataTable Commands")]
	[Description("This command gets a DataRow Value from a DataTable.")]

	public class GetDataRowValueCommand : ScriptCommand
	{

		[Required]
		[DisplayName("DataRow")]
		[Description("Enter an existing DataRow to get Values from.")]
		[SampleUsage("{vDataRow}")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(DataRow) })]
		public string v_DataRow { get; set; }

		[Required]
		[DisplayName("Search Option")]
		[PropertyUISelectionOption("Column Name")]
		[PropertyUISelectionOption("Column Index")]
		[Description("Select whether the DataRow value should be found by column index or column name.")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_Option { get; set; }

		[Required]
		[DisplayName("Search Value")]
		[Description("Enter a valid DataRow index or column name.")]
		[SampleUsage("0 || {vIndex} || Column1 || {vColumnName}")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(null, true)]
		public string v_DataValueIndex { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output Value Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("{vUserVariable}")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_OutputUserVariableName { get; set; }

		public GetDataRowValueCommand()
		{
			CommandName = "GetDataRowValueCommand";
			SelectionName = "Get DataRow Value";
			CommandEnabled = true;
			CommandIcon = Resources.command_spreadsheet;

			v_Option = "Column Index";
		}

		public override void RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var dataRowVariable = v_DataRow.ConvertUserVariableToObject(engine, nameof(v_DataRow), this);
			DataRow dataRow = (DataRow)dataRowVariable;

			var valueIndex = v_DataValueIndex.ConvertUserVariableToString(engine);
			string value = "";
			if (v_Option == "Column Index")
			{
				int index = int.Parse(valueIndex);
				value = dataRow[index].ToString();

			}
			else if (v_Option == "Column Name")
			{
				string index = valueIndex;
				value = dataRow.Field<string>(index);
			}

			value.StoreInUserVariable(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_DataRow", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_Option", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_DataValueIndex", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Get Value From Column '{v_DataValueIndex}' in '{v_DataRow}' - Store Value in '{v_OutputUserVariableName}']";
		}        
	}
}