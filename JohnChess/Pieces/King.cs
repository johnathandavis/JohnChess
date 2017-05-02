using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using JohnChess.Moves;

namespace JohnChess.Pieces
{
    public class King : ChessPiece
    {
        public King(PieceColor color, Position position, ImmutableList<Move> moveHistory)
            : base(PieceType.King, color, position, moveHistory) { }

        public override ChessPiece MoveTo(Position position)
        {
            return new King(Color, position, moveHistory);
        }
        public override ChessPiece AddMoveToHistory(Move move)
        {
            return new King(Color, Position, moveHistory.Add(move));
        }
        public override List<Move> FindMoves(Board board)
        {
            return FindKingNormalMoves(board)
                .Concat(FindKingCastleMoves(board))
                .ToList();
        }

        private List<Move> FindKingNormalMoves(Board board)
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

        private List<Move> FindKingCastleMoves(Board board)
        {
            var castles = new List<Move>();

            // Rules for castling:
            // 1. King can't have moved
            // 2. Rook on side of castle can't have moved
            // 3. All pieces in between King and rook have to be gone
            // 4. King can't be in check at any square en route to final castle square
            // 5. King can't be in check

            if (this.MoveHistory != null && this.MoveHistory.Count > 0) return new List<Move>();

            var rook1Pos = Color == PieceColor.White
                ? new Position(File.A, Rank._1)
                : new Position(File.A, Rank._8);
            var rook2Pos = Color == PieceColor.White
                ? new Position(File.H, Rank._1)
                : new Position(File.H, Rank._8);

            var rook1Castle = CheckCastleWithRookLocation(board, rook1Pos);
            var rook2Castle = CheckCastleWithRookLocation(board, rook2Pos);

            if (rook1Castle != null) castles.Add(rook1Castle);
            if (rook2Castle != null) castles.Add(rook2Castle);

            return castles;
        }
        private Move CheckCastleWithRookLocation(Board board, Position rookPos)
        {
            var pieceInPos = board[rookPos];
            if (pieceInPos == null) return null;
            if (pieceInPos.Type != PieceType.Rook) return null;
            if (pieceInPos.Color != this.Color) return null;
            if (pieceInPos.MoveHistory != null && pieceInPos.MoveHistory.Count > 0) return null;

            int startFile = (int)rookPos.File;
            int myFile = (int)Position.File;
            int increment = (myFile > startFile) ? 1 : -1;

            // Rook is eligible. Check pieces in between.
            for (int f = startFile + increment; f != myFile; f += increment)
            {
                var pieceInFile = board[(File)f, Position.Rank];
                if (pieceInFile != null) return null;
                bool kingWillPassThroughThisSquare = Math.Abs(myFile - f) <= 2;
                if (kingWillPassThroughThisSquare)
                {
                    var fakeMove = MoveBuilder.CreateNormalMove(this,
                        new Position((File)f, Position.Rank), false);
                    if (board.DoesMovePutKingInCheck(this.Color, fakeMove)) return null;
                }
            }

            return MoveBuilder.CreateCastleMove(this, (Rook)pieceInPos);
        }
    }
}
