using System;
using System.Collections.Generic;
using System.Text;

using JohnChess.Pieces;

namespace JohnChess.Moves
{
    public class MoveBuilder
    {
        public static Move CreateNormalMove(ChessPiece piece, Position newPosition)
        {
            var move = new NormalPieceMove(piece, newPosition);
            return new Move(move);
        }
    }
}
