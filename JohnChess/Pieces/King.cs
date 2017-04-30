using System;
using System.Collections.Generic;
using System.Text;

using JohnChess.Moves;

namespace JohnChess.Pieces
{
    public class King : ChessPiece
    {
        public King(PieceColor color, Position position)
            : base(PieceType.King, color, position) { }

        public override List<Move> FindMoves(Board board)
        {
            return new List<Move>();
        }
    }
}
