using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Chutpot.Nuklear.Console
{
    public class UnityNuklearConsole : MonoBehaviour
    {
        [DllImport("UnityNuklearConsole", CallingConvention = CallingConvention.Cdecl)]
        private static extern void InitializeConsole();

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }


        // Initialize Console at start to be sure Loader is initialized before initialization
        private void Start()
        {
            InitializeConsole();
        }
    }
}
