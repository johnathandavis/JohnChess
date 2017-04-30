using System;
using System.Collections.Generic;
using System.Text;

namespace JohnChess
{
    public class AlienChessException : Exception
    {
        private const string DEFAULT_MESSAGE = "It seems you are not playing chess from this planet...";

        public AlienChessException() : base(DEFAULT_MESSAGE) { }
        public AlienChessException(string message) : base(message) { }
        public AlienChessException(string message, Exception inner) : base(message, inner) { }
    }
}
