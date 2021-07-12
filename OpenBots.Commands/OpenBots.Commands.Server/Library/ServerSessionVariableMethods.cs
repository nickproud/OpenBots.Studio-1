using OpenBots.Core.Interfaces;
using OpenBots.Core.Server_Documents.User;
using OpenBots.Server.SDK.HelperMethods;
using OpenBots.Server.SDK.Model;

namespace OpenBots.Commands.Server.Library
{
    public class ServerSessionVariableMethods
    {
        public static UserInfo GetUserInfo(IAutomationEngineInstance engine)
        {
            var environmentSettings = new EnvironmentSettings();
            environmentSettings.Load();
            AuthMethods authMethods = new AuthMethods();
            authMethods.Initialize(environmentSettings.ServerType, environmentSettings.OrganizationName, environmentSettings.ServerUrl, environmentSettings.Username, environmentSettings.Password, environmentSettings.AgentId);

            var sessionVariablesDict = engine.EngineContext.SessionVariables;
            object sessionUserInfo;

            bool keyExists = sessionVariablesDict.TryGetValue("UserInfo", out sessionUserInfo);

            if (!keyExists)
            {
                sessionUserInfo = authMethods.GetUserInfo();
                SetAuthSessionVariables(engine, (UserInfo)sessionUserInfo);
            }

            return (UserInfo)sessionUserInfo;
        }

        public static void SetAuthSessionVariables(IAutomationEngineInstance engine, UserInfo userInfo)
        {
            var sessionVariablesDict = engine.EngineContext.SessionVariables;
            object sessionUserInfo;

            bool keyExists = sessionVariablesDict.TryGetValue("UserInfo", out sessionUserInfo);
            if (!keyExists)
                sessionVariablesDict.Add("UserInfo", userInfo);
            else 
            {
                var originalUserInfo = (UserInfo)sessionUserInfo;
                string token = originalUserInfo.Token;
                if (token != userInfo.Token)
                {
                    sessionVariablesDict.Remove("UserInfo");
                    sessionVariablesDict.Add("UserInfo", userInfo);
                }
            }
        }

        public static UserInfo RefreshToken(IAutomationEngineInstance engine)
        {
            var sessionVariablesDict = engine.EngineContext.SessionVariables;

            UserInfo userInfo = (UserInfo)sessionVariablesDict["UserInfo"];
            sessionVariablesDict.Remove("UserInfo");

            //use refresh token and get new auth/refresh tokens
            var environmentSettings = new EnvironmentSettings();
            environmentSettings.Load();
            AuthMethods authMethods = new AuthMethods();
            authMethods.Initialize(environmentSettings.ServerType, environmentSettings.OrganizationName, environmentSettings.ServerUrl, environmentSettings.Username, environmentSettings.Password, environmentSettings.AgentId);

            authMethods.RefreshToken(userInfo);

            return userInfo;
        }
    }
}
