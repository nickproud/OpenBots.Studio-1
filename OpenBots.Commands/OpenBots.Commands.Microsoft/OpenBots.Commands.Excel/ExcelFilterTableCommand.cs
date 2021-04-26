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
    [Description("This command filters values from a column in an Excel Table.")]
    public class ExcelFilterTableCommand : ScriptCommand
    {
        [Required]
        [DisplayName("Excel Instance Name")]
        [Description("Enter the unique instance that was specified in the **Create Application** command.")]
        [SampleUsage("MyExcelInstance")]
        [Remarks("Failure to enter the correct instance or failure to first call the **Create Application** command will cause an error.")]
        [CompatibleTypes(new Type[] { typeof(Application) })]
        public string v_InstanceName { get; set; }

        [Required]
        [DisplayName("Filter List")]
        [Description("Specify a List containing the values to filter from a selected column.")]
        [SampleUsage("vFilterList")]
        [Remarks("The filter List must be of type string.")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(List<string>) })]
        public string v_FilterList { get; set; }

        [Required]
        [DisplayName("Column Search Option")]
        [PropertyUISelectionOption("Column Name")]
        [PropertyUISelectionOption("Column Index")]
        [Description("Select whether the Table should be filtered by Column Index or Column Name.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_Option { get; set; }

        [Required]
        [DisplayName("Column Search Value")]
        [Description("Enter a valid column index or column name.")]
        [SampleUsage("1 || vIndex || \"Column1\" || vColumnName")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(string), typeof(int)})]
        public string v_DataValueIndex { get; set; }

        [Required]
        [DisplayName("Excel Table Name")]
        [Description("Enter the name of the Excel Table to filter.")]
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

        public ExcelFilterTableCommand()
        {
            CommandName = "ExcelFilterTableCommand";
            SelectionName = "Filter Excel Table";
            CommandEnabled = true;
            CommandIcon = Resources.command_spreadsheet;

            v_InstanceName = "DefaultExcel";
            v_Option = "Column Index";
        }

        public async override Task RunCommand(object sender)
        {
            var engine = (IAutomationEngineInstance)sender;
            string vSheetExcelTable = (string)await v_SheetNameExcelTable.EvaluateCode(engine);
            var vTableName = (string)await v_TableName.EvaluateCode(engine);
            dynamic vColumnValue = await v_DataValueIndex.EvaluateCode(engine);
            var filterArray = ((List<string>)await v_FilterList.EvaluateCode(engine)).ToArray();
            var excelObject = v_InstanceName.GetAppInstance(engine);
            var excelInstance = (Application)excelObject;
            var workSheetExcelTable = excelInstance.Sheets[vSheetExcelTable] as Worksheet;
            var excelTable = workSheetExcelTable.ListObjects[vTableName];

            int index;
            if (v_Option == "Column Index")
                index = (int)vColumnValue;
            else
                index = excelTable.ListColumns[(string)vColumnValue].Index;

            //Filter the Excel Table
            if (filterArray.Length <= 0)
            {
                excelTable.Range.AutoFilter(index, Type.Missing, XlAutoFilterOperator.xlAnd, Type.Missing, Type.Missing);
            }
            else
            {
                excelTable.Range.AutoFilter(index, filterArray, XlAutoFilterOperator.xlFilterValues, Type.Missing, Type.Missing);
            }
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_FilterList", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_Option", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_DataValueIndex", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_TableName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SheetNameExcelTable", this, editor));

            return RenderedControls;
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Filter '{v_TableName}' by '{v_FilterList}' - Instance Name '{v_InstanceName}']";
        }
    }
}
