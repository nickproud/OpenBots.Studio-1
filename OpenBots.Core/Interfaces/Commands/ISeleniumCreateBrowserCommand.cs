namespace OpenBots.Core.Interfaces
{
    public interface ISeleniumCreateBrowserCommand
    {
        string v_InstanceName { get; set; }
        string v_EngineType { get; set; }
        string v_URL { get; set; }
        string v_BrowserWindowOption { get; set; }
        string v_SeleniumOptions { get; set; }
    }
}
