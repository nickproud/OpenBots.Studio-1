using Newtonsoft.Json;
using OpenBots.Core.Server.Models;
using OpenBots.Server.SDK.Api;
using System;
using System.Linq;
using static OpenBots.Core.Server.User.EnvironmentSettings;

namespace OpenBots.Commands.Server.HelperMethods
{
    public class QueueMethods
    {
        public static Queue GetQueue(string token, string serverUrl, string organizationId, string filter)
        {
            var apiInstance = new QueuesApi(serverUrl);
            apiInstance.Configuration.AccessToken = token;

            try
            {
                var result = apiInstance.ApiVapiVersionQueuesGetAsyncWithHttpInfo(apiVersion, organizationId, filter).Result.Data.Items.FirstOrDefault();
                string queueString = JsonConvert.SerializeObject(result);
                var queue = JsonConvert.DeserializeObject<Queue>(queueString);
                return queue;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Exception when calling QueuesApi.ApiVapiVersionQueuesGetAsyncWithHttpInfo: "
                    + ex.Message);
            }
        }
    }
}
