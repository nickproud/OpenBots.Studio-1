namespace OpenBots.Server.SDK.Model
{
    public class UserInfo
    {
        public string ServerType { get; set; }
        public string OrganizationId { get; set; }
        public string Token { get; set; }
        public string ServerUrl { get; set; }

        public string LoginUrl { get; set; }
        public string DocumentsUrl { get; set; }
    }
}
