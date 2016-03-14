using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        //protected static ILog Log = LogManager.GetCurrentClassLogger();

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
                        if (KeyEvent != null)
                        {
                            KeyEvent(this, new ConsoleKeyEventArgs(key));
                        }
                        break;
                }
            }
            while (!exit);

           // Log.Info(m => m("Q pressed, exiting"));
        }
    }
}
