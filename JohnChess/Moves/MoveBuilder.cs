using System;
using System.Collections.Generic;
using System.Text;

using JohnChess.Pieces;

namespace JohnChess.Moves
{
    public class MoveBuilder
    {
        public static Move CreateNormalMove(ChessPiece piece, Position newPosition, bool takes)
        {
            var move = new NormalPieceMove(piece, newPosition, takes);
            return new Move(move);
        }
        public static List<Move> CreatePromotionMoves(Pawn pawn, Position newPosition, bool takes)
        {
            var moves = new List<Move>();
            foreach (var promotePieceType in PromotionMove.PromotingPieceType.PromotingPieceTypes)
            {
                var promotion = new PromotionMove(pawn, newPosition, promotePieceType, takes);
                var move = new Move(promotion);
                moves.Add(move);
            }
            return moves;
        }
        public static Move CreateEnPassantMove(Pawn pawn, Position newPosition, Position capturePosition)
        {
            var enPassant = new EnPassantMove(pawn, capturePosition, newPosition);
            return new Move(enPassant);
        }
        public static Move CreateCastleMove(King king, Rook rook)
        {
            var castle = new CastleMove(king, rook);
            return new Move(castle);
        }
    }
}
