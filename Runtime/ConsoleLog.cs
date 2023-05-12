using System;
using System.Runtime.InteropServices;
using System.Text;
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

    [StructLayout(LayoutKind.Sequential)]
    internal struct ConsoleLog
    {
        internal string message;
        internal ConsoleLogType logType;

        internal ConsoleLog(string message, ConsoleLogType logType)
        {
            this.message = (string.Format($"[{DateTime.Now.ToString("hh:mm:ss:ff")}] ") + message);
            this.logType = logType;
        }
    }
}
