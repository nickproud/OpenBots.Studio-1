using Newtonsoft.Json;
using OpenBots.Core.Server.Models;
using static OpenBots.Core.Server.User.EnvironmentSettings;
using OpenBots.Server.SDK.Api;

namespace OpenBots.Core.Server.HelperMethods
{
    public class AuthMethods
    {
        public static UserInfo GetUserInfo()
        {
            var apiInstance = new AuthApi(serverUrl);

            var response = apiInstance.GetUserInfo(apiVersion, agentId, serverType, organizationName, environment, serverUrl, username, password);
            string responseString = JsonConvert.SerializeObject(response);
            var userInfo = JsonConvert.DeserializeObject<UserInfo>(responseString);

            return userInfo;
        }
    }
}
