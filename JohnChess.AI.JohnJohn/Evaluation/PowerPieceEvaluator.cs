using System;
using System.Collections.Generic;
using System.Linq;

using JohnChess.Pieces;
using JohnChess.AI.Evaluation;
using JohnChess.AI.Enumeration;

namespace JohnChess.AI.JohnJohn.Evaluation
{
    public abstract class PowerPieceEvaluator
    {
        private const double NON_TAKING_MOVES_BONUS = 0.1;
        private const double TAKING_MOVES_BONUS = 0.25;

        protected double CalculateBonusForMoves(ChessPiece piece, MoveTreeNode moveTree)
        {
            var possibleMoves = (from m in moveTree.CounterMoves
                                 select m.Value.Move);
            var myPossibleMoves = (from move in possibleMoves
                                       where move.Type == Moves.MoveType.NormalPiece &&
                                       move.NormalPieceMove.Piece == piece
                                       select move.NormalPieceMove);

            int totalMoves = myPossibleMoves.Count();
            int takingMoves = (from move in myPossibleMoves
                                  where move.PieceCaptured
                                  select move).Count();
            int nonTakingMoves = totalMoves - takingMoves;
            return takingMoves * TAKING_MOVES_BONUS +
                nonTakingMoves * NON_TAKING_MOVES_BONUS;
        }
    }
}
