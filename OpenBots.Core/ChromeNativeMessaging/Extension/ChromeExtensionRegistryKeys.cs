
namespace OpenBots.Core.ChromeNativeMessaging.Extension
{
    public class ChromeExtensionRegistryKeys
    {
        public string SubKey64Bit { get; } = @"SOFTWARE\Google\Chrome\Extensions\loogmojebgbncnlekokmekjapmkfdahh";
        public string SubKey32Bit { get; } = @"Google\Chrome\Extensions\loogmojebgbncnlekokmekjapmkfdahh";
        public string PathValueKey { get; set; }
        public string VersionValueKey { get; set; }


    }
}
