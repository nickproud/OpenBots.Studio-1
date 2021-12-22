using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexBots.Commands.Platform.NexBots.Commands.Platform
{
    public class BotJob
    {
        public int AutomationId { get; set; }
        public int Phase { get; set; }
        public int Status { get; set; }
        public string Data { get; set; }

        public string DocumentUrl { get; set; }
        public string UniqueId { get; set; }
        public string SecondaryId { get; set; }
    }
}
