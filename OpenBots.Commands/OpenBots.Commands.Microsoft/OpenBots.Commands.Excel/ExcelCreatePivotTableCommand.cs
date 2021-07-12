using Microsoft.Office.Interop.Excel;
using OpenBots.Commands.Microsoft.Library;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Interfaces;
using OpenBots.Core.Model.ApplicationModel;
using OpenBots.Core.Properties;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Windows.Forms;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace OpenBots.Commands.Excel
{
    [Serializable]
    [Category("Excel Commands")]
    [Description("This command creates a Pivot Table from an Excel Range or Table.")]
    public class ExcelCreatePivotTableCommand : ScriptCommand
    {
        [Required]
        [DisplayName("Excel Instance Name")]
        [Description("Enter the unique instance that was specified in the **Create Application** command.")]
        [SampleUsage("MyExcelInstance")]
        [Remarks("Failure to enter the correct instance or failure to first call the **Create Application** command will cause an error.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(OBAppInstance) })]
        public string v_InstanceName { get; set; }

        [Required]
        [DisplayName("Excel Data Source Worksheet")]
        [Description("Enter the name of the Worksheet containing the Excel Table/Range being used to create the Pivot Table.")]
        [SampleUsage("\"Sheet1\" || vSheet")]
        [Remarks("An error will be thrown in the case of an invalid Worksheet Name.")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(string) })]
        public string v_SheetNameDataSource { get; set; }

        [Required]
        [DisplayName("Excel Data Source Type")]
        [PropertyUISelectionOption("Range")]
        [PropertyUISelectionOption("Table")]
        [Description("Indicate whether to the data source of the Pivot Table should come from an Excel Table or Range.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_DataSourceType { get; set; }

        [Required]
        [DisplayName("Excel Range/Table Name")]
        [Description("Enter the Range or the name of the Excel Table to extract data from for the Pivot Table.")]
        [SampleUsage("\"A1:\" || \"A1:B5\" || \"MyTable\" || vData")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(string) })]
        public string v_DataSource { get; set; }

        [Required]
        [DisplayName("Pivot Table Worksheet")]
        [Description("Enter the name of the Worksheet where the Pivot Table will be set.")]
        [SampleUsage("\"Sheet1\" || vSheet")]
        [Remarks("An error will be thrown in the case of an invalid Worksheet Name.")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(string) })]
        public string v_SheetNamePivotTable { get; set; }

        [Required]
        [DisplayName("Pivot Table Name")]
        [Description("Enter the name of the new Pivot Table to be created.")]
        [SampleUsage("\"PivotTable\" || vPivotTable")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(string) })]
        public string v_PivotTableName { get; set; }

        [Required]
        [DisplayName("Cell Location")]
        [Description("Enter the location where the Pivot Table will be set.")]
        [SampleUsage("\"A1\" || vCellLocation")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(string) })]
        public string v_CellLocation { get; set; }

        public ExcelCreatePivotTableCommand()
        {
            CommandName = "ExcelCreatePivotTableCommand";
            SelectionName = "Create Pivot Table";
            CommandEnabled = true;
            CommandIcon = Resources.command_excel;

            v_DataSourceType = "Range";
        }

        public async override Task RunCommand(object sender)
        {
            var engine = (IAutomationEngineInstance)sender;
            string vSheetExcelTable = (string)await v_SheetNameDataSource.EvaluateCode(engine);
            string vSheetPivotTable = (string)await v_SheetNamePivotTable.EvaluateCode(engine);
            var vTableNameOrRange = (string)await v_DataSource.EvaluateCode(engine);
            var vCellLocation = (string)await v_CellLocation.EvaluateCode(engine);
            var vPivotTableName = (string)await v_PivotTableName.EvaluateCode(engine);
            var excelObject = ((OBAppInstance)await v_InstanceName.EvaluateCode(engine)).Value;

            var excelInstance = (Application)excelObject;
            var workBook = excelInstance.ActiveWorkbook;
            var workSheetExcelTable = excelInstance.Sheets[vSheetExcelTable] as Worksheet;
            var workSheetPivotTable = excelInstance.Sheets[vSheetPivotTable] as Worksheet;

            Range pivotDestination = workSheetPivotTable.Range[vCellLocation, Type.Missing];
            PivotField pivotField;
            PivotTable pivotTable;
            PivotCache pivotCache;
            int columnCount; 

            if (v_DataSourceType == "Table")
            {
                var excelTable = workSheetExcelTable.ListObjects[vTableNameOrRange];          
                pivotCache = workBook.PivotCaches().Create(XlPivotTableSourceType.xlDatabase, vTableNameOrRange, Type.Missing);
                columnCount = excelTable.ListColumns.Count;             
            }
            else
            {
                Range cellRange = excelInstance.GetRange(vTableNameOrRange, workSheetExcelTable);
                pivotCache = workBook.PivotCaches().Create(XlPivotTableSourceType.xlDatabase, cellRange, Type.Missing);
                columnCount = cellRange.Columns.Count;        
            }

            pivotTable = pivotCache.CreatePivotTable(pivotDestination, vPivotTableName, Type.Missing, Type.Missing);

            for (int i = 1; i <= columnCount; i++)
            {
                pivotField = pivotTable.PivotFields(i);
                if (XlPivotFieldDataType.xlText != pivotField.DataType)
                    pivotTable.AddDataField(pivotTable.PivotFields(i), Type.Missing, Type.Missing);
                else
                    pivotTable.PivotFields(i).Orientation = XlPivotFieldOrientation.xlRowField;
            }

            pivotTable.RefreshTable();
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SheetNameDataSource", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_DataSourceType", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_DataSource", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SheetNamePivotTable", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_PivotTableName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_CellLocation", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Create '{v_PivotTableName}' From '{v_DataSource}' - Instance Name  '{v_InstanceName}']";
        }
    }
}
