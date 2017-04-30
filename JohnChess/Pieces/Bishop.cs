using System;
using System.Collections.Generic;
using System.Text;

using JohnChess.Moves;

namespace JohnChess.Pieces
{
    public class Bishop : ChessPiece
    {
        public Bishop(PieceColor color, Position position)
            : base(PieceType.Bishop, color, position) { }

        public override List<Move> FindMoves(Board board)
        {
            return new List<Move>();
        }
    }
}
