using System;
using System.Collections.Generic;
using System.Text;

using JohnChess.Pieces;

namespace JohnChess.AI
{
    public class ReinfeldPointsEvaluator : IPositionEvaluator
    {
        public const double PAWN_VALUE = 1.0;
        public const double KNIGHT_VALUE = 3.0;
        public const double BISHOP_VALUE = 3.1;
        public const double ROOK_VALUE = 5.0;
        public const double QUEEN_VALUE = 9.0;

        public double EvaluatePosition(PieceDict myPieces, PieceDict theirPieces)
        {
            double myScore = EvaluatePlayerPosition(myPieces);
            double theirScore = EvaluatePlayerPosition(theirPieces);
            return myScore - theirScore;
        }

        private double EvaluatePlayerPosition(PieceDict player)
        {
            double pawnScore = player[PieceType.Pawn].Count * PAWN_VALUE;
            double rookScore = player[PieceType.Rook].Count * ROOK_VALUE;
            double knightScore = player[PieceType.Knight].Count * KNIGHT_VALUE;
            double bishopScore = player[PieceType.Bishop].Count * BISHOP_VALUE;
            double queenScore = player[PieceType.Queen].Count * QUEEN_VALUE;

            return pawnScore + rookScore + knightScore + bishopScore + queenScore;
        }
    }
}
