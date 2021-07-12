using Microsoft.Win32;
using OpenBots.Core.Utilities.CommonUtilities;
using System;

namespace OpenBots.Core.Server_Documents.User
{
    public class RegistryManager
    {
        private ConfigurationKeys _configurationKeys;

        public RegistryManager()
        {
            _configurationKeys = new ConfigurationKeys();
        }

        public string AgentUsername
        {
            get
            {
                return GetKeyValue(_configurationKeys.UsernameKey);
            }
            set
            {
                SetKeyValue(_configurationKeys.UsernameKey, value);
            }
        }

        public string AgentPassword
        {
            get
            {
                string password = GetKeyValue(_configurationKeys.PasswordKey);
                return string.IsNullOrEmpty(password) ? "" : StringMethods.DecryptText(password, SystemInfo.GetMacAddress());
            }
            set
            {
                SetKeyValue(_configurationKeys.PasswordKey, StringMethods.EncryptText(value, SystemInfo.GetMacAddress()));
            }
        }
        public string AgentServerURL
        {
            get
            {
                return GetKeyValue(_configurationKeys.ServerURLKey);
            }
            set
            {
                SetKeyValue(_configurationKeys.ServerURLKey, value);
            }
        }
        public string AgentOrganization
        {
            get
            {
                return GetKeyValue(_configurationKeys.OrganizationKey);
            }
            set
            {
                SetKeyValue(_configurationKeys.OrganizationKey, value);
            }
        }
        public string AgentOrchestrator
        {
            get
            {
                return GetKeyValue(_configurationKeys.OrchestratorKey);
            }
            set
            {
                SetKeyValue(_configurationKeys.OrchestratorKey, value);
            }
        }
        private string GetKeyValue(string key)
        {
            string keyValue = null;
            var registryKey = Registry.CurrentUser.OpenSubKey(_configurationKeys.SubKey, true);

            try
            {
                if (registryKey != null)
                    keyValue = registryKey.GetValue(key)?.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                registryKey?.Close();
            }

            return keyValue;
        }

        private void SetKeyValue(string key, string value)
        {
            var registryKey = Registry.CurrentUser.OpenSubKey(_configurationKeys.SubKey, true);

            try
            {
                if (registryKey == null)
                    registryKey = Registry.CurrentUser.CreateSubKey(_configurationKeys.SubKey);

                registryKey.SetValue(key, value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                registryKey?.Close();
            }
        }
    }
}
