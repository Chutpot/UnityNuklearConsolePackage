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

        internal Vector4 color { get; private set; }

        internal ConsoleLog(string message, ConsoleLogType logType)
        {
            this.message = string.Format($"[{DateTime.Now.ToString("hh:mm:ss:ff")}] ") + message;
            this.logType = logType;
            this.color = GetColor(logType);
        }

        private static Vector4 GetColor(ConsoleLogType logType) 
        {

            switch (logType) 
            {
                case ConsoleLogType.Error:
                    return new Vector4(1f, 0.3f, 0.3f, 1f);
                case ConsoleLogType.Assert:
                    return new Vector4(1f, 0.3f, 0.3f, 1f);
                case ConsoleLogType.Warning:
                    return new Vector4(1f,1f,0f,1f);
                case ConsoleLogType.Log:
                    return new Vector4(1f, 1f, 0.8f, 1f);
                case ConsoleLogType.Exception:
                    return new Vector4(1f, 0f, 0f, 1f);
                case ConsoleLogType.Console:
                    return new Vector4(1f, 1f, 0.8f, 1f);
                case ConsoleLogType.Command:
                    return new Vector4(0f, 1f, 0.5f, 1f);
                default:
                    return new Vector4(1f, 1f, 0.8f, 1f);
            }
        }
    }
}
