namespace OpenBots.Core.Interfaces
{
    public interface ISendKeystrokesCommand
    {
        string v_WindowName { get; set; }
        string v_TextToSend { get; set; }
    }
}
