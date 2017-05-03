using System;
using System.Collections.Generic;
using System.Text;

using JohnChess.Moves;

namespace JohnChess.Notation
{
    public class LongFormMoveFormatter : IChessMoveFormatter
    {
        public string FormatMove(Move move, IChessPieceFormatter pieceFormatter)
        {
            switch (move.Type)
            {
                case MoveType.NormalPiece:
                    return FormatNormalMove(move.NormalPieceMove, pieceFormatter);
                case MoveType.Castle:
                    return FormatCastle(move.Castle, pieceFormatter);
                case MoveType.EnPassant:
                    return FormatEnPassant(move.EnPassant, pieceFormatter);
                case MoveType.Promotion:
                    return FormatPromotion(move.Promotion, pieceFormatter);
                default:
                    throw new AlienChessException();
            }
        }

        private string FormatNormalMove(NormalPieceMove move, IChessPieceFormatter pieceFormatter)
        {
            string pieceStr = pieceFormatter.FormatPiece(move.Piece.Type);

            return pieceStr
                + move.OldPosition.ToString() + " "
                + (move.PieceCaptured ? "x " : "")
                + move.NewPosition.ToString();
        }

        private string FormatCastle(CastleMove move, IChessPieceFormatter pieceFormatter)
        {
            return move.Type == CastleMoveType.KingSide ? "OO" : "OOO";
        }

        private string FormatEnPassant(EnPassantMove move, IChessPieceFormatter pieceFormatter)
        {
            string pieceStr = pieceFormatter.FormatPiece(Pieces.PieceType.Pawn);

            return pieceStr
                + move.AttackingPawn.Position.ToString() + " x "
                + move.DestinationPosition.ToString() + "e.p.";
        }

        private string FormatPromotion(PromotionMove move, IChessPieceFormatter pieceFormatter)
        {
            string promotingPiece = pieceFormatter.FormatPiece(Pieces.PieceType.Pawn);
            string newPiece = pieceFormatter.FormatPiece(move.NewPieceType.Type);

            return promotingPiece +
                move.OldPosition.ToString() +
                (move.PieceCaptured ? " x " : " ") +
                move.NewPosition.ToString() + " = " +
                newPiece;
        }
    }
}
