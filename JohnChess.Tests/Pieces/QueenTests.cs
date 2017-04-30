using System;
using System.Collections.Generic;
using System.Linq;

using Xunit;

using JohnChess;
using JohnChess.Moves;
using JohnChess.Pieces;

namespace JohnChess.Tests.Pieces
{
    public class QueenTests
    {
        private readonly Position primaryPos = new Position(File.D, Rank._4);
        private readonly Position friendlyPawnPos = new Position(File.A, Rank._7);
        private readonly Position friendlyKnightPos = new Position(File.D, Rank._6);
        private readonly Position enemyQueenPos = new Position(File.E, Rank._3);
        private readonly Position enemyPawnPos = new Position(File.G, Rank._4);
        
        private Board CreateBoardWithWhitePieceAt(Position position, PieceType pieceType)
        {
            var board = Board.NewEmptyBoard();
            board.AddPiece(new PieceBuilder(pieceType)
                .As(PieceColor.White)
                .At(position)
                .Create());

            return board;
        }

        [Fact]
        public void HasCorrectMovesWithNoObstacles()
        {
            var d4 = new Position(File.D, Rank._4);

            var rookD4Board = CreateBoardWithWhitePieceAt(d4, PieceType.Rook);
            var bishopD4Board = CreateBoardWithWhitePieceAt(d4, PieceType.Bishop);
            var queenD4Board = CreateBoardWithWhitePieceAt(d4, PieceType.Queen);

            var rookPositions = (from m in rookD4Board.GetPossibleMoves(PieceColor.White)
                             where m.Type == MoveType.NormalPiece
                             select m.NormalPieceMove.NewPosition).ToList();

            var bishopPositions = (from m in bishopD4Board.GetPossibleMoves(PieceColor.White)
                                 where m.Type == MoveType.NormalPiece
                                 select m.NormalPieceMove.NewPosition).ToList();

            var queenPositions = (from m in queenD4Board.GetPossibleMoves(PieceColor.White)
                                 where m.Type == MoveType.NormalPiece
                                 select m.NormalPieceMove.NewPosition).ToList();

            var rookAndBishopPositions = rookPositions.Concat(bishopPositions).ToList();
            rookAndBishopPositions = (from m in rookAndBishopPositions orderby m.ToString() select m).ToList();
            queenPositions = (from m in queenPositions orderby m.ToString() select m).ToList();
            Assert.Equal(rookAndBishopPositions, queenPositions);
            Assert.Equal(queenPositions.Count, 27);
        }

        [Fact]
        public void HasCorrectMovesWithObstacles()
        {
            var rookBoard = ConstructComplexBoardWithPrimaryPiece(PieceType.Rook);
            var bishopBoard = ConstructComplexBoardWithPrimaryPiece(PieceType.Bishop);
            var queenBoard = ConstructComplexBoardWithPrimaryPiece(PieceType.Queen);

            var rookMoves = (from m in rookBoard.GetPossibleMoves(PieceColor.White)
                                 where m.Type == MoveType.NormalPiece &&
                                 m.NormalPieceMove.Piece.Type == PieceType.Rook
                                 select m.NormalPieceMove).ToList();
            var rookPositions = (from m in rookMoves select m.NewPosition).ToList();
            var rookTakesPositions = (from m in rookMoves where m.PieceCaptured select m.NewPosition);

            var bishopMoves = (from m in bishopBoard.GetPossibleMoves(PieceColor.White)
                                   where m.Type == MoveType.NormalPiece &&
                                 m.NormalPieceMove.Piece.Type == PieceType.Bishop
                               select m.NormalPieceMove).ToList();
            var bishopPositions = (from m in bishopMoves select m.NewPosition).ToList();
            var bishopTakesPositions = (from m in bishopMoves where m.PieceCaptured select m.NewPosition);

            var queenMoves = (from m in queenBoard.GetPossibleMoves(PieceColor.White)
                                  where m.Type == MoveType.NormalPiece &&
                                 m.NormalPieceMove.Piece.Type == PieceType.Queen
                              select m.NormalPieceMove).ToList();
            var queenPositions = (from m in queenMoves select m.NewPosition).ToList();
            var queenTakesPositions = (from m in queenMoves where m.PieceCaptured select m.NewPosition);

            var rookAndBishopPositions = rookPositions.Concat(bishopPositions).ToList();
            rookAndBishopPositions = (from m in rookAndBishopPositions orderby m.ToString() select m).ToList();
            queenPositions = (from m in queenPositions orderby m.ToString() select m).ToList();
            Assert.Equal(rookAndBishopPositions, queenPositions);
            Assert.Equal(queenPositions.Count, 20);

            var rookAndBishopTakesPositions = rookTakesPositions.Concat(bishopTakesPositions).ToList();
            rookAndBishopTakesPositions = (from m in rookAndBishopTakesPositions orderby m.ToString() select m).ToList();
            queenTakesPositions = (from m in queenTakesPositions orderby m.ToString() select m).ToList();
            Assert.Equal(rookAndBishopTakesPositions, queenTakesPositions);
            Assert.Equal(queenTakesPositions.Count(), 2);
        }

        private Board ConstructComplexBoardWithPrimaryPiece(PieceType pieceType)
        {
            var boardWithPrimaryPiece = CreateBoardWithWhitePieceAt(primaryPos, pieceType);
            boardWithPrimaryPiece.AddPiece(new PieceBuilder(PieceType.Pawn)
                .As(PieceColor.White)
                .At(friendlyPawnPos)
                .Create());
            boardWithPrimaryPiece.AddPiece(new PieceBuilder(PieceType.Knight)
                .As(PieceColor.White)
                .At(friendlyKnightPos)
                .Create());
            boardWithPrimaryPiece.AddPiece(new PieceBuilder(PieceType.Queen)
                .As(PieceColor.Black)
                .At(enemyQueenPos)
                .Create());
            boardWithPrimaryPiece.AddPiece(new PieceBuilder(PieceType.Pawn)
                .As(PieceColor.Black)
                .At(enemyPawnPos)
                .Create());
            return boardWithPrimaryPiece;
        }
    }
}
