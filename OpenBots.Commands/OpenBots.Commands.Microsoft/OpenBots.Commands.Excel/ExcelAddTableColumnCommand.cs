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
    [Description("This command adds a column to an Excel Table.")]
    public class ExcelAddTableColumnCommand : ScriptCommand
    {
        [Required]
        [DisplayName("Excel Instance Name")]
        [Description("Enter the unique instance that was specified in the **Create Application** command.")]
        [SampleUsage("MyExcelInstance")]
        [Remarks("Failure to enter the correct instance or failure to first call the **Create Application** command will cause an error.")]
        [CompatibleTypes(new Type[] { typeof(Application) })]
        public string v_InstanceName { get; set; }

        [DisplayName("Column Index (Optional)")]
        [Description("Enter the index for column to be added at.")]
        [SampleUsage("1 || vIndex")]
        [Remarks("The column will be added at the last index if a column index is not provided.")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(int) })]
        public string v_ColumnIndex { get; set; }

        [Required]
        [DisplayName("Column Name")]
        [Description("Enter the name of the column to be added.")]
        [SampleUsage("\"Column1\" || vColumnName")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(string) })]
        public string v_ColumnName { get; set; }

        [Required]
        [DisplayName("Excel Table Name")]
        [Description("Enter the name of the existing Excel Table.")]
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

        public ExcelAddTableColumnCommand()
        {
            CommandName = "ExcelAddTableColumnCommand";
            SelectionName = "Add Table Column";
            CommandEnabled = true;
            CommandIcon = Resources.command_spreadsheet;

            v_InstanceName = "DefaultExcel";
        }

        public async override Task RunCommand(object sender)
        {
            var engine = (IAutomationEngineInstance)sender;
            string vSheetExcelTable = (string)await v_SheetNameExcelTable.EvaluateCode(engine);          
            var vTableName = (string)await v_TableName.EvaluateCode(engine);
            var vColumnName = (string)await v_ColumnName.EvaluateCode(engine);
            var excelObject = v_InstanceName.GetAppInstance(engine);
            var excelInstance = (Application)excelObject;
            var workSheetExcelTable = excelInstance.Sheets[vSheetExcelTable] as Worksheet;
            var excelTable = workSheetExcelTable.ListObjects[vTableName];

            int vColumnIndex = -1;
            if (string.IsNullOrEmpty(v_ColumnIndex))
                vColumnIndex = (int)await v_ColumnIndex.EvaluateCode(engine);

            var excelColumn = excelTable.ListColumns.Add(string.IsNullOrEmpty(v_ColumnIndex) ? excelTable.ListColumns.Count + 1 : vColumnIndex);
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
