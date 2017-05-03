using System;

namespace JohnChess.LichessPuzzles.Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new LichessPuzzleClient();
            var json = client.GetJsonForPuzzleAsync(1).Result;

            Console.WriteLine(json);
        }
    }
}