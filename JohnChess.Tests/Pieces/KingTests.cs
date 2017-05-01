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

            // Have to skip king check to make sure we get all moves.
            // King check functionality is tested in another class.
            var moves = kingD4Board.GetPossibleMoves(PieceColor.White, true);
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
        
        [Fact]
        public void KingCanCastleQueenside()
        {
            var kingBoard = CreateBoardWithWhiteKingAt(new Position(File.E, Rank._1));
            kingBoard.AddPiece(new PieceBuilder(PieceType.Rook)
                .As(PieceColor.White)
                .At(new Position(File.A, Rank._1))
                .Create());

            var moves = kingBoard.GetPossibleMoves(PieceColor.White);
            var castleMoves = (from m in moves
                               where m.Type == MoveType.Castle
                               select m).ToList();

            Assert.Equal(castleMoves.Count, 1);
            var castleMove = castleMoves[0];
            Assert.Equal(castleMove.Castle.Type, CastleMoveType.QueenSide);

            kingBoard = kingBoard.PerformMove(castleMoves[0]);
            var whitePieces = kingBoard.WhitePieces;
            var rook = (from p in whitePieces where p.Type == PieceType.Rook select p).First();
            var king = (from p in whitePieces where p.Type == PieceType.King select p).First();
            Assert.Equal(king.Position, new Position(File.C, Rank._1));
            Assert.Equal(rook.Position, new Position(File.D, Rank._1));
            Assert.Equal(MoveType.Castle, king.MoveHistory.Last().Type);
            Assert.Equal(MoveType.Castle, rook.MoveHistory.Last().Type);
        }

        [Fact]
        public void KingCanCastleKingside()
        {
            var kingBoard = CreateBoardWithWhiteKingAt(new Position(File.E, Rank._1));
            kingBoard.AddPiece(new PieceBuilder(PieceType.Rook)
                .As(PieceColor.White)
                .At(new Position(File.H, Rank._1))
                .Create());

            var moves = kingBoard.GetPossibleMoves(PieceColor.White);
            var castleMoves = (from m in moves
                               where m.Type == MoveType.Castle
                               select m).ToList();

            Assert.Equal(castleMoves.Count, 1);
            var castleMove = castleMoves[0];
            Assert.Equal(castleMove.Castle.Type, CastleMoveType.KingSide);

            kingBoard = kingBoard.PerformMove(castleMoves[0]);
            var whitePieces = kingBoard.WhitePieces;
            var rook = (from p in whitePieces where p.Type == PieceType.Rook select p).First();
            var king = (from p in whitePieces where p.Type == PieceType.King select p).First();
            Assert.Equal(king.Position, new Position(File.G, Rank._1));
            Assert.Equal(rook.Position, new Position(File.F, Rank._1));
            Assert.Equal(MoveType.Castle, king.MoveHistory.Last().Type);
            Assert.Equal(MoveType.Castle, rook.MoveHistory.Last().Type);
        }

        [Fact]
        public void KingCannotKingsideCastleThroughCheck()
        {
            var kingBoard = CreateBoardWithWhiteKingAt(new Position(File.E, Rank._1));
            kingBoard.AddPiece(new PieceBuilder(PieceType.Rook)
                .As(PieceColor.White)
                .At(new Position(File.H, Rank._1))
                .Create());
            kingBoard.AddPiece(new PieceBuilder(PieceType.Rook)
                .As(PieceColor.Black)
                .At(new Position(File.G, Rank._8))
                .Create());

            var moves = kingBoard.GetPossibleMoves(PieceColor.White);
            var castleMoves = (from m in moves
                               where m.Type == MoveType.Castle
                               select m).ToList();
            Assert.Equal(castleMoves.Count, 0);
        }

        [Fact]
        public void KingCannotQueensideCastleThroughCheck()
        {
            var kingBoard = CreateBoardWithWhiteKingAt(new Position(File.E, Rank._1));
            kingBoard.AddPiece(new PieceBuilder(PieceType.Rook)
                .As(PieceColor.White)
                .At(new Position(File.A, Rank._1))
                .Create());
            kingBoard.AddPiece(new PieceBuilder(PieceType.Rook)
                .As(PieceColor.Black)
                .At(new Position(File.C, Rank._8))
                .Create());

            var moves = kingBoard.GetPossibleMoves(PieceColor.White);
            var castleMoves = (from m in moves
                               where m.Type == MoveType.Castle
                               select m).ToList();
            Assert.Equal(castleMoves.Count, 0);
        }

        [Fact]
        public void KingCannotCastleIfKingAlreadyMoved()
        {
            var kingBoard = CreateBoardWithWhiteKingAt(new Position(File.E, Rank._1));
            kingBoard.AddPiece(new PieceBuilder(PieceType.Rook)
                .As(PieceColor.White)
                .At(new Position(File.A, Rank._1))
                .Create());

            var kingStart = (from p in kingBoard.WhitePieces where p.Type == PieceType.King select p).First();

            kingBoard = kingBoard.PerformMove(MoveBuilder.CreateNormalMove(
                kingStart, kingStart.Position.MoveVert(1), false));

            var kingMoveBack = (from p in kingBoard.WhitePieces where p.Type == PieceType.King select p).First();
            kingBoard = kingBoard.PerformMove(MoveBuilder.CreateNormalMove(
                kingMoveBack, kingMoveBack.Position.MoveVert(-1), false));

            var kingBackWhereHeStarted = (from p in kingBoard.WhitePieces where p.Type == PieceType.King select p).First();

            Assert.Equal(new Position(File.E, Rank._1), kingBackWhereHeStarted.Position);
            var moves = kingBoard.GetPossibleMoves(PieceColor.White);
            var castleMoves = (from m in moves
                               where m.Type == MoveType.Castle
                               select m).ToList();

            Assert.Equal(castleMoves.Count, 0);
        }


        [Fact]
        public void KingCannotCastleIfRookAlreadyMoved()
        {
            var kingBoard = CreateBoardWithWhiteKingAt(new Position(File.E, Rank._1));
            kingBoard.AddPiece(new PieceBuilder(PieceType.Rook)
                .As(PieceColor.White)
                .At(new Position(File.A, Rank._1))
                .Create());

            var rookStart = (from p in kingBoard.WhitePieces where p.Type == PieceType.Rook select p).First();

            kingBoard = kingBoard.PerformMove(MoveBuilder.CreateNormalMove(
                rookStart, rookStart.Position.MoveVert(1), false));

            var rookMoveBack = (from p in kingBoard.WhitePieces where p.Type == PieceType.Rook select p).First();
            kingBoard = kingBoard.PerformMove(MoveBuilder.CreateNormalMove(
                rookMoveBack, rookMoveBack.Position.MoveVert(-1), false));

            var rookBackWhereHeStarted = (from p in kingBoard.WhitePieces where p.Type == PieceType.Rook select p).First();

            Assert.Equal(new Position(File.A, Rank._1), rookBackWhereHeStarted.Position);
            var moves = kingBoard.GetPossibleMoves(PieceColor.White);
            var castleMoves = (from m in moves
                               where m.Type == MoveType.Castle
                               select m).ToList();

            Assert.Equal(castleMoves.Count, 0);
        }

        [Fact]
        public void KingCannotQueensideCastleWithPieceInTheWay()
        {
            var kingBoard = CreateBoardWithWhiteKingAt(new Position(File.E, Rank._1));
            kingBoard.AddPiece(new PieceBuilder(PieceType.Rook)
                .As(PieceColor.White)
                .At(new Position(File.A, Rank._1))
                .Create());
            kingBoard.AddPiece(new PieceBuilder(PieceType.Bishop)
                .As(PieceColor.White)
                .At(new Position(File.C, Rank._1))
                .Create());

            var moves = kingBoard.GetPossibleMoves(PieceColor.White);
            var castleMoves = (from m in moves
                               where m.Type == MoveType.Castle
                               select m).ToList();
            Assert.Equal(castleMoves.Count, 0);
        }

        [Fact]
        public void KingCannotKingsideCastleWithPieceInTheWay()
        {
            var kingBoard = CreateBoardWithWhiteKingAt(new Position(File.E, Rank._1));
            kingBoard.AddPiece(new PieceBuilder(PieceType.Rook)
                .As(PieceColor.White)
                .At(new Position(File.H, Rank._1))
                .Create());
            kingBoard.AddPiece(new PieceBuilder(PieceType.Bishop)
                .As(PieceColor.White)
                .At(new Position(File.F, Rank._1))
                .Create());

            var moves = kingBoard.GetPossibleMoves(PieceColor.White);
            var castleMoves = (from m in moves
                               where m.Type == MoveType.Castle
                               select m).ToList();
            Assert.Equal(castleMoves.Count, 0);
        }
    }
}
