using System;
using System.Collections.Generic;
using System.Linq;

using JohnChess.Pieces;
using JohnChess.AI.Evaluation;
using JohnChess.AI.Enumeration;

namespace JohnChess.AI.JohnJohn.Evaluation
{
    public class RookEvaluator : PowerPieceEvaluator, IPositionEvaluator
    {
        private const double ROOK_STARTGAME_VALUE = 3.5;
        private const double ROOK_ENDGAME_VALUE = 5.5;
        private const double DOUBLE_ROOK_BONUS = 0.5;

        public double EvaluatePosition(MoveTreeNode moveTree, PieceDict myPieces, PieceDict theirPieces, int moveNumber)
        {
            var myRooks = myPieces[PieceType.Rook];
            double rookValue = 0.0;
            foreach (var r in myRooks)
            {
                rookValue += EvaluatePiece(moveTree, r, moveNumber);
            }
            if (myRooks.Count >= 2)
            {
                rookValue += DOUBLE_ROOK_BONUS;
            }
            return rookValue;
        }
        private double EvaluatePiece(MoveTreeNode moveTree, ChessPiece piece, int moveNumber)
        {
            int maxMoves = 40;
            int normalizedMoveCount = Math.Min(moveNumber, maxMoves);
            double startingValue = ROOK_STARTGAME_VALUE +
                (ROOK_ENDGAME_VALUE - ROOK_STARTGAME_VALUE) *
                ((double)normalizedMoveCount / (double)maxMoves);

            double moveBonus = CalculateBonusForMoves(piece, moveTree);
            return startingValue + moveBonus;
        }
    }
}
