using System;
using System.Collections.Generic;
using System.Linq;

using Xunit;

using JohnChess;
using JohnChess.Moves;
using JohnChess.Pieces;

namespace JohnChess.Tests.Pieces
{
    public class KingTests
    {
        private Board CreateBoardWithWhiteKingAt(Position position)
        {
            var board = Board.NewEmptyBoard();
            board.AddPiece(new PieceBuilder(PieceType.King)
                .As(PieceColor.White)
                .At(position)
                .Create());

            return board;
        }

        [Fact]
        public void HasCorrectNormalMovesWithNoObstacles()
        {
            var d4 = new Position(File.D, Rank._4);
            var kingD4Board = CreateBoardWithWhiteKingAt(d4);

            var moves = kingD4Board.GetPossibleMoves(PieceColor.White);

            // We don't have the assertion that all moves are normal,
            // since kings can castle!
            var normalMoves = (from m in moves
                               where m.Type == MoveType.NormalPiece
                               select m.NormalPieceMove).ToList();

            // A king in the middle with nothing in the way (not considering check)
            // has 8 places to move
            Assert.Equal(normalMoves.Count, 8);

            foreach (var nm in normalMoves)
            {
                var newPos = nm.NewPosition;
                int newR = (int)newPos.Rank;
                int newF = (int)newPos.File;
                int rankDiff = Math.Abs((int)d4.Rank - newR);
                int fileDiff = Math.Abs((int)d4.File - newF);

                Assert.True(Position.IsOnBoard(newPos));
                Assert.NotEqual(nm.NewPosition, d4);
                Assert.True(rankDiff <= 1);
                Assert.True(fileDiff <= 1);
            }
        }

        [Fact]
        public void HasCorrectNormalMovesWithObstacles()
        {
            var d4 = new Position(File.D, Rank._4);
            var kingD4Board = CreateBoardWithWhiteKingAt(d4);

            var friendlyPos = new Position(File.D, Rank._5);
            var enemyPos = new Position(File.C, Rank._4);

            kingD4Board.AddPiece(new PieceBuilder(PieceType.Pawn)
                .As(PieceColor.White)
                .At(friendlyPos)
                .Create());

            kingD4Board.AddPiece(new PieceBuilder(PieceType.Pawn)
                .As(PieceColor.Black)
                .At(enemyPos)
                .Create());

            var moves = kingD4Board.GetPossibleMoves(PieceColor.White);
            var kingNormalMoves = (from m in moves
                               where m.Type == MoveType.NormalPiece &&
                               m.NormalPieceMove.Piece.Type == PieceType.King
                               select m.NormalPieceMove).ToList();

            Assert.Equal(kingNormalMoves.Count, 7);
            var takingMoves = (from m in kingNormalMoves
                               where m.PieceCaptured
                               select m).ToList();
            Assert.Equal(takingMoves.Count, 1);
            var takingMove = takingMoves[0];
            Assert.Equal(takingMove.NewPosition, enemyPos);
        }
    }
}
