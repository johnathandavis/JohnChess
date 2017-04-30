using System;
using System.Collections.Generic;
using System.Text;

using JohnChess.Pieces;

namespace JohnChess.Moves
{
    public class NormalPieceMove
    {
        public NormalPieceMove(ChessPiece piece, Position newPosition)
        {
            Piece = piece;
            NewPosition = newPosition;
            OldPosition = piece.Position;
        }

        public ChessPiece Piece { get; }
        public Position NewPosition { get; }
        public Position OldPosition { get; }

        public string ToString(bool pieceCaptured)
        {
            return OldPosition.ToString() +
                Piece.Type.ToNotationLetter() + " " +
                (pieceCaptured ? "x " : "") +
                NewPosition.ToString();
        }
    }
}
