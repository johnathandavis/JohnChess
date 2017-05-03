using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

using JohnChess.Moves;
using JohnChess.Pieces;

namespace JohnChess.AI.Enumeration
{
    public class MoveTreeNode
    {
        public Move Move { get; set; }
        public Board Board { get; set; }
        public bool IsCheckmate { get; set; }
        public bool IsStalemate { get; set; }
        public PieceColor ColorToPlay { get; set; }
        public double? Score { get; set; }
        public Dictionary<Move, MoveTreeNode> CounterMoves { get; set; }



    }
}
