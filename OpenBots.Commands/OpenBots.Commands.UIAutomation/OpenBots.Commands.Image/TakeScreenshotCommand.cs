﻿using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Interfaces;
using OpenBots.Core.Properties;
using OpenBots.Core.User32;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenBots.Commands.Image
{
    [Serializable]
	[Category("Image Commands")]
	[Description("This command takes a screenshot and saves it to a specified location.")]
	public class TakeScreenshotCommand : ScriptCommand
	{
		[Required]
		[DisplayName("Window Name")]
		[Description("Select the name of the window to take a screenshot of.")]
		[SampleUsage("\"Untitled - Notepad\" || \"Current Window\" || \"Desktop\" || vWindow")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[Editor("CaptureWindowHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_WindowName { get; set; }

		[Required]
		[DisplayName("Image Location")]
		[Description("Enter or Select the path of the folder to save the image to.")]
		[SampleUsage("@\"C:\\temp\" || ProjectPath + @\"\\temp\" || vDirectoryPath")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[Editor("ShowFolderSelectionHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_FolderPath { get; set; }

		[Required]
		[DisplayName("Image File Name")]
		[Description("Enter or Select the name of the image file.")]
		[SampleUsage("@\"myFile.png\" || vFilename")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_FileName { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output Image Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("vUserVariable")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(Bitmap) })]
		public string v_OutputUserVariableName { get; set; }

		public TakeScreenshotCommand()
		{
			CommandName = "TakeScreenshotCommand";
			SelectionName = "Take Screenshot";
			CommandEnabled = true;
			CommandIcon = Resources.command_camera;

			v_WindowName = "\"Desktop\"";
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			string windowName = (string)await v_WindowName.EvaluateCode(engine);
			string vFolderPath = (string)await v_FolderPath.EvaluateCode(engine);
			string vFileName = (string)await v_FileName.EvaluateCode(engine);
			
			string vFilePath = Path.Combine(vFolderPath, vFileName);
			Bitmap image = User32Functions.CaptureWindow(windowName);

			image.Save(vFilePath);
			image.SetVariableValue(engine, v_OutputUserVariableName);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultWindowControlGroupFor("v_WindowName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_FolderPath", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_FileName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Target Window '{v_WindowName}' - Save to File Path '{v_FolderPath}\\{v_FileName}' - Store Image in '{v_OutputUserVariableName}']";
		}
	}
}