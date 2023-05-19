using AOT;
using Chutpot.Nuklear.Loader;
using NuklearDotNet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using static NuklearDotNet.Nuklear;


namespace Chutpot.Nuklear.Console
{

    public unsafe class UnityNuklearConsole : MonoBehaviour, INuklearApp
    {

        private bool _isOpen;
        private StringBuilder _buffer;

        private void Awake()
        {
            _isOpen = false;
            _buffer = new StringBuilder(255);
            DontDestroyOnLoad(gameObject);
            Application.logMessageReceived += ConsoleLogger.OnLogMessageReceived;
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

        private void Start()
        {
            ConsoleCommands.List();
            UnityNuklearRenderer.AddNuklearApp(this);
        }


        private void OnDestroy()
        {
            Application.logMessageReceived -= ConsoleLogger.OnLogMessageReceived;
            UnityNuklearRenderer.RemoveNuklearApp(this);
        }

        public unsafe void Render(nk_context* ctx)
        {
            if (!_isOpen)
                return;

            const NkPanelFlags flags = NkPanelFlags.Border | NkPanelFlags.Movable | NkPanelFlags.Minimizable | NkPanelFlags.Title | NkPanelFlags.NoScrollbar | NkPanelFlags.Scalable;

            if (nk_begin(ctx, "Chutpot Console", new NkRect(50, 50, 750, 300),
                (uint)flags) != 0)
            {

                nk_layout_row_dynamic(ctx, nk_window_get_height(ctx) - 70.0f, 1);
                if (nk_group_begin(ctx, "Group", (uint)NkPanelFlags.Border) != 0)
                {
                    nk_layout_row_static(ctx, 18, (int)(nk_window_get_width(ctx) - 60.0f), 1);
                    foreach (ConsoleLog log in ConsoleLogger.Logs)
                    {
                        nk_label_colored(ctx, log.message, (uint)NkTextAlignment.NK_TEXT_LEFT, log.color);
                    }
                    nk_group_end(ctx);
                }
                nk_layout_row_dynamic(ctx, 20.0f, 1);

                
                uint result = nk_edit_string_zero_terminated(ctx, (uint)(NkEditFlags.AlwaysInsertMode |NkEditFlags.Selectable | NkEditFlags.Clipboard | NkEditFlags.SigEnter), _buffer, _buffer.MaxCapacity, (ref nk_text_edit TextBox, uint Rune) => 1);
                if ((result & (int)(NkEditEvents.Commited)) > 0)
                {
                    ConsoleCommand.ExecuteCommand(_buffer.ToString());
                    _buffer.Clear();
                    nk_group_set_scroll(ctx, "Group", uint.MaxValue, uint.MaxValue);
                }
                
            }
            nk_end(ctx);
        }
    }
}
