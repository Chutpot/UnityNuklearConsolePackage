using AOT;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEditor.Search;
using UnityEngine;
using static Chutpot.Nuklear.Loader.UnityNuklearRenderer;
using static UnityEngine.Networking.UnityWebRequest;

namespace Chutpot.Nuklear.Console
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct CommandStruct
    {
        public string message;
        public int lenght;
    }


    public class UnityNuklearConsole : MonoBehaviour
    {
        [DllImport("UnityNuklearConsole", CallingConvention = CallingConvention.Cdecl)]
        private static extern void InitializeConsole(SendCommandCallback callback);
        [DllImport("UnityNuklearConsole", CallingConvention = CallingConvention.Cdecl)]
        internal extern static void AddLog(ConsoleLog log);
        [DllImport("UnityNuklearConsole", CallingConvention = CallingConvention.Cdecl)]
        internal extern static void Clear();
        [DllImport("UnityNuklearConsole", CallingConvention = CallingConvention.Cdecl)]
        internal extern static CommandStruct PopCommand();

        public delegate void SendCommandCallback(IntPtr log, int size);


        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Application.logMessageReceived += ConsoleLogger.OnLogMessageReceived;
        }

        private void Update()
        {
            var commandStruct = PopCommand();
            var command = Marshal.PtrToStructure<>;
            while (!string.IsNullOrWhiteSpace(command))
            {
                Debug.Log(command);
                ConsoleCommand.ExecuteCommand(command);
                PopCommand(str, length);
                command = Marshal.PtrToStringAnsi(str, Marshal.PtrToStructure<int>(length));
            }
        }

        private void OnDestroy()
        {
            Application.logMessageReceived -= ConsoleLogger.OnLogMessageReceived;
        }

        // Initialize Console at start to be sure Loader is initialized before initialization
        private void Start()
        {
            InitializeConsole(OnSendCommandCallback);
            ConsoleCommands.List();
        }

#if ENABLE_MONO
        [MonoPInvokeCallback(typeof(SendCommandCallback))]
#elif ENABLE_IL2CPP
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
#endif
        private static void OnSendCommandCallback(IntPtr log, int size)
        {
            // oddly Try catch block someshow prevent from crashing
            try
            {
                string command = Marshal.PtrToStringAnsi(log, size);
                ConsoleCommand.ExecuteCommand(command);
            }
            catch (Exception e) 
            { 
            }

        }
    }
}
