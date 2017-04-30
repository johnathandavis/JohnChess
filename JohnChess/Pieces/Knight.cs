using System;
using System.Collections.Generic;
using System.Text;

using JohnChess.Moves;

namespace JohnChess.Pieces
{
    public class Knight : ChessPiece
    {
        public Knight(PieceColor color, Position position)
            : base(PieceType.Knight, color, position) { }

        public override List<Move> FindMoves(Board board)
        {
            var moves = new List<Move>();
            int[] indices = new int[] { -2, -1, 1, 2 };
            foreach (int x in indices)
            foreach (int y in indices)
            {
                if (Math.Abs(x) == Math.Abs(y)) continue;

                moves.AddMoveIfValid(() =>
                    MoveBuilder.CreateNormalMove(this, this.Position
                        .MoveHoriz(x)
                        .MoveVert(y))
                );
            }

            return moves;
        }
    }
}
