using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace JohnChess.Pieces
{
    public class PieceBuilder
    {
        private readonly PieceType pieceType;
        private Position position;
        private PieceColor color;

        public PieceBuilder(PieceType type)
        {
            pieceType = type;
        }
        
        public PieceBuilder As(PieceColor color)
        {
            this.color = color;
            return this;
        }
        public PieceBuilder At(Position position)
        {
            this.position = position;
            return this;
        }

        public ChessPiece Create()
        {
            switch (pieceType)
            {
                case PieceType.Bishop:
                    return new Bishop(color, position, ImmutableList<Moves.Move>.Empty);
                case PieceType.King:
                    return new King(color, position, ImmutableList<Moves.Move>.Empty);
                case PieceType.Knight:
                    return new Knight(color, position, ImmutableList<Moves.Move>.Empty);
                case PieceType.Pawn:
                    return new Pawn(color, position, ImmutableList<Moves.Move>.Empty);
                case PieceType.Queen:
                    return new Queen(color, position, ImmutableList<Moves.Move>.Empty);
                case PieceType.Rook:
                    return new Rook(color, position, ImmutableList<Moves.Move>.Empty);
                default:
                    throw new Exception("What alien version of chess are you playing?");
            }
        }
    }
}
