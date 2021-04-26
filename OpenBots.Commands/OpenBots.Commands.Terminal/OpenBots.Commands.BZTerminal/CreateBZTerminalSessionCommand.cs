using Microsoft.Win32;
using OpenBots.Commands.Terminal.Library;
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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenBots.Commands.BZTerminal
{
    [Serializable]
	[Category("BlueZone Terminal Commands")]
	[Description("This command creates a Rocket BlueZone terminal session.")]
	public class CreateBZTerminalSessionCommand : ScriptCommand
	{
		[Required]
		[DisplayName("BZ Terminal Instance Name")]
		[Description("Enter a unique name that will represent the application instance.")]
		[SampleUsage("MyBZTerminalInstance")]
		[Remarks("This unique name allows you to refer to the instance by name in future commands, " +
				 "ensuring that the commands you specify run against the correct application.")]
		[CompatibleTypes(new Type[] { typeof(BZTerminalContext) })]
		public string v_InstanceName { get; set; }

		[Required]
		[DisplayName("Terminal Type")]
		[PropertyUISelectionOption("Mainframe")]
		[PropertyUISelectionOption("iSeries")]
		[PropertyUISelectionOption("VT")]
		[PropertyUISelectionOption("6530")]
		[Description("Indicate whether to close any existing Excel instances before executing Excel Automation.")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_TerminalType { get; set; }

		[Required]
		[DisplayName("Session File Path")]
		[Description("Enter or Select the path of the BlueZone Session file to upload.")]
		[SampleUsage("@\"C:\\temp\\myfile.zmd\" || ProjectPath + @\"\\myfile.zmd\" || vFilePath")]
		[Remarks("This input should only be used for BlueZone Session files.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[Editor("ShowFileSelectionHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_SessionFilePath { get; set; }

		[Required]
		[DisplayName("Close All Existing Terminal Instances")]
		[PropertyUISelectionOption("Yes")]
		[PropertyUISelectionOption("No")]
		[Description("Indicate whether to close any existing BZ terminal instances before executing BZ terminal Automation.")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_CloseAllInstances { get; set; }

		public CreateBZTerminalSessionCommand()
		{
			CommandName = "CreateBZTerminalSessionCommand";
			SelectionName = "Create BZ Terminal Session";
			CommandEnabled = true;
			CommandIcon = Resources.command_system;

			v_InstanceName = "DefaultBZTerminal";
			v_TerminalType = "Mainframe";
			v_CloseAllInstances = "Yes";
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var sessionFilePath = (string)await v_SessionFilePath.EvaluateCode(engine);
			var terminalContext = new BZTerminalContext();

			if (v_CloseAllInstances == "Yes")
			{
				var registrykey = Registry.CurrentUser.OpenSubKey(@"Software\BlueZone");
				foreach (var subKeyName in registrykey.GetSubKeyNames())
				{
					var subKey = registrykey.OpenSubKey(subKeyName);
					foreach (var sub2KeyName in subKey.GetSubKeyNames())
					{
						if (sub2KeyName.StartsWith("Mainframe Display"))
						{
							while (terminalContext.BZTerminalObj.Connect() == 0)
							{
								string sessionName = terminalContext.BZTerminalObj.GetSessionName();
								terminalContext.BZTerminalObj.Disconnect();
								terminalContext.BZTerminalObj.DeleteSession(sessionName);
							}
							break;
						}
					}
				}			
			}

			int terminalType = 0;
            switch (v_TerminalType)
            {
				case "Mainframe":
					terminalType = 1;
					break;
				case "iSeries":
					terminalType = 2;
					break;
				case "VT":
					terminalType = 3;
					break;
				case "6530":
					terminalType = 6;
					break;
			}

			string session = terminalContext.BZTerminalObj.NewSession(terminalType, sessionFilePath);
			terminalContext.BZTerminalObj.Connect(session);
			terminalContext.BZTerminalObj.WaitForReady();

			terminalContext.AddAppInstance(engine, v_InstanceName);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);			

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_TerminalType", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SessionFilePath", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_CloseAllInstances", this, editor));
			
			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Close Instances '{v_CloseAllInstances}' - New Instance Name '{v_InstanceName}']";
		}
	}
}
