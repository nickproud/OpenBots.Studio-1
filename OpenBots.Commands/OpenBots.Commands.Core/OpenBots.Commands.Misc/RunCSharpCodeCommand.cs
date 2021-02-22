using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Properties;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using OBScript = OpenBots.Core.Script;

namespace OpenBots.Commands.Core.OpenBots.Commands.Misc
{
    [Serializable]
    [Category("Misc Commands")]
    [Description("This command run a block C# code.")]
    public class RunCSharpCodeCommand : ScriptCommand
    {
		[Required]
		[DisplayName("C# Script")]
		[Description("Your C# Script to be executed.")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_Code { get; set; }

        [Required]
        [Editable(false)]
        [DisplayName("Script Output Variable")]
        [Description("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("Variables not pre-defined in the Variable Manager will be automatically generated at runtime.")]
        public string v_OutputUserVariableName { get; set; }

        public RunCSharpCodeCommand()
        {
            CommandName = "RunC#ScriptCommand";
            SelectionName = "Run C# Script";
            CommandEnabled = true;
            CommandIcon = Resources.command_start_process;
        }

        public async override void RunCommand(object sender)
        {
            var engine = (IAutomationEngineInstance)sender;

            ScriptState state = await CSharpScript.RunAsync("", ScriptOptions.Default.WithImports("System.Collections.Generic"));

            string code = v_Code;
            foreach(OBScript.ScriptVariable variable in engine.AutomationEngineContext.Variables)
            {
                string varString = "{" + variable.VariableName + "}";
                state.ContinueWithAsync();
                //Regex.Replace(code, varString, varString.ConvertUserVariableToString(engine));
            }

            var result = new object();
            try
            {
                result = await CSharpScript.EvaluateAsync(code);
            }
            catch (CompilationErrorException ex)
            {
                result = ex.Diagnostics;
            }
            result = result.ToString();
            result.StoreInUserVariable(engine, v_OutputUserVariableName);
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Code", this, editor, 100, 300));
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" ['Run Custom C# Code']";
        }
    }
}
