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
    [Description("This command gets the Range of an Excel Table.")]
    public class ExcelGetTableRangeCommand : ScriptCommand
    {
        [Required]
        [DisplayName("Excel Instance Name")]
        [Description("Enter the unique instance that was specified in the **Create Application** command.")]
        [SampleUsage("MyExcelInstance")]
        [Remarks("Failure to enter the correct instance or failure to first call the **Create Application** command will cause an error.")]
        [CompatibleTypes(new Type[] { typeof(Application) })]
        public string v_InstanceName { get; set; }

        [Required]
        [DisplayName("Excel Table Name")]
        [Description("Enter the name of the Excel Table to get the Range from.")]
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

        [Required]
        [Editable(false)]
        [DisplayName("Output Range Variable")]
        [Description("Create a new variable or select a variable from the list.")]
        [SampleUsage("vUserVariable")]
        [Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
        [CompatibleTypes(new Type[] { typeof(string) })]
        public string v_OutputUserVariableName { get; set; }

        public ExcelGetTableRangeCommand()
        {
            CommandName = "ExcelGetTableRangeCommand";
            SelectionName = "Get Table Range";
            CommandEnabled = true;
            CommandIcon = Resources.command_spreadsheet;

            v_InstanceName = "DefaultExcel";
        }

        public async override Task RunCommand(object sender)
        {
            var engine = (IAutomationEngineInstance)sender;
            string vSheetExcelTable = (string)await v_SheetNameExcelTable.EvaluateCode(engine);
            var vTableName = (string)await v_TableName.EvaluateCode(engine);
            var excelObject = v_InstanceName.GetAppInstance(engine);
            var excelInstance = (Application)excelObject;
            var workSheetExcelTable = excelInstance.Sheets[vSheetExcelTable] as Worksheet;
            var excelTable = workSheetExcelTable.ListObjects[vTableName];

            //Extract a range of Excel Table and store in Output Range Variable
            excelTable.Range.Address.Replace(@"$", string.Empty).SetVariableValue(engine, v_OutputUserVariableName);
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_TableName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SheetNameExcelTable", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Get Range of '{v_TableName}' From '{v_SheetNameExcelTable}' - Instance Name '{v_InstanceName}']";
        }
    }
}
