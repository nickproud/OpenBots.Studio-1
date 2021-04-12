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
using System.Data;
using System.Windows.Forms;
using Microsoft.Office.Interop.Outlook;
using MimeKit;
using OpenQA.Selenium;
using OBDataTable = System.Data.DataTable;

namespace OpenBots.Commands.Dictionary
{
    [Serializable]
    [Category("Dictionary Commands")]
    [Description("This command removes a KeyValuePair from an existing Dictionary variable at a specified key.")]
    public class RemoveDictionaryKeyValuePairCommand : ScriptCommand
    {
		[Required]
		[DisplayName("Dictionary")]
		[Description("Provide a Dictionary variable.")]
		[SampleUsage("{vDictionary}")]
		[Remarks("Any type of variable other than Dictionary will cause error.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(Dictionary<,>) })]
		public string v_DictionaryName { get; set; }

		[Required]
		[DisplayName("Key")]
		[Description("Enter key where the KeyValuePair will be removed")]
		[SampleUsage("Hello || {vKey}")]
		[Remarks("Providing a non existing key will produce an exception.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(null, true)]
		public string v_Key { get; set; }

		public RemoveDictionaryKeyValuePairCommand()
		{
			CommandName = "RemoveDictionaryKeyValuePairCommand";
			SelectionName = "Remove Dictionary KeyValuePair";
			CommandEnabled = true;
			CommandIcon = Resources.command_dictionary;
		}

		public async override void RunCommand(object sender)
		{
			//get sending instance
			var engine = (IAutomationEngineInstance)sender;

			var vDictionaryVariable = await v_DictionaryName.EvaluateCode(engine, nameof(v_DictionaryName), this);
			var vKey = (string)await v_Key.EvaluateCode(engine);

			if (vDictionaryVariable != null)
			{
				if (vDictionaryVariable is Dictionary<string, string>)
					((Dictionary<string, string>)vDictionaryVariable).Remove(vKey);
				else if (vDictionaryVariable is Dictionary<string, OBDataTable>)
					((Dictionary<string, OBDataTable>)vDictionaryVariable).Remove(vKey);
				else if (vDictionaryVariable is Dictionary<string, MailItem>)
					((Dictionary<string, MailItem>)vDictionaryVariable).Remove(vKey);
				else if (vDictionaryVariable is Dictionary<string, MimeMessage>)
					((Dictionary<string, MimeMessage>)vDictionaryVariable).Remove(vKey);
				else if (vDictionaryVariable is Dictionary<string, IWebElement>)
					((Dictionary<string, IWebElement>)vDictionaryVariable).Remove(vKey);
				else if (vDictionaryVariable is Dictionary<string, object>)
					((Dictionary<string, object>)vDictionaryVariable).Remove(vKey);
				else
					throw new DataException("Invalid dictionary type, please provide valid dictionary type.");
			}
			else
				throw new NullReferenceException("Attempted to write data to a variable, but the variable was not found. Enclose variables within braces, ex. {vVariable}");
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_DictionaryName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Key", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [From Dictionary '{v_DictionaryName}' at Key '{v_Key}']";
		}
	}
}
