using System;
using UnityEngine;
using UnityEditor;

namespace Chutpot.Nuklear.Console
{
    internal static class ConsoleCommands
    {
        [ConsoleCommand("list", "List all existing commands", true)]
        internal static void List()
        {
            Console.Log("Total Console Commands: " + ConsoleCommand.Commands.Count);

            foreach (var command in ConsoleCommand.Commands)
            {
                var buffer = command.Key + " -> \"" + command.Value.Explanation + "\"";

                if (command.Value.ParamaterTypes != null)
                {
                    buffer += ", params";
                    foreach (var param in command.Value.ParamaterTypes)
                    {
                        buffer += $"<{param.Name}>";
                    }
                }
                Console.Log(buffer);
            }
        }

        [ConsoleCommand("help", "Get help about console", true)]
        internal static void Help() 
        {
            Console.Log("Chutpot Console");
            Console.Log("List commands with command \"List\"");
        }

        [ConsoleCommand("copyat", "Copy the message at the index", true)]
        internal static void CopyAt(int index)
        {
            GUIUtility.systemCopyBuffer = ConsoleLogger.Logs[index].message.ToString();
        }

        [ConsoleCommand("time", "Print the current time", true)]
        internal static void Time() 
        {
            Console.Log(DateTime.Now.ToString());
        }

        [ConsoleCommand("quit", "Quit the game or stop playing", true)]
        internal static void Quit()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        [ConsoleCommand("game", "Print details about build", true)]
        internal static void Game() 
        {
            Console.Log($"{Application.productName}, {Application.version}, {Application.unityVersion}");
            foreach(var arg in Environment.GetCommandLineArgs()) 
            {
                Console.Log(arg);
            }
 
        }
    }
}
