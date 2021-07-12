using Microsoft.Win32;
using System;

namespace OpenBots.Core.ChromeNative.Extension
{
    public class ChromeExtensionRegistryManager
    {
        private ChromeExtensionRegistryKeys _registryKeys;

        public ChromeExtensionRegistryManager()
        {
            _registryKeys = new ChromeExtensionRegistryKeys();
        }

        public bool IsExtensionInstalled() {
            if (string.IsNullOrEmpty(GetKeyValue(GetSubKey(true), true, "path"))) 
                return false;
            return true;
        }

        public string VersionValue
        {
            get
            {
                return GetKeyValue(GetSubKey(false), false, "version");
            }
            set
            {
                SetKeyValue(GetSubKey(false), value, GetSubKeyName(false));
            }
        }

        public string PathValue
        {
            get
            {
                return GetKeyValue(GetSubKey(true), true, "path");
            }
            set
            {
                SetKeyValue(GetSubKey(true), value, GetSubKeyName(true));
            }
        }

        public string NativeServerKey
        {
            set
            {
                SetKeyValue(Environment.Is64BitOperatingSystem ? _registryKeys.SubKeyNativeSever64Bit : _registryKeys.SubKeyNativeServer32Bit, value, "");
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

        private string GetKeyValue(string key, bool IsPathSubKey, string keyName )
        {
            string keyValue = null;
            var registryKey = Registry.CurrentUser.OpenSubKey(GetSubKey(IsPathSubKey), true);

            try
            {
                if (registryKey != null)
                    keyValue = registryKey.GetValue(keyName)?.ToString();
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

        private void SetKeyValue(string key, string value, string keyName)
        {
            var registryKey = Registry.CurrentUser.OpenSubKey(key, true);

            try
            {
                if (registryKey == null)
                    registryKey = Registry.CurrentUser.CreateSubKey(key);

                registryKey.SetValue(keyName, value);
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
            var registryKey = Registry.CurrentUser.OpenSubKey(GetSubKey(true), true);
            var nativeRegistryKey = Registry.CurrentUser.OpenSubKey(Environment.Is64BitOperatingSystem ? _registryKeys.SubKeyNativeSever64Bit : _registryKeys.SubKeyNativeServer32Bit, true);

            try
            {
                if (registryKey != null)
                    registryKey.DeleteSubKeyTree("");
                if (nativeRegistryKey != null)
                    nativeRegistryKey.DeleteSubKeyTree("");
            }
            catch (Exception)
            {
                //throw ex;
            }
            finally
            {
                registryKey?.Close();
                nativeRegistryKey?.Close();
            }
        }

    }
}
