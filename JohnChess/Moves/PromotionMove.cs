using System;
using System.Collections.Generic;
using System.Text;

using JohnChess.Pieces;

namespace JohnChess.Moves
{
    public class PromotionMove
    {
        public PromotionMove(Pawn promotingPiece, PromotingPieceType newPieceType, bool takes)
        {
            PromotingPiece = promotingPiece;
            NewPieceType = newPieceType;
            PieceCaptured = takes;
        }

        public Pawn PromotingPiece { get; }
        public PromotingPieceType NewPieceType { get; }
        public bool PieceCaptured { get; }

        public string ToString(bool pieceCaptured)
        {
            return "";
        }

        public class PromotingPieceType
        {
            private PromotingPieceType(PieceType type)
            {
                Type = type;
            }
            public PieceType Type { get; }

            public static PromotingPieceType Bishop
            {
                get { return new PromotingPieceType(PieceType.Bishop); }
            }
            public static PromotingPieceType Knight
            {
                get { return new PromotingPieceType(PieceType.Knight); }
            }
            public static PromotingPieceType Rook
            {
                get { return new PromotingPieceType(PieceType.Rook); }
            }
            public static PromotingPieceType Queen
            {
                get { return new PromotingPieceType(PieceType.Queen); }
            }
        }
    }
}
