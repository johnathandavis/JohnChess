using System;
using System.Collections.Generic;
using System.Linq;

using JohnChess.Pieces;
using JohnChess.AI.Evaluation;
using JohnChess.AI.Enumeration;

namespace JohnChess.AI.JohnJohn.Evaluation
{
    public class KnightEvaluator : PowerPieceEvaluator, IPositionEvaluator
    {
        private const double KNIGHT_VALUE = 2.8;
        private const double DOUBLE_KNIGHT_BONUS = 0.5;

        public double EvaluatePosition(MoveTreeNode moveTree, PieceDict myPieces, PieceDict theirPieces, int moveNumber)
        {
            var myKnights = myPieces[PieceType.Knight];
            double knightValue = 0.0;
            foreach (var r in myKnights)
            {
                knightValue += EvaluatePiece(moveTree, r, moveNumber);
            }
            if (myKnights.Count >= 2)
            {
                knightValue += DOUBLE_KNIGHT_BONUS;
            }
            return knightValue;
        }

        private double EvaluatePiece(MoveTreeNode moveTree, ChessPiece piece, int moveNumber)
        {
            int maxMoves = 40;
            int normalizedMoveCount = Math.Min(moveNumber, maxMoves);
            
            double moveBonus = CalculateBonusForMoves(piece, moveTree);
            return KNIGHT_VALUE + moveBonus;
        }
    }
}
