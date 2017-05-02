using System;
using System.Collections.Concurrent;
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

        private bool cacheSet = false;
        private List<ChessPiece> whitePieceCache;
        private List<ChessPiece> blackPieceCache;
        private object pieceCacheLocked = new object();

        private Board()
        {
            moveHistory = new List<Move>();
            pieces = new ChessPiece[8, 8];
        }
        private Board(List<Move> moveHistory, ChessPiece[,] pieces)
        {
            ChessPiece[,] piecesCopy = new ChessPiece[pieces.GetLength(0), pieces.GetLength(1)];
            for (int x = 0; x < pieces.GetLength(0); x++)
            for (int y = 0; y < pieces.GetLength(1); y++)
            {
                var pieceAtSpot = pieces[x, y];
                if (pieceAtSpot != null)
                {
                    piecesCopy[x, y] = pieceAtSpot.MoveTo(pieceAtSpot.Position);
                }

            }
            this.pieces = piecesCopy;
            this.moveHistory = new List<Move>(moveHistory);
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
                if (pieces[p.RowX, p.ColY]?.Type == PieceType.King && value != null)
                {
                    { };
                }
                pieces[p.RowX, p.ColY] = value;
                cacheSet = false;
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

        public void AddPiece(ChessPiece piece)
        {
            this[piece.Position] = piece;
            cacheSet = false;
        }
        public IReadOnlyList<ChessPiece> BlackPieces
        {
            get
            {
                if (!cacheSet) LoadPieceCache();
                return blackPieceCache;
            }
        }
        public IReadOnlyList<ChessPiece> WhitePieces
        {
            get
            {
                if (!cacheSet) LoadPieceCache();
                return whitePieceCache;
            }
        }
        internal bool DoesMovePutKingInCheck(PieceColor color, Move move)
        {
            var tempBoard = PerformMove(move);
            return IsKingInCheck(tempBoard, color);
        }

        internal List<Move> GetPossibleMoves(PieceColor color)
        {
            return GetPossibleMoves(color, false);
        }
        internal List<Move> GetPossibleMoves(PieceColor color, bool skipKingCheck)
        {
            string key = color.ToString() + skipKingCheck.ToString();
            var ls = (color == PieceColor.Black) ? BlackPieces : WhitePieces;

            List<Move> moves = new List<Move>();
            foreach (var p in ls)
            {
                moves.AddRange(p.FindMoves(this));
            }
            var finalResults = (from m in moves
                                where IsValidMove(m) && (skipKingCheck || !DoesMovePutKingInCheck(color, m))
                                select m).ToList();

            if (finalResults.Count == 0)
            {
                // Okay, we have nowhere to go.
                // Is this because we are in stalemate (currently not in checK?)
                // Or checkmate? (currently in check)
                // Third option: if there are no pieces, it is a 'pass'.
                // This is used for unit testing.
                if (ls.Count == 0) return new List<Move>();

                bool currentlyInCheck = IsKingInCheck(this, color);
                if (currentlyInCheck) throw new CheckmateException();
                else throw new StalemateException();
            }

            return finalResults;
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
        private void LoadPieceCache()
        {
            lock (pieceCacheLocked)
            {
                whitePieceCache = new List<ChessPiece>();
                blackPieceCache = new List<ChessPiece>();
                for (int x = 0; x < pieces.GetLength(0); x++)
                {
                    for (int y = 0; y < pieces.GetLength(1); y++)
                    {
                        var pieceAt = pieces[x, y];
                        if (pieceAt == null) continue;
                        if (pieceAt.Color == PieceColor.White) whitePieceCache.Add(pieceAt);
                        else blackPieceCache.Add(pieceAt);
                    }
                }
                var kings = (from p in whitePieceCache where p.Type == PieceType.King select p).Count();
                kings = (from p in blackPieceCache where p.Type == PieceType.King select p).Count();
                cacheSet = true;
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
            var newBoard = new Board(moveHistory, pieces);
            newBoard.moveHistory.Add(move);

            switch (move.Type)
            {
                case MoveType.NormalPiece:
                    PerformNormalPieceMove(newBoard, move);
                    break;
                case MoveType.Promotion:
                    PerformPromotionPieceMove(newBoard, move);
                    break;
                case MoveType.EnPassant:
                    PerformEnPassantPieceMove(newBoard, move);
                    break;
                case MoveType.Castle:
                    PerformCastleMove(newBoard, move);
                    break;
                default:
                    throw new AlienChessException("Unknown move type!");
            }
            return newBoard;
        }
        private void PerformNormalPieceMove(Board newBoard, Move move)
        {
            var normalMove = move.NormalPieceMove;
            newBoard[normalMove.Piece.Position] = null;

            var newPiece = normalMove.Piece
                .MoveTo(normalMove.NewPosition)
                .AddMoveToHistory(move);
            newBoard[normalMove.NewPosition] = newPiece;
        }
        private void PerformPromotionPieceMove(Board newBoard, Move move)
        {
            var promotion = move.Promotion;
            var pieceHistory = new List<Move>(promotion.PromotingPiece.MoveHistory);
            var newPiece = new PieceBuilder(promotion.NewPieceType.Type)
                .As(promotion.PromotingPiece.Color)
                .At(promotion.NewPosition)
                .Create()
                .AddRangeToMoveHistory(pieceHistory)
                .AddMoveToHistory(move);

            newBoard.AddPiece(newPiece);
            newBoard[promotion.OldPosition] = null;
        }
        private void PerformEnPassantPieceMove(Board newBoard, Move move)
        {
            var enPassant = move.EnPassant;
            var newPiece = enPassant.AttackingPawn
                .AddMoveToHistory(move)
                .MoveTo(enPassant.DestinationPosition);
            newBoard[enPassant.AttackingPawn.Position] = null;
            newBoard[enPassant.CapturePosition] = null;
            newBoard[enPassant.DestinationPosition] = newPiece;
        }
        private void PerformCastleMove(Board newBoard, Move move)
        {
            var castle = move.Castle;
            var king = castle.King;
            var rook = castle.Rook;

            int kingHorizMove = castle.Type == CastleMoveType.KingSide ? 2 : -2;

            newBoard[king.Position] = null;
            king = (King)king
                .MoveTo(king.Position.MoveHoriz(kingHorizMove))
                .AddMoveToHistory(move);
            newBoard[king.Position] = king;

            int rookHorizMove = castle.Type == CastleMoveType.KingSide ? -1 : 1;
            newBoard[rook.Position] = null;
            rook = (Rook)rook
                .MoveTo(king.Position.MoveHoriz(rookHorizMove))
                .AddMoveToHistory(move);
            newBoard[rook.Position] = rook;
            
        }

        public static bool IsKingInCheck(Board board, PieceColor color)
        {
            // Is there a king on the board for this color? (There might not be
            // when this is used for constructions or unit tests)
            var pieces = color == PieceColor.Black ? board.BlackPieces : board.WhitePieces;
            var kingPieces = (from p in pieces where p.Type == PieceType.King select p).ToList();
            if (kingPieces.Count == 0) return false;
            var king = kingPieces[0];

            var moves = board.GetPossibleMoves(color.Opposite(), true);
            var takingMoves = (from m in moves
                               where m.PieceCaptured &&
                               m.Type == MoveType.NormalPiece
                               select m.NormalPieceMove).ToList();

            var takeKingMoves = (from m in takingMoves
                                 where m.NewPosition.Equals(king.Position)
                                 select m).ToList();
            return takeKingMoves.Count > 0;
        }
    }
}