using System;
using System.Collections.Generic;
using System.Text;

namespace JohnChess.Moves
{
    public class MoveSnapshot
    {
        public PieceColor Color { get; set; }
        public Move Move { get; set; }
        public List<Move> Moves { get; set; }
        public double EvaluatedScore { get; set; }
    }
}
