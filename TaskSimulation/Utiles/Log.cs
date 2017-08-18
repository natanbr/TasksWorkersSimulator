using System;
using System.IO;

namespace TaskSimulation
{
    public class Log
    {
        private static Log _logInstance ;

        public static Log Instance { get { return _logInstance = _logInstance ?? new Log(); } }

        private Log()
        {
        }

        public void Info(string msg, ConsoleColor color)
        {
            var before = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(msg, color);
            Console.ForegroundColor = before;
        }

        private void Debug(string msg, ConsoleColor color)
        {
            Info(msg, color);
        }

        public static void I(string msg = "", ConsoleColor color = ConsoleColor.Gray)
        {
            Instance.Info(msg,color);
        }

        public static void D(string msg = "", ConsoleColor color = ConsoleColor.DarkGray)
        {
#if DEBUG
            Instance.Debug(msg, color);
#endif
        }

        public static void Event(string msg = "")
        {
            Instance.Info("" + msg, ConsoleColor.Green);
        }

        public static void Err(string msg = "")
        {
            Instance.Info("!!! "+ msg + "!!!", ConsoleColor.Red);
        }
    }
}
