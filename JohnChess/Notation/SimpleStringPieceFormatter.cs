using System;
using System.Collections.Generic;
using System.Text;
using JohnChess.Pieces;

namespace JohnChess.Notation
{
    public abstract class SimpleStringPieceFormatter : IChessPieceFormatter
    {
        public abstract string PawnStr { get; }
        public abstract string RookStr { get; }
        public abstract string KnightStr { get; }
        public abstract string BishopStr { get; }
        public abstract string QueenStr { get; }
        public abstract string KingStr { get; }

        public string FormatPiece(PieceType pieceType)
        {
            switch (pieceType)
            {
                case PieceType.Bishop:
                    return BishopStr;
                case PieceType.King:
                    return KingStr;
                case PieceType.Knight:
                    return KnightStr;
                case PieceType.Pawn:
                    return PawnStr;
                case PieceType.Queen:
                    return QueenStr;
                case PieceType.Rook:
                    return RookStr;
                default:
                    throw new AlienChessException();
            }
        }
    }
}
