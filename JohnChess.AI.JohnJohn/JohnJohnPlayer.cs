using System;
using System.Collections.Generic;
using System.Linq;

using JohnChess.Moves;
using JohnChess.Pieces;

namespace JohnChess.AI.JohnJohn
{
    public class JohnJohnPlayer : AbstractPlayer<DevelopingPositionEvaluator>
    {
        private readonly Random rnd;
        
        public JohnJohnPlayer()
        {
            rnd = new Random();
        }

        public override Move SelectMove(Board board, PieceColor color)
        {
            var allMoves = board.GetPossibleMoves(color);
            var moveDict = allMoves.ToDictionary(
                (m) => m,
                (m) => ScoreMove(board, m, color));
            var sortedescending = (from kvp in moveDict
                                   orderby kvp.Value descending
                                   select kvp).ToList();

            var topValue = sortedescending.First().Value;
            var candidates = new List<Move>();
            foreach (var move in sortedescending)
            {
                if (move.Value == topValue) candidates.Add(move.Key);
                else break;
            }

            return candidates[rnd.Next(0, candidates.Count)];
        }

        private double ScoreMove(Board board, Move move, PieceColor color, int recurseDepth = 0)
        {
            var newBoard = board.PreviewMove(move);
            if (recurseDepth == 2)
            {
                return EvaluateBoardFor(newBoard, color);
            }
            else
            {
                int newRecurseDepth = recurseDepth + 1;
                var responseMoves = board.GetPossibleMoves(color.Opposite());
                if (responseMoves.Count == 0) return double.MaxValue;
                var responseDict = responseMoves.AsParallel().ToDictionary(
                    (m) => m,
                    (m) => ScoreMove(newBoard, m, color, recurseDepth + 1));

                var movesFromBestToWorst = (from kvp in responseDict
                                orderby kvp.Value descending
                                select kvp);
                var bestMove = (recurseDepth % 2 == 0 ? movesFromBestToWorst.First() : movesFromBestToWorst.Last());
                return bestMove.Value;
            }
        }
    }
}
