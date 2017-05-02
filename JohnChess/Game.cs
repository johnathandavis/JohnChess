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
        private readonly IPlayer whitePlayer;
        private readonly IPlayer blackPlayer;
        private readonly Random rnd = new Random();
        private bool whiteTurn = true;

        public Game(IPlayer whitePlayer, IPlayer blackPlayer)
        {
            this.whitePlayer = whitePlayer;
            this.blackPlayer = blackPlayer;
            Board = Board.NewStandardBoard();
        }

        public void NextTurn()
        {
            whiteTurn = !whiteTurn;
        }
        public IPlayer PreviousPlayer
        {
            get
            {
                if (WhiteTurn) return blackPlayer;
                else return whitePlayer;
            }
        }
        public IPlayer CurrentPlayer
        {
            get
            {
                if (WhiteTurn) return whitePlayer;
                else return blackPlayer;
            }
        }
        public List<Move> GetCurrentPlayerMoves()
        {
            PieceColor color = whiteTurn ? PieceColor.White : PieceColor.Black;
            return Board.GetPossibleMoves(color);
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
    }
}
