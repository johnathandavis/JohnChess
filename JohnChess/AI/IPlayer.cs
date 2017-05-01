using System;
using System.Collections.Generic;
using System.Text;

using JohnChess.Moves;

namespace JohnChess.AI
{
    public interface IPlayer
    {
        Move SelectMove(Board board, PieceColor color);
    }
}
