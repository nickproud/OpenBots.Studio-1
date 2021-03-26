using Microsoft.Office.Interop.Outlook;
using MimeKit;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Properties;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Windows.Forms;
using OBDataTable = System.Data.DataTable;

namespace OpenBots.Commands.Dictionary
{
    [Serializable]
	[Category("Dictionary Commands")]
	[Description("This command returns a value from a KeyValuePair.")]
	class GetValueFromKeyValuePairCommand : ScriptCommand
	{
		[Required]
		[DisplayName("KeyValuePair")]
		[Description("Specify the KeyValuePair variable to get a value from.")]
		[SampleUsage("{vKeyValuePair}")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(KeyValuePair<,>) })]
		public string v_InputKeyValuePair { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output Value Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("{vUserVariable}")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(string), typeof(OBDataTable), typeof(MailItem), typeof(MimeMessage), typeof(IWebElement), typeof(object) })]
		public string v_OutputUserVariableName { get; set; }

		public GetValueFromKeyValuePairCommand()
		{
			CommandName = "GetValueFromKeyValuePairCommand";
			SelectionName = "Get Value From KeyValuePair";
			CommandEnabled = true;
			CommandIcon = Resources.command_dictionary;
		}

		public override void RunCommand(object sender)
		{
			//Get Value from KeyValuePair
			var engine = (IAutomationEngineInstance)sender;

			dynamic keyValuePair;
			if (v_InputKeyValuePair.ConvertUserVariableToObject(engine, nameof(v_InputKeyValuePair), this) is KeyValuePair<string, string>)
				keyValuePair = (KeyValuePair<string, string>)v_InputKeyValuePair.ConvertUserVariableToObject(engine, nameof(v_InputKeyValuePair), this);
			else if (v_InputKeyValuePair.ConvertUserVariableToObject(engine, nameof(v_InputKeyValuePair), this) is KeyValuePair<string, OBDataTable>)
				keyValuePair = (KeyValuePair<string, OBDataTable>)v_InputKeyValuePair.ConvertUserVariableToObject(engine, nameof(v_InputKeyValuePair), this);
			else if (v_InputKeyValuePair.ConvertUserVariableToObject(engine, nameof(v_InputKeyValuePair), this) is KeyValuePair<string, MailItem>)
				keyValuePair = (KeyValuePair<string, MailItem>)v_InputKeyValuePair.ConvertUserVariableToObject(engine, nameof(v_InputKeyValuePair), this);
			else if (v_InputKeyValuePair.ConvertUserVariableToObject(engine, nameof(v_InputKeyValuePair), this) is KeyValuePair<string, MimeMessage>)
				keyValuePair = (KeyValuePair<string, MimeMessage>)v_InputKeyValuePair.ConvertUserVariableToObject(engine, nameof(v_InputKeyValuePair), this);
			else if (v_InputKeyValuePair.ConvertUserVariableToObject(engine, nameof(v_InputKeyValuePair), this) is KeyValuePair<string, IWebElement>)
				keyValuePair = (KeyValuePair<string, IWebElement>)v_InputKeyValuePair.ConvertUserVariableToObject(engine, nameof(v_InputKeyValuePair), this);
			else if (v_InputKeyValuePair.ConvertUserVariableToObject(engine, nameof(v_InputKeyValuePair), this) is KeyValuePair<string, object>)
				keyValuePair = (KeyValuePair<string, object>)v_InputKeyValuePair.ConvertUserVariableToObject(engine, nameof(v_InputKeyValuePair), this);
			else
				throw new DataException("Invalid dictionary value type, please provide valid dictionary value type.");

			((object)keyValuePair.Value).StoreInUserVariable(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InputKeyValuePair", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Store Value From '{v_InputKeyValuePair}' in '{v_OutputUserVariableName}']";
		}
	}
}
