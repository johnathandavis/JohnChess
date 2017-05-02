using System;
using System.Collections.Generic;
using System.Linq;

using JohnChess.Moves;
using JohnChess.Pieces;

namespace JohnChess.AI
{
    public abstract class AbstractPlayer<T> : IPlayer where T : IPositionEvaluator, new()
    {
        private MoveTreeNode moveTree;
        private readonly int maxRecurseDepth;

        public AbstractPlayer()
        {
            PositionEvaluator = new T();
            maxRecurseDepth = 3;
        }
        public AbstractPlayer(int maxRecurseDepth)
        {
            PositionEvaluator = new T();
            this.maxRecurseDepth = maxRecurseDepth;
        }

        public Move DecideMove(Board board, PieceColor color)
        {
            if (moveTree == null)
            {
                moveTree = BuildInitialMoveTree(board, color, maxRecurseDepth);
            }
            else
            {
                if (board.MoveHistory.Count > 0)
                {
                    moveTree = moveTree.CounterMoves[board.MoveHistory.Last()];
                }
                FillNodeWithMoves(moveTree, maxRecurseDepth, 0);
            }

            var selectedMove = SelectMove(board, moveTree, color);
            moveTree = moveTree.CounterMoves[selectedMove];
            return selectedMove;
        }

        public abstract Move SelectMove(Board board, MoveTreeNode moveTree, PieceColor color);

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


        private MoveTreeNode BuildInitialMoveTree(Board board, PieceColor color, int maxDepth)
        {
            var node = new MoveTreeNode();
            node.Move = null;
            node.Board = board;
            node.ColorToPlay = color;

            FillNodeWithMoves(node, maxDepth, 0);
            return node;
        }

        private void FillNodeWithMoves(MoveTreeNode node, int maxDepth, int currentDepth)
        {
            if (currentDepth == maxDepth) return;

            var possibleMoves = node.Board.GetPossibleMoves(node.ColorToPlay);

            if (node.CounterMoves == null)
            {
                var counterMoves = new Dictionary<Move, MoveTreeNode>();
                foreach (var move in possibleMoves)
                {
                    var child = new MoveTreeNode()
                    {
                        ColorToPlay = node.ColorToPlay.Opposite(),
                        Board = node.Board.PerformMove(move),
                        Move = move
                    };
                    FillNodeWithMoves(child, maxDepth, currentDepth + 1);
                    counterMoves.Add(move, child);
                }
                node.CounterMoves = counterMoves;
            }
            else
            {
                foreach (var kvp in node.CounterMoves)
                {
                    FillNodeWithMoves(kvp.Value, maxDepth, currentDepth + 1);
                }
            }
        }
    }
}
