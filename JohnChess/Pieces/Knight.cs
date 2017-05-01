using System;
using System.Collections.Generic;
using System.Text;

using JohnChess.Moves;

namespace JohnChess.Pieces
{
    public class Knight : ChessPiece
    {
        public Knight(PieceColor color, Position position, List<Moves.Move> moveHistory)
            : base(PieceType.Knight, color, position, moveHistory) { }

        public override ChessPiece MoveTo(Position position)
        {
            return new Knight(Color, position, MoveHistory);
        }
        public override List<Move> FindMoves(Board board)
        {
            var moves = new List<Move>();
            int[] indices = new int[] { -2, -1, 1, 2 };
            foreach (int x in indices)
            {
                foreach (int y in indices)
                {
                    if (Math.Abs(x) == Math.Abs(y)) continue;
                    if (!Position.IsOnBoard((int)Position.Rank + y, (int)Position.File + x)) continue;
                    var newPos = Position.MoveHoriz(x).MoveVert(y);
                    bool enemyOccupied = (board[newPos]?.Color.Opposite() == this.Color);
                    moves.Add(MoveBuilder.CreateNormalMove(this, newPos, enemyOccupied));
                }
            }

            return moves;
        }
    }
}
