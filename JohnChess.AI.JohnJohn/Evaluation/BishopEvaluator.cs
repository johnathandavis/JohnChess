using System;
using System.Collections.Generic;
using System.Linq;

using JohnChess.Pieces;
using JohnChess.AI.Evaluation;
using JohnChess.AI.Enumeration;

namespace JohnChess.AI.JohnJohn.Evaluation
{
    public class BishopEvaluator : PowerPieceEvaluator, IPositionEvaluator
    {
        private const double BISHOP_STARTGAME_VALUE = 3.6;
        private const double BISHOP_ENDGAME_VALUE = 3.1;
        private const double DOUBLE_BISHOP_BONUS = 0.75;

        public double EvaluatePosition(MoveTreeNode moveTree, PieceDict myPieces, PieceDict theirPieces, int moveNumber)
        {
            var myBishops = myPieces[PieceType.Bishop];
            double bishopValue = 0.0;
            foreach (var r in myBishops)
            {
                bishopValue += EvaluatePiece(r, moveTree, moveNumber);
            }
            if (myBishops.Count >= 2)
            {
                bishopValue += DOUBLE_BISHOP_BONUS;
            }
            return bishopValue;
        }

        private double EvaluatePiece(ChessPiece piece, MoveTreeNode moveTree, int moveNumber)
        {
            int maxMoves = 40;
            int normalizedMoveCount = Math.Min(moveNumber, maxMoves);
            double startingValue = BISHOP_STARTGAME_VALUE +
                (BISHOP_ENDGAME_VALUE - BISHOP_STARTGAME_VALUE) *
                ((double)normalizedMoveCount / (double)maxMoves);

            double moveBonus = CalculateBonusForMoves(piece, moveTree);
            return startingValue + moveBonus;
        }
    }
}
