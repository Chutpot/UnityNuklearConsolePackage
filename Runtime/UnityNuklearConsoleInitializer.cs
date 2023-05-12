using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chutpot.Nuklear.Console
{
    public class UnityNuklearConsoleInitializer : MonoBehaviour
    {
        public static GameObject GO { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void RuntimeInitializeOnLoadMethodAfterSceneLoad()
        {
            Debug.developerConsoleVisible = false;
            GO = MonoBehaviour.Instantiate(Resources.Load("UnityNuklearConsole", typeof(GameObject))) as GameObject;
        }
    }
}
