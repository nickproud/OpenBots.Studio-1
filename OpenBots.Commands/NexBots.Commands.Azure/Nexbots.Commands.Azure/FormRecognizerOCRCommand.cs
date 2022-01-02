using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;
using Azure.Storage.Blobs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NexBots.Commands.Azure.Utilities;
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace NexBots.Commands.Azure
{
    [Serializable]
    [Category("NexBotix Azure Commands")]
    [Description("Commands for connecting Azure resources")]
    public class FormRecognizerOCRCommand : ScriptCommand
    {
		[Required] //remove if not required
		[DisplayName("Blob Storage Account")]
		[Description("Storage account to use when uploading blob before OCR")]
		[SampleUsage("")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		//can be changed to accept a specific data type
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_storageAccount { get; set; } //holds value added to field

		[Required] //remove if not required
		[DisplayName("Blob Container")]
		[Description("Container to upload document to for OCR")]
		[SampleUsage("")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		//can be changed to accept a specific data type
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_containerName { get; set; } //holds value added to field

		[Required] //remove if not required
		[DisplayName("Blob Storage Access Key")]
		[Description("Access key for blob storage account")]
		[SampleUsage("")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		//can be changed to accept a specific data type
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_blobAccessKey { get; set; } //holds value added to field

		[Required] //remove if not required
		[DisplayName("File Path")]
		[Description("Location of file to be sent through OCR")]
		[SampleUsage("")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		[Editor("ShowFileSelectionHelper", typeof(UIAdditionalHelperType))]
		//can be changed to accept a specific data type
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_filePath { get; set; } //holds value added to field

		[Required] //remove if not required
		[DisplayName("Form Recognizer Endpoint URL")]
		[Description("Url of the form recognizer resource in Azure")]
		[SampleUsage("")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		//can be changed to accept a specific data type
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_endpointUrl { get; set; } //holds value added to field

		[Required] //remove if not required
		[DisplayName("Form Recognizer Subscription Key")]
		[Description("Subscription key for the Azure Form Recognizer endpoint")]
		[SampleUsage("")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		//can be changed to accept a specific data type
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_subscriptionKey { get; set; } //holds value added to field

		[Required] //remove if not required
		[DisplayName("Training Set Id")]
		[Description("Id for specific custom model to be used, or 'prebuilt-invoice' for built-in invoice OCR")]
		[SampleUsage("")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		//can be changed to accept a specific data type
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_trainingSetId { get; set; } //holds value added to field


		[DisplayName("Training Set Id (Additional Pages)")]
		[Description("Id for specific custom model to be used on all pages following the first page")]
		[SampleUsage("")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		//can be changed to accept a specific data type
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_additionalTrainingSetId { get; set; } //holds value added to field

		[Required]
		[Editable(false)]
		[DisplayName("Assign returned OCR JSON to variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("vUserVariable")]
		[Remarks("New variables/arguments may be instantiated by utilizing the Ctrl+K/Ctrl+J shortcuts.")]
		[CompatibleTypes(new Type[] { typeof(string) })]
		public string v_OutputUserVariableName { get; set; }

		public FormRecognizerOCRCommand()
		{
			CommandName = "FormRecognizerOcrCommand";
			SelectionName = "Perform OCR (Azure Form Recognizer)";
			CommandEnabled = true;
			CommandIcon = Resources.command_run_code; //change this as needed
		}

		public async override Task RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			if (v_filePath.StartsWith(@"@"""))
			{
				v_filePath = v_filePath.Substring(2, v_filePath.Length - 3);
			}
			var additionalTrainingId = v_trainingSetId.Length > 0 ? "" : v_trainingSetId.GetVariableValue(engine);
			var result = GetOCRJSON(v_containerName.GetVariableValue(engine), v_storageAccount.GetVariableValue(engine), v_blobAccessKey.GetVariableValue(engine),
				v_filePath.GetVariableValue(engine), v_endpointUrl.GetVariableValue(engine), v_subscriptionKey.GetVariableValue(engine),
				v_trainingSetId.GetVariableValue(engine), additionalTrainingId);
			result.SetVariableValue(v_OutputUserVariableName);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			//need this
			base.Render(editor, commandControls);

			//render controls for your field variables - access methods on commandsControls to do this
			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_storageAccount", this));
			RenderedControls.Add(commandControls.CreateDefaultInputFor("v_storageAccount", this));
			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_containerName", this));
			RenderedControls.Add(commandControls.CreateDefaultInputFor("v_containerName", this));
			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_blobAccessKey", this));
			RenderedControls.Add(commandControls.CreateDefaultInputFor("v_blobAccessKey", this));
			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_containerName", this));
			RenderedControls.Add(commandControls.CreateDefaultInputFor("v_containerName", this));
			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_accessKey", this));
			RenderedControls.Add(commandControls.CreateDefaultInputFor("v_accessKey", this));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_filePath", this, editor));
			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_endpointUrl", this));
			RenderedControls.Add(commandControls.CreateDefaultInputFor("v_endpointUrl", this));
			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_subscriptionKey", this));
			RenderedControls.Add(commandControls.CreateDefaultInputFor("v_subscriptionKey", this));
			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_trainingSetId", this));
			RenderedControls.Add(commandControls.CreateDefaultInputFor("v_trainingSetId", this));
			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_additionalTrainingSetId", this));
			RenderedControls.Add(commandControls.CreateDefaultInputFor("v_additionalTrainingSetId", this));
			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			//RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			//shows in command sequence once saved - used field variables
			return base.GetDisplayValue() + $" Endpoint - {v_endpointUrl}";
		}

		public string GetOCRJSON(string blobContainer, string blobStorageAccount,
	  string blobAccessKey, string filePath, string azureEndpoint, string subscriptionKey, string trainingSetId,
	  string additionalPageTrainingSetId = "", string platformUrl = "")
		{
			
			var fileExtension = System.IO.Path.GetExtension(filePath).ToLower();
			var documentUrl = new UploadBlobCommand().UploadFile(filePath, blobStorageAccount, blobContainer, blobAccessKey);
			var fileName = System.IO.Path.GetFileName(filePath);
			filePath = filePath.Replace($"\\{fileName}", "");
			fileName = documentUrl.Split('/').Last();
			filePath = System.IO.Path.Combine(filePath, fileName);
			if (fileExtension == ".pdf")
			{
				var pdfManager = new PdfTool();
				if (!trainingSetId.StartsWith("prebuilt"))
				{
					if (pdfManager.GetTotalPages(filePath) > 1)
					{
						var splitPagesFolder = @"C:\temp\pdfsplitpages";
						if (!Directory.Exists(splitPagesFolder))
						{
							Directory.CreateDirectory(splitPagesFolder);
                        }
						System.IO.Directory.GetFiles(splitPagesFolder).ToList().ForEach(x =>
						{
							System.IO.File.Delete(x);
						});
						var splitPages = pdfManager.SplitPages(filePath, splitPagesFolder);
						var pageList = System.IO.Directory.GetFiles(splitPagesFolder).ToList();
						var multiPageResultObject = JObject.Parse(ExecuteOCROnPage(pageList[0], azureEndpoint, subscriptionKey, trainingSetId));
						pageList.RemoveAt(0);
						var currentTrainingModelId = additionalPageTrainingSetId.Length > 0 ? additionalPageTrainingSetId : trainingSetId;
						if (!multiPageResultObject.ContainsKey("LineItems"))
						{
							multiPageResultObject.Add("LineItems", new JArray());
						}
						var parentItemsArray = JArray.Parse(multiPageResultObject["LineItems"].ToString());
						pageList.ForEach(x =>
						{
							var ocrResult = ExecuteOCROnPage(x, azureEndpoint, subscriptionKey, currentTrainingModelId);
							//check page is not blank before appending to parent
							if (ocrResult.Length > 0)
							{
								var pageObject = JObject.Parse(ocrResult);
								foreach (var token in pageObject)
								{
									if (token.Key == "LineItems" || token.Key == "Items")
									{
										var itemsArray = JArray.Parse(token.Value.ToString());
										parentItemsArray.Merge(itemsArray);
									}
									else
									{
										if (multiPageResultObject.ContainsKey(token.Key))
										{
											if (!multiPageResultObject[token.Key].HasValues)
											{
												multiPageResultObject[token.Key] = token.Value;
											}
										}
										else
										{
											multiPageResultObject.Add(token.Key, token.Value);
										}
									}
								}
							}

						});
						multiPageResultObject["LineItems"] = parentItemsArray;
						multiPageResultObject.Add("DocumentUrl", documentUrl);
						return multiPageResultObject.ToString();
					}
				}

				var resultObject = JObject.Parse(ExecuteOCROnPage(filePath, azureEndpoint, subscriptionKey, trainingSetId));
				resultObject.Add("DocumentUrl", documentUrl);
				return resultObject.ToString();

			}
			else
			{
				var resultObject = JObject.Parse(ExecuteOCROnPage(filePath, azureEndpoint, subscriptionKey, trainingSetId));
				resultObject.Add("DocumentUrl", documentUrl);
				return resultObject.ToString();
			}
		}

		private DocumentAnalysisClient GetFormRecognizerClient(string subscriptionKey, string endpoint)
		{
			var credential = new AzureKeyCredential(subscriptionKey);
			return new DocumentAnalysisClient(new Uri(endpoint), credential);
		}


		private string ExecuteOCROnPage(string filePath, string azureEndpoint, string subscriptionKey, string trainingSetId)
		{
			var client = GetFormRecognizerClient(subscriptionKey, azureEndpoint);
			var retries = 5;
			using (var fileStream = new FileStream(filePath, FileMode.Open))
			{
				var ocrOperation = client.StartAnalyzeDocument(trainingSetId, fileStream);
				Thread.Sleep(5000);
				ocrOperation.UpdateStatus();
				for (var i = 0; i <= retries; i++)
				{
					if (!ocrOperation.HasCompleted)
					{
						if (i == retries)
						{
							throw new Exception($"Failed to get response from Azure Form Recognizer after {retries} retries");
						}
						Thread.Sleep(5000);
						ocrOperation.UpdateStatus();
					}
					else
					{
						break;
					}
				}

				return ocrOperation.Value.Documents.FirstOrDefault() != null ? ExtractFields(ocrOperation.Value.Documents.First().Fields) : "";
			}
		}

		private string ExtractFields(IReadOnlyDictionary<string, DocumentField> fields)
		{
			var result = new JObject();
			var lineItemArray = new JArray();
			foreach (var field in fields)
			{
				if (field.Key == "Items" || field.Key == "LineItems")
				{
					var lineItemList = field.Value.AsList();
					foreach (var lineItem in lineItemList)
					{
						var parsedLineItem = lineItem.AsDictionary();
						var lineItemObject = new JObject();
						foreach (var lineItemColumn in parsedLineItem)
						{
							switch (lineItemColumn.Value.ValueType)
							{
								case DocumentFieldType.Date:
									var newDate = new DateTime();
									var dateParsedSuccessfully = DateTime.TryParse(field.Value.Content, out newDate);
									if (dateParsedSuccessfully)
									{
										result.Add(field.Key, newDate.ToString("dd/MM/yyyy"));
									}
									else
									{
										result.Add(field.Key, field.Value.Content);
									}
									break;
								case DocumentFieldType.String:
									lineItemObject.Add(lineItemColumn.Key, lineItemColumn.Value.AsString());
									break;
								case DocumentFieldType.Double:

									lineItemObject.Add(lineItemColumn.Key, lineItemColumn.Value.Content);
									break;
								case DocumentFieldType.Int64:
									lineItemObject.Add(lineItemColumn.Key, lineItemColumn.Value.AsInt64().ToString("G"));
									break;
								default:
									throw new InvalidOperationException($"Cannot parse item value of type {lineItemColumn.Value.ValueType.ToString()}");
							}
							lineItemObject.Add("Content", lineItem.Content);
						}
						lineItemArray.Add(lineItemObject);
					}
				}
				else
				{
					switch (field.Value.ValueType)
					{
						case DocumentFieldType.Date:
							var newDate = new DateTime();
							var dateParsedSuccessfully = DateTime.TryParse(field.Value.Content, out newDate);
							if (dateParsedSuccessfully)
							{
								result.Add(field.Key, newDate.ToString("dd/MM/yyyy"));
							}
							else
							{
								result.Add(field.Key, field.Value.Content.ToString());
							}
							break;
						case DocumentFieldType.String:
							result.Add(field.Key, field.Value.AsString());
							break;
						case DocumentFieldType.Double:
							result.Add(field.Key, field.Value.Content);
							break;
						case DocumentFieldType.Int64:
							result.Add(field.Key, field.Value.AsInt64().ToString("G"));
							break;
						default:
							throw new InvalidOperationException($"Cannot parse field value of type {field.Value.ValueType.ToString()}");
					}
				}
			}

			if (lineItemArray.Count > 0)
			{
				result.Add("LineItems", lineItemArray);
			}
			return result.ToString();
		}


	}
}
