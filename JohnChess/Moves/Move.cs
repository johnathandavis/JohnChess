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
        public bool PieceCaptured
        {
            get
            {
                switch (Type)
                {
                    // Definition of En Passant means a piece was captured
                    case MoveType.EnPassant:
                        return true;
                    // Definition of Castle means no piece was captured    
                    case MoveType.Castle:
                        return false;
                    case MoveType.NormalPiece:
                        return NormalPieceMove.PieceCaptured;
                    case MoveType.Promotion:
                        return Promotion.PieceCaptured;
                    default:
                        throw new AlienChessException();
                }
            }
        }

        public override string ToString()
        {
            switch (Type)
            {
                case MoveType.Castle:
                    return Castle.ToString();
                case MoveType.EnPassant:
                    return EnPassant.ToString();
                case MoveType.NormalPiece:
                    return NormalPieceMove.ToString();
                case MoveType.Promotion:
                    return Promotion.ToString();
                default:
                    throw new AlienChessException();
            }
        }
        
    }
}
