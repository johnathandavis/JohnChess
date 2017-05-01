using System;
using System.Collections.Generic;
using System.Linq;

using JohnChess.Moves;
using JohnChess.Pieces;

namespace JohnChess.AI.JohnJohn
{
    public class JohnsBestGuessScorer
    {
        public double ScorePieces(IEnumerable<ChessPiece> myPieces, IEnumerable<ChessPiece> theirPieces)
        {
            // General Strategy:
            // 1. Convert the IEnumerables to a dict with piece type -> list of pieces of that type
            var myPieceDict = ConvertPieceListToDict(myPieces);
            var theirPieceDict = ConvertPieceListToDict(theirPieces);

            if (myPieceDict[PieceType.King].Count == 0) return 3.0;
            else if (theirPieceDict[PieceType.King].Count == 0) return 3.0;

            double myScore = ScorePlayer1(myPieceDict, theirPieceDict);
            double theirScore = ScorePlayer1(theirPieceDict, myPieceDict);
            return myScore - theirScore;
        }
        private double ScorePlayer1(PieceDict p1, PieceDict p2)
        {
            double pawnScore = ScorePawns(p1, p2);
            double rookScore = ScoreRooks(p1, p2);
            double knightScore = ScoreKnights(p1, p2);
            double bishopScore = ScoreBishops(p1, p2);
            double queenScore = ScoreQueen(p1, p2);
            return pawnScore + rookScore + knightScore + bishopScore + queenScore;
        }
        private PieceDict ConvertPieceListToDict(IEnumerable<ChessPiece> pieces)
        {
            var pieceDict = new PieceDict();
            foreach (var piece in pieces)
            {
                if (!pieceDict.ContainsKey(piece.Type))
                {
                    pieceDict.Add(piece.Type, new List<ChessPiece>());
                }
                pieceDict[piece.Type].Add(piece);
            }
            if (pieceDict[PieceType.King].Count == 0)
            {
                { }
            }
            return pieceDict;
        }
        private double ScorePawns(PieceDict p1, PieceDict p2)
        {
            // Pawns are worth slightly more the closer they are to the opponent's king
            // A pawn in the same file as the opponents king is worth 1.25
            // Each file away is less 0.05
            // Exception is if a pawn is doubled - 0.25 penalty for each pawn.
            // Same rule for Rank distance from king. This means the player attempts
            // to push the pawns forward.
            var kingFile = p2[PieceType.King].First()
                .Position.File;
            var kingRank = p2[PieceType.King].First()
                .Position.Rank;

            double totalPawnScore = 0.0;

            var pawns = p1[PieceType.Pawn];
            var pawnFileDict = new Dictionary<File, List<Pawn>>();
            foreach (var p in pawns)
            {
                if (!pawnFileDict.ContainsKey(p.Position.File))
                    pawnFileDict.Add(p.Position.File, new List<Pawn>());
                pawnFileDict[p.Position.File].Add((Pawn)p);
            }

            foreach (var p in pawns)
            {
                var pawnFile = p.Position.File;
                var pawnRank = p.Position.Rank;

                var fileDistance = Math.Abs((int)kingFile - (int)pawnFile);
                var rankDistance = Math.Abs((int)kingRank - (int)pawnRank);
                fileDistance = fileDistance * fileDistance;
                rankDistance = rankDistance * rankDistance;
                double absDistance = (double)Math.Sqrt((double)(fileDistance+rankDistance));
                double pawnScore = 1.5 - 0.1 * absDistance;
                if (pawnFileDict.ContainsKey(p.Position.File) &&
                    pawnFileDict[p.Position.File].Count > 1) pawnScore -= 0.15;
                totalPawnScore += pawnScore;
            }

            return totalPawnScore;
        }
        private double ScoreRooks(PieceDict p1, PieceDict p2)
        {
            // Rooks are matched up against bishops:
            // 2 rooks, 2 bishops? Rooks = 4.5
            // 2 rooks, 1 or 0 bishops? Rooks = 5
            // 1 rook, 2 bishops? Rooks = 4
            // 1 rook, 1 bishop? Rook = 4.5
            int rookCount = p1[PieceType.Rook].Count;
            int opposingBishopCount = p2[PieceType.Bishop].Count;

            double rookValue = 0;
            if (rookCount == 2)
            {
                if (opposingBishopCount == 2) rookValue = 4.5;
                else rookValue = 5;
            }
            else
            {
                if (opposingBishopCount == 2) rookValue = 4;
                else rookValue = 4.5;
            }
            return rookValue * rookCount;
        }
        private double ScoreBishops(PieceDict p1, PieceDict p2)
        {
            // Bishops are 3.5 points each, unless you have two,
            // then they are 3.75 points each.
            var bishopCount = p1[PieceType.Bishop].Count;
            if (bishopCount == 2) return 7.5;
            else if (bishopCount == 1) return 3.5;
            return 0.0;
        }
        private double ScoreKnights(PieceDict p1, PieceDict p2)
        {
            // Knights are worth 3 each
            var knights = p1[PieceType.Knight];
            return knights.Count * 3.0;
        }
        private double ScoreQueen(PieceDict p1, PieceDict p2)
        {
            int myQueenCount = p1[PieceType.Queen].Count;
            int theirQueenCount = p2[PieceType.Queen].Count;
            int bonus = myQueenCount - theirQueenCount;
            return (myQueenCount * 8.0 + bonus * 3.0);
        }


        private class PieceDict : Dictionary<PieceType, List<ChessPiece>>
        {
            internal PieceDict()
            {
                foreach (PieceType pt in (PieceType[])Enum.GetValues(typeof(PieceType)))
                {
                    Add(pt, new List<ChessPiece>());
                }
            }
        }
    }
}
