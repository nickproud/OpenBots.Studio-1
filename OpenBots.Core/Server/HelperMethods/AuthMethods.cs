using Newtonsoft.Json;
using OpenBots.Core.Server.Models;
using static OpenBots.Core.Server.User.EnvironmentSettings;
using OpenBots.Server.SDK.Api;
using OpenBots.Core.Server.User;

namespace OpenBots.Core.Server.HelperMethods
{
    public class AuthMethods
    {
        public static UserInfo GetUserInfo()
        {
            var settings = new EnvironmentSettings();
            var apiInstance = new AuthApi(settings.ServerUrl);

            var response = apiInstance.GetUserInfo(apiVersion, settings.AgentId, settings.ServerType, settings.OrganizationName, environment, settings.ServerUrl, username, password);
            string responseString = JsonConvert.SerializeObject(response);
            var userInfo = JsonConvert.DeserializeObject<UserInfo>(responseString);

            return userInfo;
        }
    }
}
