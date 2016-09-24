using System;

namespace Swimbait.Server.Services
{
    public class ConsoleKeyEventArgs : EventArgs
    {
        public ConsoleKeyInfo KeyInfo { get; set; }

        public ConsoleKeyEventArgs(ConsoleKeyInfo keyInfo)
        {
            KeyInfo = keyInfo;
        }
    }

    public class KeyHandler
    {
        public event EventHandler<ConsoleKeyEventArgs> KeyEvent;

        public void WaitForExit()
        {
            bool exit = false;
            do
            {
                var key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.Q:
                        exit = true;
                        break;
                    default:
                        KeyEvent?.Invoke(this, new ConsoleKeyEventArgs(key));
                        break;
                }
            }
            while (!exit);

           // Log.Info(m => m("Q pressed, exiting"));
        }
    }
}
