using System;
using System.Collections.Generic;
using System.Text;

using JohnChess.Moves;

namespace JohnChess.Pieces
{
    public class King : ChessPiece
    {
        public King(PieceColor color, Position position)
            : base(PieceType.King, color, position) { }

        public override List<Move> FindMoves(Board board)
        {
            var moves = new List<Move>();
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    // Don't consider 'not moving'
                    if (x == 0 && y == 0) continue;
                    // Would this move take us 'out of bounds'?
                    if (!Position.IsOnBoard((int)Position.Rank + y, (int)Position.File + x)) continue;
                    
                    var newPos = Position.MoveHoriz(x).MoveVert(y);
                    var existingPiece = board[newPos];
                    if (existingPiece != null)
                    {
                        if (existingPiece.Color != this.Color)
                        {
                            // We can take!
                            moves.Add(MoveBuilder.CreateNormalMove(this, newPos, true));
                        }
                    }
                    else
                    {
                        moves.Add(MoveBuilder.CreateNormalMove(this, newPos, false));
                    }
                }
            }
            return moves;
        }
    }
}
