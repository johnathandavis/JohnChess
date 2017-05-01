using System;
using System.Collections.Generic;
using System.Linq;

using Xunit;
using JohnChess.Moves;
using JohnChess.Pieces;

namespace JohnChess.Tests.Pieces
{
    public class PawnTests
    {
        private Board CreateBoardWithPawnAt(PieceColor color, Position position)
        {
            var board = Board.NewEmptyBoard();
            board.AddPiece(new PieceBuilder(PieceType.Pawn)
                .As(color)
                .At(position)
                .Create());

            return board;
        }

        [Fact]
        public void WhiteHasCorrectNormalMovesOnRank2()
        {
            var wPawnC2Board = CreateBoardWithPawnAt(PieceColor.White, new Position(File.C, Rank._2));
            var wPawnC2Moves = wPawnC2Board.GetPossibleMoves(PieceColor.White);
            var wPawnC2NormalMoves = (from m in wPawnC2Moves where m.Type == MoveType.NormalPiece select m).ToList();
            var wPawnC2Positions = (from m in wPawnC2Moves select m.NormalPieceMove.NewPosition).ToList();
            // For this scenario, all moves should be normal.
            Assert.Equal(wPawnC2NormalMoves.Count, wPawnC2Moves.Count);
            Assert.Equal(wPawnC2Moves.Count, 2);
            // This pawn can go C3 and C4
            Assert.Contains(new Position(File.C, Rank._3), wPawnC2Positions);
            Assert.Contains(new Position(File.C, Rank._4), wPawnC2Positions);

            var takesMoves = (from m in wPawnC2NormalMoves where m.PieceCaptured select m).ToList();
            Assert.Equal(takesMoves.Count, 0);
        }

        [Fact]
        public void WhiteHasCorrectNormalMovesOnNonRank2()
        {
            var wPawnC3Board = CreateBoardWithPawnAt(PieceColor.White, new Position(File.C, Rank._3));
            var wPawnC3Moves = wPawnC3Board.GetPossibleMoves(PieceColor.White);
            var wPawnC3NormalMoves = (from m in wPawnC3Moves where m.Type == MoveType.NormalPiece select m).ToList();
            var wPawnC3Positions = (from m in wPawnC3Moves select m.NormalPieceMove.NewPosition).ToList();
            // For this scenario, all moves should be normal.
            Assert.Equal(wPawnC3NormalMoves.Count, wPawnC3Moves.Count);
            Assert.Equal(wPawnC3Moves.Count, 1);
            // This pawn can only go C4
            var onlyMove = wPawnC3Positions[0];
            Assert.Equal(new Position(File.C, Rank._4), onlyMove);

            var takesMoves = (from m in wPawnC3NormalMoves where m.PieceCaptured select m).ToList();
            Assert.Equal(takesMoves.Count, 0);
        }

        [Fact]
        public void BlackHasCorrectNormalMovesOnRank7()
        {
            var bPawnC7Board = CreateBoardWithPawnAt(PieceColor.Black, new Position(File.C, Rank._7));
            var bPawnC7Moves = bPawnC7Board.GetPossibleMoves(PieceColor.Black);
            var bPawnC7NormalMoves = (from m in bPawnC7Moves where m.Type == MoveType.NormalPiece select m).ToList();
            var bPawnC7Positions = (from m in bPawnC7Moves select m.NormalPieceMove.NewPosition).ToList();
            // For this scenario, all moves should be normal.
            Assert.Equal(bPawnC7NormalMoves.Count, bPawnC7Moves.Count);
            Assert.Equal(bPawnC7Moves.Count, 2);
            // This pawn can go C3 and C4
            Assert.Contains(new Position(File.C, Rank._6), bPawnC7Positions);
            Assert.Contains(new Position(File.C, Rank._5), bPawnC7Positions);

            var takesMoves = (from m in bPawnC7NormalMoves where m.PieceCaptured select m).ToList();
            Assert.Equal(takesMoves.Count, 0);
        }

        [Fact]
        public void BlackHasCorrectNormalMovesOnNonRank7()
        {
            var bPawnC6Board = CreateBoardWithPawnAt(PieceColor.Black, new Position(File.C, Rank._6));
            var bPawnC6Moves = bPawnC6Board.GetPossibleMoves(PieceColor.Black);
            var bPawnC6NormalMoves = (from m in bPawnC6Moves where m.Type == MoveType.NormalPiece select m).ToList();
            var bPawnC6Positions = (from m in bPawnC6Moves select m.NormalPieceMove.NewPosition).ToList();
            // For this scenario, all moves should be normal.
            Assert.Equal(bPawnC6NormalMoves.Count, bPawnC6Moves.Count);
            Assert.Equal(bPawnC6Moves.Count, 1);
            // This pawn can only go C5
            var onlyMove = bPawnC6Positions[0];
            Assert.Equal(new Position(File.C, Rank._5), onlyMove);

            var takesMoves = (from m in bPawnC6NormalMoves where m.PieceCaptured select m).ToList();
            Assert.Equal(takesMoves.Count, 0);
        }

        [Fact]
        public void HasCorrectCaptureMoves()
        {
            var pawnC2Board = CreateBoardWithPawnAt(PieceColor.White, new Position(File.C, Rank._2));
            var enemyPawnPos1 = new Position(File.B, Rank._3);
            var enemyPawnPos2 = new Position(File.D, Rank._3);
            pawnC2Board.AddPiece(new PieceBuilder(PieceType.Pawn)
                .As(PieceColor.Black)
                .At(enemyPawnPos1)
                .Create());
            pawnC2Board.AddPiece(new PieceBuilder(PieceType.Pawn)
                .As(PieceColor.Black)
                .At(enemyPawnPos2)
                .Create());

            var moves = pawnC2Board.GetPossibleMoves(PieceColor.White);
            var normalMoves = (from m in moves where m.Type == MoveType.NormalPiece select m).ToList();
            Assert.Equal(moves.Count, normalMoves.Count);

            var takesMoves = (from m in normalMoves where m.PieceCaptured select m.NormalPieceMove).ToList();
            var takesMovesPositions = (from m in takesMoves select m.NewPosition).ToList();
            Assert.Equal(takesMovesPositions.Count, 2);
            Assert.Contains(new Position(File.B, Rank._3), takesMovesPositions);
            Assert.Contains(new Position(File.D, Rank._3), takesMovesPositions);
        }

        [Fact]
        public void PromotionsAvailableStraightAhead()
        {
            var pawnD7Board = CreateBoardWithPawnAt(PieceColor.White, new Position(File.D, Rank._7));
            var moves = pawnD7Board.GetPossibleMoves(PieceColor.White);

            // There should be 4 moves - one for each promoting
            // piece type for pD7 promotes on D8
            Assert.Equal(moves.Count, 4);
            var promotingMoves = (from m in moves
                                  where m.Type == MoveType.Promotion
                                  select m.Promotion).ToList();
            var promotingTypes = (from m in promotingMoves select m.NewPieceType).ToList();
            Assert.Equal(moves.Count, promotingMoves.Count);

            foreach (var promotingPieceType in PromotionMove.PromotingPieceType.PromotingPieceTypes)
            {
                Assert.Contains(promotingPieceType, promotingTypes);
            }
        }

        [Fact]
        public void PawnDoesntMoveToLastRankWithoutPromoting()
        {
            var pawnC7Board = CreateBoardWithPawnAt(PieceColor.White, new Position(File.C, Rank._7));

            var enemyKnight1Pos = new Position(File.B, Rank._8);
            var enemyKnight2Pos = new Position(File.D, Rank._8);

            pawnC7Board.AddPiece(new PieceBuilder(PieceType.Knight)
                .As(PieceColor.Black)
                .At(enemyKnight1Pos)
                .Create());

            pawnC7Board.AddPiece(new PieceBuilder(PieceType.Knight)
                .As(PieceColor.Black)
                .At(enemyKnight2Pos)
                .Create());

            var moves = pawnC7Board.GetPossibleMoves(PieceColor.White);
            // Pawn can take on B7 or D8, or advance to C8. All are promotions,
            // and there should be four moves per (for the four different promoting
            // piece types), so 12 total.
            Assert.Equal(moves.Count, 12);
            var promotingMoves = (from m in moves
                                  where m.Type == MoveType.Promotion
                                  select m.Promotion).ToList();
            // All moves must be promotions, a pawn can't reach the last rank
            // without promoting
            Assert.Equal(promotingMoves.Count, moves.Count);
        }

        [Fact]
        public void PawnPromotionPerformedSuccessfully()
        {
            var pawnD7Board = CreateBoardWithPawnAt(PieceColor.White, new Position(File.D, Rank._7));
            var moves = pawnD7Board.GetPossibleMoves(PieceColor.White);
            var promotingMoves = (from m in moves
                                  where m.Type == MoveType.Promotion
                                  select m).ToList();
            var promotion = promotingMoves[0];
            pawnD7Board = pawnD7Board.PerformMove(promotion);

            Assert.Equal(pawnD7Board.WhitePieces.Count, 1);
            var whitePiece = pawnD7Board.WhitePieces[0];
            Assert.Equal(whitePiece.Type, promotion.Promotion.NewPieceType.Type);
            Assert.Equal(whitePiece.MoveHistory.Count, promotion.Promotion.PromotingPiece.MoveHistory.Count + 1);
            Assert.Equal(pawnD7Board[File.D, Rank._8], whitePiece);
        }

        [Fact]
        public void PawnSeesEnPassant()
        {
            var pawnD5Board = CreateEnPassantBoardSetup();

            var whitePawnResponses = pawnD5Board.GetPossibleMoves(PieceColor.White);
            var enPassantMoves = (from m in whitePawnResponses
                                  where m.Type == MoveType.EnPassant
                                  select m.EnPassant).ToList();
            Assert.Equal(enPassantMoves.Count, 1);
            var enPassantMove = enPassantMoves[0];
            Assert.Equal(enPassantMove.DestinationPosition, new Position(File.C, Rank._6));
            Assert.Equal(enPassantMove.CapturePosition, new Position(File.C, Rank._5));
        }

        [Fact]
        public void PawnCantEnPassantWhenAdvanceWasntLatestMove()
        {
            var pawnD5Board = CreateEnPassantBoardSetup();
            var blackPiece = pawnD5Board.BlackPieces[0];
            blackPiece.MoveHistory.Clear();

            var whitePawnResponses = pawnD5Board.GetPossibleMoves(PieceColor.White);
            var enPassantMoves = (from m in whitePawnResponses
                                  where m.Type == MoveType.EnPassant
                                  select m.EnPassant).ToList();
            Assert.Equal(enPassantMoves.Count, 0);
        }
        
        [Fact]
        public void PawnEnPassantPerformedSuccessfully()
        {
            var enPassantBoard = CreateEnPassantBoardSetup();
            var moves = enPassantBoard.GetPossibleMoves(PieceColor.White);
            var enPassantMove = (from m in moves
                                 where m.Type == MoveType.EnPassant
                                 select m).First();
            enPassantBoard = enPassantBoard.PerformMove(enPassantMove);

            var whitePieces = enPassantBoard.WhitePieces;
            Assert.Equal(whitePieces.Count, 1);
            var wp = whitePieces[0];
            Assert.Equal(wp.Position, new Position(File.C, Rank._6));
        }

        private Board CreateEnPassantBoardSetup()
        {
            var pawnD5Board = CreateBoardWithPawnAt(PieceColor.White, new Position(File.D, Rank._5));
            var enemyPawnPosition = new Position(File.C, Rank._7);
            pawnD5Board.AddPiece(new PieceBuilder(PieceType.Pawn)
                .As(PieceColor.Black)
                .At(enemyPawnPosition)
                .Create());

            var blackMoves = pawnD5Board.GetPossibleMoves(PieceColor.Black);
            var blackPawnAdvance2Move = (from m in blackMoves
                                         where m.Type == MoveType.NormalPiece &&
                                         m.NormalPieceMove.NewPosition.Equals(new Position(File.C, Rank._5))
                                         select m).First();

            return pawnD5Board.PerformMove(blackPawnAdvance2Move);
        }
    }
}
