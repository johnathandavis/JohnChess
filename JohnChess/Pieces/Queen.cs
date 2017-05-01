using System;
using System.Collections.Generic;
using System.Linq;

using JohnChess.Moves;

namespace JohnChess.Pieces
{
    public class Queen : ChessPiece
    {
        public Queen(PieceColor color, Position position)
            : base(PieceType.Queen, color, position) { }

        public override ChessPiece MoveTo(Position position)
        {
            return new Queen(Color, position);
        }
        public override List<Move> FindMoves(Board board)
        {
            IEnumerable<Move> moves = new List<Move>();
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    // Don't consider the zero-magnitude, this would cause
                    // an infinite loop!
                    if (x == y && x == 0) continue;

                    moves = moves.Concat(GetMovesInDirection(board, x, y));
                }
            }
            return moves.ToList();
        }

        private List<Move> GetMovesInDirection(Board board, int vert, int horiz)
        {
            var moves = new List<Move>();
            int currentRank = (int)Position.Rank;
            int currentFile = (int)Position.File;
            int magnitude = 1;

            while (true)
            {
                int newRank = currentRank + magnitude * vert;
                int newFile = currentFile + magnitude * horiz;
                var newPos = new Position((File)newFile, (Rank)newRank);

                if (!Position.IsOnBoard(newPos)) break;
                var currentPiece = board[newPos];
                if (currentPiece != null)
                {
                    if (currentPiece.Color != this.Color)
                    {
                        // We can take!
                        moves.Add(MoveBuilder.CreateNormalMove(this, newPos, true));
                    }
                    break;
                }
                moves.Add(MoveBuilder.CreateNormalMove(this, newPos, false));
                magnitude++;
            }

            return moves;
        }
    }
}
