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
using System.Threading.Tasks;
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
        [CompatibleTypes(new Type[] { typeof(Application) })]
        public string v_InstanceName { get; set; }

        [Required]
        [DisplayName("Excel Table Worksheet")]
        [Description("Enter the name of the Worksheet containing the Excel Table being used to create the Pivot Table.")]
        [SampleUsage("\"Sheet1\" || vSheet")]
        [Remarks("An error will be thrown in the case of an invalid Worksheet Name.")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(string) })]
        public string v_SheetNameExcelTable { get; set; }

        [Required]
        [DisplayName("Excel Table Name")]
        [Description("Enter the name of the Excel Table to extract data from for the Pivot Table.")]
        [SampleUsage("\"Table\" || vTable")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(string) })]
        public string v_TableName { get; set; }

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
        [Description("Enter the name of Pivot Table to be created.")]
        [SampleUsage("\"PivotTable\" || vPivotTable")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(string) })]
        public string v_PivotTable { get; set; }

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
            CommandIcon = Resources.command_spreadsheet;

            v_InstanceName = "DefaultExcel";
        }

        public async override Task RunCommand(object sender)
        {
            var engine = (IAutomationEngineInstance)sender;
            string vSheetExcelTable = (string)await v_SheetNameExcelTable.EvaluateCode(engine);
            string vSheetPivotTable = (string)await v_SheetNamePivotTable.EvaluateCode(engine);
            var vTableName = (string)await v_TableName.EvaluateCode(engine);
            var vCellLocation = (string)await v_CellLocation.EvaluateCode(engine);
            var vPivotTable = (string)await v_PivotTable.EvaluateCode(engine);
            var excelObject = v_InstanceName.GetAppInstance(engine);
            var excelInstance = (Application)excelObject;
            var workBook = excelInstance.ActiveWorkbook;
            var workSheetExcelTable = excelInstance.Sheets[vSheetExcelTable] as Worksheet;
            var workSheetPivotTable = excelInstance.Sheets[vSheetPivotTable] as Worksheet;
            var excelTable = workSheetExcelTable.ListObjects[vTableName];

            object useDefault = Type.Missing;
            Range pivotDestination = workSheetPivotTable.Range[vCellLocation, useDefault];
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
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SheetNameExcelTable", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_TableName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SheetNamePivotTable", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_PivotTable", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_CellLocation", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Create '{v_PivotTable}' From '{v_TableName}' - Instance Name  '{v_InstanceName}']";
        }
    }
}
