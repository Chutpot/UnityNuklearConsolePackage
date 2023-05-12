using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Chutpot.Nuklear.Console
{
    internal static class ConsoleLogger
    {
        private static readonly List<ConsoleLog> _logs = new List<ConsoleLog>();
        public static List<ConsoleLog> Logs { get { return _logs; } }

        static ConsoleLogger() 
        {
            Log("Chutpot Console", ConsoleLogType.Console);
            Log("List commands with command \"List\"", ConsoleLogType.Console);
        }

        [ConsoleCommand("Clear", "Clear the logs", true)]
        internal static void Clear() 
        {
            _logs.Clear();
            UnityNuklearConsole.Clear();
        }

        internal static void Log(string log, ConsoleLogType type)
        {
            _logs.Add(new ConsoleLog($"[{_logs.Count}] {log}", type));
            UnityNuklearConsole.AddLog(new ConsoleLog($"[{_logs.Count}] {log}", type));
        }

        internal static void OnLogMessageReceived(string condition, string stackTrace, LogType type)
        {
            _logs.Add(new ConsoleLog($"[{_logs.Count}] " + condition + " @ " + stackTrace, (ConsoleLogType)type));
            UnityNuklearConsole.AddLog(new ConsoleLog($"[{_logs.Count}] " + condition + " @ " + stackTrace, (ConsoleLogType)type));
        }
    }
}
