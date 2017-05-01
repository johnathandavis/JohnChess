using System;
using System.Collections.Generic;
using System.Text;

using JohnChess.Pieces;

namespace JohnChess.Moves
{
    public class CastleMove
    {
        public CastleMove(King king, Rook rook)
        {
            King = king;
            Rook = rook;

            var kingFile = (int)King.Position.File;
            var rookFile = (int)Rook.Position.File;
            var fileDiff = Math.Abs(kingFile - rookFile);
            if (fileDiff == 4) Type = CastleMoveType.QueenSide;
            else Type = CastleMoveType.KingSide;
        }

        public King King { get; }
        public Rook Rook { get; }
        public CastleMoveType Type { get; }
    }
}
