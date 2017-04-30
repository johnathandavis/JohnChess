using System;
using System.Collections.Generic;
using System.Text;

using JohnChess.Pieces;

namespace JohnChess.Moves
{
    public class NormalPieceMove
    {
        public NormalPieceMove(ChessPiece piece, Position newPosition, bool takes)
        {
            Piece = piece;
            NewPosition = newPosition;
            OldPosition = piece.Position;
            PieceCaptured = takes;
        }

        public ChessPiece Piece { get; }
        public Position NewPosition { get; }
        public Position OldPosition { get; }
        public bool PieceCaptured { get; }

        public override string ToString()
        {
            return OldPosition.ToString() +
                Piece.Type.ToNotationLetter() + " " +
                (PieceCaptured ? "x " : "") +
                NewPosition.ToString();
        }
    }
}
