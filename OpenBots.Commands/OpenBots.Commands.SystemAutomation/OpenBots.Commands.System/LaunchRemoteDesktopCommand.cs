using OpenBots.Commands.System.Forms;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Properties;
using OpenBots.Core.UI.Controls;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenBots.Commands.System
{
	[Serializable]
	[Category("System Commands")]
	[Description("This command launches a remote desktop session.")]
	public class LaunchRemoteDesktopCommand : ScriptCommand
	{
		[Required]
		[DisplayName("Machine Name")]
		[Description("Define the name of the machine to log on to.")]
		[SampleUsage("\"myMachine\" || vMachineName")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_MachineName { get; set; }

		[Required]
		[DisplayName("Username")]
		[Description("Define the username to use when connecting to the machine.")]
		[SampleUsage("\"myRobot\" || vUsername")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_UserName { get; set; }

		[Required]
		[DisplayName("Password")]
		[Description("Define the password to use when connecting to the machine.")]
		[SampleUsage("vPassword")]
		[Remarks("Password input must be a SecureString variable.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(SecureString) })]
		public string v_Password { get; set; }

		[Required]
		[DisplayName("RDP Window Width")]
		[Description("Define the width for the Remote Desktop Window.")]
		[SampleUsage("1000 || vWidth")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(int) })]
		public string v_RDPWidth { get; set; }

		[Required]
		[DisplayName("RDP Window Height")]
		[Description("Define the height for the Remote Desktop Window.")]
		[SampleUsage("800 || vHeight")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(int) })]
		public string v_RDPHeight { get; set; }

		public LaunchRemoteDesktopCommand()
		{
			CommandName = "LaunchRemoteDesktopCommand";
			SelectionName = "Launch Remote Desktop";
			CommandEnabled = true;
			CommandIcon = Resources.command_system;

			v_RDPWidth = SystemInformation.PrimaryMonitorSize.Width.ToString();
			v_RDPHeight = SystemInformation.PrimaryMonitorSize.Height.ToString();
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var machineName = (string)await v_MachineName.EvaluateCode(engine);
			var userName = (string)await v_UserName.EvaluateCode(engine);
			var password = ((SecureString)await v_Password.EvaluateCode(engine)).ConvertSecureStringToString();
			var width = (int)await v_RDPWidth.EvaluateCode(engine);
			var height = (int)await v_RDPHeight.EvaluateCode(engine);

			if (engine.AutomationEngineContext.ScriptEngine != null)
			{
				var result = ((Form)engine.AutomationEngineContext.ScriptEngine).Invoke(new Action(() =>
				{
					LaunchRDPSession(machineName, userName, password, width, height);
				}));
			}
			else
				LaunchRDPSession(machineName, userName, password, width, height);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			CommandItemControl helperControl = new CommandItemControl();

			helperControl.Padding = new Padding(10, 0, 0, 0);
			helperControl.ForeColor = Color.AliceBlue;
			helperControl.Font = new Font("Segoe UI Semilight", 10);
			helperControl.CommandImage = Resources.command_system;
			helperControl.CommandDisplay = "RDP Display Manager";
			helperControl.Click += new EventHandler((s, e) => LaunchRDPDisplayManager(s, e));

			RenderedControls.Add(helperControl);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_MachineName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_UserName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Password", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_RDPWidth", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_RDPHeight", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Machine '{v_MachineName}']";
		}

		public void LaunchRDPDisplayManager(object sender, EventArgs e)
		{
			frmDisplayManager displayManager = new frmDisplayManager();
			displayManager.ShowDialog();
			displayManager.Dispose();            
		}

		public void LaunchRDPSession(string machineName, string userName, string password, int width, int height)
		{
			var remoteDesktopForm = new frmRemoteDesktopViewer(machineName, userName, password, width, height, false, false);
			remoteDesktopForm.Show();
		}
	}
}
