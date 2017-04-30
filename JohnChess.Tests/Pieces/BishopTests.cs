using System;
using System.Collections.Generic;
using System.Linq;

using Xunit;

using JohnChess;
using JohnChess.Moves;
using JohnChess.Pieces;

namespace JohnChess.Tests.Pieces
{
    public class BishopTests
    {
        private Board CreateBoardWithWhiteBishopAt(Position position)
        {
            var board = Board.NewEmptyBoard();
            board.AddPiece(new PieceBuilder(PieceType.Bishop)
                .As(PieceColor.White)
                .At(position)
                .Create());

            return board;
        }

        [Fact]
        public void HasCorrectMovesWithNoObstacles()
        {
            var c3 = new Position(File.C, Rank._3);
            var e4 = new Position(File.E, Rank._4);
            var bishopC3Board = CreateBoardWithWhiteBishopAt(c3);
            var bishopE4Board = CreateBoardWithWhiteBishopAt(e4);

            var c3Moves = bishopC3Board.GetPossibleMoves(PieceColor.White);
            var c3NormalMoves = (from m in c3Moves where m.Type == MoveType.NormalPiece select m.NormalPieceMove).ToList();

            var e4Moves = bishopE4Board.GetPossibleMoves(PieceColor.White);
            var e4NormalMoves = (from m in e4Moves where m.Type == MoveType.NormalPiece select m.NormalPieceMove).ToList();

            // From C3 with no obstacles, a bishop should have 11 possible moves
            Assert.Equal(c3NormalMoves.Count, 11);
            // Likewise, E4 has 13 possible moves
            Assert.Equal(e4NormalMoves.Count, 13);

            // Make sure each move for C3 is on the diagonal
            foreach (var m in c3NormalMoves)
            {
                var newPos = m.NewPosition;
                bool newPosSumDivBy2 = ((int)newPos.File + (int)newPos.Rank) % 2 == 0;
                bool c3SumDivBy2 = ((int)c3.File + (int)c3.Rank) % 2 == 0;
                Assert.Equal(newPosSumDivBy2, c3SumDivBy2);
                Assert.True(Position.IsOnBoard(newPos));
            }

            // Make sure each move for E4 is on the diagonal
            foreach (var m in e4NormalMoves)
            {
                var newPos = m.NewPosition;
                bool newPosSumDivBy2 = ((int)newPos.File + (int)newPos.Rank) % 2 == 0;
                bool e4SumDivBy2 = ((int)e4.File + (int)e4.Rank) % 2 == 0;
                Assert.Equal(newPosSumDivBy2, e4SumDivBy2);
                Assert.True(Position.IsOnBoard(newPos));
            }
        }

        [Fact]
        public void HasCorrectMovesWithObstacles()
        {
            var e4 = new Position(File.E, Rank._4);
            var bishopE4Board = CreateBoardWithWhiteBishopAt(e4);

            var friendlyPos = new Position(File.C, Rank._2);
            var enemyPos = new Position(File.B, Rank._7);
            bishopE4Board.AddPiece(new PieceBuilder(PieceType.Pawn)
                .As(PieceColor.White)
                .At(friendlyPos)
                .Create());
            bishopE4Board.AddPiece(new PieceBuilder(PieceType.Rook)
                .As(PieceColor.Black)
                .At(enemyPos)
                .Create());

            var moves = bishopE4Board.GetPossibleMoves(PieceColor.White);
            var bishopMoves = (from m in moves where m.Type == MoveType.NormalPiece &&
               m.NormalPieceMove.Piece.Type == PieceType.Bishop select m.NormalPieceMove).ToList();

            // With this setup, bishop should have 10 moves (1 is taking)
            Assert.Equal(bishopMoves.Count, 10);

            foreach (var m in bishopMoves)
            {
                // If this is a move in the down-left direction, make sure
                // it isn't capturing the pawn or jumping over it!
                var newPos = m.NewPosition;
                if ((int)newPos.Rank < (int)e4.Rank && (int)newPos.File < (int)e4.File)
                {
                    Assert.True((int)newPos.Rank > (int)friendlyPos.Rank);
                    Assert.True((int)newPos.File > (int)friendlyPos.File);
                }

                // If this is a move in the up-left direction, make sure
                // it isn't going past the enemy rook (but it can take)
                if ((int)newPos.Rank > (int)e4.Rank && (int)newPos.File < (int)e4.File)
                {
                    Assert.True((int)newPos.Rank <= (int)enemyPos.Rank);
                    Assert.True((int)newPos.File >= (int)enemyPos.File);
                }
            }

            var bishopTakingMoves = (from m in bishopMoves where m.PieceCaptured select m).ToList();
            Assert.Equal(bishopTakingMoves.Count, 1);
            var takingMove = bishopTakingMoves[0];
            Assert.Equal(takingMove.NewPosition, enemyPos);
        }
    }
}
