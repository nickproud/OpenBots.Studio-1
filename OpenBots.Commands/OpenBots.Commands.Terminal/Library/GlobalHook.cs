using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace OpenBots.Commands.Terminal.Library
{
    public class GlobalHook
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int ToUnicode(uint virtualKeyCode, uint scanCode, byte[] keyboardState, StringBuilder receivingBuffer, int bufferSize, uint flags);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern short GetKeyState(int keyCode);

        [Flags]
        private enum _keyStates
        {
            None = 0,
            Down = 1,
            Toggled = 2
        }

        public static bool IsKeyDown(Keys key)
        {
            return _keyStates.Down == (GetKeyState(key) & _keyStates.Down);
        }

        public static bool IsKeyToggled(Keys key)
        {
            return _keyStates.Toggled == (GetKeyState(key) & _keyStates.Toggled);
        }

        private static _keyStates GetKeyState(Keys key)
        {
            _keyStates state = _keyStates.None;

            short retVal = GetKeyState((int)key);

            //If the high-order bit is 1, the key is down
            //otherwise, it is up.
            if ((retVal & 0x8000) == 0x8000)
                state |= _keyStates.Down;

            //If the low-order bit is 1, the key is toggled.
            if ((retVal & 1) == 1)
                state |= _keyStates.Toggled;

            return state;
        }

    }
}
