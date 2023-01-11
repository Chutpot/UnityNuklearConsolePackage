using System;

namespace Chutpot.Nuklear.Console
{
    public class ConsoleException : Exception
    {
        public ConsoleException(string message) : base(message) { }
    }

    public class ConsoleInvalidArgsException : ConsoleException
    {
        public ConsoleInvalidArgsException(string message) : base(message)
        {
        }
    }
}
