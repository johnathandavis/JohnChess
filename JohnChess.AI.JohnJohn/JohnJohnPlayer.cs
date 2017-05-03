using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

using JohnChess.Moves;
using JohnChess.Pieces;

using JohnChess.AI.Evaluation;
using JohnChess.AI.Enumeration;

namespace JohnChess.AI.JohnJohn
{
    public class JohnJohnPlayer : AbstractPositionalPlayer<Evaluation.DevelopingPositionEvaluator>
    {
        private const int MAX_TREE_DEPTH = 3;
        private readonly Random rnd;
        
        public JohnJohnPlayer() : base(MAX_TREE_DEPTH +1)
        {
            rnd = new Random();
        }

        public override Move SelectMove(Board board, MoveTreeNode moveTree, PieceColor color)
        {
            var allMoves = moveTree.CounterMoves.Values;
            foreach (var move in allMoves)
            {
                move.Score = ScoreMove(move, 0);
            }

            var sortedDescending = (from m in allMoves
                                    orderby m.Score descending
                                    select m);

            var topMove = sortedDescending.First();
            var candidates = new List<Move>();
            foreach (var move in sortedDescending)
            {
                if (move.Score == topMove.Score) candidates.Add(topMove.Move);
                else break;
            }

            var finalChoice = candidates[rnd.Next(0, candidates.Count)];
            moveTree = moveTree.CounterMoves[finalChoice];
            return finalChoice;
        }

        private double ScoreMove(MoveTreeNode moveTree, int recurseDepth)
        {
            if (recurseDepth == 2)
            {
                double colorToPlay = EvaluateBoardFor(moveTree, moveTree.ColorToPlay);
                double colorAgainst = EvaluateBoardFor(moveTree, moveTree.ColorToPlay.Opposite());
                return colorToPlay - colorAgainst;
            }
            else
            {

                Parallel.ForEach(moveTree.CounterMoves, (mkvp) =>
                {
                    mkvp.Value.Score = ScoreMove(mkvp.Value, recurseDepth + 1);
                });

                var movesFromBestToWorst = (from kvp in moveTree.CounterMoves
                                            orderby kvp.Value.Score descending
                                            select kvp);
                var bestMove = (recurseDepth % 2 == 0 ? movesFromBestToWorst.First() : movesFromBestToWorst.Last());
                return bestMove.Value.Score.Value;
            }

        }

    }
}
