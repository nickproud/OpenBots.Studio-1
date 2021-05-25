using System;
using Newtonsoft.Json.Linq;

namespace OpenBots.NativeServer.Library
{
    public class MessageReceievedArgs : EventArgs
    {
        public JObject Data
        {
            get;
            set;
        }
    }

    public delegate void MessageReceivedHandler(object sender, MessageReceievedArgs args);

    public class ChromeServerHost : Host
    {
        public event MessageReceivedHandler MessageReceived;

        public override string Hostname
        {
            get { return "com.openbots.chromeserver.message"; }
        }

        public ChromeServerHost()
        {

        }

        protected override void ProcessReceivedMessage(JObject data)
        {
            if (MessageReceived != null)
            {
                MessageReceived(this, new MessageReceievedArgs() {Data = data});
            }
        }
    }
}
