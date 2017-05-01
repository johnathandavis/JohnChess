using System;
using System.Collections.Generic;
using System.Text;

namespace JohnChess
{
    public class StalemateException : Exception
    {
        public StalemateException() { }
        public StalemateException(string message) : base(message) { }
        public StalemateException(string message, Exception inner) : base(message, inner) { }
    }
}
