using System;
using System.Collections.Generic;
using System.Linq;

using JohnChess.Moves;

namespace JohnChess.Pieces
{
    public class Rook : ChessPiece
    {
        public Rook(PieceColor color, Position position)
            : base(PieceType.Rook, color, position) { }

        public override List<Move> FindMoves(Board board)
        {
            return new List<Move>()
                .Concat(GetMovesInDirection<File>(board, true))
                .Concat(GetMovesInDirection<File>(board, false))
                .Concat(GetMovesInDirection<Rank>(board, true))
                .Concat(GetMovesInDirection<Rank>(board, false))
                .ToList();
        }

        private List<Move> GetMovesInDirection<T>(Board board, bool positive)
        {
            var moves = new List<Move>();

            bool isFile = typeof(T) == typeof(File);
            int currentValue = isFile
                ? (int)Position.File
                : (int)Position.Rank;
            int incrementValue = positive ? 1 : -1;
            for (int r = currentValue + incrementValue; r <= 8 && r >= 1; r += incrementValue)
            {
                Position newPos;
                if (isFile) newPos = new Position((File)r, Position.Rank);
                else newPos = new Position(Position.File, (Rank)r);
                var newPosPiece = board[newPos];
                if (newPosPiece != null)
                {
                    if (newPosPiece.Color != this.Color)
                    {
                        // Its not null, but its capturable
                        moves.Add(MoveBuilder.CreateNormalMove(this, newPos));
                    }
                    break;
                }
                moves.Add(MoveBuilder.CreateNormalMove(this, newPos));
            }

            return moves;
        }
    }
}
