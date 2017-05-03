using System;
using System.Collections.Generic;
using System.Linq;

using JohnChess.Moves;
using JohnChess.AI.Enumeration;

namespace JohnChess.AI
{
    public class RandomPlayer : AbstractPlayer
    {
        private readonly Random _rnd;

        public RandomPlayer()
        {
            _rnd = new Random();
        }

        public override Move SelectMove(Board board, MoveTreeNode moveTree, PieceColor color)
        {
            var allMoves = moveTree.CounterMoves.Values.ToList();
            var randomMove = allMoves[_rnd.Next(0, allMoves.Count)];
            return randomMove.Move;
        }
    }
}
