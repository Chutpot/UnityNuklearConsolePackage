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
            string buffer = "Total Console Commands: " + ConsoleCommand.Commands.Count + "\n";

            foreach (var command in ConsoleCommand.Commands)
            {
                buffer += "\t\t\t\t" + command.Key + " -> \"" + command.Value.Explanation + "\"";
                if (command.Value.ParamaterTypes != null) 
                {
                    var paramsBuffer = string.Empty;
                    foreach (var param in command.Value.ParamaterTypes)
                    {
                        paramsBuffer += $"<{param.Name}>";
                    }
                    if(paramsBuffer != string.Empty) 
                    {
                        paramsBuffer = ": params" + paramsBuffer;
                    }
                    buffer += paramsBuffer + "\n";
                }
            }

            Console.Log(buffer);
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
            GUIUtility.systemCopyBuffer = ConsoleLogger.Logs[index].message;

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
            string buffer = string.Empty;
            buffer += $"{Application.productName}, {Application.version}, {Application.unityVersion} \n";
            foreach(var arg in Environment.GetCommandLineArgs()) 
            {
                buffer += $"\t\t\t\t{arg} \n";
            }
 
            Console.Log(buffer);
        }
    }
}
