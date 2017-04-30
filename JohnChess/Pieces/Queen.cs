using System;
using System.Collections.Generic;
using System.Text;

using JohnChess.Moves;

namespace JohnChess.Pieces
{
    public class Queen : ChessPiece
    {
        public Queen(PieceColor color, Position position)
            : base(PieceType.Queen, color, position) { }

        public override List<Move> FindMoves(Board board)
        {
            return new List<Move>();
        }
    }
}
