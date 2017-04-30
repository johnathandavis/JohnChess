using System;
using System.Collections.Generic;
using System.Text;

using JohnChess.Pieces;

namespace JohnChess.Moves
{
    public class Move
    {
        public Move(NormalPieceMove normal)
        {
            NormalPieceMove = normal;
            Type = MoveType.NormalPiece;
        }
        public Move(CastleMove castle)
        {
            Castle = castle;
            Type = MoveType.Castle;
        }
        public Move(EnPassantMove enPassant)
        {
            EnPassant = enPassant;
            Type = MoveType.EnPassant;
        }
        public Move(PromotionMove promotion)
        {
            Promotion = promotion;
            Type = MoveType.Promotion;
        }

        public MoveType Type { get; }
        public NormalPieceMove NormalPieceMove { get; }
        public CastleMove Castle { get; }
        public EnPassantMove EnPassant { get; }
        public PromotionMove Promotion { get; }
        // Only set after move evaluated by board object!
        public bool PieceCaptured { get; set; }

        public override string ToString()
        {
            switch (Type)
            {
                case MoveType.Castle:
                    return Castle.ToString();
                case MoveType.EnPassant:
                    return EnPassant.ToString();
                case MoveType.NormalPiece:
                    return NormalPieceMove.ToString(PieceCaptured);
                case MoveType.Promotion:
                    return Promotion.ToString(PieceCaptured);
                default:
                    throw new Exception("Alien Chess Error");
            }
        }
        
    }
}
