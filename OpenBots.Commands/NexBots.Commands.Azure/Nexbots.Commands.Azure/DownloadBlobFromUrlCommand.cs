using Azure.Storage.Blobs;
using Newtonsoft.Json;
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
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NexBots.Commands.Azure
{
    [Serializable]
    [Category("NexBotix Azure Commands")]
    [Description("Commands for connecting Azure resources")]
    public class DownloadBlobFromUrlCommand : ScriptCommand
    {
		[Required] //remove if not required
		[DisplayName("Blob Url")]
		[Description("URL of the blob you wish to download")]
		[SampleUsage("")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		//can be changed to accept a specific data type
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_blobUrl { get; set; } //holds value added to field

		[Required] //remove if not required
		[DisplayName("Save to Path")]
		[Description("Path where you wish to save the blob")]
		[SampleUsage("")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		//can be changed to accept a specific data type
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_filePath { get; set; } //holds value added to field

		[Required] //remove if not required
		[DisplayName("Storage Account")]
		[Description("Name of the storage account in which the blob container resides")]
		[SampleUsage("")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		//can be changed to accept a specific data type
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_storageAccount { get; set; } //holds value added to field

		[Required] //remove if not required
		[DisplayName("Blob Container Name")]
		[Description("Name of the blob container in which the document resides")]
		[SampleUsage("")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		//can be changed to accept a specific data type
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_containerName { get; set; } //holds value added to field

		[Required] //remove if not required
		[DisplayName("Access Key")]
		[Description("Access key for the blob container")]
		[SampleUsage("")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		//can be changed to accept a specific data type
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_accessKey { get; set; } //holds value added to field

		public DownloadBlobFromUrlCommand()
		{
			CommandName = "DownloadBlobFromUrlCommand";
			SelectionName = "Download Blob from Url";
			CommandEnabled = true;
			CommandIcon = Resources.command_run_code; //change this as needed
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;

			var blobUri = new Uri(v_blobUrl.GetVariableValue(engine));
			var blobName = blobUri.Segments[blobUri.Segments.Length - 1];
			var blobClient = InitializeBlobClient(ConnectionString(v_storageAccount.GetVariableValue(engine), v_accessKey.GetVariableValue(engine)), v_containerName.GetVariableValue(engine), blobName);
			blobClient.DownloadTo(System.IO.Path.Combine(v_filePath.GetVariableValue(engine), blobName));
		}

		private string ConnectionString(string storageAccountName, string accessKey)
		{
			return String.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}",
												storageAccountName,
												accessKey);
		}

		private BlobClient InitializeBlobClient(string connectionString, string containerName, string blobName)
		{
			var blobContainerClient = new BlobContainerClient(connectionString, containerName);
			return blobContainerClient.GetBlobClient(blobName);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			//need this
			base.Render(editor, commandControls);

			//render controls for your field variables - access methods on commandsControls to do this
			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_blobUrl", this));
			RenderedControls.Add(commandControls.CreateDefaultInputFor("v_blobUrl", this));	
			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_filePath", this));
			RenderedControls.Add(commandControls.CreateDefaultInputFor("v_filePath", this));
			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_storageAccount", this)); 
			RenderedControls.Add(commandControls.CreateDefaultInputFor("v_storageAccount", this));
			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_containerName", this)); 
			RenderedControls.Add(commandControls.CreateDefaultInputFor("v_containerName", this));
			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_accessKey", this));
			RenderedControls.Add(commandControls.CreateDefaultInputFor("v_accessKey", this));
			//RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			//shows in command sequence once saved - used field variables
			return base.GetDisplayValue() + $" at {v_blobUrl}";
		}


	}
}
