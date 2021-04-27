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

namespace OpenBots.Commands.Data
{
    [Serializable]
    [Category("Data Commands")]
    [Description("This command checks if a string contains a specified substring and returns the result.")]
    class StringContainsCommand : ScriptCommand
    {
        [Required]
        [DisplayName("Full Text")]
        [Description("Provide a variable or text value.")]
        [SampleUsage("\"Hello World\" || vFullText")]
        [Remarks("Providing data of a type other than a 'String' will result in an error.")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(string) })]
        public string v_FullText { get; set; }

        [Required]
        [DisplayName("Comparison Text")]
        [Description("Provide a variable or text value.")]
        [SampleUsage("\"Hello\" || vComparisonText")]
        [Remarks("Providing data of a type other than a 'String' will result in an error.")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [CompatibleTypes(new Type[] { typeof(string) })]
        public string v_ComparisonText { get; set; }

        [Required]
        [Editable(false)]
        [DisplayName("Output Bool Variable")]
        [Description("Create a new variable or select a variable from the list.")]
        [SampleUsage("vUserVariable")]
        [Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
        [CompatibleTypes(new Type[] { typeof(bool) })]
        public string v_OutputUserVariableName { get; set; }

        public StringContainsCommand()
        {
            CommandName = "StringContainsCommand";
            SelectionName = "String Contains";
            CommandEnabled = true;
            CommandIcon = Resources.command_string;
        }

        public async override Task RunCommand(object sender)
        {
            var engine = (IAutomationEngineInstance)sender;
            var fullText = (string)await v_FullText.EvaluateCode(engine);
            var comparisonText = (string)await v_ComparisonText.EvaluateCode(engine);

            bool outputUserVar = fullText.Contains(comparisonText);

            outputUserVar.SetVariableValue(engine, v_OutputUserVariableName);
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_FullText", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_ComparisonText", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [If '{v_FullText}' Contains '{v_ComparisonText}' - " +
                $"Store Bool in '{v_OutputUserVariableName}']";
        }
    }
}
