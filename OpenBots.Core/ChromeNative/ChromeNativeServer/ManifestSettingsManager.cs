using Newtonsoft.Json;
using OpenBots.Core.Enums;
using OpenBots.Core.IO;
using System;
using System.IO;

namespace OpenBots.Core.ChromeNative.ChromeNativeServer
{
    public class ManifestSettingsManager
    {
        public void Save()
        {
            ManifestSettings manifestSettings = new ManifestSettings();
            string extensionLocalDir;
            //create settings directory if not provided
            extensionLocalDir = Folders.GetFolder(FolderType.LocalAppDataExtensionsFolder);

            //create file path
            var filePath = Path.Combine(extensionLocalDir, "com.openbots.chromeserver.message-manifest.json");

            if (!File.Exists(filePath))
            {
                var serializerSettings = new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.Objects
                };
                JsonSerializer serializer = JsonSerializer.Create(serializerSettings);

                //output to json file
                //if output path was provided
                try
                {
                    using (StreamWriter sw = new StreamWriter(filePath))
                    using (JsonWriter writer = new JsonTextWriter(sw) { Formatting = Formatting.Indented })
                    {
                        serializer.Serialize(writer, manifestSettings, typeof(ManifestSettings));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
    }
}
