using Chutpot.Nuklear.Loader;
using NuklearDotNet;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Profiling;
using static NuklearDotNet.Nuklear;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif


namespace Chutpot.Nuklear.Console
{

    public unsafe class UnityNuklearConsole : MonoBehaviour, INuklearApp
    {
        internal static Action<bool> ConsoleActive;

        private readonly List<string> _commandHistory = new List<string>();
        private int _commandHistoryIndex = -1;
        private bool _isConsoleActive;
        private StringBuilder _command;
#if ENABLE_INPUT_SYSTEM
        private Keyboard _keyboard = Keyboard.current;
#endif

        private void Awake()
        {
            _command = new StringBuilder(255);
            DontDestroyOnLoad(gameObject);
            Application.logMessageReceived += ConsoleLogger.OnLogMessageReceived;
        }

        private void Update()
        {
#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKeyDown(KeyCode.Tilde) || Input.GetKeyDown(KeyCode.BackQuote))
            {
                _isConsoleActive = !_isConsoleActive;
                ConsoleActive?.Invoke(_isConsoleActive);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                _commandHistoryIndex = Mathf.Clamp(_commandHistoryIndex + 1, -1, _commandHistory.Count - 1);
                if (_commandHistoryIndex == -1)
                {
                    _command.Clear();
                }
                else
                {
                    _command.Clear();
                    _command.Insert(0,_commandHistory[_commandHistoryIndex]);
                }

            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                _commandHistoryIndex = Mathf.Clamp(_commandHistoryIndex - 1, -1, _commandHistory.Count);
                if (_commandHistoryIndex == -1)
                {
                    _command.Clear();
                }
                else
                {
                    _command.Insert(0, _commandHistory[_commandHistoryIndex]);
                }

            }
#elif ENABLE_INPUT_SYSTEM
            if (_keyboard.quoteKey.wasPressedThisFrame || _keyboard.backquoteKey.wasPressedThisFrame)
            {
                _isConsoleActive = !_isConsoleActive;
                ConsoleActive?.Invoke(_isConsoleActive);
            }
            else if (_keyboard.upArrowKey.wasPressedThisFrame)
            {
                _commandHistoryIndex = Mathf.Clamp(_commandHistoryIndex + 1, -1, _commandHistory.Count - 1);
                if (_commandHistoryIndex == -1)
                {
                    _command.Clear();
                }
                else
                {
                    _command.Clear();
                    _command.Insert(0,_commandHistory[_commandHistoryIndex]);
                }

            }
            else if (_keyboard.downArrowKey.wasPressedThisFrame)
            {
                _commandHistoryIndex = Mathf.Clamp(_commandHistoryIndex - 1, -1, _commandHistory.Count);
                if (_commandHistoryIndex == -1)
                {
                    _command.Clear();
                }
                else
                {
                    _command.Insert(0, _commandHistory[_commandHistoryIndex]);
                }

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
            if (!_isConsoleActive)
                return;

            const NkPanelFlags flags = NkPanelFlags.Border | NkPanelFlags.Movable | NkPanelFlags.Minimizable | NkPanelFlags.Title | NkPanelFlags.NoScrollbar | NkPanelFlags.Scalable;

            var fps = (int)(1f / Time.smoothDeltaTime);
            var mem = string.Format("{0:0.00} GB", Profiler.GetTotalAllocatedMemoryLong() / 1024.0 / 1024.0 / 1024.0);
            var session = TimeSpan.FromSeconds(Time.realtimeSinceStartup).ToString(@"hh\:mm\:ss\:fff");

            if (nk_begin_titled(ctx, "Chutpot Console", $"Chutpot Console | FPS: {fps} | MEM: {mem} | Session: {session}", new NkRect(50, 50, 750, 300),
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

                
                
                uint result = nk_edit_string_zero_terminated(ctx, (uint)(NkEditFlags.AlwaysInsertMode |NkEditFlags.Selectable | NkEditFlags.Clipboard | NkEditFlags.SigEnter), _command, _command.MaxCapacity, UnityNuklearRenderer.NkPluginFilterCallback);
                if ((result & (int)(NkEditEvents.Commited)) > 0)
                {
                    Console.Log("> " + _command.ToString(), ConsoleLogType.Command);
                    ConsoleCommand.ExecuteCommand(_command.ToString());
                    _commandHistory.Add(_command.ToString());
                    _command.Clear();
                    nk_group_set_scroll(ctx, "Group", uint.MaxValue, uint.MaxValue);
                }
                
                
            }
            nk_end(ctx);
        }
    }
}
