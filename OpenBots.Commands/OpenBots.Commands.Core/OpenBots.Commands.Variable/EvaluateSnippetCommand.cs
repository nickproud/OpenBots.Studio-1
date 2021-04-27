using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Properties;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;
using Tasks = System.Threading.Tasks;

namespace OpenBots.Commands.Variable
{
    [Serializable]
	[Category("Variable Commands")]
	[Description("This command runs a snippet and updates a variable if it is modified.")]
	public class EvaluateSnippetCommand : ScriptCommand
	{
		[Required]
		[DisplayName("Code Snippet")]
		[Description("Enter any valid C# code to be evaluated.")]
		[SampleUsage("myText = \"hello\" || myList.Add(\"hello\")")]
		[Remarks("")]
		[CompatibleTypes(new Type[] { typeof(object) })]
		public string v_Input { get; set; }

		public EvaluateSnippetCommand()
		{
			CommandName = "EvaluateSnippetCommand";
			SelectionName = "Evaluate Snippet";
			CommandEnabled = true;
			CommandIcon = Resources.command_parse;
		}

		public async override Tasks.Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;

			await v_Input.EvaluateSnippet(engine);

			VariableMethods.SyncVariableValues(engine);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Input", this, editor, 200, 300));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [{v_Input}]";
		}
	}
}