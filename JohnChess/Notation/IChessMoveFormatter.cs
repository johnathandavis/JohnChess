using System;
using System.Collections.Generic;
using System.Text;

using JohnChess.Moves;

namespace JohnChess.Notation
{
    public interface IChessMoveFormatter
    {
        string FormatMove(Move move, IChessPieceFormatter pieceFormatter);
    }
}
