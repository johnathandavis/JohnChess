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

        public override List<Move> FindMoves(Board board)
        {
            var advancingMoves = GetAdvancingMoves(board);
            var capturingMoves = GetCapturingMoves(board);

            // TODO: En Passant
            return advancingMoves
                .Concat(capturingMoves)
                .ToList();
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

            if (Position.File != File.A)
            {
                var capturePosition = Position
                    .MoveVert(IncrementDirection)
                    .MoveHoriz(-1);
                var enemyPosition = Position
                    .MoveHoriz(-1);
                if (!IsPositionEnemyOccupied(board, enemyPosition)) return new List<Move>();
                //enPassantMoves.Add(MoveBuilder.CreateNormalMove(this, leftDiagPosition));
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
