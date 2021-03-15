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
    [Description("This command returns the count of items contained in a Dictionary.")]
    public class GetDictionaryCountCommand : ScriptCommand
    {
		[Required]
		[DisplayName("Dictionary")]
		[Description("Provide a Dictionary variable.")]
		[SampleUsage("{vDictionary}")]
		[Remarks("Any type of variable other than Dictionary will cause error.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(Dictionary<,>)})]
		public string v_DictionaryName { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output Count Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("{vUserVariable}")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(int) })]
		public string v_OutputUserVariableName { get; set; }

		public GetDictionaryCountCommand()
		{
			CommandName = "GetDictionaryCountCommand";
			SelectionName = "Get Dictionary Count";
			CommandEnabled = true;
			CommandIcon = Resources.command_dictionary;
		}

		public override void RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			//get variable by regular name
			var DictionaryVariable = v_DictionaryName.ConvertUserVariableToObject(engine, nameof(v_DictionaryName), this);

			//if still null then throw exception
			if (DictionaryVariable == null)
			{
				throw new NullReferenceException("Complex Variable '" + v_DictionaryName +
					"' not found. Ensure the variable exists before using it.");
			}

			dynamic DictionaryToCount;
			if (DictionaryVariable is Dictionary<string, string>)
				DictionaryToCount = (Dictionary<string, string>)DictionaryVariable;
			else if (DictionaryVariable is Dictionary<string, OBDataTable>)
				DictionaryToCount = (Dictionary<string, OBDataTable>)DictionaryVariable;
			else if (DictionaryVariable is Dictionary<string, MailItem>)
				DictionaryToCount = (Dictionary<string, MailItem>)DictionaryVariable;
			else if (DictionaryVariable is Dictionary<string, MimeMessage>)
				DictionaryToCount = (Dictionary<string, MimeMessage>)DictionaryVariable;
			else if (DictionaryVariable is Dictionary<string, IWebElement>)
				DictionaryToCount = (Dictionary<string, IWebElement>)DictionaryVariable;
			else if (DictionaryVariable is Dictionary<string, object>)
				DictionaryToCount = (Dictionary<string, object>)DictionaryVariable;
			else
				throw new DataException("Invalid dictionary type, please provide valid dictionary type.");

			int count = DictionaryToCount.Count;
			count.StoreInUserVariable(engine, v_OutputUserVariableName, nameof(v_OutputUserVariableName), this);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_DictionaryName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [From '{v_DictionaryName}' - Store Count in '{v_OutputUserVariableName}']";
		}
	}
}
