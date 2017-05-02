using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace JohnChess.Pieces
{
    public abstract class ChessPiece
    {
        protected ImmutableList<Moves.Move> moveHistory;

        public ChessPiece(PieceType type, PieceColor color, Position position, ImmutableList<Moves.Move> moveHistory)
        {
            Type = type;
            Color = color;
            Position = new Position(position.File, position.Rank);
            this.moveHistory = moveHistory;
        }

        public Position Position { get; }
        public PieceColor Color { get; }
        public PieceType Type { get; }
        public IReadOnlyList<Moves.Move> MoveHistory
        {
            get
            {
                return moveHistory;
            }
        }

        public abstract List<Moves.Move> FindMoves(Board board);

        protected int IncrementDirection
        {
            get
            {
                return (Color == PieceColor.White) ? 1 : -1;
            }
        }

        public abstract ChessPiece MoveTo(Position position);
        public abstract ChessPiece AddMoveToHistory(Moves.Move move);
        public ChessPiece AddRangeToMoveHistory(List<Moves.Move> moves)
        {
            ChessPiece piece = this;
            foreach (var m in moves)
            {
                piece = piece.AddMoveToHistory(m);
            }
            return piece;
        }

        internal void ClearHistory()
        {
            moveHistory = ImmutableList<Moves.Move>.Empty;
        }
    }
}
