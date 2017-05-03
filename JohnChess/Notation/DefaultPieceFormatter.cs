using System;
using System.Collections.Generic;
using System.Text;

namespace JohnChess.Notation
{
    public class DefaultPieceFormatter : SimpleStringPieceFormatter, IChessPieceFormatter
    {
        private readonly bool uppercase = false;

        public DefaultPieceFormatter() { }
        public DefaultPieceFormatter(bool uppercase)
        {
            this.uppercase = uppercase;
        }

        public override string PawnStr => uppercase ? "P" : "p";
        public override string RookStr => uppercase ? "R" : "r";
        public override string KnightStr => uppercase ? "N" : "n";
        public override string BishopStr => uppercase ? "B" : "b";
        public override string QueenStr => uppercase ? "Q" : "q";
        public override string KingStr => uppercase ? "K" : "k";
    }
}
