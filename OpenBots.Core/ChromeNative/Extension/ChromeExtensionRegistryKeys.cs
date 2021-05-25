
namespace OpenBots.Core.ChromeNative.Extension
{
    public class ChromeExtensionRegistryKeys
    {
        public string SubKey64Bit { get; } = @"Software\Google\Chrome\Extensions\kkepankimcahnjamnimeijpplgjpmdpp";
        public string SubKey32Bit { get; } = @"Google\Chrome\Extensions\kkepankimcahnjamnimeijpplgjpmdpp";
        public string PathValueKey { get; set; }
        public string VersionValueKey { get; set; }

        // Native Server
        public string SubKeyNativeSever64Bit { get; } = @"Software\Google\Chrome\NativeMessagingHosts\com.openbots.chromeserver.message";
        public string SubKeyNativeServer32Bit { get; } = @"Google\Chrome\NativeMessagingHosts\com.openbots.chromeserver.message";

    }
}
