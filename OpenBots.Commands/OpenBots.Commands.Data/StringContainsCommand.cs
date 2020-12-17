using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
        [SampleUsage("Text which contains string || {vFullText}")]
        [Remarks("Providing data of a type other than a 'String' will result in an error.")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        public string v_FullText { get; set; }

        [Required]
        [DisplayName("Comparison Text")]
        [Description("Provide a variable or text value.")]
        [SampleUsage("Text to be compared from a string || {vComparisonText}")]
        [Remarks("Providing data of a type other than a 'String' will result in an error.")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        public string v_ComparisonText { get; set; }

        [Required]
        [Editable(false)]
        [DisplayName("Output Bool Variable")]
        [Description("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("Variables not pre-defined in the Variable Manager will be automatically generated at runtime.")]
        public string v_OutputUserVariableName { get; set; }

        public StringContainsCommand()
        {
            CommandName = "StringContainsCommand";
            SelectionName = "String Contains";
            CommandEnabled = true;
        }
        public override void RunCommand(object sender)
        {
            var engine = (IAutomationEngineInstance)sender;
            var fullText = v_FullText.ConvertUserVariableToString(engine);
            var comparisonText = v_ComparisonText.ConvertUserVariableToString(engine);
            string outputUserVar = fullText.Contains(comparisonText).ToString(); outputUserVar.StoreInUserVariable(engine, v_OutputUserVariableName);
        }
        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            //create standard group controls
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
