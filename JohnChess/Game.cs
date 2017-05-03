using System;
using System.Collections.Generic;
using System.Text;

using JohnChess.AI;
using JohnChess.Moves;
using JohnChess.Pieces;

namespace JohnChess
{
    public class Game
    {
        private readonly AbstractPlayer whitePlayer;
        private readonly AbstractPlayer blackPlayer;
        private readonly Random rnd = new Random();
        private readonly List<MoveSnapshot> moveSnapshots;
        private bool whiteTurn = true;

        public Game(AbstractPlayer whitePlayer, AbstractPlayer blackPlayer)
        {
            this.whitePlayer = whitePlayer;
            this.blackPlayer = blackPlayer;
            this.moveSnapshots = new List<MoveSnapshot>();
            Board = Board.NewStandardBoard();
        }
        public Game(AbstractPlayer whitePlayer, AbstractPlayer blackPlayer, Board startingBoard)
        {
            this.whitePlayer = whitePlayer;
            this.blackPlayer = blackPlayer;
            this.moveSnapshots = new List<MoveSnapshot>();
            Board = startingBoard;
        }

        public void NextTurn()
        {
            whiteTurn = !whiteTurn;
        }
        public AbstractPlayer PreviousPlayer
        {
            get
            {
                if (WhiteTurn) return blackPlayer;
                else return whitePlayer;
            }
        }
        public AbstractPlayer CurrentPlayer
        {
            get
            {
                if (WhiteTurn) return whitePlayer;
                else return blackPlayer;
            }
        }
        public AbstractPlayer WhitePlayer => whitePlayer;
        public AbstractPlayer BlackPlayer => blackPlayer;
        public IReadOnlyList<Move> GetCurrentPlayerMoves()
        {
            return CurrentPlayer.PossibleMoves;
        }
        public void MakePlayerMove()
        {
            Move moveToMake;
            if (WhiteTurn)
            {
                moveToMake = whitePlayer.DecideMove(Board, PieceColor.White);
            }
            else
            {
                moveToMake = blackPlayer.DecideMove(Board, PieceColor.Black);
            }
            var moveSnapshot = new MoveSnapshot()
            {
                Color = WhiteTurn ? PieceColor.White : PieceColor.Black,
                EvaluatedScore = CurrentPlayer.CurrentScore,
                Move = moveToMake,
                Moves = new List<Move>(CurrentPlayer.PossibleMoves)
            };
            moveSnapshots.Add(moveSnapshot);
            Board = Board.PerformMove(moveToMake);
            NextTurn();
        }

        public Board Board { get; private set; }
        public bool WhiteTurn
        {
            get
            {
                return whiteTurn;
            }
        }
        public IReadOnlyList<MoveSnapshot> MoveSnapshots => moveSnapshots;
    }
}
