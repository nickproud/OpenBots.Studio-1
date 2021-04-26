using Microsoft.Office.Interop.Excel;
using OpenBots.Commands.Microsoft.Library;
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
    [Description("This command creates an Excel Table from a provided range of data.")]
    public class ExcelCreateTableCommand : ScriptCommand
    {
        [Required]
        [DisplayName("Excel Instance Name")]
        [Description("Enter the unique instance that was specified in the **Create Application** command.")]
        [SampleUsage("MyExcelInstance")]
        [Remarks("Failure to enter the correct instance or failure to first call the **Create Application** command will cause an error.")]
        [CompatibleTypes(new Type[] { typeof(Application) })]
        public string v_InstanceName { get; set; }

        [Required]
        [DisplayName("Range")]
        [Description("Enter the Range to extract data from for the creation of an Excel Table.")]
        [SampleUsage("\"A1:B10\" || \"A1:\" || vRange || vStart + \":\" + vEnd || vStart + \":\"")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(string) })]
        public string v_Range { get; set; }

        [Required]
        [DisplayName("Excel Table Name")]
        [Description("Enter the name of the Excel Table to be created.")]
        [SampleUsage("\"TableName\" || vTableName")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(string) })]
        public string v_TableName { get; set; }

        [Required]
        [DisplayName("Range Worksheet Name")]
        [Description("Enter the name of the Worksheet containing the Range being used to create the Excel Table.")]
        [SampleUsage("\"Sheet1\" || vSheet")]
        [Remarks("An error will be thrown in the case of an invalid Worksheet Name.")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(string) })]
        public string v_SheetNameExcelTable { get; set; }

        public ExcelCreateTableCommand()
        {
            CommandName = "ExcelCreateTableCommand";
            SelectionName = "Create Excel Table";
            CommandEnabled = true;
            CommandIcon = Resources.command_spreadsheet;

            v_InstanceName = "DefaultExcel";
        }

        public async override Task RunCommand(object sender)
        {
            var engine = (IAutomationEngineInstance)sender;
            string vSheetExcelTable = (string)await v_SheetNameExcelTable.EvaluateCode(engine);
            var vRange = (string)await v_Range.EvaluateCode(engine);
            var vTableName = (string)await v_TableName.EvaluateCode(engine);
            var excelObject = v_InstanceName.GetAppInstance(engine);
            var excelInstance = (Application)excelObject;
            var workSheetExcelTable = excelInstance.Sheets[vSheetExcelTable] as Worksheet;

            //Extract a range of cells
            var splitRange = vRange.Split(':');
            Range cellRange;
            Range sourceRange = workSheetExcelTable.UsedRange;

            if (splitRange[1] == "")
            {
                var cell = excelInstance.GetLastIndexOfNonEmptyCell(sourceRange, sourceRange.Range["A1"]);
                if (cell == "")
                    throw new Exception("No data found in sheet.");
                cellRange = workSheetExcelTable.Range[splitRange[0], cell];
            }
            else
            {
                cellRange = workSheetExcelTable.Range[splitRange[0], splitRange[1]];
            }

            workSheetExcelTable.ListObjects.Add(XlListObjectSourceType.xlSrcRange, cellRange,
                Type.Missing, XlYesNoGuess.xlYes, Type.Missing).Name = vTableName;
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Range", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_TableName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SheetNameExcelTable", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Create '{v_TableName}' From Range '{v_Range}' - Instance Name '{v_InstanceName}']";
        }
    }
}
