using System;
using System.Collections.Generic;
using System.Linq;

using JohnChess.Moves;

namespace JohnChess.Pieces
{
    public class Bishop : ChessPiece
    {
        public Bishop(PieceColor color, Position position)
            : base(PieceType.Bishop, color, position) { }

        public override ChessPiece MoveTo(Position position)
        {
            return new Bishop(Color, position);
        }
        public override List<Move> FindMoves(Board board)
        {
            return GetMovesWithDirection(board, 1, 1)
                .Concat(GetMovesWithDirection(board, -1, 1))
                .Concat(GetMovesWithDirection(board, -1, -1))
                .Concat(GetMovesWithDirection(board, 1, -1))
                .ToList();
        }

        private List<Move> GetMovesWithDirection(Board board, int vertIncrement, int horizIncrement)
        {
            var moves = new List<Move>();

            int r = (int)Position.Rank;
            int f = (int)Position.File;

            while (true)
            {
                r = r + vertIncrement;
                f = f + horizIncrement;

                bool validPosition = (f <= 8 && f >= 1) && (r <= 8 && r >= 1);
                if (!validPosition) break;

                var newPos = new Position((File)f, (Rank)r);
                var currentPiece = board[newPos];
                if (currentPiece != null)
                {
                    if (currentPiece.Color != this.Color)
                    {
                        // We can't go any further, but we can take!
                        moves.Add(MoveBuilder.CreateNormalMove(this,
                            newPos, true));
                    }
                    break;
                }
                moves.Add(MoveBuilder.CreateNormalMove(this,
                    newPos, false));
            }
            return moves;
        }
    }
}
