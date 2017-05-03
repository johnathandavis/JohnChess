using System;
using System.Collections.Generic;
using System.Text;

using JohnChess.Pieces;
using JohnChess.AI.Enumeration;

namespace JohnChess.AI.Evaluation
{
    public class ReinfeldMaterialPointsEvaluator : IPositionEvaluator
    {
        /* Pawn - 1 point
           Knight - 3 points
           Bishop - 3 points
           Rook - 5 points
           Queen - 9 points
         */

        private double PAWN_VAL = 1.0;
        private double KNIGHT_VAL = 3.0;
        private double BISHOP_VAL = 3.0;
        private double ROOK_VAL = 5.0;
        private double QUEEN_VAL = 9.0;
        private double KING_VAL = 1000.0;

        public double EvaluatePosition(MoveTreeNode moveTree, PieceDict myPieces, PieceDict theirPieces, int moveNumber)
        {
            double myPoints = EvaluatePieces(myPieces);
            double theirPoints = EvaluatePieces(theirPieces);
            return myPoints - theirPoints;
        }

        private double EvaluatePieces(PieceDict pieces)
        {
            var pawns = pieces[PieceType.Pawn];
            var rooks = pieces[PieceType.Rook];
            var knights = pieces[PieceType.Knight];
            var bishops = pieces[PieceType.Bishop];
            var queens = pieces[PieceType.Queen];
            var kings = pieces[PieceType.King];
            
            double pawnValue = pawns.Count * PAWN_VAL;
            double rookValue = rooks.Count * ROOK_VAL;
            double knightValue = knights.Count * KNIGHT_VAL;
            double bishopsValue = bishops.Count * BISHOP_VAL;
            double queensValue = queens.Count * QUEEN_VAL;
            double kingsValue = kings.Count * KING_VAL;

            return pawnValue + rookValue + knightValue + bishopsValue + queensValue + kingsValue;
        }
    }
}
