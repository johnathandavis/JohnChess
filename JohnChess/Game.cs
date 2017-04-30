using System;
using System.Collections.Generic;
using System.Text;

using JohnChess.Moves;
using JohnChess.Pieces;

namespace JohnChess
{
    public class Game
    {
        private readonly Random rnd = new Random();
        private bool whiteTurn = true;

        public Game()
        {
            Board = Board.NewStandardBoard();
        }

        public void NextTurn()
        {
            whiteTurn = !whiteTurn;
        }
        public List<Move> GetCurrentPlayerMoves()
        {
            PieceColor color = whiteTurn ? PieceColor.White : PieceColor.Black;
            return Board.GetPossibleMoves(color);
        }
        public void MakeRandomMove(List<Move> moves)
        {
            var randomMove = moves[rnd.Next(0, moves.Count)];
            Board = Board.PerformMove(randomMove);
            NextTurn();
        }

        public Board Board { get; private set; }
    }
}
