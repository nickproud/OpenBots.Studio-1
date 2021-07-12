using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using OpenBots.NativeServer.Library;
using Newtonsoft.Json.Linq;

namespace OpenBots.NativeServer
{
    class Program
    {
        private static ChromeServerHost Host;
        private static bool ChromeConnectionStable = true;
        private static NamedPipeServerStream PipeServer;

        static void Main(string[] args)
        {
            Host = new ChromeServerHost();
            Host.LostConnectionToChrome += Host_LostConnectionToChrome;
            Thread serverThread = new Thread(ServerThread);
            serverThread.Start();
            Thread.Sleep(250);
            Host.Listen();
        }

        public static string OpenStandardStreamIn()
        {
            //Read 4 bytes of length information
            System.IO.Stream stdin = Console.OpenStandardInput();
            int length = 0;
            byte[] bytes = new byte[4];
            stdin.Read(bytes, 0, 4);
            length = System.BitConverter.ToInt32(bytes, 0);

            char[] buffer = new char[length];
            using (System.IO.StreamReader sr = new System.IO.StreamReader(stdin))
            {
                while (sr.Peek() >= 0)
                {
                    sr.Read(buffer, 0, buffer.Length);
                }
            }

            string input = new string(buffer);

            return input;
        }

        private static void Host_LostConnectionToChrome(object sender, EventArgs e)
        {
            ChromeConnectionStable = false;
            ResetEvent.Set();
        }

        static void ProcessConnection(IAsyncResult result)
        {
            try
            {
                PipeServer.EndWaitForConnection(result);

                // Read the request from the client. Once the client has
                // written to the pipe its security token will be available.

                ServerCommunication serverCommunication = new ServerCommunication(PipeServer);

                // Verify our identity to the connected client using a
                // string that the client anticipates.

                serverCommunication.SendMessage("OpenBots Chrome Server");

                string command = serverCommunication.ReadMessage();
                var j = new ResponseConfirmation(command);
                var commandData = JObject.Parse(j.Message);
                var timeout = Convert.ToInt32(commandData["Timeout"]);

                bool waitForReponse = true;
                bool processResponse = true;
                JObject reply = null;
                Host.MessageReceived += (s, a) =>
                {
                    //Handle Response
                    if (processResponse)
                    {
                        reply = a.Data;
                    }

                    if(reply != null)
                    {
                        if (reply.ToString().Contains("CommandSuccess") || reply.ToString().Contains("outerText") || reply.ToString().Contains("stopped"))
                        {
                            processResponse = false;
                            waitForReponse = false;
                        }
                        else if (processResponse)
                            Host.SendMessage(j.GetJObject());
                    }
                };
                Host.SendMessage(j.GetJObject());

                Stopwatch sw = new Stopwatch();
                sw.Start();
                while (waitForReponse)
                {
                    if (sw.ElapsedMilliseconds > (timeout * 1000))
                    {
                        if (sw.ElapsedMilliseconds > (30 * 1000) || reply != null)
                        {
                            //We've timed out
                            waitForReponse = false;
                            processResponse = false;
                        }
                    }
                }

                string response = "";
                if (reply != null)
                {
                    //Send reply back
                    response = reply.ToString();
                }
                else
                {
                    //Reponse failed
                    response = "{\"text\": \"Failed to communicate with chrome extension\"}";
                    var stopMessage = new ResponseConfirmation("stoplisteners");
                    Host.SendMessage(stopMessage.GetJObject());
                }

                PipeServer.RunAsClient(() => serverCommunication.SendMessage(response));
            }
            catch (IOException e)
            {
                Console.WriteLine("ERROR: {0}", e.Message);
            }

            PipeServer.Close();

            ProcessFinishedEvent.Set();
        }

        private static ManualResetEvent ResetEvent;
        private static ManualResetEvent ProcessFinishedEvent;

        private static void ServerThread(object data)
        {
            ResetEvent = new ManualResetEvent(false);

            while (true)
            {
                PipeServer = new NamedPipeServerStream("openBotsChromeServerPipe", PipeDirection.InOut, 1, PipeTransmissionMode.Message, PipeOptions.Asynchronous);
                ProcessFinishedEvent = new ManualResetEvent(false);
                // Wait for a client to connect
                PipeServer.BeginWaitForConnection(ProcessConnection, PipeServer);

                int result = WaitHandle.WaitAny(new[] { ResetEvent, ProcessFinishedEvent });

                if (!ChromeConnectionStable || result == 0)
                {
                    PipeServer.Close();
                    break;
                }
            }
        }
    }
}
