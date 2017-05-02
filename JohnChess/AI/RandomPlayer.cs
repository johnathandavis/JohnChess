using System;
using System.Collections.Generic;
using System.Text;

using JohnChess.Moves;

namespace JohnChess.AI
{
    public class RandomPlayer : IPlayer
    {
        private readonly Random _rnd;

        public RandomPlayer()
        {
            _rnd = new Random();
        }

        public Move DecideMove(Board board, PieceColor color)
        {
            var allMoves = board.GetPossibleMoves(color);
            var randomMove = allMoves[_rnd.Next(0, allMoves.Count)];
            return randomMove;
        }
    }
}
