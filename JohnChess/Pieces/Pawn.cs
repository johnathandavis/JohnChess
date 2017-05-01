using System;
using System.Collections.Generic;
using System.Linq;

using JohnChess.Moves;

namespace JohnChess.Pieces
{
    public class Pawn : ChessPiece
    {
        public Pawn(PieceColor color, Position position)
            : base(PieceType.Pawn, color, position) { }

        public override ChessPiece MoveTo(Position position)
        {
            return new Pawn(Color, position);
        }
        public override List<Move> FindMoves(Board board)
        {
            var advancingMoves = GetAdvancingMoves(board);
            var capturingMoves = GetCapturingMoves(board);
            
            var advancingAndCapturingMoves = GetAdvancingMoves(board)
                .Concat(GetCapturingMoves(board))
                .ToList();

            // We need to go through and swap out capturing moves for
            // promotion moves if the pawn gets to the last rank
            var movesWithPromotions = new List<Move>();
            foreach (var m in advancingAndCapturingMoves)
            {
                var newPos = m.NormalPieceMove.NewPosition;
                if (newPos.Rank == Rank._8 || newPos.Rank == Rank._1)
                {
                    // Promotion move!
                    movesWithPromotions.AddRange(MoveBuilder.CreatePromotionMoves(this, newPos, true));
                }
                else
                {
                    movesWithPromotions.Add(m);
                }
            }

            return movesWithPromotions.Concat(GetEnPassantMoves(board)).ToList();
        }

        private List<Move> GetAdvancingMoves(Board board)
        {
            var moves = new List<Move>();

            bool canMoveTwo = false;
            if ((Color == PieceColor.White && Position.Rank == Rank._2) ||
                (Color == PieceColor.Black && Position.Rank == Rank._7))
            {
                canMoveTwo = true;
            }

            var singleMovePos = Position.MoveVert(IncrementDirection);
            if (board[singleMovePos] == null)
            {
                moves.Add(MoveBuilder.CreateNormalMove(this, singleMovePos, false));
            }
            else
            {
                // Can't move two if you can't move one
                canMoveTwo = false;
            }
            if (canMoveTwo)
            {
                var moveTwoPos = Position.MoveVert(IncrementDirection * 2);
                if (board[moveTwoPos] == null)
                {
                    moves.Add(MoveBuilder.CreateNormalMove(this, moveTwoPos, false));
                }
            }
            
            return moves;
        }

        private List<Move> GetCapturingMoves(Board board)
        {
            var capturingMoves = new List<Move>();

            if (Position.File != File.A)
            {
                var leftDiagPosition = Position
                    .MoveVert(IncrementDirection)
                    .MoveHoriz(-1);
                if (IsPositionEnemyOccupied(board, leftDiagPosition))
                    capturingMoves.Add(MoveBuilder.CreateNormalMove(this, leftDiagPosition, true));
            }
            if (Position.File != File.H)
            {
                var rightDiagPosition = Position
                    .MoveVert(IncrementDirection)
                    .MoveHoriz(1);
                if (IsPositionEnemyOccupied(board, rightDiagPosition))
                    capturingMoves.Add(MoveBuilder.CreateNormalMove(this, rightDiagPosition, true));
            }
            
            return capturingMoves;
        }

        private List<Move> GetEnPassantMoves(Board board)
        {
            var enPassantMoves = new List<Move>();

            // Is the pawn in the right rank?
            if (Color == PieceColor.Black && Position.Rank != Rank._4) return new List<Move>();
            if (Color == PieceColor.White && Position.Rank != Rank._5) return new List<Move>();

            // Create a few KVP of Key = new position and value = piece to capture position
            var positions = new List<KeyValuePair<Position, Position>>();
            if ((int)Position.File >= (int)File.B)
            {
                positions.Add(
                    new KeyValuePair<Position, Position>(
                        Position.MoveVert(IncrementDirection).MoveHoriz(-1),
                        Position.MoveHoriz(-1)));
            }
            if ((int)Position.File <= (int)File.G)
            {
                positions.Add(
                    new KeyValuePair<Position, Position>(
                        Position.MoveVert(IncrementDirection).MoveHoriz(1),
                        Position.MoveHoriz(1)));
            }
            foreach (var kvp in positions)
            {
                var newPos = kvp.Key;
                var capturePos = kvp.Value;
                
                // There has to be an enemy piece there
                var pieceToCapture = board[capturePos];
                if (pieceToCapture == null ||
                    pieceToCapture.Color == this.Color) continue;

                // The piece must have made a pawn advances 2 move.
                if (pieceToCapture.MoveHistory == null ||
                    pieceToCapture.MoveHistory.Count != 1) continue;

                // The move must be the most recently played move in the game.
                var pieceToCaptureLastMove = pieceToCapture.MoveHistory.Last();
                if (pieceToCaptureLastMove != board.LastMove) continue;

                // We have a candidate for en passant!
                enPassantMoves.Add(MoveBuilder.CreateEnPassantMove(this, newPos, capturePos));
            }

            return enPassantMoves;
        }
        
        private bool IsPositionEnemyOccupied(Board board, Position position)
        {
            var piece = board[position];
            return piece != null && piece.Color != this.Color;
        }
    }
}
