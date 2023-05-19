using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace Chutpot.Nuklear.Console
{
    internal class ConsoleCommand
    {
        private static readonly Dictionary<string, ConsoleCommand> _commands = new Dictionary<string, ConsoleCommand>();

        internal static Dictionary<string, ConsoleCommand> Commands { get { return _commands; } }

        internal string Command { get; private set; }
        internal string Explanation { get; private set; }
        internal Type[] ParamaterTypes { get; private set; }
        internal bool IsPersistent { get; private set; }
        private Action _callback;
        private Action<object[]> _callbackParameter;


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void InitializeAttributeCommands()
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    foreach (MethodInfo method in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
                    {
                        foreach (object attribute in method.GetCustomAttributes(typeof(ConsoleCommandAttribute), false))
                        {
                            ConsoleCommandAttribute commandAttribute = (ConsoleCommandAttribute)attribute;
                            if (commandAttribute != null)
                            {
                                AddCommand(commandAttribute, method);
                            }
                        }
                    }
                }
            }
        }

        private ConsoleCommand(string command, string explanation,Action callback)
        {
            Command = command;
            Explanation = explanation;
            _callback = callback;
            _callbackParameter = null;
            ParamaterTypes = null;
            IsPersistent = false;
        }

        private ConsoleCommand(string command, string explanation, Action<object[]> callbackParameter, Type[] paramterTypes, bool isPersistent)
        {
            Command = command;
            Explanation = explanation;
            _callbackParameter = callbackParameter;
            _callback = null;
            ParamaterTypes = paramterTypes;
            IsPersistent = isPersistent;
        }

        internal static void AddCommand(string command, string explanation, Action callback)
        {
            if (string.IsNullOrEmpty(command))
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (!string.IsNullOrEmpty(command) && !_commands.ContainsKey(command))
            {
                _commands.Add(command.ToLower(), new ConsoleCommand(command, explanation, callback));
            }
        }

        internal static void AddCommand<T1>(string command, string explanation, Action<T1> callback)
        {
            if (string.IsNullOrEmpty(command))
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (!string.IsNullOrEmpty(command) && !_commands.ContainsKey(command))
            {
                _commands.Add(command.ToLower(), new ConsoleCommand(command, explanation,
                    o => callback((T1)o[0]), new Type[] {typeof(T1)}, false));
            }
        }

        internal static void AddCommand<T1, T2>(string command, string explanation, Action<T1,T2> callback)
        {
            if (string.IsNullOrEmpty(command))
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (!string.IsNullOrEmpty(command) && !_commands.ContainsKey(command))
            {
                _commands.Add(command.ToLower(), new ConsoleCommand(command, explanation,
                    o => callback((T1)o[0],(T2)o[1]), new Type[] { typeof(T1), typeof(T2) }, false));
            }
        }

        internal static void AddCommand<T1, T2, T3>(string command, string explanation, Action<T1, T2, T3> callback)
        {
            if (string.IsNullOrEmpty(command))
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (!string.IsNullOrEmpty(command) && !_commands.ContainsKey(command))
            {
                _commands.Add(command.ToLower(), new ConsoleCommand(command, explanation,
                    o => callback((T1)o[0], (T2)o[1], (T3)o[2]), new Type[] { typeof(T1), typeof(T2), typeof(T3) }, false));
            }
        }

        internal static void AddCommand<T1, T2, T3, T4>(string command, string explanation, Action<T1, T2, T3, T4> callback)
        {
            if (string.IsNullOrEmpty(command))
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (!string.IsNullOrEmpty(command) && !_commands.ContainsKey(command))
            {
                _commands.Add(command.ToLower(), new ConsoleCommand(command, explanation,
                    o => callback((T1)o[0], (T2)o[1], (T3)o[2], (T4)o[3]), new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) }, false));
            }
        }

        internal static void AddCommand(ConsoleCommandAttribute attribute, MethodInfo method)
        {
            ParameterInfo[] parameterInfos = method.GetParameters();
            Type[] parameterTypes = new Type[parameterInfos.Length];

            for (int i = 0; i < parameterTypes.Length; i++)
            {
                parameterTypes[i] = parameterInfos[i].ParameterType;
            }

            if (!_commands.ContainsKey(attribute.CommandName))
            {
                _commands.Add(attribute.CommandName, new ConsoleCommand(attribute.CommandName, attribute.Explanation, o => method.Invoke(null, o), parameterTypes, attribute.IsPersistent));
            }


        }

        internal static void RemoveCommand(string command)
        {
            if (string.IsNullOrEmpty(command))
            {
                throw new ArgumentNullException(nameof(command));
            }

            ConsoleCommand _removeCommand = GetCommand(command);

            if(_removeCommand == null) 
            {
                Debug.LogException(new NullReferenceException());
                return;
            }


            if (!_removeCommand.IsPersistent)
            {
                _commands.Remove(_removeCommand.Command);
            }
            else
            {
                Console.Log("Can't remove console commands");
            }

        }

        internal static ConsoleCommand GetCommand(string command)
        {
            if (_commands.TryGetValue(command.ToLower(), out ConsoleCommand getCommand))
            {
                return getCommand;
            }
            else
            {
                return null; 
            }
        }

        // Need Refactoring
        internal static void ExecuteCommand(string rawInput)
        {
            if (string.IsNullOrEmpty(rawInput))
            {
                //throw new ArgumentNullException(nameof(rawInput));
                return;
            }

            string[] args = GetArgs(rawInput);

            ConsoleCommand command = GetCommand(args[0]);

            if(command == null) 
            {
                Console.Log("Command could not be found");
                return;
            }

            if (command.ParamaterTypes != null) 
            {
                if (args.Skip(1).ToArray().Length != command.ParamaterTypes.Length)
                {
                    Console.Log("Invalid Argument Counts");
                    return;
                }
            }


            command.Execute(args.Skip(1).ToArray());
        }

        // Need Refactoring
        internal static string[] GetArgs(string rawInput) 
        {
            return rawInput.Split(' ');
        }

        // Need Refactoring
        internal void Execute(string[] args)
        {
            if(args.Count() == 0) 
            {
                if (_callback != null) 
                {
                    _callback();
                }
                else 
                {
                    _callbackParameter(null);
                }

            }
            else 
            {
                object[] parameters = new object[args.Count()];

                for (int i = 0; i < parameters.Length; i++)
                {
                    parameters[i] = Convert.ChangeType(args[i], ParamaterTypes[i]);
                }

                _callbackParameter(parameters);
            }
        }
    }
}
