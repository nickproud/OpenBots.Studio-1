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
using System.Windows.Forms;
using Exception = System.Exception;
using OBDataTable = System.Data.DataTable;

namespace OpenBots.Commands.List
{
	[Serializable]
	[Category("List Commands")]
	[Description("This command adds an item to an existing List variable.")]
	public class AddListItemCommand : ScriptCommand
	{
		[Required]
		[DisplayName("List")]
		[Description("Provide a List variable.")]
		[SampleUsage("{vList}")]
		[Remarks("Any type of variable other than List will cause error.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(List<>) })]
		public string v_ListName { get; set; }

		[Required]
		[DisplayName("List Item")]
		[Description("Enter the item to add to the List.")]
		[SampleUsage("Hello || {vItem}")]
		[Remarks("List item can only be a String, DataTable, MailItem or IWebElement.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string), typeof(OBDataTable), typeof(MailItem), typeof(MimeMessage), typeof(IWebElement)}, true)]
		public string v_ListItem { get; set; }

		public AddListItemCommand()
		{
			CommandName = "AddListItemCommand";
			SelectionName = "Add List Item";
			CommandEnabled = true;
			CommandIcon = Resources.command_function;

		}

		public override void RunCommand(object sender)
		{
			//get sending instance
			var engine = (IAutomationEngineInstance)sender;

			var vListVariable = v_ListName.ConvertUserVariableToObject(engine, nameof(v_ListName), this);

			if (vListVariable != null)
			{
				if (vListVariable is List<string>)
				{
					((List<string>)vListVariable).Add(v_ListItem.ConvertUserVariableToString(engine));
				}
				else if (vListVariable is List<OBDataTable>)
				{
					OBDataTable dataTable;
					var dataTableVariable = v_ListItem.ConvertUserVariableToObject(engine, nameof(v_ListItem), this);
					if (dataTableVariable != null && dataTableVariable is OBDataTable)
						dataTable = (OBDataTable)dataTableVariable;
					else
						throw new Exception("Invalid List Item type, please provide valid List Item type.");
					((List<OBDataTable>)vListVariable).Add(dataTable);
				}
				else if (vListVariable is List<MailItem>)
				{
					MailItem mailItem;
					var mailItemVariable = v_ListItem.ConvertUserVariableToObject(engine, nameof(v_ListItem), this);
					if (mailItemVariable != null && mailItemVariable is MailItem)
						mailItem = (MailItem)mailItemVariable;
					else
						throw new Exception("Invalid List Item type, please provide valid List Item type.");
					((List<MailItem>)vListVariable).Add(mailItem);
				}
				else if (vListVariable is List<MimeMessage>)
				{
					MimeMessage mimeMessage;
					var mimeMessageVariable = v_ListItem.ConvertUserVariableToObject(engine, nameof(v_ListItem), this);
					if (mimeMessageVariable != null && mimeMessageVariable is MimeMessage)
						mimeMessage = (MimeMessage)mimeMessageVariable;
					else
						throw new Exception("Invalid List Item type, please provide valid List Item type.");
					((List<MimeMessage>)vListVariable).Add(mimeMessage);
				}
				else if (vListVariable is List<IWebElement>)
				{
					IWebElement webElement;
					var webElementVariable = v_ListItem.ConvertUserVariableToObject(engine, nameof(v_ListItem), this);
					if (webElementVariable != null && webElementVariable is IWebElement)
						webElement = (IWebElement)webElementVariable;
					else
						throw new Exception("Invalid List Item type, please provide valid List Item type.");
					((List<IWebElement>)vListVariable).Add(webElement);
				}
				else
				{
					throw new Exception("Complex Variable List Type<T> Not Supported");
				}
			}
			else
			{
				throw new Exception("Attempted to add data to a variable, but the variable was not found. Enclose variables within braces, ex. {vVariable}");
			}
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_ListName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_ListItem", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Add Item '{v_ListItem}' to List '{v_ListName}']";
		}
	}
}
