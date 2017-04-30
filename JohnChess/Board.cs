using System;
using System.Collections.Generic;
using System.Linq;

using JohnChess.Moves;
using JohnChess.Pieces;

namespace JohnChess
{
    public partial class Board
    {
        private readonly List<Move> moveHistory;
        private readonly ChessPiece[,] pieces;
        private readonly List<ChessPiece> whitePieces;
        private readonly List<ChessPiece> blackPieces;

        private Board()
        {
            moveHistory = new List<Move>();
            pieces = new ChessPiece[8, 8];
            whitePieces = new List<ChessPiece>();
            blackPieces = new List<ChessPiece>();
        }
        private Board(List<Move> moveHistory, ChessPiece[,] pieces,
            List<ChessPiece> whitePieces, List<ChessPiece> blackPieces)
        {
            ChessPiece[,] piecesCopy = new ChessPiece[pieces.GetLength(0), pieces.GetLength(1)];
            for (int x = 0; x < pieces.GetLength(0); x++)
            for (int y = 0; y < pieces.GetLength(1); y++)
            {
                piecesCopy[x, y] = pieces[x, y];
            }
            this.pieces = piecesCopy;
            this.moveHistory = new List<Move>(moveHistory);
            this.whitePieces = new List<ChessPiece>(whitePieces);
            this.blackPieces = new List<ChessPiece>(blackPieces);
        }

        #region "Indexers"

        public ChessPiece this[Position p]
        {
            get
            {
                return pieces[p.RowX, p.ColY];
            }
            private set
            {
                pieces[p.RowX, p.ColY] = value;
            }
        }
        public ChessPiece this[File f, Rank r]
        {
            get
            {
                return this[new Position(f, r)];
            }
            private set
            {
                this[new Position(f, r)] = value;
            }
        }

        #endregion

        public Move LastMove
        {
            get
            {
                if (moveHistory.Count == 0) return null;
                return moveHistory.Last();
            }
        }
        public IReadOnlyList<Move> MoveHistory
        {
            get
            {
                return moveHistory;
            }
        }

        internal void AddPiece(ChessPiece piece)
        {
            this[piece.Position] = piece;
            if (piece.Color == PieceColor.Black) blackPieces.Add(piece);
            if (piece.Color == PieceColor.White) whitePieces.Add(piece);
        }


        public List<Move> GetPossibleMoves(PieceColor color)
        {
            var ls = (color == PieceColor.Black) ? blackPieces : whitePieces;
            List<Move> moves = new List<Move>();
            foreach (var p in ls)
            {
                moves.AddRange(p.FindMoves(this));
            }
            return (from m in moves where IsValidMove(m) select m).ToList(); ;
        }

        private bool IsValidMove(Move move)
        {
            switch (move.Type)
            {
                case MoveType.NormalPiece:
                    return IsValidNormalMove(move.NormalPieceMove);
                default:
                    return true;
            }
        }
        private bool IsValidNormalMove(NormalPieceMove move)
        {
            var newPos = move.NewPosition;
            var newPosPiece = this[newPos];
            return newPosPiece == null || newPosPiece.Color != move.Piece.Color;
        }



        public Board PerformMove(Move move)
        {
            switch (move.Type)
            {
                case MoveType.NormalPiece:
                    return PerformNormalPieceMove(move);
                default:
                    return this;
            }
        }
        private Board PerformNormalPieceMove(Move move)
        {
            var normalMove = move.NormalPieceMove;
            var newBoard = new Board(moveHistory, pieces, whitePieces, blackPieces);
            newBoard.moveHistory.Add(move);
            var existingPiece = newBoard[normalMove.NewPosition];
            if (existingPiece != null)
            {
                move.PieceCaptured = true;
                if (existingPiece.Color == PieceColor.Black)
                    newBoard.blackPieces.Remove(existingPiece);
                else newBoard.whitePieces.Remove(existingPiece);
            }
            normalMove.Piece.MoveHistory.Add(move);
            newBoard[normalMove.Piece.Position] = null;
            newBoard[normalMove.NewPosition] = normalMove.Piece;
            normalMove.Piece.Position = normalMove.NewPosition;
            return newBoard;
        }
    }
}