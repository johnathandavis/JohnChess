using System;
using System.Collections.Generic;
using System.Text;

using JohnChess.Moves;
using JohnChess.Pieces;

namespace JohnChess.AI
{
    public abstract class AbstractPlayer<T> : IPlayer where T : IPositionEvaluator, new()
    {
        public AbstractPlayer()
        {
            PositionEvaluator = new T();
        }


        public abstract Move SelectMove(Board board, PieceColor color);

        private IPositionEvaluator PositionEvaluator { get; }
        protected double EvaluateBoardFor(Board board, PieceColor color)
        {
            var myPieces = color == PieceColor.White ? board.WhitePieces : board.BlackPieces;
            var theirPieces = color == PieceColor.Black ? board.WhitePieces : board.BlackPieces;

            var myPieceDict = ConvertPieceListToDict(myPieces);
            var theirPieceDict = ConvertPieceListToDict(myPieces);

            return PositionEvaluator.EvaluatePosition(myPieceDict, theirPieceDict);
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
            return pieceDict;
        }
    }
}
