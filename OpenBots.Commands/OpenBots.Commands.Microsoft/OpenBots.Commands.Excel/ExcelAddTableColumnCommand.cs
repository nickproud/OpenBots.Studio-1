using Microsoft.Office.Interop.Excel;
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
using System.Windows.Forms;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace OpenBots.Commands.Microsoft
{
    [Serializable]
    [Category("Excel Commands")]
    [Description("This command adds a column to an Excel Table.")]
    public class ExcelAddTableColumnCommand : ScriptCommand
    {
        [Required]
        [DisplayName("Excel Instance Name")]
        [Description("Enter the unique instance that was specified in the **Create Application** command.")]
        [SampleUsage("MyExcelInstance")]
        [Remarks("Failure to enter the correct instance or failure to first call the **Create Application** command will cause an error.")]
        public string v_InstanceName { get; set; }

        [Required]
        [DisplayName("Column Index")]
        [Description("Enter the index for column to be added at.")]
        [SampleUsage("1 || {vIndex}")]
        [Remarks("The column will be added at the last index if a column index is not provided.")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        public string v_ColumnIndex { get; set; }

        [Required]
        [DisplayName("Column Name")]
        [Description("Enter the name of the column to be added.")]
        [SampleUsage("Column1 || {vColumnName}")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        public string v_ColumnName { get; set; }

        [Required]
        [DisplayName("Excel Table Name")]
        [Description("Enter the name of the existing Excel Table.")]
        [SampleUsage("TableName || {vTableName}")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        public string v_TableName { get; set; }

        [Required]
        [DisplayName("Worksheet Name")]
        [Description("Enter the name of the Worksheet containing the existing Excel Table.")]
        [SampleUsage("Sheet1 || {vSheet}")]
        [Remarks("An error will be thrown in the case of an invalid Worksheet Name.")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        public string v_SheetNameExcelTable { get; set; }

        public ExcelAddTableColumnCommand()
        {
            CommandName = "ExcelAddTableColumnCommand";
            SelectionName = "Add Table Column";
            CommandEnabled = true;
            CommandIcon = Resources.command_spreadsheet;

            v_InstanceName = "DefaultExcel";
        }

        public override void RunCommand(object sender)
        {
            var engine = (IAutomationEngineInstance)sender;
            string vSheetExcelTable = v_SheetNameExcelTable.ConvertUserVariableToString(engine);
            var vColumnIndex = v_ColumnIndex.ConvertUserVariableToString(engine);
            var vTableName = v_TableName.ConvertUserVariableToString(engine);
            var vColumnName = v_ColumnName.ConvertUserVariableToString(engine);
            var excelObject = v_InstanceName.GetAppInstance(engine);
            var excelInstance = (Application)excelObject;
            var workSheetExcelTable = excelInstance.Sheets[vSheetExcelTable] as Worksheet;
            var excelTable = workSheetExcelTable.ListObjects[vTableName];

            var excelColumn = excelTable.ListColumns.Add(String.IsNullOrEmpty(vColumnIndex) ? excelTable.ListColumns.Count + 1 : int.Parse(vColumnIndex));
            excelColumn.Name = vColumnName;
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_ColumnIndex", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_ColumnName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_TableName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SheetNameExcelTable", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Add Column '{v_ColumnName}' to Table '{v_TableName}' at Index '{v_ColumnIndex}' - Instance Name '{v_InstanceName}']";
        }
    }
}
