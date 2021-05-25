using Newtonsoft.Json;
using OpenBots.Server.SDK.Api;
using System;
using static OpenBots.Core.Server.User.EnvironmentSettings;
using CredentialModel = OpenBots.Core.Server.Models.Credential;
using SDKCredential = OpenBots.Server.SDK.Model.Credential;

namespace OpenBots.Core.Server.HelperMethods
{
    public class CredentialMethods
    {
        public static CredentialModel GetCredential(string token, string serverUrl, string organizationId, string name)
        {
            var apiInstance = GetApiInstance(token, serverUrl);

            try
            {
                var result = apiInstance.ApiVapiVersionCredentialsGetCredentialByNameCredentialNameGetAsyncWithHttpInfo(name, apiVersion, organizationId).Result.Data;
                var resultJson = JsonConvert.SerializeObject(result);
                var credential = JsonConvert.DeserializeObject<CredentialModel>(resultJson);

                return credential;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling CredentialsApi.ApiVapiVersionCredentialsGetAsyncWithHttpInfo: "
                    + ex.Message);
            }
        }

        public static void PutCredential(string token, string serverUrl, string organizationId, CredentialModel credential)
        {
            var apiInstance = GetApiInstance(token, serverUrl);

            try
            {
                var credentialJson = JsonConvert.SerializeObject(credential);
                var credentialSDK = JsonConvert.DeserializeObject<SDKCredential>(credentialJson);
                apiInstance.ApiVapiVersionCredentialsIdPutAsyncWithHttpInfo(credential.Id.ToString(), apiVersion, organizationId, credentialSDK).Wait();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling CredentialsApi.ApiVapiVersionCredentialsIdPutAsyncWithHttpInfo: "
                    + ex.Message);
            }
        }

        private static CredentialsApi GetApiInstance(string token, string serverUrl)
        {
            var apiInstance = new CredentialsApi(serverUrl);
            apiInstance.Configuration.AccessToken = token;
            return apiInstance;
        }
    }
}
