using System;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;

namespace OpenBots.NativeServer.Library
{
    public class ServerCommunication
    {
        private readonly Stream _stream;
        private readonly UnicodeEncoding _streamEncoding;

        public ServerCommunication(Stream stream)
        {
            _stream = stream;
            _streamEncoding = new UnicodeEncoding();
        }

        public string ReadMessage()
        {
            int length = _stream.ReadByte()*256;
            length += _stream.ReadByte();
            byte[] buffer = new byte[length];
            _stream.Read(buffer, 0, length);

            return _streamEncoding.GetString(buffer);
        }

        public JObject ReadMessageAsJObject()
        {
            int length = _stream.ReadByte()*256;
            length += _stream.ReadByte();
            byte[] buffer = new byte[length];
            _stream.Read(buffer, 0, length);

            string msg = _streamEncoding.GetString(buffer);

            return JObject.Parse(msg);
        }

        public int SendMessage(string outString)
        {
            byte[] buffer = _streamEncoding.GetBytes(outString);
            int length = buffer.Length;
            if (length > UInt16.MaxValue)
            {
                length = (int) UInt16.MaxValue;
            }
            _stream.WriteByte((byte) (length / 256));
            _stream.WriteByte((byte) (length & 255));
            _stream.Write(buffer, 0, length);
            _stream.Flush();

            return buffer.Length + 2;
        }
    }
}
