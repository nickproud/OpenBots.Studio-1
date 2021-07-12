using System;
using Newtonsoft.Json;

namespace OpenBots.Core.ChromeNativeClient
{
    public class NativeResponse
    {
        [JsonProperty("result")]
        public string Result { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
