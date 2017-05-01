using System;
using System.Collections.Generic;
using System.Text;

using JohnChess.Pieces;

namespace JohnChess.Moves
{
    public class PromotionMove
    {
        public PromotionMove(Pawn promotingPiece, Position newPosition, PromotingPieceType newPieceType, bool takes)
        {
            PromotingPiece = promotingPiece;
            NewPosition = newPosition;
            OldPosition = promotingPiece.Position;
            NewPieceType = newPieceType;
            PieceCaptured = takes;
        }

        public Pawn PromotingPiece { get; }
        public Position OldPosition { get; }
        public Position NewPosition { get; }
        public PromotingPieceType NewPieceType { get; }
        public bool PieceCaptured { get; }

        public override string ToString()
        {
            return PromotingPiece.Type.ToNotationLetter() +
                OldPosition.ToString() + " " +
                (PieceCaptured ? "x " : "") +
                NewPosition.ToString() + " = " +
                NewPieceType.Type.ToNotationLetter();
        }

        public class PromotingPieceType
        {
            private PromotingPieceType(PieceType type)
            {
                Type = type;
            }
            public PieceType Type { get; }

            public static readonly PromotingPieceType Bishop = new PromotingPieceType(PieceType.Bishop);
            public static readonly PromotingPieceType Knight = new PromotingPieceType(PieceType.Knight);
            public static readonly PromotingPieceType Rook = new PromotingPieceType(PieceType.Rook);
            public static readonly PromotingPieceType Queen = new PromotingPieceType(PieceType.Queen);

            public static IReadOnlyList<PromotingPieceType> PromotingPieceTypes
            {
                get
                {
                    return new List<PromotingPieceType>(new PromotingPieceType[]
                    {
                        Bishop,
                        Knight,
                        Rook,
                        Queen
                    });
                }
            }
        }
    }
}
