using System;
using System.Collections.Generic;
using System.Text;

using JohnChess.Moves;
using JohnChess.Pieces;

namespace JohnChess.AI.Enumeration
{
    public class PieceDict : Dictionary<PieceType, List<ChessPiece>>
    {
        internal PieceDict()
        {
            foreach (PieceType pt in (PieceType[])Enum.GetValues(typeof(PieceType)))
            {
                Add(pt, new List<ChessPiece>());
            }
        }
    }
}
