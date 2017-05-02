using System;
using System.Collections.Generic;
using System.Text;

using Xunit;

using JohnChess;
using JohnChess.AI;
using JohnChess.Pieces;
using JohnChess.AI.JohnJohn.Evaluation;

namespace JohnChess.AI.JohnJohn.Tests.Evaluation
{
    public class PawnEvaluatorTests
    {
        private Board CreateBoardWithWhitePawnAt(Position position)
        {
            var board = Board.NewEmptyBoard();
            board.AddPiece(new PieceBuilder(PieceType.Pawn)
                .As(PieceColor.White)
                .At(position)
                .Create());
            return board;
        }

        [Fact]
        public void DetectsPassedPawns()
        {
            var pawnEvaluator = new PawnEvaluator();

            var whitePawnC7 = CreateBoardWithWhitePawnAt(new Position(File.C, Rank._7));
            Assert.True(pawnEvaluator)
        }
    }
}
