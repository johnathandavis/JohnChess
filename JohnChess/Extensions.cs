using System;
using System.Collections.Generic;
using System.Text;

namespace JohnChess
{
    public static class Extensions
    {
        public static Rank Increment(this Rank rank, int increment = 0)
        {
            return (Rank)((int)rank + increment);
        }

        public static Position MoveVert(this Position pos, int magnitude)
        {
            var file = pos.File;
            var rankVal = (int)pos.Rank + magnitude;
            if (rankVal <= 0 || rankVal > 8)
                throw new InvalidOperationException("Not a piece on the board!");
            var rank = (Rank)rankVal;
            
            return new Position(file, rank);
        }
        public static Position MoveHoriz(this Position pos, int magnitude)
        {
            var fileVal = (int)pos.File + magnitude;
            var file = (File)(fileVal);
            if (fileVal <= 0 || fileVal > 8)
                throw new InvalidOperationException("Not a piece on the board!");
            var rank = pos.Rank;
            return new Position(file, rank);
        }
        public static string ToNotationLetter(this Pieces.PieceType type)
        {
            switch (type)
            {
                case Pieces.PieceType.Bishop:
                    return "b";
                case Pieces.PieceType.King:
                    return "k";
                case Pieces.PieceType.Knight:
                    return "n";
                case Pieces.PieceType.Pawn:
                    return "p";
                case Pieces.PieceType.Queen:
                    return "q";
                case Pieces.PieceType.Rook:
                    return "r";
                default:
                    throw new Exception("Alien Chess Error");
            }
        }
        public static PieceColor Opposite(this PieceColor color)
        {
            return color == PieceColor.White ? PieceColor.Black : PieceColor.White;
        }
    }
}
