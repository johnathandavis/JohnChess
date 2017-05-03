using System;
using System.Collections.Generic;
using System.Text;

using JohnChess.AI.Enumeration;

namespace JohnChess.AI.Evaluation
{
    public interface IPositionEvaluator
    {
        double EvaluatePosition(MoveTreeNode moveTree, PieceDict myPieces, PieceDict theirPieces, int moveNumber);
    }
}
