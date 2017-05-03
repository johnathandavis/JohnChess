using System;
using System.Collections.Generic;
using System.Text;

using JohnChess.Pieces;
using JohnChess.AI.Evaluation;
using JohnChess.AI.Enumeration;

namespace JohnChess.AI
{
    public abstract class AbstractPositionalPlayer<T> : AbstractPlayer where T : IPositionEvaluator, new()
    {
        public AbstractPositionalPlayer()
        {
            PositionEvaluator = new T();
        }
        public AbstractPositionalPlayer(int maxRecurseDepth) : base(maxRecurseDepth)
        {
            PositionEvaluator = new T();
        }


        private IPositionEvaluator PositionEvaluator { get; }
        protected double EvaluateBoardFor(MoveTreeNode currentMoveTree, PieceColor color)
        {
            var board = currentMoveTree.Board;
            var myPieces = color == PieceColor.White ? board.WhitePieces : board.BlackPieces;
            var theirPieces = color == PieceColor.Black ? board.WhitePieces : board.BlackPieces;

            var myPieceDict = ConvertPieceListToDict(board, myPieces);
            var theirPieceDict = ConvertPieceListToDict(board, myPieces);

            double myScore = PositionEvaluator.EvaluatePosition(currentMoveTree, myPieceDict, theirPieceDict, board.MoveHistory.Count);
            return myScore;
        }
        private PieceDict ConvertPieceListToDict(Board board, IEnumerable<ChessPiece> pieces)
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
            return pieceDict;
        }
    }
}
