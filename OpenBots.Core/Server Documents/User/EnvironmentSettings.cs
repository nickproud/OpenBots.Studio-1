using Newtonsoft.Json;
using OpenBots.Core.Enums;
using System;
using System.Collections.Generic;
using System.IO;

namespace OpenBots.Core.Server_Documents.User
{
    public class EnvironmentSettings
    {
        private ConfigurationKeys _configurationKeys;
        public void Load()
        {
            _configurationKeys = new ConfigurationKeys();
            var settings = GetAgentSettings();

            // Get "AgentId" to check whether the Agent is connected to the Server or not
            AgentId = settings["AgentId"];

            if (string.IsNullOrEmpty(AgentId))
                throw new Exception("Agent is not connected");

            var serverConfigSource = (ServerConfigurationSource)(string.IsNullOrEmpty(settings["ServerConfigurationSource"]) ? 
                ServerConfigurationSource.Registry : 
                Enum.Parse(typeof(ServerConfigurationSource), settings["ServerConfigurationSource"]));

            if(serverConfigSource == ServerConfigurationSource.Environment)
                LoadFromEnvironment();
            else
                LoadFromRegistry();
        }
        

        public string AgentId { get; set; } 
        public string ServerType { get; set; }
        public string OrganizationName { get; set; }
        public string ServerUrl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public string EnvironmentVariableName { get; } = "OpenBots_Agent_Data_Path";
        public string SettingsFileName { get; } = "OpenBots.settings";

        private void LoadFromEnvironment()
        {
            ServerType = GetEnvironmentVariable(_configurationKeys.OrchestratorKey);
            OrganizationName = GetEnvironmentVariable(_configurationKeys.OrganizationKey);
            ServerUrl = GetEnvironmentVariable(_configurationKeys.ServerURLKey);
            Username = GetEnvironmentVariable(_configurationKeys.UsernameKey);
            Password = GetEnvironmentVariable(_configurationKeys.PasswordKey);
        }

        private void LoadFromRegistry()
        {
            ServerType = new RegistryManager().AgentOrchestrator;
            OrganizationName = new RegistryManager().AgentOrganization;
            ServerUrl = new RegistryManager().AgentServerURL;
            Username = new RegistryManager().AgentUsername;
            Password = new RegistryManager().AgentPassword;
        }

        public string GetEnvironmentVariable()
        {
            return Environment.GetEnvironmentVariable(EnvironmentVariableName, EnvironmentVariableTarget.User);
        }

        private string GetEnvironmentVariable(string variableName)
        {
            try
            {
                return Environment.GetEnvironmentVariable(variableName, EnvironmentVariableTarget.User);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public Dictionary<string, string> GetAgentSettings()
        {
            if (GetEnvironmentVariable() == null)
                throw new Exception("Agent environment variable not found");

            string agentSettingsPath = Path.Combine(GetEnvironmentVariable(), SettingsFileName);

            if (agentSettingsPath == null || !File.Exists(agentSettingsPath))
                throw new Exception("Agent settings file not found");

            string agentSettingsText = File.ReadAllText(agentSettingsPath);
            var settings = JsonConvert.DeserializeObject<Dictionary<string, string>>(agentSettingsText);
            return settings;
        }
    }
}