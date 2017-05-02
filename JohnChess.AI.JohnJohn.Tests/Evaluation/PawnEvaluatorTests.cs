using System;
using System.Collections.Generic;
using System.Linq;

using Xunit;

using JohnChess;
using JohnChess.AI;
using JohnChess.Pieces;
using JohnChess.AI.JohnJohn.Evaluation;

namespace JohnChess.AI.JohnJohn.Tests.Evaluation
{
    public class PawnEvaluatorTests
    {
        private Board CreateBoardWithWhitePawnAt(Position position)
        {
            var board = Board.NewEmptyBoard();
            board.AddPiece(new PieceBuilder(PieceType.Pawn)
                .As(PieceColor.White)
                .At(position)
                .Create());
            return board;
        }
        private Board CreateBoardWithBlackPawnAt(Position position)
        {
            var board = Board.NewEmptyBoard();
            board.AddPiece(new PieceBuilder(PieceType.Pawn)
                .As(PieceColor.Black)
                .At(position)
                .Create());
            return board;
        }

        [Fact]
        public void DetectsPassedPawns()
        {
            var pawnEvaluator = new PawnEvaluator();

            var whitePawnC7Board = CreateBoardWithWhitePawnAt(new Position(File.C, Rank._7));
            var whitePawnC7 = whitePawnC7Board.WhitePieces[0];
            Assert.True(pawnEvaluator.IsPawnPassed(whitePawnC7,
                whitePawnC7Board.WhitePieces, whitePawnC7Board.BlackPieces));
        }

        [Fact]
        public void PawnNotPassedIfOpponentInSameOrAdjacentFile()
        {
            var pawnEvaluator = new PawnEvaluator();

            var whitePawnC7OpponentSameFileBoard = CreateBoardWithWhitePawnAt(new Position(File.C, Rank._7));
            whitePawnC7OpponentSameFileBoard.AddPiece(new PieceBuilder(PieceType.Pawn)
                .As(PieceColor.Black)
                .At(new Position(File.C, Rank._8))
                .Create());

            var whitePawnC71 = whitePawnC7OpponentSameFileBoard.WhitePieces[0];
            Assert.False(pawnEvaluator.IsPawnPassed(whitePawnC71,
                whitePawnC7OpponentSameFileBoard.WhitePieces, whitePawnC7OpponentSameFileBoard.BlackPieces));

            var whitePawnC7OpponentAdjacentFileBoard = CreateBoardWithWhitePawnAt(new Position(File.C, Rank._7));
            whitePawnC7OpponentAdjacentFileBoard.AddPiece(new PieceBuilder(PieceType.Pawn)
                .As(PieceColor.Black)
                .At(new Position(File.B, Rank._8))
                .Create());

            var whitePawnC72 = whitePawnC7OpponentAdjacentFileBoard.WhitePieces[0];
            Assert.False(pawnEvaluator.IsPawnPassed(whitePawnC72,
                whitePawnC7OpponentAdjacentFileBoard.WhitePieces, whitePawnC7OpponentAdjacentFileBoard.BlackPieces));
        }

        [Fact]
        public void PawnNotPassedIfNotPastMiddleBoard()
        {
            var pawnEvaluator = new PawnEvaluator();

            var whitePawnC3Board = CreateBoardWithWhitePawnAt(new Position(File.C, Rank._3));
            var whitePawnC3 = whitePawnC3Board.WhitePieces[0];
            Assert.False(pawnEvaluator.IsPawnPassed(whitePawnC3,
                whitePawnC3Board.WhitePieces, whitePawnC3Board.BlackPieces));
        }

        [Fact]
        public void DoubledPawnsDetected()
        {
            var pawnEvaluator = new PawnEvaluator();

            var doubledBoard = CreateBoardWithWhitePawnAt(new Position(File.D, Rank._4));
            doubledBoard.AddPiece(new PieceBuilder(PieceType.Pawn)
                .As(PieceColor.White)
                .At(new Position(File.D, Rank._3))
                .Create());

            var doubledPawn = doubledBoard.WhitePieces[0];
            Assert.True(pawnEvaluator.IsPawnDoubled(doubledPawn,
                doubledBoard.WhitePieces, doubledBoard.BlackPieces));

            var notDoubledBoard = CreateBoardWithWhitePawnAt(new Position(File.D, Rank._4));
            notDoubledBoard.AddPiece(new PieceBuilder(PieceType.Pawn)
                .As(PieceColor.White)
                .At(new Position(File.C, Rank._3))
                .Create());

            var notDoubledPawn = notDoubledBoard.WhitePieces[0];
            Assert.False(pawnEvaluator.IsPawnDoubled(notDoubledPawn,
                notDoubledBoard.WhitePieces, notDoubledBoard.BlackPieces));
        }

        [Fact]
        public void PawnProtectionIndexCalculatedCorrectly()
        {
            var pawnEvaluator = new PawnEvaluator();

            // A single pawn by itself has a protection index of 0.0
            var lonePawnBoard = CreateBoardWithWhitePawnAt(new Position(File.C, Rank._4));
            var lonePawn = lonePawnBoard.WhitePieces[0];
            Assert.Equal(pawnEvaluator.GetPawnProtectionIndex(lonePawn,
                lonePawnBoard.WhitePieces, lonePawnBoard.BlackPieces), 0.0);

            // A pawn with a friend that protects it (lower-left corner) has
            // a protection index of 1.0
            var pawnOneFriendBoard = CreateBoardWithWhitePawnAt(new Position(File.C, Rank._4));
            pawnOneFriendBoard.AddPiece(new PieceBuilder(PieceType.Pawn)
                .As(PieceColor.White)
                .At(new Position(File.B, Rank._3))
                .Create());
            var pawnWithOneFriend = (from p in pawnOneFriendBoard.WhitePieces
                                     where p.Position.Equals(new Position(File.C, Rank._4))
                                     select p).First();
            Assert.Equal(pawnEvaluator.GetPawnProtectionIndex(pawnWithOneFriend,
                pawnOneFriendBoard.WhitePieces, pawnOneFriendBoard.BlackPieces), 1.0);

            // A pawn with a friend that isn't at the corner is still at 0.0 :'(
            var pawnOneDistantFriendBoard = CreateBoardWithWhitePawnAt(new Position(File.C, Rank._4));
            pawnOneDistantFriendBoard.AddPiece(new PieceBuilder(PieceType.Pawn)
                .As(PieceColor.White)
                .At(new Position(File.A, Rank._3))
                .Create());
            var pawnWithOneDistantFriend = (from p in pawnOneDistantFriendBoard.WhitePieces
                                     where p.Position.Equals(new Position(File.C, Rank._4))
                                     select p).First();
            Assert.Equal(pawnEvaluator.GetPawnProtectionIndex(pawnWithOneDistantFriend,
                pawnOneDistantFriendBoard.WhitePieces, pawnOneDistantFriendBoard.BlackPieces), 0.0);

            var pawnUnderAttackOnceBoard = CreateBoardWithWhitePawnAt(new Position(File.A, Rank._4));
            pawnUnderAttackOnceBoard.AddPiece(new PieceBuilder(PieceType.Pawn)
                .As(PieceColor.Black)
                .At(new Position(File.B, Rank._5))
                .Create());
            var pawnUnderAttackOnce = (from p in pawnUnderAttackOnceBoard.WhitePieces
                                            where p.Position.Equals(new Position(File.A, Rank._4))
                                            select p).First();
            Assert.Equal(pawnEvaluator.GetPawnProtectionIndex(pawnUnderAttackOnce,
                pawnUnderAttackOnceBoard.WhitePieces, pawnUnderAttackOnceBoard.BlackPieces), -1.0);

            var pawnUnderAttackTwiceBoard = CreateBoardWithWhitePawnAt(new Position(File.C, Rank._4));
            pawnUnderAttackTwiceBoard.AddPiece(new PieceBuilder(PieceType.Pawn)
                .As(PieceColor.Black)
                .At(new Position(File.D, Rank._5))
                .Create());
            pawnUnderAttackTwiceBoard.AddPiece(new PieceBuilder(PieceType.Pawn)
                .As(PieceColor.Black)
                .At(new Position(File.B, Rank._5))
                .Create());
            var pawnUnderAttackTwice = (from p in pawnUnderAttackTwiceBoard.WhitePieces
                                       where p.Position.Equals(new Position(File.C, Rank._4))
                                       select p).First();
            Assert.Equal(pawnEvaluator.GetPawnProtectionIndex(pawnUnderAttackTwice,
                pawnUnderAttackTwiceBoard.WhitePieces, pawnUnderAttackTwiceBoard.BlackPieces), -2.0);
        }

        [Fact]
        public void PawnFileCountCalculatedCorrectly()
        {
            var pawnEvaluator = new PawnEvaluator();

            var pawnA2Board = CreateBoardWithWhitePawnAt(new Position(File.A, Rank._2));
            var pawnA2 = pawnA2Board.WhitePieces[0];
            Assert.Equal(pawnEvaluator.GetPawnFileCount(pawnA2), 1);

            var pawnB2Board = CreateBoardWithWhitePawnAt(new Position(File.B, Rank._2));
            var pawnB2 = pawnB2Board.WhitePieces[0];
            Assert.Equal(pawnEvaluator.GetPawnFileCount(pawnB2), 2);

            var pawnE2Board = CreateBoardWithWhitePawnAt(new Position(File.E, Rank._2));
            var pawnE2 = pawnE2Board.WhitePieces[0];
            Assert.Equal(pawnEvaluator.GetPawnFileCount(pawnE2), 4);

            var pawnH4Board = CreateBoardWithWhitePawnAt(new Position(File.H, Rank._4));
            var pawnH4 = pawnH4Board.WhitePieces[0];
            Assert.Equal(pawnEvaluator.GetPawnFileCount(pawnH4), 1);
        }

        [Fact]
        public void PawnRankCountCalculatedCorrectly()
        {
            var pawnEvaluator = new PawnEvaluator();

            var wPawnC2Board = CreateBoardWithWhitePawnAt(new Position(File.C, Rank._2));
            var wPawnC2 = wPawnC2Board.WhitePieces[0];
            Assert.Equal(pawnEvaluator.GetPawnRankCount(wPawnC2), 1);

            var bPawnC2Board = CreateBoardWithBlackPawnAt(new Position(File.C, Rank._2));
            var bPawnC2 = bPawnC2Board.BlackPieces[0];
            Assert.Equal(pawnEvaluator.GetPawnRankCount(bPawnC2), 6);

            var wPawnC7Board = CreateBoardWithWhitePawnAt(new Position(File.C, Rank._7));
            var wPawnC7 = wPawnC7Board.WhitePieces[0];
            Assert.Equal(pawnEvaluator.GetPawnRankCount(wPawnC7), 6);

            var bPawnC7Board = CreateBoardWithBlackPawnAt(new Position(File.C, Rank._7));
            var bPawnC7 = bPawnC7Board.BlackPieces[0];
            Assert.Equal(pawnEvaluator.GetPawnRankCount(bPawnC7), 1);

        }
    }
}
