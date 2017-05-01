using System;
using System.Collections.Generic;
using System.Text;

namespace JohnChess.AI
{
    public interface IPositionEvaluator
    {
        double EvaluatePosition(PieceDict myPieces, PieceDict theirPieces);
    }
}
