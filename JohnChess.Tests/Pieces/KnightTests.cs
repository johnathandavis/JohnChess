using System;
using System.Collections.Generic;
using System.Linq;

using Xunit;

using JohnChess;
using JohnChess.Moves;
using JohnChess.Pieces;

namespace JohnChess.Tests.Pieces
{
    public class KnightTests
    {
        private Board CreateBoardWithWhiteKnightAt(Position position)
        {
            var board = Board.NewEmptyBoard();
            board.AddPiece(new PieceBuilder(PieceType.Knight)
                .As(PieceColor.White)
                .At(position)
                .Create());

            return board;
        }

        [Fact]
        public void HasCorrectMovesWithNoObstacles()
        {
            var b3 = new Position(File.B, Rank._3);
            var knightB3Board = CreateBoardWithWhiteKnightAt(b3);

            var moves = knightB3Board.GetPossibleMoves(PieceColor.White);
            var normalMoves = (from m in moves where m.Type == MoveType.NormalPiece select m.NormalPieceMove).ToList();

            // A knight in B3 has 6 moves with no obstacles
            Assert.Equal(moves.Count, 6);
            // All a knights moves should be normal
            Assert.Equal(moves.Count, normalMoves.Count);

            foreach (var m in normalMoves)
            {
                var newPos = m.NewPosition;
                int newR = (int)newPos.Rank;
                int newF = (int)newPos.File;

                int rankDiff = Math.Abs(newR - (int)b3.Rank);
                int fileDiff = Math.Abs(newF - (int)b3.File);
                Assert.True(rankDiff == 1 || rankDiff == 2);
                Assert.True(fileDiff == 1 || fileDiff == 2);
                Assert.NotEqual(rankDiff, fileDiff);
                Assert.True(Position.IsOnBoard(newPos));
            }
        }

        [Fact]
        public void HasCorrectMovesWithObstacles()
        {
            var b3 = new Position(File.B, Rank._3);
            var knightB3Board = CreateBoardWithWhiteKnightAt(b3);

            var friendlyPos = new Position(File.D, Rank._4);
            var enemyPos = new Position(File.C, Rank._5);
            knightB3Board.AddPiece(new PieceBuilder(PieceType.Pawn)
                .At(friendlyPos)
                .As(PieceColor.White)
                .Create());
            knightB3Board.AddPiece(new PieceBuilder(PieceType.Rook)
                .At(enemyPos)
                .As(PieceColor.Black)
                .Create());

            var moves = knightB3Board.GetPossibleMoves(PieceColor.White);
            var knightMoves = (from m in moves
                               where m.Type == MoveType.NormalPiece &&
               m.NormalPieceMove.Piece.Type == PieceType.Knight
                               select m.NormalPieceMove).ToList();

            // In B3, the knight has 6 valid moves - but with a friendly pawn
            // at D4, this is no longer valid. There should be 5 moves, and one
            // should take the enemy rook on D5.
            Assert.Equal(knightMoves.Count, 5);

            var takingMoves = (from m in knightMoves where m.PieceCaptured select m).ToList();
            Assert.Equal(takingMoves.Count, 1);
            var takingMove = takingMoves[0];
            Assert.Equal(takingMove.NewPosition, enemyPos);

        }
    }
}
