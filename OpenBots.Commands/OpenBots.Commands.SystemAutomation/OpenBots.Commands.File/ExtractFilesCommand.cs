using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
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
using System.IO;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Forms;
using IO = System.IO;

namespace OpenBots.Commands.File
{
	[Serializable]
	[Category("File Operation Commands")]
	[Description("This command extracts file(s) from a Zip file.")]
	public class ExtractFilesCommand : ScriptCommand
	{
		[Required]
		[DisplayName("Source File Path")]
		[Description("Enter or Select the Path to the source zip file.")]
		[SampleUsage("@\"C:\\temp\\myfile.zip\" || ProjectPath + @\"\\myfile.zip\" || vFilePath")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[Editor("ShowFileSelectionHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_FilePathOrigin { get; set; }

		[DisplayName("Password (Optional)")]
		[Description("Define the password to use if required to extract files.")]
		[SampleUsage("vPassword")]
		[Remarks("Password input must be a SecureString variable.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(SecureString) })]
		public string v_Password { get; set; }

		[Required]
		[DisplayName("Extracted File(s) Directory Path")]
		[Description("Enter or Select the Folder Path to move extracted file(s) to.")]
		[SampleUsage("@\"C:\\temp\" || ProjectPath + @\"\\temp\" || vDirectoryPath")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[Editor("ShowFolderSelectionHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_PathDestination { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output Extracted File Path(s) List Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("vUserVariable")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(List<string>) })]
		public string v_OutputUserVariableName { get; set; }

		public ExtractFilesCommand()
		{
			CommandName = "ExtractFilesCommand";
			SelectionName = "Extract Files";
			CommandEnabled = true;
			CommandIcon = Resources.command_files;
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			//get variable path to source file
			var vSourceFilePathOrigin = (string)await v_FilePathOrigin.EvaluateCode(engine);

			if (!(IO.File.Exists(vSourceFilePathOrigin) && vSourceFilePathOrigin.Contains(".zip")))
				throw new FileNotFoundException($"{vSourceFilePathOrigin} is not a valid zip file");

			// get file path to destination files
			var vFilePathDestination = (string)await v_PathDestination.EvaluateCode(engine);

			if (!Directory.Exists(vFilePathDestination))
				throw new DirectoryNotFoundException($"{vFilePathDestination} is not a valid directory");

			vFilePathDestination = Path.Combine(vFilePathDestination, Path.GetFileNameWithoutExtension(vSourceFilePathOrigin));

			if (Directory.Exists(vFilePathDestination))
				Directory.Delete(vFilePathDestination, true);

			Directory.CreateDirectory(vFilePathDestination);

			// get password to extract files
			var vPassword = "";
			if (string.IsNullOrEmpty(v_Password))
				vPassword = ((SecureString)await v_Password.EvaluateCode(engine)).ConvertSecureStringToString();

			FileStream fs = IO.File.OpenRead(vSourceFilePathOrigin);
			ZipFile file = new ZipFile(fs);

			if (!string.IsNullOrEmpty(vPassword))
			{
				// AES encrypted entries are handled automatically
				file.Password = vPassword;
			}

			foreach (ZipEntry zipEntry in file)
			{
				if (!zipEntry.IsFile)
				{
					// Ignore directories but create them in case they're empty
					Directory.CreateDirectory(Path.Combine(vFilePathDestination, zipEntry.Name));
					continue;
				}

				string entryFileName = zipEntry.Name;

				// 4K is optimum
				byte[] buffer = new byte[4096];
				Stream zipStream = file.GetInputStream(zipEntry);

				// Manipulate the output filename here as desired.
				string fullZipToPath = Path.Combine(vFilePathDestination, entryFileName);
				string directoryName = Path.GetDirectoryName(fullZipToPath);

				if (directoryName.Length > 0)
					Directory.CreateDirectory(directoryName);

				// Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
				// of the file, but does not waste memory.
				// The "using" will close the stream even if an exception occurs.
				using (FileStream streamWriter = IO.File.Create(fullZipToPath))
					StreamUtils.Copy(zipStream, streamWriter, buffer);
			}
		   
			if (file != null)
			{
				file.IsStreamOwner = true;
				file.Close(); 
				//Get File Paths from the folder
				var filesList = Directory.GetFiles(vFilePathDestination, ".", SearchOption.AllDirectories).ToList();

				//Add File Paths to the output variable
				filesList.SetVariableValue(engine, v_OutputUserVariableName);
			}           
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			//create standard group controls
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_FilePathOrigin", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Password", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_PathDestination", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Extract From '{v_FilePathOrigin}' to '{v_PathDestination}' - " +
				$"Store Extracted File Path(s) List in '{v_OutputUserVariableName}']";
		}
	}
}