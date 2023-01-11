using System;

namespace Chutpot.Nuklear.Console
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class ConsoleCommandAttribute : Attribute
    {
        internal string CommandName;
        internal string Explanation;
        internal bool IsPersistent;

        public ConsoleCommandAttribute(string commandName, string explanation)
        {
            CommandName = commandName;
            Explanation = explanation;
            IsPersistent = false;
        }

        internal ConsoleCommandAttribute(string commandName, string explanation,bool isPersistent) 
        {
            CommandName = commandName;
            Explanation= explanation;
            IsPersistent = isPersistent;
        }
    }
}
