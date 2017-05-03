using System;

namespace JohnChess.Plugins.Lichess.Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new LichessClient();
            var game = client.GetTrainingPuzzleAsync(1).Result;

            foreach (var move in game.UCIMoves)
            {
                Console.WriteLine(move);
            }
            Console.ReadKey();
        }
    }
}