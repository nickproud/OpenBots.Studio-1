using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBots.Core.ChromeNativeClient
{
    public class RequestMessage
    {
        public string Method { get; set; }
        public string Body { get; set; }
    }
}
