using System;
using System.Collections.Generic;
using System.Text;

namespace JohnChess.Pieces
{
    public abstract class ChessPiece
    {
        public ChessPiece(PieceType type, PieceColor color, Position position)
        {
            Type = type;
            Color = color;
            Position = new Position(position.File, position.Rank);
        }

        public Position Position { get; }
        public PieceColor Color { get; }
        public PieceType Type { get; }
        public List<Moves.Move> MoveHistory { get; } = new List<Moves.Move>();

        public abstract List<Moves.Move> FindMoves(Board board);


        protected int IncrementDirection
        {
            get
            {
                return (Color == PieceColor.White) ? 1 : -1;
            }
        }

        public abstract ChessPiece MoveTo(Position position);
    }
}
