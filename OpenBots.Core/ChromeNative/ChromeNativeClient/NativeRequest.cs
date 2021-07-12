using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO.Pipes;
using System.Security.Principal;

namespace OpenBots.Core.ChromeNativeClient
{
    public static class NativeRequest
    {
        public static bool ProcessRequest(string requestMethod, string input, int timeout, out string output)
        {
            int timer = 30;
            using (NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", "openBotsChromeServerPipe", PipeDirection.InOut, PipeOptions.None, TokenImpersonationLevel.Impersonation))
            {
                try
                {
                    while (timer > 0)                
                    {
                        try
                        {
                            pipeClient.Connect(1000);
                            break;
                        }
                        catch (System.TimeoutException)
                        {
                            timer--;
                        }
                        catch(Exception ex)
                        {
                            throw ex;
                        }
                    }

                    ServerCommunication serverCommunication = new ServerCommunication(pipeClient);

                    if (serverCommunication.ReadMessage() == "OpenBots Chrome Server")
                    {
                        RequestMessage messageRequest = new RequestMessage
                        {
                            Method = requestMethod,
                            Body = input,
                            Timeout = timeout.ToString()
                        };

                        string messageRequestStr = JsonConvert.SerializeObject(messageRequest);
                        serverCommunication.SendMessage(messageRequestStr);
                        JObject responseObject = serverCommunication.ReadMessageAsJObject();
                        output = responseObject["text"].ToString();
                        if (output == "Failed to communicate with chrome extension")
                            throw new Exception(output);

                        return true;
                    }

                    output = null;
                    return false;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
