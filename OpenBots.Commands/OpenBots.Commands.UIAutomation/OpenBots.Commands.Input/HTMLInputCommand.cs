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
using System.Security;
using System.Windows.Forms;

namespace OpenBots.Commands.Input
{
	[Serializable]
	[Category("Input Commands")]
	[Description("This command provides the user with an HTML form to input and store a collection of data.")]
	public class HTMLInputCommand : ScriptCommand
	{

		[Required]
		[DisplayName("HTML")]
		[Description("Define the form to be displayed using the HTML Builder.")]
		[SampleUsage("")]
		[Remarks("")]
		[Editor("ShowHTMLBuilder", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(null, true)]
		public string v_InputHTML { get; set; }

		[Required]
		[DisplayName("Error On Close")]
		[PropertyUISelectionOption("Yes")]
		[PropertyUISelectionOption("No")]
		[Description("Specify if an exception should be thrown on any result other than 'OK'.")]
		[SampleUsage("")]
		[Remarks("")]      
		public string v_ErrorOnClose { get; set; }

		public HTMLInputCommand()
		{
			CommandName = "HTMLInputCommand";
			SelectionName = "Prompt for HTML Input";
			CommandEnabled = true;
			CommandIcon = Resources.command_input;

			v_InputHTML = Resources.HTMLInputSample;
			v_ErrorOnClose = "No";
		}

		public override void RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;

			if (engine.AutomationEngineContext.ScriptEngine == null)
			{
				engine.ReportProgress("HTML UserInput Supported With UI Only");
				MessageBox.Show("HTML UserInput Supported With UI Only", "UserInput Command", 
								MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			//sample for temp testing
			var htmlInput = v_InputHTML.ConvertUserVariableToString(engine);

			//invoke ui for data collection
			var result = ((Form)engine.AutomationEngineContext.ScriptEngine).Invoke(new Action(() =>
			{				
				var variables = engine.AutomationEngineContext.ScriptEngine.ShowHTMLInput(htmlInput);

				//if user selected Ok then process variables
				//null result means user cancelled/closed
				if (variables != null)
				{
					//store each one into context
					foreach (var variable in variables)
						variable.VariableValue.StoreInUserVariable(engine, ConvertStringToVariableName(variable.VariableName), variable.VariableType);
				}
				else if (v_ErrorOnClose == "Yes")
					throw new Exception("Input Form was closed by the user");
			}
			));
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InputHTML", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_ErrorOnClose", this, editor));
			
			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue();
		}

		private static string ConvertStringToVariableName(string variableName)
		{
			if (!variableName.Contains("{"))
				return "{" + variableName + "}";
			else
				return variableName;
		}
	}
}