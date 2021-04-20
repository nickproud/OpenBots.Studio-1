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
using System.Threading.Tasks;
using System.Windows.Forms;
using Exception = System.Exception;
using OBDataTable = System.Data.DataTable;

namespace OpenBots.Commands.List
{
	[Serializable]
	[Category("List Commands")]
	[Description("This command removes an item from an existing List variable at a specified index.")]
	public class RemoveListItemCommand : ScriptCommand
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
		[DisplayName("List Index")]
		[Description("Enter the List index where the item will be removed")]
		[SampleUsage("0 || {vIndex}")]
		[Remarks("Providing an out of range index will produce an exception.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(null, true)]
		public string v_ListIndex { get; set; }

		public RemoveListItemCommand()
		{
			CommandName = "RemoveListItemCommand";
			SelectionName = "Remove List Item";
			CommandEnabled = true;
			CommandIcon = Resources.command_function;

		}

		public async override Task RunCommand(object sender)
		{
			//get sending instance
			var engine = (IAutomationEngineInstance)sender;

			//var vListVariable = v_ListName.ConvertUserVariableToObject(engine, nameof(v_ListName), this);
			var vListVariable = await VariableMethods.EvaluateCode(v_ListName, engine, typeof(List<>));
			var vListIndex = (int)await VariableMethods.EvaluateCode(v_ListIndex, engine, typeof(int));

			if (vListVariable != null)
			{
				if (vListVariable is List<string>)
					((List<string>)vListVariable).RemoveAt(vListIndex);
				else if (vListVariable is List<OBDataTable>)
					((List<OBDataTable>)vListVariable).RemoveAt(vListIndex);
				else if (vListVariable is List<MailItem>)
					((List<MailItem>)vListVariable).RemoveAt(vListIndex);
				else if (vListVariable is List<MimeMessage>)
					((List<MimeMessage>)vListVariable).RemoveAt(vListIndex);
				else if (vListVariable is List<IWebElement>)
					((List<IWebElement>)vListVariable).RemoveAt(vListIndex);
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
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_ListIndex", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Remove Item from List '{v_ListName}' at Index '{v_ListIndex}']";
		}
	}
}