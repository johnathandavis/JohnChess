using System;
using System.Linq;
using System.Collections.Generic;

using JohnChess;
using JohnChess.Moves;
using JohnChess.Pieces;
using JohnChess.AI;
using JohnChess.AI.JohnJohn;

namespace JohnChessUI
{
    class Program
    {
        private const char H_DIVIDE = '-';
        private const char V_DIVIDE = '|';
        private const char UNOCCUPIED = ' ';
        private const char PAWN = 'p';
        private const char ROOK = 'r';
        private const char BISHOP = 'b';
        private const char KNIGHT = 'n';
        private const char QUEEN = 'Q';
        private const char KING = 'K';

        static void Main(string[] args)
        {
            var johnJohn = new JohnJohnPlayer();
            var randomPlayer = new RandomPlayer();
            var game = new Game(johnJohn, randomPlayer);

            while (true)
            {
                var moves = game.GetCurrentPlayerMoves();
                DrawMoveList("Move History:", 38, game.Board.MoveHistory);
                DrawMoveList("Possible moves:", 82, moves);
                DrawBoard(game.Board);
                Console.WriteLine();
                Console.WriteLine("Enter Move (blank for AI):");
                string move = Console.ReadLine();
                game.MakePlayerMove();
            }
        }

        private static void DrawBoard(Board board)
        {
            var reversedRanks = (Rank[])Enum.GetValues(typeof(Rank));
            var ranks = reversedRanks.Reverse().ToArray();
            var files = (File[])Enum.GetValues(typeof(File));

            // Two spaces for File label and an extra
            // for the first horizontal boundary
            Console.Write("   ");
            foreach (var f in files)
            {
                Console.Write("  " + f.ToString() + " ");
            }
            Console.WriteLine();
            foreach (var r in ranks)
            {
                Console.Write("   ");
                for (int x = 0; x < (files.Length * 4 + 1); x++)
                {
                    Console.Write(H_DIVIDE.ToString());
                }
                Console.WriteLine();
                Console.Write(" " + ((int)r).ToString() + " ");

                bool highlightLast = false;
                foreach (var f in files)
                {
                    var newPos = board.LastMove?.NormalPieceMove?.NewPosition;
                    if ((newPos?.File == f && newPos?.Rank == r) || highlightLast)
                    {
                        highlightLast = !highlightLast;
                        WriteWithColor(V_DIVIDE.ToString(), board.LastMove.PieceCaptured ? ConsoleColor.DarkRed : ConsoleColor.DarkGreen);
                    }
                    else Console.Write(V_DIVIDE.ToString());
                    var piece = board[f, r];
                    char p = PieceToChar(piece);
                    
                    ConsoleColor bg;
                    ConsoleColor fg;
                    ConsoleColor pieceCol;
                    if (IsWhiteSquare(f, r))
                    {
                        fg = ConsoleColor.DarkGray;
                        bg = ConsoleColor.Gray;
                    }
                    else
                    {
                        fg = ConsoleColor.Gray;
                        bg = ConsoleColor.DarkGray;
                    }
                    if (piece == null || piece.Color == PieceColor.White) pieceCol = ConsoleColor.White;
                    else pieceCol = ConsoleColor.Black;
                    WriteWithColor(p.ToString(), pieceCol, fg, bg);
                }
                if (highlightLast)
                    WriteWithColor(V_DIVIDE.ToString(), board.LastMove.PieceCaptured ? ConsoleColor.DarkRed : ConsoleColor.DarkGreen);
                else Console.Write(V_DIVIDE.ToString());
                Console.WriteLine();
            }
            Console.Write("   ");
            for (int x = 0; x < (files.Length * 4 + 1); x++)
            {
                Console.Write(H_DIVIDE.ToString());
            }
        }
        private static char PieceToChar(ChessPiece piece)
        {
            if (piece == null) return UNOCCUPIED;
            switch (piece.Type)
            {
                case PieceType.Pawn: return PAWN;
                case PieceType.Bishop: return BISHOP;
                case PieceType.King: return KING;
                case PieceType.Knight: return KNIGHT;
                case PieceType.Queen: return QUEEN;
                case PieceType.Rook: return ROOK;
                default:
                    return '?';
            }
        }
        private static bool IsWhiteSquare(File f, Rank r)
        {
            var p = new Position(f, r);
            if (p.RowX % 2 == 0) return p.ColY % 2 == 0;
            else return p.ColY % 2 == 1;
        }
        private static void WriteWithColor(string str, ConsoleColor fg)
        {
            var currentFg = Console.ForegroundColor;
            Console.ForegroundColor = fg;
            Console.Write(str);
            Console.ForegroundColor = currentFg;
        }
        private static void WriteWithColor(string piece, ConsoleColor pieceColor, ConsoleColor fg, ConsoleColor bg)
        {
            var currentFg = Console.ForegroundColor;
            var currentBg = Console.BackgroundColor;
            Console.ForegroundColor = fg;
            Console.BackgroundColor = bg;
            Console.Write(" ");
            Console.ForegroundColor = pieceColor;
            Console.Write(piece);
            Console.ForegroundColor = fg;
            Console.Write(" ");
            Console.ForegroundColor = currentFg;
            Console.BackgroundColor = currentBg;
        }

        private static void DrawMoveList(string title, int margin, IReadOnlyList<Move> moves)
        {
            int colMax = 25;
            Console.CursorLeft = margin;
            Console.CursorTop = 0;
            Console.Write(title);

            List<Move> firstCol = moves.Take(Math.Min(moves.Count, colMax)).ToList();
            List<Move> secondCol = moves.Count < colMax
                ? new List<Move>()
                : moves.Skip(colMax).ToList();

            DrawListColumn(firstCol, margin, 1, colMax);
            DrawListColumn(secondCol, margin + 16, firstCol.Count + 1, colMax);
            Console.CursorLeft = 0;
            Console.CursorTop = 0;
        }
        private static void DrawListColumn(List<Move> moves, int leftMargin, int startNumber, int colMax)
        {
            int rowNumber = 1;
            var drawMoves = new List<Move>(moves);
            for (int x = drawMoves.Count; x < colMax; x++) drawMoves.Add(null);
            foreach (var move in drawMoves)
            {
                Console.CursorLeft = leftMargin;
                Console.CursorTop = rowNumber;

                if (move == null)
                {
                    string drawStr = "";
                    for (int x = 0; x < 15; x++) drawStr += " ";
                    Console.Write(drawStr);
                }
                else
                {
                    Console.Write(startNumber.ToString() + ":  " + move + "          ");
                }
                startNumber++;
                rowNumber++;
            }
        }
    }
}