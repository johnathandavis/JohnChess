using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

using JohnChess.Moves;
using JohnChess.Pieces;
using JohnChess.AI.Evaluation;
using JohnChess.AI.Enumeration;

namespace JohnChess.AI
{
    public abstract class AbstractPlayer
    {
        private MoveTreeNode moveTree;
        private HighSpeedTelemetry telemetry;
        private readonly int maxRecurseDepth;

        public AbstractPlayer()
        {
            telemetry = new HighSpeedTelemetry();
            maxRecurseDepth = 3;
        }
        public AbstractPlayer(int maxRecurseDepth)
        {
            telemetry = new HighSpeedTelemetry();
            this.maxRecurseDepth = maxRecurseDepth;
        }

        public void InitializePlayer(Board board, PieceColor color)
        {
            if (moveTree == null)
            {
                moveTree = BuildInitialMoveTree(board, color);
            }
        }
        public Move DecideMove(Board board, PieceColor color)
        {
            telemetry = new HighSpeedTelemetry();

            var now = DateTime.Now;

            ExpandMoveTree(board, color);

            var duration = (DateTime.Now - now).TotalSeconds;
            Console.Title = "Tree Generation took: " + duration;


            var selectedMove = SelectMove(board, moveTree, color);
            moveTree = moveTree.CounterMoves[selectedMove];
            return selectedMove;
        }

        public abstract Move SelectMove(Board board, MoveTreeNode moveTree, PieceColor color);


        private void ExpandMoveTree(Board board, PieceColor color)
        {
            if (board.MoveHistory.Count > 0)
            {
                var lastMove = board.MoveHistory.Last();
                if (moveTree.CounterMoves.ContainsKey(lastMove))
                {
                    moveTree = moveTree.CounterMoves[lastMove];
                }
            }
            FillNodeWithMoves(moveTree, MaximumRecurseDepth, 0);
        }
        private MoveTreeNode BuildInitialMoveTree(Board board, PieceColor color)
        {
            var node = new MoveTreeNode();
            node.Move = null;
            node.Score = 0.0;
            node.Board = board;
            node.ColorToPlay = color;
            FillNodeWithMoves(node, MaximumRecurseDepth, MaximumRecurseDepth-1);
            return node;
        }

        private void FillNodeWithMoves(MoveTreeNode node, int maxDepth, int currentDepth)
        {
            if (currentDepth == maxDepth) return;

            List<Move> possibleMoves;
            try
            {
                possibleMoves = node.Board.GetPossibleMoves(node.ColorToPlay);
            }
            catch (StalemateException)
            {
                node.CounterMoves = new Dictionary<Move, MoveTreeNode>();
                node.IsStalemate = true;
                node.Score = 10000;
                return;
            }
            catch (CheckmateException)
            {
                node.CounterMoves = new Dictionary<Move, MoveTreeNode>();
                node.IsCheckmate = true;
                node.Score = 1000000;
                return;
            }

            if (node.CounterMoves == null)
            {
                var counterMoves = new ConcurrentDictionary<Move, MoveTreeNode>();
                Action<IEnumerable<Move>, Action<Move>> moveProcessor;
                if (currentDepth < 99)
                {
                    moveProcessor = (moves, action) => Parallel.ForEach(moves, (m) => action(m));
                }
                else
                {
                    moveProcessor = (moves, action) => { foreach (var m in moves) action(m); };
                }
                moveProcessor(possibleMoves, (move) =>
                {
                    var child = new MoveTreeNode()
                    {
                        ColorToPlay = node.ColorToPlay.Opposite(),
                        Board = node.Board.PerformMove(move),
                        Move = move
                    };
                    telemetry.IncrementCounter(Counters.GeneratedMoves);
                    FillNodeWithMoves(child, maxDepth, currentDepth + 1);
                    counterMoves.AddOrUpdate(move, child, (m, c) => child);
                });
                node.CounterMoves = counterMoves.ToDictionary(
                    (kvp) => kvp.Key,
                    (kvp) => kvp.Value);
            }
            else
            {
                foreach (var kvp in node.CounterMoves)
                {
                    FillNodeWithMoves(kvp.Value, maxDepth, currentDepth + 1);
                }
            }
        }

        public HighSpeedTelemetry Telemetry
        {
            get
            {
                return telemetry;
            }
        }
        public double CurrentScore
        {
            get
            {
                return moveTree.Score.Value;
            }
        }
        public IReadOnlyList<Move> PossibleMoves
        {
            get
            {
                return (from m in moveTree.CounterMoves.Values
                        select m.Move).ToList();
            }
        }
        protected int MaximumRecurseDepth
        {
            get
            {
                return maxRecurseDepth;
            }
        }
    }
}
