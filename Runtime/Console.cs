using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chutpot.Nuklear.Console
{
    public static class Console
    {
        public static Action<bool> ConsoleActive;

        static Console() 
        {
            //ConsoleMono.ConsoleMonoActive += o => { ConsoleActive?.Invoke(o); };
        }

        public static void AddCommand(string command, string explation, Action callback)
        {
            ConsoleCommand.AddCommand(command, explation ,callback);
        }

        public static void AddCommand<T1>(string command, string explation, Action<T1> callback)
        {
            ConsoleCommand.AddCommand(command, explation, callback);
        }

        public static void AddCommand<T1,T2>(string command, string explation, Action<T1,T2>callback)
        {
            ConsoleCommand.AddCommand(command, explation, callback);
        }

        public static void AddCommand<T1, T2, T3>(string command, string explation, Action<T1, T2, T3> callback)
        {
            ConsoleCommand.AddCommand(command, explation, callback);
        }

        public static void AddCommand<T1, T2, T3, T4>(string command, string explation, Action<T1, T2, T3, T4> callback)
        {
            ConsoleCommand.AddCommand(command, explation, callback);
        }

        [ConsoleCommand("remove","Remove an existing non-console command", true)]
        public static void RemoveCommand(string command)
        {
            ConsoleCommand.RemoveCommand(command);
        }

        [ConsoleCommand("puts","Print console message", true)]
        [ConsoleCommand("print", "Print console message", true)]
        public static void Log(string message) 
        {
            ConsoleLogger.Log(message, ConsoleLogType.Log);
        }


        internal static void Log(string log, ConsoleLogType type)
        {
            ConsoleLogger.Log(log, type);
        }

        [ConsoleCommand("clear", "Clear console messages", true)]
        internal static void Clear() 
        {
            ConsoleLogger.Clear();
        }
    }
}
