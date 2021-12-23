using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexBots.Commands.Platform
{
    public class ProcessHistory
    {
        public int ProcessId { get; set; }
        public int ProcessPhase { get; set; }
        public int ProcessSubPhase { get; set; }
        public string OutputTitle { get; set; }
        public string Output { get; set; }
        public int OutputType { get; set; }
        public string OutputGroup { get; set; }
        public int CompletionTimeInSeconds { get; set; }
        public int JobId { get; set; }
        public int TaskId { get; set; }
    }
}
