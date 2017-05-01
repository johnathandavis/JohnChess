using System;
using System.Collections.Generic;
using System.Text;

namespace JohnChess
{
    public class CheckmateException : Exception
    {
        public CheckmateException() { }
        public CheckmateException(string message) : base(message) { }
        public CheckmateException(string message, Exception inner) : base(message, inner) { }
    }
}
