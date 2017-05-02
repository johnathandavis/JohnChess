using System;
using System.Collections.Generic;
using System.Text;

namespace JohnChess.AI
{
    public interface IPositionEvaluator
    {
        double EvaluatePosition(MoveTreeNode moveTree, PieceDict myPieces, PieceDict theirPieces, int moveNumber);
    }
}
