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

namespace OpenBots.Commands.Microsoft
{
    [Serializable]
    [Category("Excel Commands")]
    [Description("This command sorts an Excel Table by the ascending or descending order of a specified column.")]
    public class ExcelSortTableCommand : ScriptCommand
    {
        [Required]
        [DisplayName("Excel Instance Name")]
        [Description("Enter the unique instance that was specified in the **Create Application** command.")]
        [SampleUsage("MyExcelInstance")]
        [Remarks("Failure to enter the correct instance or failure to first call the **Create Application** command will cause an error.")]
        [CompatibleTypes(new Type[] { typeof(Application) })]
        public string v_InstanceName { get; set; }

        [Required]
        [DisplayName("Sort Type")]
        [PropertyUISelectionOption("Ascending")]
        [PropertyUISelectionOption("Descending")]
        [Description("Select whether the Table should be sorted by ascending or descending order.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_SortType { get; set; }

        [Required]
        [DisplayName("Column Search Option")]
        [PropertyUISelectionOption("Column Name")]
        [PropertyUISelectionOption("Column Index")]
        [Description("Select whether the column used to sort the Excel Table should be referenced by index or name.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_Option { get; set; }

        [Required]
        [DisplayName("Column Search Value")]
        [Description("Enter a valid column index or column name.")]
        [SampleUsage("0 || vIndex || \"Column1\" || vColumnName")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(string), typeof(int) })]
        public string v_DataValueIndex { get; set; }

        [Required]
        [DisplayName("Excel Table Name")]
        [Description("Enter the name of the Excel Table to sort.")]
        [SampleUsage("\"TableName\" || vTableName")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(string) })]
        public string v_TableName { get; set; }

        [Required]
        [DisplayName("Worksheet Name")]
        [Description("Enter the name of the Worksheet containing the existing Excel Table.")]
        [SampleUsage("\"Sheet1\" || vSheet")]
        [Remarks("An error will be thrown in the case of an invalid Worksheet Name.")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(string) })]
        public string v_SheetNameExcelTable { get; set; }

        public ExcelSortTableCommand()
        {
            CommandName = "ExcelSortTableCommand";
            SelectionName = "Sort Excel Table";
            CommandEnabled = true;
            CommandIcon = Resources.command_spreadsheet;

            v_InstanceName = "DefaultExcel";
            v_Option = "Column Index";
            v_SortType = "Ascending";
        }

        public async override Task RunCommand(object sender)
        {
            var engine = (IAutomationEngineInstance)sender;
            string vSheetExcelTable = (string)await v_SheetNameExcelTable.EvaluateCode(engine);
            var vTableName = (string)await v_TableName.EvaluateCode(engine);
            var vColumnValue = await v_DataValueIndex.EvaluateCode(engine);
            var excelObject = v_InstanceName.GetAppInstance(engine);
            var excelInstance = (Application)excelObject;
            var workSheetExcelTable = excelInstance.Sheets[vSheetExcelTable] as Worksheet;
            var excelTable = workSheetExcelTable.ListObjects[vTableName];

            ListColumn column;
            if (v_Option == "Column Index")
                column = excelTable.ListColumns[(int)vColumnValue];
            else
                column = excelTable.ListColumns[(string)vColumnValue];

            XlSortOrder sortType; 
            if (v_SortType == "Ascending")
                sortType = XlSortOrder.xlAscending;
            else
                sortType = XlSortOrder.xlDescending;

            //Sort the Excel Table
            excelTable.Range.Sort(column, sortType, Type.Missing, Type.Missing, sortType, Type.Missing, sortType, XlYesNoGuess.xlYes, 
                                  Type.Missing, Type.Missing, XlSortOrientation.xlSortColumns, XlSortMethod.xlPinYin, 
                                  XlSortDataOption.xlSortNormal, XlSortDataOption.xlSortNormal, XlSortDataOption.xlSortNormal);
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_SortType", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_Option", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_DataValueIndex", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_TableName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SheetNameExcelTable", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Sort '{v_TableName}' by '{v_SortType}' - Instance Name '{v_InstanceName}']";
        }
    }
}
