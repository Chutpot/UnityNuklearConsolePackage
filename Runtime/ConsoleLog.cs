using NuklearDotNet;
using System;
using UnityEngine;

namespace Chutpot.Nuklear.Console
{
    internal enum ConsoleLogType
    {
        Error = LogType.Error,
        Assert = LogType.Assert,
        Warning = LogType.Warning,
        Log = LogType.Log,
        Exception = LogType.Exception,
        Console,
        Command,
    }

    internal struct ConsoleLog
    {
        internal string message { get; private set; }
        internal ConsoleLogType logType { get; private set; }

        internal NkColor color { get; private set; }

        internal ConsoleLog(string message, ConsoleLogType logType)
        {
            this.message = string.Format($"[{DateTime.Now.ToString("hh:mm:ss:ff")}] ") + message;
            this.logType = logType;
            this.color = GetColor(logType);
        }

        private static NkColor GetColor(ConsoleLogType logType)
        {

            switch (logType)
            {
                case ConsoleLogType.Error:
                    return new NkColor(255, 76, 76, 255);
                case ConsoleLogType.Assert:
                    return new NkColor(255, 76, 76, 255);
                case ConsoleLogType.Warning:
                    return new NkColor(255, 255, 0, 255);
                case ConsoleLogType.Log:
                    return new NkColor(255, 255, 200, 255);
                case ConsoleLogType.Exception:
                    return new NkColor(255, 0, 0, 255);
                case ConsoleLogType.Console:
                    return new NkColor(255, 255, 200, 255);
                case ConsoleLogType.Command:
                    return new NkColor(0, 255, 128, 255);
                default:
                    return new NkColor(255, 255, 200, 255);
            }
        }
    }
}