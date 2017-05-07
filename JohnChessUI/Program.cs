using System;
using System.Linq;
using System.Collections.Generic;

using JohnChess;
using JohnChess.Moves;
using JohnChess.Pieces;
using JohnChess.Notation;
using JohnChess.AI;
using JohnChess.AI.SimpleReinfeld;
using JohnChess.Plugins.Lichess;

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
        private static Game game;
        private static IChessMoveFormatter moveFormatter = new LongFormMoveFormatter();
        private static IChessPieceFormatter pieceFormatter = new DefaultPieceFormatter(true);

        static void Main(string[] args)
        {
            var johnJohn = new ReinfeldPlayer();
            var randomPlayer = new ReinfeldPlayer();

            var lichessClient = new LichessClient();
            var lichessGame = lichessClient.GetTrainingPuzzleAsync(1).Result;
            var johnChessBoard = LichessGameJohnChessConverter.ToJohnChessGame(lichessGame);

            game = new Game(johnJohn, randomPlayer, johnChessBoard);

            johnJohn.InitializePlayer(game.Board, PieceColor.White);
            randomPlayer.InitializePlayer(game.Board, PieceColor.White);

            Func<Move, string> movePrinter = (move) => moveFormatter.FormatMove(move, pieceFormatter);
            Func<MoveSnapshot, string> snapshotPrinter = (snapshot) =>
            {
                string moveStr = moveFormatter.FormatMove(snapshot.Move, pieceFormatter);
                return moveStr +  " (" + snapshot.EvaluatedScore + ")";
            };

            Console.WriteLine("Starting game...");
            while (true)
            {
                try
                {
                    var moves = game.GetCurrentPlayerMoves();
                    DrawList("Move History:", 38, game.MoveSnapshots, snapshotPrinter);
                    DrawList("Possible moves:", 82, game.GetCurrentPlayerMoves(), movePrinter);
                    DrawBoard(game.Board);
                    Console.WriteLine();
                    Console.WriteLine("Enter Move (blank for AI):");
                    game.MakePlayerMove();
                    long generatedMoves = ((ReinfeldPlayer)game.PreviousPlayer).Telemetry.GetCounter(Counters.GeneratedMoves);
                    Console.Title = Console.Title + " (Generated Moves: " + generatedMoves + ")";
                }
                catch (CheckmateException)
                {
                    bool blackWon = game.BlackPlayer == game.PreviousPlayer;
                    Console.WriteLine(blackWon ? "Black Wins!" : "White wins!");
                    Console.ReadKey();
                }
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

        private static void DrawList<T>(string title, int margin, IReadOnlyList<T> moves, Func<T, string> strGenerator) where T : class
        {
            int colMax = 25;
            Console.CursorLeft = margin;
            Console.CursorTop = 0;
            Console.Write(title);

            var firstCol = moves.Take(Math.Min(moves.Count, colMax)).ToList();
            var secondCol = moves.Count < colMax
                ? new List<T>()
                : moves.Skip(colMax).ToList();

            DrawListColumn(firstCol, strGenerator, margin, 1, colMax);
            DrawListColumn(secondCol, strGenerator, margin + 16, firstCol.Count + 1, colMax);
            Console.CursorLeft = 0;
            Console.CursorTop = 0;
        }
        private static void DrawListColumn<T>(List<T> moves, Func<T, string> strGenerator, int leftMargin, int startNumber, int colMax) where T : class
        {
            int rowNumber = 1;
            var drawMoves = new List<T>(moves);
            for (int x = drawMoves.Count; x < colMax; x++) drawMoves.Add(default(T));
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
                    string moveStr = strGenerator(move);

                    Console.Write(startNumber.ToString() + ":  " + moveStr + "          ");
                }
                startNumber++;
                rowNumber++;
            }
        }
    }
}