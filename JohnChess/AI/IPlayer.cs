using System;
using System.Collections.Generic;
using System.Text;

using JohnChess.Moves;

namespace JohnChess.AI
{
    public interface IPlayer
    {
        Move DecideMove(Board board, PieceColor color);
    }
}
