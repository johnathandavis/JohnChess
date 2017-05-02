using System;
using System.Collections.Generic;
using System.Linq;

using JohnChess.Pieces;

namespace JohnChess.AI.JohnJohn.Evaluation
{
    public class PawnEvaluator : IPositionEvaluator
    {
        // Pawns start the game at 0.5 value.
        // Starting from the outside files, A and H, each pawn gets
        // a bonus for each square from the outside file they are.
        // This bonus is 0.05. The same bonus applies to the pawn's rank.
        // As they get closer to the Rank where the king is, 
        private const double START_VALUE = 0.50;
        private const double FILE_BONUS = 0.05;
        private const double RANK_BONUS = 0.05;
        private const double BUDDY_PAWN_BONUS = 0.1;
        private const double PASSED_PAWN_BONUS = 0.25;
        private const double DOUBLE_PAWN_PENALTY = 0.15;

        public double EvaluatePosition(PieceDict myPieces, PieceDict theirPieces)
        {
            var myPawns = myPieces[PieceType.Pawn].Cast<Pawn>().ToList();
            var theirPawns = theirPieces[PieceType.Pawn].Cast<Pawn>().ToList();

            double pawnScore = 0.0;
            foreach (var p in myPawns)
            {
                var pawnEval = EvaluatePawn(p, myPawns, theirPawns);
                pawnScore += pawnEval.TotalValue;
            }

            return pawnScore;
        }

        private PawnPositionEvaluation EvaluatePawn(Pawn evalPawn, List<Pawn> myPawns, List<Pawn> theirPawns)
        {
            var eval = new PawnPositionEvaluation();
            eval.FileBonusCount = GetPawnFileCount(evalPawn);
            eval.RankBonusCount = GetPawnFileCount(evalPawn);
            eval.ProtectionIndexCount = GetPawnProtectionIndex(evalPawn, myPawns, theirPawns);
            eval.IsDoubled = IsPawnDoubled(evalPawn, myPawns, theirPawns);
            eval.IsPassed = IsPawnPassed(evalPawn, myPawns, theirPawns);

            // Calculate bonus / penalty values.
            eval.InitialValue = START_VALUE;
            eval.FileBonusValue = eval.FileBonusCount * FILE_BONUS;
            eval.RankBonusValue = eval.RankBonusCount * RANK_BONUS;
            eval.ProtectionIndexValue = eval.ProtectionIndexCount * BUDDY_PAWN_BONUS;
            if (eval.IsDoubled) eval.DoubledPenalty = DOUBLE_PAWN_PENALTY;
            if (eval.IsPassed) eval.PassedBonus = PASSED_PAWN_BONUS;

            return eval;
        }

        internal int GetPawnFileCount(Pawn evalPawn)
        {
            int pawnFile = (int)evalPawn.Position.File;
            // Add file bonus
            return (pawnFile <= 4) ? pawnFile : 8 - pawnFile;
        }

        internal int GetPawnRankCount(Pawn evalPawn)
        {
            int pawnRank = (int)evalPawn.Position.Rank;
            // Add rank bonus
            return (evalPawn.Color == PieceColor.White)
                ? pawnRank - 1
                : 7 - pawnRank;
        }

        internal int GetPawnProtectionIndex(Pawn evalPawn, List<Pawn> myPawns, List<Pawn> theirPawns)
        {
            // Check for buddy pawn
            int vertDir = (evalPawn.Color == PieceColor.White) ? 1 : -1;
            var buddyPositions = new List<Position>();
            var enemyPositions = new List<Position>();
            if ((int)evalPawn.Position.File > 1)
            {
                buddyPositions.Add(evalPawn.Position
                    .MoveVert(vertDir)
                    .MoveHoriz(-1));
                enemyPositions.Add(evalPawn.Position
                    .MoveVert(vertDir * -1)
                    .MoveHoriz(-1));
            }
            if ((int)evalPawn.Position.File < 8)
            {
                buddyPositions.Add(evalPawn.Position
                    .MoveVert(vertDir)
                    .MoveHoriz(1));
                enemyPositions.Add(evalPawn.Position
                    .MoveVert(vertDir * -1)
                    .MoveHoriz(1));
            }

            int pawnProtectionIndex = 0;
            foreach (var p in myPawns)
            {
                if (buddyPositions.Contains(p.Position)) pawnProtectionIndex++;
            }
            foreach (var p in theirPawns)
            {
                if (enemyPositions.Contains(p.Position)) pawnProtectionIndex--;
            }
            return pawnProtectionIndex;
        }

        internal bool IsPawnDoubled(Pawn evalPawn, List<Pawn> myPawns, List<Pawn> theirPawns)
        {
            int pawnsInMyFile = 0;
            foreach (var p in myPawns)
            {
                if (p.Position.File == evalPawn.Position.File) pawnsInMyFile++;
            }
            return pawnsInMyFile >= 2;
        }

        internal bool IsPawnPassed(Pawn evalPawn, List<Pawn> myPawns, List<Pawn> theirPawns)
        {
            // Don't even consider a pawn potentially passed unless it is in or past the fifth rank
            if ((evalPawn.Color == PieceColor.White && (int)evalPawn.Position.Rank < 5) ||
                (evalPawn.Color == PieceColor.Black && (int)evalPawn.Position.Rank > 4)) return false;

            var passedFiles = new List<File>();
            if ((int)evalPawn.Position.File > 1) passedFiles.Add(evalPawn.Position.MoveHoriz(-1).File);
            if ((int)evalPawn.Position.File < 8) passedFiles.Add(evalPawn.Position.MoveHoriz(1).File);
            passedFiles.Add(evalPawn.Position.File);

            Predicate<Rank> isPawnPastRankFunction = (rank) =>
                evalPawn.Color == PieceColor.White
                ? ((int)evalPawn.Position.Rank > (int)rank)
                : ((int)evalPawn.Position.Rank < (int)rank);

            bool isEnemyPreventingPassedPawn = false;
            foreach (var enemyPawn in theirPawns)
            {
                if (passedFiles.Contains(enemyPawn.Position.File) &&
                    isPawnPastRankFunction(enemyPawn.Position.Rank))
                {
                    isEnemyPreventingPassedPawn = true;
                    break;
                }
            }

            return !isEnemyPreventingPassedPawn;
        }
    }
}
