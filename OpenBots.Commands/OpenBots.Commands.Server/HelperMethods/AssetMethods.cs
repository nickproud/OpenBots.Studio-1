using Newtonsoft.Json;
using OpenBots.Server.SDK.Api;
using System;
using static OpenBots.Core.Server.User.EnvironmentSettings;
using IOFile = System.IO.File;
using SDKAsset = OpenBots.Server.SDK.Model.Asset;
using AssetModel = OpenBots.Core.Server.Models.Asset;
using System.IO;
using System.Linq;

namespace OpenBots.Commands.Server.HelperMethods
{
    public class AssetMethods
    {
        public static AssetModel GetAsset(string token, string serverUrl, string organizationId, string assetName, string assetType)
        {
            var apiInstance = GetApiInstance(token, serverUrl);

            try
            {
                var result = apiInstance.ApiVapiVersionAssetsGetAssetByNameAssetNameGetAsyncWithHttpInfo(assetName, apiVersion, organizationId, assetType).Result.Data;
                string assetString = JsonConvert.SerializeObject(result);
                var asset = JsonConvert.DeserializeObject<AssetModel>(assetString);
                return asset;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling AssetsApi.ApiVapiVersionAssetsGetAssetByNameAssetNameGetAsyncWithHttpInfo: "
                    + ex.Message);
            }
        }

        public static void PutAsset(string token, string serverUrl, string organizationId, AssetModel asset)
        {
            var apiInstance = GetApiInstance(token, serverUrl);

            try
            {
                var assetString = JsonConvert.SerializeObject(asset);
                var assetSDK = JsonConvert.DeserializeObject<SDKAsset>(assetString);
                apiInstance.ApiVapiVersionAssetsIdPutAsyncWithHttpInfo(asset.Id.ToString(), apiVersion, organizationId, assetSDK).Wait();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling AssetsApi.ApiVapiVersionAssetsIdPutAsyncWithHttpInfo: "
                    + ex.Message);
            }
        }

        public static void DownloadFileAsset(string token, string serverUrl, string organizationId, AssetModel asset, string directoryPath)
        {
            var apiInstance = GetApiInstance(token, serverUrl);

            try
            {
                var response = apiInstance.ExportAssetAsyncWithHttpInfo(asset.Id.ToString(), apiVersion, organizationId).Result;
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
                    var fileId = asset.FileId;
                    var fileApiInstance = new FilesApi(serverUrl);
                    fileApiInstance.Configuration.AccessToken = token;
                    var driveApiInstance = new DrivesApi(serverUrl);
                    driveApiInstance.Configuration.AccessToken = token;
                    string filter = "IsDefault eq true";
                    var driveResponse = driveApiInstance.ApiVapiVersionStorageDrivesGetAsyncWithHttpInfo(apiVersion, organizationId, filter).Result.Data.Items.FirstOrDefault();
                    var fileResponse = fileApiInstance.GetFileFolderAsyncWithHttpInfo(asset.FileId.ToString(), apiVersion, organizationId, driveResponse.Id.ToString()).Result.Data;
                    fileName = fileResponse.Name;
                }
                MemoryStream data = response.Data;
                byte[] file = data.ToArray();
                IOFile.WriteAllBytes(Path.Combine(directoryPath, fileName), file);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling AssetsApi.ExportAssetAsyncWithHttpInfo: "
                    + ex.Message);
            }
        }

        public static void UpdateFileAsset(string token, string serverUrl, string organizationId, AssetModel asset, string filePath)
        {
            var apiInstance = GetApiInstance(token, serverUrl);

            try
            {
                using (FileStream _file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    apiInstance.ApiVapiVersionAssetsIdUpdatePutAsyncWithHttpInfo(asset.Id.ToString(), apiVersion, organizationId, asset.Name, asset.Type, asset.FileId.Value, _file).Wait();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling AssetsApi.ApiVapiVersionAssetsIdUpdatePutAsyncWithHttpInfo: "
                    + ex.Message);
            }
        }

        public static void AppendAsset(string token, string serverUrl, string organizationId, Guid? assetId, string appendText)
        {
            var apiInstance = GetApiInstance(token, serverUrl);

            try
            {
                apiInstance.ApiVapiVersionAssetsIdAppendPutAsyncWithHttpInfo(assetId.ToString(), apiVersion, organizationId, appendText).Wait();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling AssetsApi.ApiVapiVersionAssetsGetAssetByNameAssetNameGetAsyncWithHttpInfo: "
                    + ex.Message);
            }
        }

        public static void IncrementAsset(string token, string serverUrl, string organizationId, Guid? assetId)
        {
            var apiInstance = GetApiInstance(token, serverUrl);

            try
            {
                apiInstance.ApiVapiVersionAssetsIdIncrementPutAsyncWithHttpInfo(assetId.ToString(), apiVersion, organizationId).Wait();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling ApiVapiVersionAssetsIdIncrementPutAsyncWithHttpInfo: "
                    + ex.Message);
            }
        }

        public static void DecrementAsset(string token, string serverUrl, string organizationId, Guid? assetId)
        {
            var apiInstance = GetApiInstance(token, serverUrl);

            try
            {
                apiInstance.ApiVapiVersionAssetsIdDecrementPutAsyncWithHttpInfo(assetId.ToString(), apiVersion, organizationId).Wait();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling ApiVapiVersionAssetsIdDecrementPutAsyncWithHttpInfo: "
                    + ex.Message);
            }
        }

        public static void AddAsset(string token, string serverUrl, string organizationId, Guid? assetId, int value)
        {
            var apiInstance = GetApiInstance(token, serverUrl);

            try
            {
                apiInstance.ApiVapiVersionAssetsIdAddPutAsyncWithHttpInfo(assetId.ToString(), apiVersion, organizationId, value).Wait();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling ApiVapiVersionAssetsIdAddPutAsyncWithHttpInfo: "
                    + ex.Message);
            }
        }

        public static void SubtractAsset(string token, string serverUrl, string organizationId, Guid? assetId, int value)
        {
            var apiInstance = GetApiInstance(token, serverUrl);

            try
            {
                apiInstance.ApiVapiVersionAssetsIdSubtractPutAsyncWithHttpInfo(assetId.ToString(), apiVersion, organizationId, value).Wait();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling ApiVapiVersionAssetsIdSubtractPutAsyncWithHttpInfo: "
                    + ex.Message);
            }
        }

        private static AssetsApi GetApiInstance(string token, string serverUrl)
        {
            var apiInstance = new AssetsApi(serverUrl);
            apiInstance.Configuration.AccessToken = token;
            return apiInstance;
        }
    }
}
