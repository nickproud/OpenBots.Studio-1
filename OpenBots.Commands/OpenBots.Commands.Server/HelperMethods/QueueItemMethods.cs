using Newtonsoft.Json;
using OpenBots.Core.Server.Models;
using OpenBots.Server.SDK.Api;
using System;
using System.Collections.Generic;
using System.IO;
using static OpenBots.Core.Server.User.EnvironmentSettings;
using SDKQueueItem = OpenBots.Server.SDK.Model.QueueItem;
using QueueItemModel = OpenBots.Core.Server.Models.QueueItem;
using System.Linq;

namespace OpenBots.Commands.Server.HelperMethods
{
    public class QueueItemMethods
    {
        public static QueueItemModel GetQueueItemById(string token, string serverUrl, string organizationId, Guid? id)
        {
            var apiInstance = GetQueueItemApiInstance(token, serverUrl);
            apiInstance.Configuration.AccessToken = token;

            try
            {
                var result = apiInstance.GetQueueItemAsyncWithHttpInfo(id.ToString(), apiVersion, organizationId).Result.Data;
                string queueItemString = JsonConvert.SerializeObject(result);
                var queueItem = JsonConvert.DeserializeObject<QueueItemModel>(queueItemString);
                return queueItem;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling QueueItemsApi.GetQueueItemAsyncWithHttpInfo: "
                    + ex.Message);
            }
        }

        public static QueueItemModel GetQueueItemByLockTransactionKey(string token, string serverurl, string organizationId, string transactionKey)
        {
            var apiInstance = GetQueueItemApiInstance(token, serverUrl);

            try
            {
                string filter = $"LockTransactionKey eq guid'{transactionKey}'";
                var result = apiInstance.ApiVapiVersionQueueItemsGetAsyncWithHttpInfo(apiVersion, organizationId, filter).Result.Data.Items.FirstOrDefault();
                string queueItemString = JsonConvert.SerializeObject(result);
                var queueItem = JsonConvert.DeserializeObject<QueueItemModel>(queueItemString);
                return queueItem;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling QueueItemsApi.ApiVapiVersionQueueItemsGetAsyncWithHttpInfo: "
                    + ex.Message);
            }
        }

        public static void EnqueueQueueItem(string token, string serverUrl, string organizationId, QueueItemModel queueItem)
        {
            var apiInstance = GetQueueItemApiInstance(token, serverUrl);

            try
            {
                var queueItemString = JsonConvert.SerializeObject(queueItem);
                var queueItemSDK = JsonConvert.DeserializeObject<SDKQueueItem>(queueItemString);
                apiInstance.ApiVapiVersionQueueItemsEnqueuePostAsyncWithHttpInfo(apiVersion, organizationId, queueItemSDK).Wait();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling QueueItemsApi.ApiVapiVersionQueueItemsEnqueuePostAsyncWithHttpInfo: "
                    + ex.Message);
            }
        }

        public static void AttachFiles(string token, string serverUrl, string organizationId, Guid? queueItemId, List<string> attachments)
        {
            var apiInstance = GetAttachmentsApiInstance(token, serverUrl);

            try
            {
                if (attachments != null && attachments.Count > 0)
                {
                    List<FileStream> attachmentsList = new List<FileStream>();
                    foreach (var attachment in attachments)
                    {
                        FileStream _file = new FileStream(attachment, FileMode.Open, FileAccess.Read);
                        attachmentsList.Add(_file);
                    }
                    apiInstance.ApiVapiVersionQueueItemsQueueItemIdQueueItemAttachmentsPostAsyncWithHttpInfo(queueItemId.ToString(), apiVersion, organizationId, attachmentsList).Wait();

                    foreach (var file in attachmentsList)
                    {
                        file.Flush();
                        file.Dispose();
                        file.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling QueueItemAttachmentsApi.ApiVapiVersionQueueItemsQueueItemIdQueueItemAttachmentsPostAsyncWithHttpInfo: "
                    + ex.Message);
            }
        }

        public static QueueItemModel DequeueQueueItem(string token, string serverUrl, string organizationId, string agentId, Guid? queueId)
        {
            var apiInstance = GetQueueItemApiInstance(token, serverUrl);

            try
            {
                var result = apiInstance.ApiVapiVersionQueueItemsDequeueGetAsyncWithHttpInfo(apiVersion, organizationId, agentId, queueId.ToString()).Result.Data;
                //may have to map here since result returns a view model
                var queueItemString = JsonConvert.SerializeObject(result);
                var queueItem = JsonConvert.DeserializeObject<QueueItemModel>(queueItemString);
                return queueItem;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling QueueItemsApi.ApiVapiVersionQueueItemsDequeueGetAsyncWithHttpInfo: "
                    + ex.Message);
            }
        }

        public static void CommitQueueItem(string token, string serverUrl, string organizationId, Guid transactionKey)
        {
            var apiInstance = GetQueueItemApiInstance(token, serverUrl);

            try
            {
                apiInstance.ApiVapiVersionQueueItemsCommitPutAsyncWithHttpInfo(apiVersion, organizationId, transactionKey.ToString()).Wait();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling QueueItemsApi.ApiVapiVersionQueueItemsCommitPutAsyncWithHttpInfo: "
                    + ex.Message);
            }
        }

        public static void RollbackQueueItem(string token, string serverUrl, string organizationId, Guid transactionKey, string code, string error, bool isFatal)
        {
            var apiInstance = GetQueueItemApiInstance(token, serverUrl);

            try
            {
                apiInstance.ApiVapiVersionQueueItemsRollbackPutAsyncWithHttpInfo(apiVersion, organizationId, transactionKey.ToString(), code, error, isFatal).Wait();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling QueueItemsApi.ApiVapiVersionQueueItemsRollbackPutAsyncWithHttpInfo: "
                    + ex.Message);
            }
        }

        public static void ExtendQueueItem(string token, string serverUrl, string organizationId, Guid transactionKey)
        {
            var apiInstance = GetQueueItemApiInstance(token, serverUrl);

            try
            {
                apiInstance.ApiVapiVersionQueueItemsExtendPutAsyncWithHttpInfo(apiVersion, organizationId, transactionKey.ToString()).Wait();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling QueueItemsApi.ApiVapiVersionQueueItemsExtendPutAsyncWithHttpInfo: "
                    + ex.Message);
            }
        }

        public static List<QueueItemAttachment> GetAttachments(string token, string serverUrl, string organizationId, Guid? queueItemId)
        {
            var apiInstance = GetAttachmentsApiInstance(token, serverUrl);

            try
            {
                var attachments = apiInstance.ApiVapiVersionQueueItemsQueueItemIdQueueItemAttachmentsGetAsyncWithHttpInfo(queueItemId.ToString(), apiVersion, organizationId).Result.Data.Items;
                var listString = JsonConvert.SerializeObject(attachments);
                var attachmentsList = JsonConvert.DeserializeObject<List<QueueItemAttachment>>(listString);

                return attachmentsList;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling QueueItemAttachmentsApi.GetQueueItemAttachmentsAsyncWithHttpInfo: "
                    + ex.Message);
            }
        }

        public static void DownloadFile(string token, string serverUrl, string organizationId, QueueItemAttachment attachment, string directoryPath)
        {
            var apiInstance = GetAttachmentsApiInstance(token, serverUrl);

            try
            {
                var response = apiInstance.ExportQueueItemAttachmentAsyncWithHttpInfo(attachment.Id.ToString(), apiVersion, organizationId, attachment.QueueItemId.ToString()).Result;
                string value;
                var headers = response.Headers.TryGetValue("Content-Disposition", out value);
                string fileName;
                if (headers == true)
                {
                    string[] valueArray = value.Split('=');
                    string[] fileNameArray = valueArray[1].Split(';');
                    fileName = fileNameArray[0];
                }
                else
                {
                    var fileId = attachment.FileId;
                    var fileApiInstance = new FilesApi(serverUrl);
                    fileApiInstance.Configuration.AccessToken = token;
                    var driveApiInstance = new DrivesApi(serverUrl);
                    driveApiInstance.Configuration.AccessToken = token;
                    string filter = "IsDefault eq true";
                    var driveResponse = driveApiInstance.ApiVapiVersionStorageDrivesGetAsyncWithHttpInfo(apiVersion, organizationId, filter).Result.Data.Items.FirstOrDefault();
                    var fileResponse = fileApiInstance.GetFileFolderAsyncWithHttpInfo(attachment.FileId.ToString(), apiVersion, organizationId, driveResponse.Id.ToString()).Result.Data;
                    fileName = fileResponse.Name;
                }
                var data = response.Data;
                byte[] fileArray = data.ToArray();
                System.IO.File.WriteAllBytes(Path.Combine(directoryPath, fileName), fileArray);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling QueueItemAttachmentsApi.ExportQueueItemAttachmentAsyncWithHttpInfo: "
                    + ex.Message);
            }
        }

        private static QueueItemsApi GetQueueItemApiInstance(string token, string serverUrl)
        {
            var apiInstance = new QueueItemsApi(serverUrl);
            apiInstance.Configuration.AccessToken = token;

            return apiInstance;
        }

        private static QueueItemAttachmentsApi GetAttachmentsApiInstance(string token, string serverUrl)
        {
            var apiInstance = new QueueItemAttachmentsApi(serverUrl);
            apiInstance.Configuration.AccessToken = token;

            return apiInstance;
        }
    }
}
