using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Interfaces;
using OpenBots.Core.Model.ApplicationModel;
using OpenBots.Core.Properties;
using OpenBots.Core.Utilities.CommonUtilities;
using SimpleNLG;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenBots.Commands.NLG
{
	[Serializable]
	[Category("NLG Commands")]
	[Description("This command produces a Natural Language Generation phrase.")]
	public class GenerateNLGPhraseCommand : ScriptCommand
	{
		[Required]
		[DisplayName("NLG Instance Name")]
		[Description("Enter the unique instance that was specified in the **Create NLG Instance** command.")]
		[SampleUsage("MyNLGInstance")]
		[Remarks("Failure to enter the correct instance name or failure to first call the **Create NLG Instance** command will cause an error.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(OBAppInstance) })]
		public string v_InstanceName { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output Phrase Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("vUserVariable")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_OutputUserVariableName { get; set; }

		public GenerateNLGPhraseCommand()
		{
			CommandName = "GenerateNLGPhraseCommand";
			SelectionName = "Generate NLG Phrase";
			CommandEnabled = true;
			CommandIcon = Resources.command_nlg;
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var p = (SPhraseSpec)((OBAppInstance)await v_InstanceName.EvaluateCode(engine)).Value;

			Lexicon lexicon = Lexicon.getDefaultLexicon();
			Realiser realiser = new Realiser(lexicon);

			string phraseOutput = realiser.realiseSentence(p);
			phraseOutput.SetVariableValue(engine, v_OutputUserVariableName);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Store Phrase in '{v_OutputUserVariableName}' - Instance Name '{v_InstanceName}']";
		}
	}
}