using System;
using System.Collections.Generic;
using System.Text;

using JohnChess.Pieces;

namespace JohnChess.Moves
{
    public class EnPassantMove
    {
        public Pawn AttackingPawn { get; set; }
        public Position CapturePosition { get; set; }
        public Position DestinationPosition { get; set; }
    }
}
