using System;
using System.Collections.Generic;
using System.Text;

using JohnChess.Pieces;

namespace JohnChess.Moves
{
    public class MoveBuilder
    {
        public static Move CreateNormalMove(ChessPiece piece, Position newPosition, bool takes)
        {
            var move = new NormalPieceMove(piece, newPosition, takes);
            return new Move(move);
        }
    }
}
