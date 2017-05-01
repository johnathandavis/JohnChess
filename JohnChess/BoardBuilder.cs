using System;
using System.Collections.Generic;
using System.Text;

using JohnChess.Pieces;

namespace JohnChess
{
    public partial class Board
    {
        public static Board NewEmptyBoard()
        {
            return new Board();
        }
        public static Board NewStandardBoard()
        {
            var board = new Board();
            SetupBoardForColor(board, PieceColor.White);
            SetupBoardForColor(board, PieceColor.Black);
            return board;
        }
        
        private static void SetupBoardForColor(Board board, PieceColor color)
        {
            // Setup pawns
            Rank pawnRank = (color == PieceColor.White) ? Rank._2 : Rank._7;
            SetupPiecesAt(board, color, PieceType.Pawn, pawnRank,
                (File[])Enum.GetValues(typeof(File)));

            Rank primaryRank = (color == PieceColor.White) ? Rank._1 : Rank._8;

            // Rooks
            SetupPiecesAt(board, color, PieceType.Rook,
                primaryRank, File.A, File.H);

            // Knights
            SetupPiecesAt(board, color, PieceType.Knight,
                primaryRank, File.B, File.G);
            // Bishops
            SetupPiecesAt(board, color, PieceType.Bishop,
                primaryRank, File.C, File.F);
                
            // Setup the King and Queen
            SetupPiecesAt(board, color, PieceType.Queen,
                primaryRank, File.D);
            SetupPiecesAt(board, color, PieceType.King,
                primaryRank, File.E);
        }

        private static void SetupPiecesAt(Board board,
            PieceColor color, PieceType pieceType, Rank rank, params File[] files)
        {
            foreach (var f in files)
            {
                board.AddPiece(new PieceBuilder(pieceType)
                    .As(color).At(new Position(f, rank))
                    .Create());
            }
        }
    }
}
