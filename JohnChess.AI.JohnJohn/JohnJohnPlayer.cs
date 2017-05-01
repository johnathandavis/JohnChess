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
            var sortedMoves = (from m in allMoves
                               orderby ScoreMove(board, m, color) descending
                               select m).ToList();

            return sortedMoves.First();
        }

        private double ScoreMove(Board board, Move move, PieceColor color)
        {
            var newBoard = board.PreviewMove(move);
            var myPieces = color == PieceColor.White ? newBoard.WhitePieces : newBoard.BlackPieces;
            var theirPieces = color == PieceColor.Black ? newBoard.WhitePieces : newBoard.BlackPieces;
            return scorer.ScorePieces(myPieces, theirPieces);
        }
    }
}
