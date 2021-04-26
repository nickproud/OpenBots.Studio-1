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
    [Description("This command refreshes a Pivot Table.")]
    public class ExcelRefreshPivotTableCommand : ScriptCommand
    {
        [Required]
        [DisplayName("Excel Instance Name")]
        [Description("Enter the unique instance that was specified in the **Create Application** command.")]
        [SampleUsage("MyExcelInstance")]
        [Remarks("Failure to enter the correct instance or failure to first call the **Create Application** command will cause an error.")]
        [CompatibleTypes(new Type[] { typeof(Application) })]
        public string v_InstanceName { get; set; }

        [Required]
        [DisplayName("Worksheet Name")]
        [Description("Specify the name of the Worksheet containing the Pivot Table.")]
        [SampleUsage("\"Sheet1\" || vSheet")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(string) })]
        public string v_SheetName { get; set; }

        [Required]
        [DisplayName("Pivot Table Name")]
        [Description("Enter the name of Pivot Table to be refreshed.")]
        [SampleUsage("\"PivotTable\" || vPivotTable")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(string) })]
        public string v_PivotTable { get; set; }

        public ExcelRefreshPivotTableCommand()
        {
            CommandName = "ExcelRefreshPivotTableCommand";
            SelectionName = "Refresh Pivot Table";
            CommandEnabled = true;
            CommandIcon = Resources.command_spreadsheet;

            v_InstanceName = "DefaultExcel";
        }

        public async override Task RunCommand(object sender)
        {
            var engine = (IAutomationEngineInstance)sender;
            string vSheet = (string)await v_SheetName.EvaluateCode(engine);
            var vPivotTable = (string)await v_PivotTable.EvaluateCode(engine);
            var excelObject = v_InstanceName.GetAppInstance(engine);
            var excelInstance = (Application)excelObject;
            var workSheet = excelInstance.Sheets[vSheet] as Worksheet;

            PivotTable pivotTable = (PivotTable)workSheet.PivotTables(vPivotTable);
            pivotTable.PivotCache().Refresh();
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SheetName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_PivotTable", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Refresh '{v_PivotTable}' in '{v_SheetName}' - Instance Name   '{v_InstanceName}']";
        }
    }
}
