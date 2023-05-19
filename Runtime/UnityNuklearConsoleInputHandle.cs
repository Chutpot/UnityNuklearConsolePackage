using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Chutpot.Nuklear.Console
{
    public class UnityNuklearConsoleInputHandle : MonoBehaviour
    {

        private bool _isOpen;

        private void Awake()
        {
            _isOpen = false;
        }

        private void Update()
        {
#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKeyDown(KeyCode.Tilde) || Input.GetKeyDown(KeyCode.BackQuote))
            {
                _isOpen = !_isOpen;
            }
#endif
        }
    }
}
