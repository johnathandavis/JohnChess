using System;
using System.Collections.Generic;
using System.Text;

namespace JohnChess.Notation
{
    public class UnicodeSymbolPieceFormatter : SimpleStringPieceFormatter, IChessPieceFormatter
    {
        public override string PawnStr => "♙";
        public override string RookStr => "♖";
        public override string KnightStr => "♘";
        public override string BishopStr => "♗";
        public override string QueenStr => "♕";
        public override string KingStr => "♔";
    }
}
