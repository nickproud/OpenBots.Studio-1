﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OpenBots.Commands.Data
{
    [Serializable]
    [Category("Data Commands")]
    [Description("This command returns the length of a string.")]
    public class GetTextLengthCommand : ScriptCommand
    {

        [Required]
		[DisplayName("Text Data")]
        [Description("Provide a variable or text value.")]
        [SampleUsage("Hello World || {vStringVariable}")]
        [Remarks("Providing data of a type other than a 'String' will result in an error.")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        public string v_InputValue { get; set; }

        [Required]
        [Editable(false)]
        [DisplayName("Output Length Variable")]
        [Description("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("Variables not pre-defined in the Variable Manager will be automatically generated at runtime.")]
        public string v_OutputUserVariableName { get; set; }

        public GetTextLengthCommand()
        {
            CommandName = "GetTextLengthCommand";
            SelectionName = "Get Text Length";
            CommandEnabled = true;           
        }

        public override void RunCommand(object sender)
        {
            //get engine
            var engine = (AutomationEngineInstance)sender;

            //get input value
            var stringRequiringLength = v_InputValue.ConvertUserVariableToString(engine);

            //count number of words
            var stringLength = stringRequiringLength.Length;

            //store word count into variable
            stringLength.ToString().StoreInUserVariable(engine, v_OutputUserVariableName);
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            //create standard group controls
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InputValue", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Of Text '{v_InputValue}' - Store Length in '{v_OutputUserVariableName}']";
        }
    }
}