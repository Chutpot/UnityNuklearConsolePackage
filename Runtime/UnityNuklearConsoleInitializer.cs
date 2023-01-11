using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chutpot.Nuklear.Console
{
    public class UnityNuklearConsoleInitializer : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void RuntimeInitializeOnLoadMethodAfterSceneLoad()
        {
            Debug.developerConsoleVisible = false;
            GameObject console = MonoBehaviour.Instantiate(Resources.Load("UnityNuklearConsole", typeof(GameObject))) as GameObject;
        }
    }
}
