﻿using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Interfaces;
using OpenBots.Core.Properties;
using OpenBots.Core.Utilities.CommonUtilities;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Windows.Forms;
using Tasks = System.Threading.Tasks;

namespace OpenBots.Commands.Engine
{
	[Serializable]
	[Category("Engine Commands")]
	[Description("This command pauses the script for a set amount of time specified in milliseconds.")]
	public class PauseScriptCommand : ScriptCommand
	{
		[Required]
		[DisplayName("Pause Time (Milliseconds)")]      
		[Description("Select or provide a specific amount of time in milliseconds.")]
		[SampleUsage("1000 || vTime")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(int) })]
		public string v_PauseLength { get; set; }

		public PauseScriptCommand()
		{
			CommandName = "PauseScriptCommand";
			SelectionName = "Pause Script";
			CommandEnabled = true;
			CommandIcon = Resources.command_pause;
			
			v_PauseLength = "1000";
		}

		public async override Tasks.Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var pauseLength = (int)await v_PauseLength.EvaluateCode(engine);
			Thread.Sleep(pauseLength);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_PauseLength", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Pause for '{v_PauseLength} ms']";
		}
	}
}