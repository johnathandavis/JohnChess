using System;
using System.Collections.Generic;
using System.Text;

using Xunit;

using JohnChess;
using JohnChess.Moves;
using JohnChess.Pieces;

namespace JohnChess.Tests
{
    public class CheckTests
    {
        [Fact]
        public void KingCannotMoveIntoCheck()
        {
            var board = Board.NewEmptyBoard();
            var king = new PieceBuilder(PieceType.King)
                .As(PieceColor.White)
                .At(new Position(File.D, Rank._1))
                .Create();
            board.AddPiece(king);
            board.AddPiece(new PieceBuilder(PieceType.Rook)
                .As(PieceColor.Black)
                .At(new Position(File.E, Rank._8))
                .Create());

            var move = MoveBuilder.CreateNormalMove(king,
                new Position(File.E, Rank._1), false);

            bool putsMeInCheck = board.DoesMovePutKingInCheck(PieceColor.White, move);
            Assert.True(putsMeInCheck);
        }

        [Fact]
        public void KingCannotMoveIsStalemate()
        {
            var board = Board.NewEmptyBoard();
            board.AddPiece(new PieceBuilder(PieceType.King)
                .As(PieceColor.White)
                .At(new Position(File.H, Rank._1))
                .Create());
            board.AddPiece(new PieceBuilder(PieceType.Rook)
                .As(PieceColor.Black)
                .At(new Position(File.G, Rank._3))
                .Create());
            board.AddPiece(new PieceBuilder(PieceType.Queen)
                .As(PieceColor.Black)
                .At(new Position(File.F, Rank._2))
                .Create());

            Assert.Throws<StalemateException>(() =>
            {
                var whiteMoves = board.GetPossibleMoves(PieceColor.White);
            });
        }

        [Fact]
        public void KingCannotLeaveCheckIsCheckmate()
        {
            var board = Board.NewEmptyBoard();
            board.AddPiece(new PieceBuilder(PieceType.King)
                .As(PieceColor.White)
                .At(new Position(File.H, Rank._1))
                .Create());
            board.AddPiece(new PieceBuilder(PieceType.Rook)
                .As(PieceColor.Black)
                .At(new Position(File.G, Rank._1))
                .Create());
            board.AddPiece(new PieceBuilder(PieceType.Queen)
                .As(PieceColor.Black)
                .At(new Position(File.F, Rank._2))
                .Create());

            Assert.Throws<CheckmateException>(() =>
            {
                var whiteMoves = board.GetPossibleMoves(PieceColor.White);
            });
        }
    }
}
