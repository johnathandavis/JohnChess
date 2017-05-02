using System;
using System.Collections.Generic;
using System.Text;

using JohnChess.Moves;
using JohnChess.Pieces;

namespace JohnChess.AI
{
    public class MoveTreeNode
    {
        public Move Move { get; set; }
        public Board Board { get; set; }
        public bool IsCheckmate { get; set; }
        public bool IsStalemate { get; set; }
        public PieceColor ColorToPlay { get; set; }
        public double Score { get; set; }
        public Dictionary<Move, MoveTreeNode> CounterMoves { get; set; }
    }
}
