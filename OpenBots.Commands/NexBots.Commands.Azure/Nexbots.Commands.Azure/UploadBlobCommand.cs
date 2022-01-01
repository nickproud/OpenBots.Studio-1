using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
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
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NexBots.Commands.Azure
{
    [Serializable]
    [Category("NexBotix Azure Commands")]
    [Description("Commands for connecting Azure resources")]
    public class UploadBlobCommand : ScriptCommand
    {


        [Required] //remove if not required
        [DisplayName("File to upload")]
        [Description("File you wish to upload to blob storage")]
        [SampleUsage("")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [Editor("ShowFileSelectionHelper", typeof(UIAdditionalHelperType))]
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


        [Required]
        [Editable(false)]
        [DisplayName("Assign returned document Url to variable")]
        [Description("Create a new variable or select a variable from the list.")]
        [SampleUsage("vUserVariable")]
        [Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
        [CompatibleTypes(new Type[] { typeof(string) })]
        public string v_OutputUserVariableName { get; set; }

        public UploadBlobCommand()
        {
            CommandName = "UploadBlobCommand";
            SelectionName = "Upload to Blob Storage";
            CommandEnabled = true;
            CommandIcon = Resources.command_run_code; //change this as needed
        }

        public async override Task RunCommand(object sender)
        {
            var engine = (IAutomationEngineInstance)sender;
            string docUrl = UploadFile(v_filePath.GetVariableValue(engine), v_storageAccount.GetVariableValue(engine), v_containerName.GetVariableValue(engine), v_accessKey.GetVariableValue(engine));
            docUrl.SetVariableValue(engine, v_OutputUserVariableName);
        }

        public string UploadFile(string filePath, string blobStorageAccount, string blobContainer, string blobAccessKey)
        {
            if (filePath.StartsWith(@"@"""))
            {
                filePath = filePath.Substring(2, filePath.Length - 3);
            }
            var fileExtension = System.IO.Path.GetExtension(filePath).ToLower();
            switch (fileExtension)
            {
                case ".pdf":
                    return UploadPdf(blobStorageAccount, blobContainer, blobAccessKey, filePath);
                case ".png":
                    return UploadPng(blobStorageAccount, blobContainer, blobAccessKey, filePath);
                case ".jpg":
                    return UploadJpg(blobStorageAccount, blobContainer, blobAccessKey, filePath);
                case ".tiff":
                    return UploadTiff(blobStorageAccount, blobContainer, blobAccessKey, filePath);
                default:
                    throw new ArgumentException($"Filetype {fileExtension} not supported");
            }
        }

        public string UploadPdf(string storageAccountName, string containerName, string accessKey, string filePath)
        {
            try
            {
                var blobHttpHeader = new BlobHttpHeaders();
                blobHttpHeader.ContentType = "application/pdf";

                return Upload(storageAccountName, containerName, accessKey, filePath, blobHttpHeader);
            }
            catch
            {
                throw;
            }
        }

        public string UploadJpg(string storageAccountName, string containerName, string accessKey, string filePath)
        {
            try
            {
                var blobHTTPHeader = new BlobHttpHeaders();
                blobHTTPHeader.ContentType = "image/jpg";
                return Upload(storageAccountName, containerName, accessKey, filePath, blobHTTPHeader);
            }
            catch
            {
                throw;
            }
        }

        public string UploadTiff(string storageAccountName, string containerName, string accessKey, string filePath)
        {
            try
            {
                var blobHTTPHeader = new BlobHttpHeaders();
                blobHTTPHeader.ContentType = "image/tiff";
                return Upload(storageAccountName, containerName, accessKey, filePath, blobHTTPHeader);
            }
            catch
            {
                throw;
            }
        }

        public string UploadPng(string storageAccountName, string containerName, string accessKey, string filePath)
        {
            try
            {
                var blobHTTPHeader = new BlobHttpHeaders();
                blobHTTPHeader.ContentType = "image/png";
                return Upload(storageAccountName, containerName, accessKey, filePath, blobHTTPHeader);
            }
            catch
            {
                throw;
            }
        }

        private string Upload(string storageAccountName, string containerName, string accessKey, string filePath, BlobHttpHeaders blobHeaders = null)
        {
            try
            {
                var connectionString = String.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}",
                                               storageAccountName,
                                               accessKey);

                var fileInfo = new FileInfo(filePath);
                var directory = Path.GetDirectoryName(filePath);
                var newPath = $"{directory}\\{Guid.NewGuid().ToString()}{fileInfo.Extension}";
                fileInfo.MoveTo(newPath);

                var blobName = Path.GetFileName(newPath);
                var blobClient = InitializeBlobClient(ConnectionString(storageAccountName, accessKey), containerName, blobName);

                using (FileStream fileStream = File.OpenRead(newPath))
                {
                    var result = blobClient.Upload(fileStream, blobHeaders).GetRawResponse();
                    if (result.Status != 201)
                    {
                        throw new Exception("Blob could not be created. Error: " + result.ReasonPhrase);
                    }
                    System.IO.File.WriteAllText(@"C:\temp\bloblog.txt", blobClient.Uri.AbsoluteUri);
                    return blobClient.Uri.AbsoluteUri;
                }
            }
            catch (AggregateException ex)
            {
                var builder = new StringBuilder();
                foreach (var exc in ex.InnerExceptions)
                {
                    builder.Append(exc.Message);
                    builder.AppendLine();
                }
                System.IO.File.WriteAllText(@"C:\temp\bloblog.txt", builder.ToString());
                throw;
            }

        }

        private BlobClient InitializeBlobClient(string connectionString, string containerName, string blobName)
        {
            var blobContainerClient = new BlobContainerClient(connectionString, containerName);
            return blobContainerClient.GetBlobClient(blobName);
        }

        private string ConnectionString(string storageAccountName, string accessKey)
        {
            return String.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}",
                                                storageAccountName,
                                                accessKey);
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            //need this
            base.Render(editor, commandControls);

            //render controls for your field variables - access methods on commandsControls to do this
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_filePath", this, editor));
            RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_storageAccount", this));
            RenderedControls.Add(commandControls.CreateDefaultInputFor("v_storageAccount", this));
            RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_containerName", this));
            RenderedControls.Add(commandControls.CreateDefaultInputFor("v_containerName", this));
            RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_accessKey", this));
            RenderedControls.Add(commandControls.CreateDefaultInputFor("v_accessKey", this));
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            //shows in command sequence once saved - used field variables
            return base.GetDisplayValue() + $" to container {v_containerName}";
        }

    }
}
