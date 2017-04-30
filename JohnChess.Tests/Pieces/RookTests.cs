using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using Xunit;

using JohnChess;
using JohnChess.Moves;
using JohnChess.Pieces;

namespace JohnChess.Tests.Pieces
{
    public class RookTests
    {
        private Board CreateBoardWithWhiteRookAt(Position position)
        {
            var board = Board.NewEmptyBoard();
            board.AddPiece(new PieceBuilder(PieceType.Rook)
                .As(PieceColor.White)
                .At(position)
                .Create());

            return board;
        }

        [Fact]
        public void HasCorrectMovesWithNoObstacles()
        {
            var b2 = new Position(File.B, Rank._2);
            var rookAtB2Board = CreateBoardWithWhiteRookAt(b2);
            var moves = rookAtB2Board.GetPossibleMoves(PieceColor.White);

            Assert.Equal(moves.Count, 14);

            var normalMoves = (from m in moves select m.NormalPieceMove).ToList();
            foreach (var m in normalMoves)
            {
                // All moves should be normal moves
                Assert.NotNull(m);
                // All moves should have at least the rank = 2 or File = B
                var newPosition = m.NewPosition;
                Assert.True(newPosition.Rank == b2.Rank || newPosition.File == b2.File);
                Assert.NotEqual(newPosition, b2);
            }
        }
        
        [Fact]
        public void HasCorrectMovesWithObstacles()
        {
            // We need to make sure the rook can't take friendlies,
            // and can't hop over them
            var rookAtB2Board = CreateBoardWithWhiteRookAt(new Position(File.B, Rank._2));
            
            var friendlyPosition = new Position(File.B, Rank._4);
            rookAtB2Board.AddPiece(new PieceBuilder(PieceType.Pawn)
                .As(PieceColor.White)
                .At(friendlyPosition)
                .Create());

            var enemyPosition = new Position(File.H, Rank._2);
            rookAtB2Board.AddPiece(new PieceBuilder(PieceType.Queen)
                .As(PieceColor.Black)
                .At(enemyPosition)
                .Create());


            var moves = rookAtB2Board.GetPossibleMoves(PieceColor.White);
            var rookMoves = (from m in moves where m.Type == MoveType.NormalPiece &&
                             m.NormalPieceMove.Piece.Color == PieceColor.White &&
                             m.NormalPieceMove.Piece.Type == PieceType.Rook select m).ToList();

            bool foundTakesMove = false;

            foreach (var m in rookMoves)
            {
                var newPos = m.NormalPieceMove.NewPosition;
                // Make sure rook isn't 'taking' a piece of its own color
                Assert.NotEqual(newPos, friendlyPosition);

                // If this is one of the vertical moves, make sure
                // rook isn't moving past its own piece
                if (newPos.File == friendlyPosition.File)
                {
                    Assert.True((int)newPos.Rank < (int)friendlyPosition.Rank);
                }

                // If this is one of the horizontal moves, make sure
                // rook isn't moving past an enemy piece (but can take here)
                if (newPos.Rank == enemyPosition.Rank)
                {
                    if ((int)newPos.File == (int)enemyPosition.File) foundTakesMove = true;
                    Assert.True((int)newPos.File < (int)enemyPosition.File || foundTakesMove);
                }
            }

            // Make sure it includes the "takes" option
            Assert.True(foundTakesMove);
        }
    }
}
