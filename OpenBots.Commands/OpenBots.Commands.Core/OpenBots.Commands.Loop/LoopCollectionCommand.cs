using Microsoft.Office.Interop.Outlook;
using Newtonsoft.Json;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Properties;
using OpenBots.Core.Script;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Tasks = System.Threading.Tasks;

namespace OpenBots.Commands.Loop
{
    [Serializable]
	[Category("Loop Commands")]
	[Description("This command iterates over a collection to let user perform actions on the collection items.")]
	public class LoopCollectionCommand : ScriptCommand
	{
		[Required]
		[DisplayName("Input Collection")]
		[Description("Provide a collection variable.")]
		[SampleUsage("vMyList || vMyDictionary.ToList() || vMyDataTable.Rows || new List<int>() { 1, 2, 3 }")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(DataRowCollection), typeof(List<>) })]
		public string v_LoopParameter { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output Collection Item Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("vUserVariable")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(DataRow), typeof(KeyValuePair<,>), typeof(object) })]
		public string v_OutputUserVariableName { get; set; }

		public LoopCollectionCommand()
		{
			CommandName = "LoopCollectionCommand";
			SelectionName = "Loop Collection";
			CommandEnabled = true;
			CommandIcon = Resources.command_startloop;
			ScopeStartCommand = true;
		}

		public async override Tasks.Task RunCommand(object sender, ScriptAction parentCommand)
		{
			LoopCollectionCommand loopCommand = (LoopCollectionCommand)parentCommand.ScriptCommand;
			var engine = (IAutomationEngineInstance)sender;

			var complexVariable = await v_LoopParameter.EvaluateCode(engine);
			dynamic dynamicLoopVariable = complexVariable;

			int loopTimes = dynamicLoopVariable.Count;

			for (int i = 0; i < loopTimes; i++)
			{
				engine.ReportProgress("Starting Loop Number " + (i + 1) + "/" + loopTimes + " From Line " + loopCommand.LineNumber);
				
				((object)dynamicLoopVariable[i]).SetVariableValue(engine, v_OutputUserVariableName);

				foreach (var cmd in parentCommand.AdditionalScriptCommands)
				{
					if (engine.IsCancellationPending)
						return;

					await engine.ExecuteCommand(cmd);

					if (engine.CurrentLoopCancelled)
					{
						engine.ReportProgress("Exiting Loop From Line " + loopCommand.LineNumber);
						engine.CurrentLoopCancelled = false;
						return;
					}

					if (engine.CurrentLoopContinuing)
					{
						engine.ReportProgress("Continuing Next Loop From Line " + loopCommand.LineNumber);
						engine.CurrentLoopContinuing = false;
						break;
					}
				}

				engine.ReportProgress("Finished Loop From Line " + loopCommand.LineNumber);
			}
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_LoopParameter", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));
			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return $"Loop Collection '{v_LoopParameter}' - Store Collection Item in '{v_OutputUserVariableName}'";
		}
	}
}