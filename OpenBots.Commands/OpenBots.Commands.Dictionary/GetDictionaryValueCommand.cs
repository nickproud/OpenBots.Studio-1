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
using System.Windows.Forms;
using System.Data;
using Microsoft.Office.Interop.Outlook;
using MimeKit;
using OpenQA.Selenium;
using Exception = System.Exception;

namespace OpenBots.Commands.Dictionary
{
	[Serializable]
	[Category("Dictionary Commands")]
	[Description("This command returns a dictionary value based on a specified key.")]
	public class GetDictionaryValueCommand : ScriptCommand
	{

		[Required]
		[DisplayName("Dictionary")]
		[Description("Specify the dictionary variable to get a value from.")]
		[SampleUsage("{vDictionary}")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		public string v_InputDictionary { get; set; }

		[Required]
		[DisplayName("Key")]
		[Description("Specify the key to get the value for.")]
		[SampleUsage("SomeKey || {vKey}")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		public string v_Key { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output Value Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("{vUserVariable}")]
		[Remarks("Variables not pre-defined in the Variable Manager will be automatically generated at runtime.")]
		public string v_OutputUserVariableName { get; set; }

		public GetDictionaryValueCommand()
		{
			CommandName = "GetDictionaryValueCommand";
			SelectionName = "Get Dictionary Value";
			CommandEnabled = true;
			CommandIcon = Resources.command_dictionary;

		}

		public override void RunCommand(object sender)
		{
			//Retrieve Dictionary by name
			var engine = (IAutomationEngineInstance)sender;
			var vKey = v_Key.ConvertUserVariableToString(engine);

			dynamic dict;

			//Declare local dictionary and assign output
			if (v_InputDictionary.ConvertUserVariableToObject(engine) is Dictionary<string, string>)
				dict = (Dictionary<string,string>)v_InputDictionary.ConvertUserVariableToObject(engine);
			else if (v_InputDictionary.ConvertUserVariableToObject(engine) is Dictionary<string, DataTable>)
				dict = (Dictionary<string, DataTable>)v_InputDictionary.ConvertUserVariableToObject(engine);
			else if (v_InputDictionary.ConvertUserVariableToObject(engine) is Dictionary<string, MailItem>)
				dict = (Dictionary<string, MailItem>)v_InputDictionary.ConvertUserVariableToObject(engine);
			else if (v_InputDictionary.ConvertUserVariableToObject(engine) is Dictionary<string, MimeMessage>)
				dict = (Dictionary<string, MimeMessage>)v_InputDictionary.ConvertUserVariableToObject(engine);
			else if (v_InputDictionary.ConvertUserVariableToObject(engine) is Dictionary<string, IWebElement>)
				dict = (Dictionary<string, IWebElement>)v_InputDictionary.ConvertUserVariableToObject(engine);
			else if (v_InputDictionary.ConvertUserVariableToObject(engine) is Dictionary<string, object>)
				dict = (Dictionary<string, object>)v_InputDictionary.ConvertUserVariableToObject(engine);
			else
				throw new DataException("Invalid dictionary value type, please provide valid dictionary value type.");

			((object)dict[vKey]).StoreInUserVariable(engine, v_OutputUserVariableName);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InputDictionary", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Key", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [From '{v_InputDictionary}' for Key '{v_Key}' - Store Value in '{v_OutputUserVariableName}']";
		}        
	}
}