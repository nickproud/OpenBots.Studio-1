using Newtonsoft.Json;

namespace OpenBots.Core.ChromeNativeClient
{
    public class WebElement
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("outerText")]
        public string OuterText { get; set; }

        [JsonProperty("tagName")]
        public string TagName { get; set; }

        [JsonProperty("className")]
        public string ClassName { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
        
        [JsonProperty("cssSelector")]
        public string CssSelector { get; set; }
        
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("linkText")]
        public string LinkText { get; set; }

        [JsonProperty("xpath")]
        public string XPath { get; set; }

        [JsonProperty("relXPath")]
        public string RelXPath { get; set; }

        [JsonProperty("cssSelectors")]
        public string CssSelectors { get; set; }

        [JsonProperty("selectionRules")]
        public string SelectionRules { get; set; }

    }
}
