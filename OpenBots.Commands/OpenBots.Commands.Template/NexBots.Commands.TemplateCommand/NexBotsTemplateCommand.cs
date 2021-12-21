using Newtonsoft.Json;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Interfaces;
using OpenBots.Core.Properties;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NexBots.Commands.Template 
{ 
	[Serializable]
	[Category("Command category")]
	[Description("This command does something really cool")]
	public class ExecuteAPICommand : ScriptCommand
	{
		[Required] //remove if not required
		//[Editable(false)] add if you want to block editing
		[DisplayName("Command field label")]
		[Description("Tooltip contents")]
		[SampleUsage("example of usage")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		//can be changed to accept a specific data type
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_variableName { get; set; } //holds value added to field

	
		//examples of grid view that can be added
		//[JsonIgnore]
		//[Browsable(false)]
		//private DataGridView _parametersGridViewHelper;

		//[JsonIgnore]
		//[Browsable(false)]
		//private DataGridView _advancedParametersGridViewHelper;

		public ExecuteAPICommand()
		{
			CommandName = "MyCommand";
			SelectionName = "NameOfCommandInList";
			CommandEnabled = true;
			CommandIcon = Resources.command_run_code; //change this as needed
		}

		public async override Task RunCommand(object sender)
		{
			//command logic
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			//need this
			base.Render(editor, commandControls);

			//render controls for your field variables - access methods on commandsControls to do this
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_variableName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			//shows in command sequence once saved - used field variables
			return base.GetDisplayValue() + $"field set to {v_variableName}";
		}
	}
}

