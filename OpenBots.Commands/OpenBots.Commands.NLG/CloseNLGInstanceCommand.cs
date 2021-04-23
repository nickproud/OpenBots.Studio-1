using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Infrastructure;
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
	[Description("This command closes a Natural Language Generation Instance.")]
	public class CloseNLGInstanceCommand : ScriptCommand
	{
		[Required]
		[DisplayName("NLG Instance Name")]
		[Description("Enter the unique instance that was specified in the **Create NLG Instance** command.")]
		[SampleUsage("MyNLGInstance")]
		[Remarks("Failure to enter the correct instance name or failure to first call the **Create NLG Instance** command will cause an error.")]
		[CompatibleTypes(new Type[] { typeof(SPhraseSpec) })]
		public string v_InstanceName { get; set; }

		public CloseNLGInstanceCommand()
		{
			CommandName = "CloseNLGInstanceCommand";
			SelectionName = "Close NLG Instance";
			CommandEnabled = true;
			CommandIcon = Resources.command_nlg;

			v_InstanceName = "DefaultNLG";
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			v_InstanceName.RemoveAppInstance(engine);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Instance Name '{v_InstanceName}']";
		}
	}
}