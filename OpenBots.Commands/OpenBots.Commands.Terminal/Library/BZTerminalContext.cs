using BZWHLLLib;
using System;
using System.IO;
using System.Security;

namespace OpenBots.Commands.Terminal.Library
{
    public class BZTerminalContext
    {
        //public dynamic BZTerminalObj { get; set; }
        public WhllObj BZTerminalObj { get; set; }
        public string Username { get; set; }
        public SecureString Password { get; set; }

        public BZTerminalContext()
        {
            string blueZonePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "BlueZone");
            if (!Directory.Exists(blueZonePath))
                throw new DirectoryNotFoundException("Unable to find BlueZone in Program Files (x86).");

            //BZTerminalObj = Activator.CreateInstance(Type.GetTypeFromProgID("BZWhll.WhllObj"));
            BZTerminalObj = new WhllObj();
        }
    }
}
