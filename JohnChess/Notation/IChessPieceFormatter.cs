using System;
using System.Collections.Generic;
using System.Text;

using JohnChess.Pieces;

namespace JohnChess.Notation
{
    public interface IChessPieceFormatter
    {
        string FormatPiece(PieceType pieceType);
    }
}
