using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBots.Core.ChromeNativeMessaging.Extension
{
    public class ChromeExtensionRegistryManager
    {
        private ChromeExtensionRegistryKeys _registryKeys;

        public ChromeExtensionRegistryManager()
        {
            _registryKeys = new ChromeExtensionRegistryKeys();
        }

        public bool IsExtensionInstalled() {
            if (string.IsNullOrEmpty(GetKeyValue(_registryKeys.PathValueKey, true))) 
                return false;
            return true;
        }

        public string VersionValue
        {
            get
            {
                return GetKeyValue(GetSubKey(false), false);
            }
            set
            {
                SetKeyValue(GetSubKey(false), value, false);
            }
        }

        public string PathValue
        {
            get
            {
                return GetKeyValue(GetSubKey(true), true);
            }
            set
            {
                SetKeyValue(GetSubKey(true), value, true);
            }
        }

        private string GetSubKey(bool IsPathSubKey)
        {
            if (IsPathSubKey)
            {
                if (Environment.Is64BitOperatingSystem)
                    return _registryKeys.SubKey64Bit;
                return _registryKeys.SubKey32Bit;
            }
            else
            {
                if (Environment.Is64BitOperatingSystem)
                    return _registryKeys.SubKey64Bit;
                return _registryKeys.SubKey32Bit;
            }

        }
        private string GetSubKeyName(bool IsPathSubKey)
        {
            if (IsPathSubKey)
                return "path";
            return "version";
        }

        private string GetKeyValue(string key, bool IsPathSubKey)
        {
            string keyValue = null;
            var registryKey = Registry.LocalMachine.OpenSubKey(GetSubKey(IsPathSubKey), true);

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

        private void SetKeyValue(string key, string value, bool IsPathSubKey)
        {
            var registryKey = Registry.LocalMachine.OpenSubKey(key, true);

            try
            {
                if (registryKey == null)
                    registryKey = Registry.LocalMachine.CreateSubKey(key);

                registryKey.SetValue(GetSubKeyName(IsPathSubKey), value);
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

        public void DeleteSubKey()
        {
            var pathRegistryKey = Registry.LocalMachine.OpenSubKey(GetSubKey(true), true);
            var versionRegistryKey = Registry.LocalMachine.OpenSubKey(GetSubKey(false), true);

            try
            {
                if (pathRegistryKey != null)
                    pathRegistryKey.DeleteSubKey(GetSubKeyName(true));
                if (versionRegistryKey != null)
                    versionRegistryKey.DeleteSubKey(GetSubKeyName(false));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                pathRegistryKey?.Close();
                versionRegistryKey?.Close();
            }
        }

    }
}
