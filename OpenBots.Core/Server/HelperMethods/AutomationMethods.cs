using Newtonsoft.Json;
using OpenBots.Core.Server.Models;
using OpenBots.Core.Server.User;
using OpenBots.Server.SDK.Api;
using System;
using System.Collections.Generic;
using System.IO;
using static OpenBots.Core.Server.User.EnvironmentSettings;
using AutomationModel = OpenBots.Core.Server.Models.Automation;
using SDKAutomationParameter = OpenBots.Server.SDK.Model.AutomationParameter;

namespace OpenBots.Core.Server.HelperMethods
{
    public class AutomationMethods
    {
        public static AutomationModel UploadAutomation(string name, string filePath, string automationEngine)
        {
            UserInfo userInfo = AuthMethods.GetUserInfo();
            var settings = new EnvironmentSettings();
            var apiInstance = GetApiInstance(userInfo.Token, settings.ServerUrl);

            try
            {
                using (FileStream _file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    var result = apiInstance.ApiVapiVersionAutomationsPostAsyncWithHttpInfo(apiVersion, userInfo.OrganizationId, name, _file, automationEngine).Result.Data;
                    string automationString = JsonConvert.SerializeObject(result);
                    var automation = JsonConvert.DeserializeObject<AutomationModel>(automationString);
                    return automation;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling AutomationsApi.ApiVapiVersionAutomationsPostAsyncWithHttpInfo: "
                    + ex.Message);
            }
        }

        public static void UpdateParameters(Guid? automationId, IEnumerable<AutomationParameter> automationParameters)
        {
            UserInfo userInfo = AuthMethods.GetUserInfo();
            var settings = new EnvironmentSettings();
            var apiInstance = GetApiInstance(userInfo.Token, settings.ServerUrl);

            try
            {
                var automationParametersList = new List<SDKAutomationParameter>();
                foreach (var parameter in automationParameters)
                {
                    var parameterString = JsonConvert.SerializeObject(parameter);
                    var parameterSDK = JsonConvert.DeserializeObject<SDKAutomationParameter>(parameterString);
                    automationParametersList.Add(parameterSDK);
                }
                apiInstance.ApiVapiVersionAutomationsAutomationIdUpdateParametersPostAsyncWithHttpInfo(automationId.ToString(), userInfo.OrganizationId, apiVersion, automationParametersList).Wait();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling AutomationsApi.ApiVapiVersionAutomationsPostAsyncWithHttpInfo: "
                    + ex.Message);
            }
        }

        private static AutomationsApi GetApiInstance(string token, string serverUrl)
        {
            var apiInstance = new AutomationsApi(serverUrl);
            apiInstance.Configuration.AccessToken = token;

            return apiInstance;
        }
    }
}
