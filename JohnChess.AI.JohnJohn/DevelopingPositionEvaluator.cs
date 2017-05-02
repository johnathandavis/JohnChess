using System;
using System.Collections.Generic;
using System.Text;

using JohnChess.AI.JohnJohn.Evaluation;

namespace JohnChess.AI.JohnJohn
{
    public class DevelopingPositionEvaluator : IPositionEvaluator
    {
        private readonly IPositionEvaluator pawnEvaluator = new PawnEvaluator();

        public double EvaluatePosition(PieceDict myPieces, PieceDict theirPieces)
        {
            double pawnValue = pawnEvaluator.EvaluatePosition(myPieces, theirPieces);

            double totalValue = pawnValue;
            return totalValue;
        }
    }
}
