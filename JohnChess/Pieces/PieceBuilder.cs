﻿using System;
using System.Collections.Generic;
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
                    return new Bishop(color, position);
                case PieceType.King:
                    return new King(color, position);
                case PieceType.Knight:
                    return new Knight(color, position);
                case PieceType.Pawn:
                    return new Pawn(color, position);
                case PieceType.Queen:
                    return new Queen(color, position);
                case PieceType.Rook:
                    return new Rook(color, position);
                default:
                    throw new Exception("What alien version of chess are you playing?");
            }
        }
    }
}