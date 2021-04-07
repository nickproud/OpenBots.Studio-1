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
	[Description("This command updates an item in an existing List variable at a specified index.")]
	public class UpdateListItemCommand : ScriptCommand
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
		[Description("Enter the item to write to the List.")]
		[SampleUsage("Hello || {vItem}")]
		[Remarks("List item can only be a String, DataTable, MailItem or IWebElement.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string), typeof(OBDataTable), typeof(MailItem), typeof(MimeMessage), typeof(IWebElement) }, true)]
		public string v_ListItem { get; set; }

		[Required]
		[DisplayName("List Index")]
		[Description("Enter the List index where the item will be written to.")]
		[SampleUsage("0 || {vIndex}")]
		[Remarks("Providing an out of range index will produce an exception.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(null, true)]
		public string v_ListIndex { get; set; }

		public UpdateListItemCommand()
		{
			CommandName = "UpdateListItemCommand";
			SelectionName = "Update List Item";
			CommandEnabled = true;
			CommandIcon = Resources.command_function;

		}

		public async override void RunCommand(object sender)
		{
			//get sending instance
			var engine = (IAutomationEngineInstance)sender;

			//var vListVariable = v_ListName.ConvertUserVariableToObject(engine, nameof(v_ListName), this);
			var vListVariable = await VariableMethods.EvaluateCode($"{v_ListName}", engine, typeof(List<>));
			//var vListIndex = int.Parse(v_ListIndex.ConvertUserVariableToString(engine));
			var vListIndex = (int)await VariableMethods.EvaluateCode($"{v_ListIndex}", engine, typeof(int));
			if (vListVariable != null)
			{
				if (vListVariable is List<string>)
				{
					((List<string>)vListVariable)[vListIndex] = (string)await VariableMethods.EvaluateCode($"{v_ListItem}", engine, typeof(string));
				}
				else if (vListVariable is List<OBDataTable>)
				{
					OBDataTable dataTable;
					var dataTableVariable = await VariableMethods.EvaluateCode($"{v_ListItem}", engine, typeof(OBDataTable));
					if (dataTableVariable != null && dataTableVariable is OBDataTable)
						dataTable = (OBDataTable)dataTableVariable;
					else
						throw new Exception("Invalid List Item type, please provide valid List Item type.");
					((List<OBDataTable>)vListVariable)[vListIndex] = dataTable;
				}
				else if (vListVariable is List<MailItem>)
				{
					MailItem mailItem;
					var mailItemVariable = await VariableMethods.EvaluateCode($"{v_ListItem}", engine, typeof(MailItem));
					if (mailItemVariable != null && mailItemVariable is MailItem)
						mailItem = (MailItem)mailItemVariable;
					else
						throw new Exception("Invalid List Item type, please provide valid List Item type.");
					((List<MailItem>)vListVariable)[vListIndex] = mailItem;
				}
				else if (vListVariable is List<MimeMessage>)
				{
					MimeMessage mimeMessage;
					var mimeMessageVariable = await VariableMethods.EvaluateCode($"{v_ListItem}", engine, typeof(MimeMessage));
					if (mimeMessageVariable != null && mimeMessageVariable is MimeMessage)
						mimeMessage = (MimeMessage)mimeMessageVariable;
					else
						throw new Exception("Invalid List Item type, please provide valid List Item type.");
					((List<MimeMessage>)vListVariable)[vListIndex] = mimeMessage;
				}
				else if (vListVariable is List<IWebElement>)
				{
					IWebElement webElement;
					var webElementVariable = await VariableMethods.EvaluateCode($"{v_ListItem}", engine, typeof(IWebElement));
					if (webElementVariable != null && webElementVariable is IWebElement)
						webElement = (IWebElement)webElementVariable;
					else
						throw new Exception("Invalid List Item type, please provide valid List Item type.");
					((List<IWebElement>)vListVariable)[vListIndex] = webElement;
				}
				else
					throw new Exception("Complex Variable List Type<T> Not Supported");
			}
			else
				throw new Exception("Attempted to write data to a variable, but the variable was not found. Enclose variables within braces, ex. {vVariable}");
			vListVariable.SetVariableValue(engine, v_ListName, typeof(List<>));
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_ListName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_ListItem", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_ListIndex", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Write Item '{v_ListItem}' to List '{v_ListName}' at Index '{v_ListIndex}']";
		}
	}
}