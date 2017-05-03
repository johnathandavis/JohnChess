using System;
using System.Collections.Generic;
using System.Linq;

using JohnChess;
using JohnChess.Moves;
using JohnChess.Pieces;
using JohnChess.Plugins.UCI;

namespace JohnChess.Plugins.Lichess
{
    public class LichessGameJohnChessConverter
    {
        public static Board ToJohnChessGame(LichessGame game)
        {
            var board = Board.NewStandardBoard();
            foreach (var uciMove in game.UCIMoves)
            {
                var move = DetermineMove(board, uciMove);
                board = board.PerformMove(move);
            }

            return board;
        }

        private static Move DetermineMove(Board board, UCIChessMove uciMove)
        {
            string from = uciMove.From;
            string to = uciMove.To;
            var fromPos = Position.Parse(from);
            var toPos = Position.Parse(to);
            var fromPiece = board[fromPos];

            int fileDiff = Math.Abs((int)fromPiece.Position.File - (int)toPos.File);
            if (fromPiece.Type == PieceType.King && (fileDiff == 3 || fileDiff == 4))
            {
                King king = (King)fromPiece;
                Rook rook;
                if (fileDiff == 4) rook = (Rook)board[File.A, fromPiece.Position.Rank];
                else rook = (Rook)board[File.H, fromPiece.Position.Rank];

                return MoveBuilder.CreateCastleMove(king, rook);
            }

            if (uciMove.PromotionChar != null)
            {
                string promotionChar = uciMove.PromotionChar.ToUpper();
                var promotionType = (from p in (PieceType[])Enum.GetValues(typeof(PieceType))
                                     where p.ToNotationLetter().ToUpper() == promotionChar.ToUpper()
                                     select p).First();
                var promotionMoves = MoveBuilder.CreatePromotionMoves((Pawn)fromPiece, toPos, board[toPos] != null);
                return (from m in promotionMoves
                        where m.Promotion.NewPieceType.Type == promotionType
                        select m).First();
            }

            // Check for en passant
            var existingPieceRank = fromPiece.Position.Rank;
            var isCorrectRank = fromPiece.Color == PieceColor.Black ? existingPieceRank == Rank._4 : existingPieceRank == Rank._5;

            try
            {
                int vertDir = fromPiece.Color == PieceColor.White ? -1 : 1;
                var existingPieceTo = board[toPos];
                var capturedPiecePos = toPos.MoveVert(vertDir);
                var capturedPiece = board[capturedPiecePos];
                if (capturedPiece.Color == fromPiece.Color.Opposite() &&
                    capturedPiece.MoveHistory.Count == 1)
                {
                    // This is an en passant
                    return MoveBuilder.CreateEnPassantMove((Pawn)fromPiece, toPos, capturedPiecePos);
                }
            }
            catch { }

            // Only other option is that this is a normal move.
            return MoveBuilder.CreateNormalMove(fromPiece, toPos, board[toPos] != null);
        }
    }
}
