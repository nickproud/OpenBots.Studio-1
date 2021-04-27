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
using System.Security;
using System.Threading.Tasks;
using System.Windows.Forms;
using IO = System.IO;

namespace OpenBots.Commands.File
{
    [Serializable]
	[Category("File Operation Commands")]
	[Description("This command compresses file(s) from a directory into a Zip file.")]
	public class CompressFilesCommand : ScriptCommand
	{
		[Required]
		[DisplayName("Source Directory Path")]
		[Description("Enter or Select the Path to the source directory.")]
		[SampleUsage("@\"C:\\temp\" || ProjectPath + @\"\\temp\" || vDirectoryPath")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[Editor("ShowFolderSelectionHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_DirectoryPathOrigin { get; set; }

		[DisplayName("Password (Optional)")]
		[Description("Define the password to use for file compression.")]
		[SampleUsage("vPassword")]
		[Remarks("Password input must be a SecureString variable.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(SecureString) })]
		public string v_Password { get; set; }

		[Required]
		[DisplayName("Compressed File Directory Path")]
		[Description("Enter or Select the Folder Path to place the compressed file in.")]
		[SampleUsage("@\"C:\\temp\" || ProjectPath + @\"\\temp\" || vDirectoryPath")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[Editor("ShowFolderSelectionHelper", typeof(UIAdditionalHelperType))]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_PathDestination { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output Compressed File Path Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("vUserVariable")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_OutputUserVariableName { get; set; }

		public CompressFilesCommand()
		{
			CommandName = "CompressFilesCommand";
			SelectionName = "Compress Files";
			CommandEnabled = true;
			CommandIcon = Resources.command_files;
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			//get variable path to source file
			var vSourceDirectoryPathOrigin = (string)await v_DirectoryPathOrigin.EvaluateCode(engine);

			// get file path to destination files
			var vFilePathDestination = (string)await v_PathDestination.EvaluateCode(engine);

			var vPassword = "";
			// get password to extract files
			if (!string.IsNullOrEmpty(v_Password))
				vPassword = ((SecureString)await v_Password.EvaluateCode(engine)).ConvertSecureStringToString();

            if (IO.File.Exists(vSourceDirectoryPathOrigin))
				throw new ArgumentException($"{vSourceDirectoryPathOrigin} is a file when it should be a directory");
			else if (!Directory.Exists(vSourceDirectoryPathOrigin))
				throw new DirectoryNotFoundException($"{vSourceDirectoryPathOrigin} is not a valid directory");

			string[] filenames = Directory.GetFiles(vSourceDirectoryPathOrigin, "*.*", SearchOption.AllDirectories);
			string[] directorynames = Directory.GetDirectories(vSourceDirectoryPathOrigin, "*", SearchOption.AllDirectories);

			string sourceDirectoryName = new DirectoryInfo(vSourceDirectoryPathOrigin).Name;
			string compressedFileName = Path.Combine(vFilePathDestination, sourceDirectoryName + ".zip");
			using (ZipOutputStream OutputStream = new ZipOutputStream(IO.File.Create(compressedFileName)))
			{
				// Define a password for the file (if provided)
				OutputStream.Password = vPassword;

				// Define the compression level
				// 0 - store only to 9 - means best compression
				OutputStream.SetLevel(9);

				byte[] buffer = new byte[4096];

				foreach(string directory in directorynames)
				{
					ZipEntry dirEntry = new ZipEntry(directory.Replace(vSourceDirectoryPathOrigin + "\\", "") + "\\");
					dirEntry.DateTime = DateTime.Now;
					OutputStream.PutNextEntry(dirEntry);
				}

				foreach (string file in filenames)
				{

					ZipEntry entry = new ZipEntry(file.Replace(vSourceDirectoryPathOrigin, ""));
					entry.DateTime = DateTime.Now;
					OutputStream.PutNextEntry(entry);

					using (FileStream fs = IO.File.OpenRead(file))
					{
						int sourceBytes;

						do
						{
							sourceBytes = fs.Read(buffer, 0, buffer.Length);
							OutputStream.Write(buffer, 0, sourceBytes);
						} while (sourceBytes > 0);
					}
				}

				// Finish is important to ensure trailing information for a Zip file is appended.  Without this
				// the created file would be invalid.
				OutputStream.Finish();

				// Close is important to wrap things up and unlock the file.
				OutputStream.Close();                     
			}

			//Add File Path to the output variable
			compressedFileName.SetVariableValue(engine, v_OutputUserVariableName);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			//create standard group controls
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_DirectoryPathOrigin", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Password", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_PathDestination", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Compress From '{v_DirectoryPathOrigin}' to '{v_PathDestination}' - " +
				$"Store Compressed File Path in '{v_OutputUserVariableName}']";
		}
	}
}