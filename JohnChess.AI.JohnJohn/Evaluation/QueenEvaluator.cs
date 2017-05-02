using System;
using System.Collections.Generic;
using System.Linq;

using JohnChess.Pieces;

namespace JohnChess.AI.JohnJohn.Evaluation
{
    public class QueenEvaluator : PowerPieceEvaluator, IPositionEvaluator
    {
        private const double QUEEN_VALUE = 2.8;

        public double EvaluatePosition(MoveTreeNode moveTree, PieceDict myPieces, PieceDict theirPieces, int moveNumber)
        {
            var myQueens = myPieces[PieceType.Queen];
            double queenValue = 0.0;
            foreach (var r in myQueens)
            {
                queenValue += EvaluatePiece(moveTree, r, moveNumber);
            }
            return queenValue;
        }

        private double EvaluatePiece(MoveTreeNode moveTree, ChessPiece piece, int moveNumber)
        {
            int maxMoves = 40;
            int normalizedMoveCount = Math.Min(moveNumber, maxMoves);

            double moveBonus = CalculateBonusForMoves(piece, moveTree);
            return QUEEN_VALUE + moveBonus;
        }
    }
}
