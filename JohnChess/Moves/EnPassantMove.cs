using System;
using System.Collections.Generic;
using System.Text;

using JohnChess.Pieces;

namespace JohnChess.Moves
{
    public class EnPassantMove
    {
        public EnPassantMove(Pawn attackingPawn, Position capturePos, Position destPos)
        {
            AttackingPawn = attackingPawn;
            CapturePosition = capturePos;
            DestinationPosition = destPos;
        }

        public Pawn AttackingPawn { get; }
        public Position CapturePosition { get; }
        public Position DestinationPosition { get; }
    }
}
