using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Interfaces;
using OpenBots.Core.Properties;
using OpenBots.Core.Utilities.CommonUtilities;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tasks = System.Threading.Tasks;

namespace OpenBots.Commands.Misc
{
	[Serializable]
	[Category("Misc Commands")]
	[Description("This command displays a message to the user.")]
	public class ShowMessageCommand : ScriptCommand
	{

		[Required]
		[DisplayName("Message")]      
		[Description("Specify any text or variable value that should be displayed on screen.")]
		[SampleUsage("\"Hello World\" || vMyVar || \"Hello \" + vName")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(object) })]
		public string v_Message { get; set; }

		[Required]
		[DisplayName("Close After X (Seconds)")]
		[Description("Specify how many seconds to display the message on screen. After the specified time," + 
					 "\nthe message box will be automatically closed and script will resume execution.")]
		[SampleUsage("0 || 5 || vSeconds")]
		[Remarks("Set value to 0 to remain open indefinitely.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(int) })]
		public string v_AutoCloseAfter { get; set; }

		public ShowMessageCommand()
		{
			CommandName = "ShowMessageCommand";
			SelectionName = "Show Message";
			CommandEnabled = true;
			CommandIcon = Resources.command_message;

			v_AutoCloseAfter = "0";
		}

		public async override Tasks.Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;

			int closeAfter = (int)await v_AutoCloseAfter.EvaluateCode(engine);
			
			var variableMessage = await v_Message.EvaluateCode(engine);
			Type varMessageType = null;
			if (variableMessage != null)
				varMessageType = variableMessage.GetType();
			var message = StringMethods.ConvertObjectToString(variableMessage, varMessageType);

			if (engine.EngineContext.ScriptEngine == null)
			{
				engine.ReportProgress("Complex Messagebox Supported With UI Only");

				if (closeAfter > 0)
				{
					var autoCloseForm = new Form() { Size = new Size(0, 0) };
					Tasks.Task.Delay(TimeSpan.FromSeconds(closeAfter))
						.ContinueWith((t) => autoCloseForm.Close(), TaskScheduler.FromCurrentSynchronizationContext());

					MessageBox.Show(autoCloseForm, message, "Message Box Command", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				else
					MessageBox.Show(message, "Message Box Command", MessageBoxButtons.OK, MessageBoxIcon.Information);

				return;
			}

			var result = ((Form)engine.EngineContext.ScriptEngine).Invoke(new Action(() =>
				{
					engine.EngineContext.ScriptEngine.ShowMessage(message, "MessageBox", DialogType.OkOnly, closeAfter);
				}
			));

		}
		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Message", this, editor, 100, 300));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_AutoCloseAfter", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" ['{v_Message}']";
		}
	}
}
