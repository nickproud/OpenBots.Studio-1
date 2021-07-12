using Newtonsoft.Json;
using OpenBots.Core.Enums;
using OpenBots.Core.IO;
using System.Collections.Generic;

namespace OpenBots.Core.ChromeNative.ChromeNativeServer
{
    public class ManifestSettings
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("allowed_origins")]
        public List<string> AllowedOrigins { get; set; }

        public ManifestSettings()
        {
            AllowedOrigins = new List<string>();
            Name = "com.openbots.chromeserver.message";
            Description = "OpenBots Native Messaging Host for Chrome";
            Path = System.IO.Path.Combine(Folders.GetFolder(FolderType.ProgramFilesExtensionsFolder), "NativeServer", "OpenBots.NativeServer.exe"); ;
            Type = "stdio";
            AllowedOrigins.Add("chrome-extension://kkepankimcahnjamnimeijpplgjpmdpp/");
        }
    }
}
