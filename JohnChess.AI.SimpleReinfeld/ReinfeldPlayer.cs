﻿using System;
using System.Collections.Generic;
using System.Linq;

using JohnChess.AI.Evaluation;
using JohnChess.AI.Enumeration;
using JohnChess.Moves;

namespace JohnChess.AI.SimpleReinfeld
{
    public class ReinfeldPlayer : AbstractPositionalPlayer<ReinfeldMaterialPointsEvaluator>
    {
        public ReinfeldPlayer() : base(4) { }

        public override Move SelectMove(Board board, MoveTreeNode moveTree, PieceColor color)
        {
            return ChooseBestMove(moveTree, color);
        }

        private Move ChooseBestMove(MoveTreeNode moveTree, PieceColor color, int recurseDepth = 0)
        {
            if (recurseDepth == MaximumRecurseDepth)
            {
                moveTree.Score = EvaluateBoardFor(moveTree, color);
                return moveTree.Move;
            }
            else
            {
                foreach (var tree in moveTree.CounterMoves)
                {
                    if (tree.Value.Score == null)
                    {
                        ChooseBestMove(tree.Value, color, recurseDepth + 1);
                    }
                }
                var sortedMoves = (from m in moveTree.CounterMoves
                                   orderby m.Value.Score descending
                                   select m.Value).ToList();
                var bestMove = sortedMoves[0];
                moveTree.Score = bestMove.Score;
                return bestMove.Move;
            }
        }
    }
}