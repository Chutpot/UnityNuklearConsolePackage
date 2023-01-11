using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chutpot.Nuklear.Console
{
    internal class ConsoleLogger : IEnumerable
    {
        private static readonly List<ConsoleLog> _logs = new List<ConsoleLog>();

        public static List<ConsoleLog> Logs { get { return _logs; } }

        internal ConsoleLogger() 
        {
            Application.logMessageReceived += OnLogMessageReceived;
            Log("Chutpot Console", ConsoleLogType.Console);
            Log("List commands with command \"List\"", ConsoleLogType.Console);
        }

        ~ConsoleLogger() 
        {
            Application.logMessageReceived -= OnLogMessageReceived;
        }

        public IEnumerator GetEnumerator()
        {
            return _logs.GetEnumerator();
        }

        internal static void Clear() 
        {
            _logs.Clear();
        }

        internal static void Log(string log, ConsoleLogType type)
        {
            _logs.Add(new ConsoleLog($"[{_logs.Count}] {log}", type));
        }

        private void OnLogMessageReceived(string condition, string stackTrace, LogType type)
        {
            _logs.Add(new ConsoleLog($"[{_logs.Count}]" + condition + " @ " + stackTrace, (ConsoleLogType)type));
        }
    }
}
