using System;
using System.Collections.Generic;
using System.Linq;

using JohnChess.Moves;
using JohnChess.Pieces;

namespace JohnChess.AI.JohnJohn
{
    public class JohnJohnPlayer : IPlayer
    {
        private readonly JohnsBestGuessScorer scorer;
        
        public JohnJohnPlayer()
        {
            scorer = new JohnsBestGuessScorer();
        }

        public Move SelectMove(Board board, PieceColor color)
        {
            var allMoves = board.GetPossibleMoves(color);
            var moveDict = allMoves.ToDictionary(
                (m) => m,
                (m) => ScoreMove(board, m, color));
            var sortedescending = (from kvp in moveDict
                                   orderby kvp.Value descending
                                   select kvp).ToList();

            return sortedescending.First().Key;
        }

        private double ScoreMove(Board board, Move move, PieceColor color, int recurseDepth = 0)
        {
            var newBoard = board.PreviewMove(move);
            if (recurseDepth == 2)
            {
                var myPieces = color == PieceColor.White ? newBoard.WhitePieces : newBoard.BlackPieces;
                var theirPieces = color == PieceColor.Black ? newBoard.WhitePieces : newBoard.BlackPieces;
                return scorer.ScorePieces(myPieces, theirPieces);
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
