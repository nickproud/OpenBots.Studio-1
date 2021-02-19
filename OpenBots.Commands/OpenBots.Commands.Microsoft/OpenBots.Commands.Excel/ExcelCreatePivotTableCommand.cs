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

namespace OpenBots.Commands.Excel
{
    [Serializable]
    [Category("Excel Commands")]
    [Description("This command creates a Pivot Table.")]
    public class ExcelCreatePivotTableCommand : ScriptCommand
    {
        [Required]
        [DisplayName("Excel Instance Name")]
        [Description("Enter the unique instance that was specified in the **Create Application** command.")]
        [SampleUsage("MyExcelInstance")]
        [Remarks("Failure to enter the correct instance or failure to first call the **Create Application** command will cause an error.")]
        public string v_InstanceName { get; set; }

        [Required]
        [DisplayName("Worksheet Name")]
        [Description("Specify the Worksheet within the Workbook to extract Range and create Pivot Table.")]
        [SampleUsage("Sheet1 || {vSheet}")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        public string v_SheetName { get; set; }

        [Required]
        [DisplayName("Excel Table Name")]
        [Description("Enter the Excel Table name to extract data for Pivot Table.")]
        [SampleUsage("Table || {vTable}")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        public string v_TableName { get; set; }

        [Required]
        [DisplayName("Cell Location")]
        [Description("Enter the location where Pivot Table will be created.")]
        [SampleUsage("A1 || {vCellLocation}")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        public string v_CellLocation { get; set; }

        [Required]
        [DisplayName("Pivot Table Name")]
        [Description("Enter the name of Pivot Table to be created.")]
        [SampleUsage("PivotTable || {vPivotTable}")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        public string v_PivotTable { get; set; }

        public ExcelCreatePivotTableCommand()
        {
            CommandName = "ExcelCreatePivotTableCommand";
            SelectionName = "Create Pivot Table";
            CommandEnabled = true;
            CommandIcon = Resources.command_spreadsheet;

            v_InstanceName = "DefaultExcel";
        }

        public override void RunCommand(object sender)
        {
            var engine = (IAutomationEngineInstance)sender;
            string vSheet = v_SheetName.ConvertUserVariableToString(engine);
            var vTableName = v_TableName.ConvertUserVariableToString(engine);
            var vCellLocation = v_CellLocation.ConvertUserVariableToString(engine);
            var vPivotTable = v_PivotTable.ConvertUserVariableToString(engine);
            var excelObject = v_InstanceName.GetAppInstance(engine);
            var excelInstance = (Application)excelObject;
            var workBook = excelInstance.ActiveWorkbook;
            var workSheet = excelInstance.Sheets[vSheet] as Worksheet;
            var excelTable = workSheet.ListObjects[vTableName];

            object useDefault = Type.Missing;
            Range pivotDestination = workSheet.Range[vCellLocation, useDefault];
            var pivotCache = workBook.PivotCaches().Create(XlPivotTableSourceType.xlDatabase, vTableName, Type.Missing);
            PivotTable pivotTable = pivotCache.CreatePivotTable(pivotDestination, vPivotTable, Type.Missing, Type.Missing);
            
            PivotField pivotField;
            for (int i = 1; i <= excelTable.ListColumns.Count; i++)
            {
                pivotField = pivotTable.PivotFields(i);
                if (XlPivotFieldDataType.xlText != pivotField.DataType)
                {
                    pivotTable.AddDataField(pivotTable.PivotFields(i), Type.Missing, Type.Missing);
                }
                else
                {
                    pivotTable.PivotFields(i).Orientation = XlPivotFieldOrientation.xlRowField;
                }
            }
            pivotTable.RefreshTable();
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SheetName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_TableName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_CellLocation", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_PivotTable", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Create '{v_PivotTable}' From '{v_TableName}' - Instance Name  '{v_InstanceName}']";
        }
    }
}
