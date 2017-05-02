using System;
using System.Collections.Generic;
using System.Text;

namespace JohnChess.AI.JohnJohn.Evaluation
{
    public class DevelopingPositionEvaluator : IPositionEvaluator
    {
        private readonly IPositionEvaluator pawnEvaluator = new PawnEvaluator();
        private readonly IPositionEvaluator rookEvaluator = new RookEvaluator();
        private readonly IPositionEvaluator bishopEvaluator = new BishopEvaluator();
        private readonly IPositionEvaluator knightEvaluator = new KnightEvaluator();
        private readonly IPositionEvaluator queenEvaluator = new QueenEvaluator();

        public double EvaluatePosition(MoveTreeNode moveTree, PieceDict myPieces, PieceDict theirPieces, int moveCount)
        {
            double pawnValue = 0; // pawnEvaluator.EvaluatePosition(myPieces, theirPieces, moveCount);
            double rookValue = rookEvaluator.EvaluatePosition(moveTree, myPieces, theirPieces, moveCount);
            double bishopValue = bishopEvaluator.EvaluatePosition(moveTree, myPieces, theirPieces, moveCount);
            double knightValue = knightEvaluator.EvaluatePosition(moveTree, myPieces, theirPieces, moveCount);
            double queenValue = queenEvaluator.EvaluatePosition(moveTree, myPieces, theirPieces, moveCount);

            return pawnValue + rookValue + bishopValue + knightValue + queenValue;
        }
    }
}
