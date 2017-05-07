using System;
using System.Collections.Generic;
using System.Text;
using JohnChess.Plugins.UCI;

namespace JohnChess.Plugins.Lichess
{
    public class LichessGame
    {
        public int StartingPly { get; set; }
        public string Clock { get; set; }
        public List<UCIChessMove> UCIMoves { get; set; } = new List<UCIChessMove>();

        public static LichessGame FromDynamicPuzzleJson(dynamic json)
        {
            var game = json.puzzle.data.game;
            var parts = game.treeParts;

            var lichess = new LichessGame();
            lichess.Clock = game.clock.ToString();
            lichess.StartingPly = (int)json.puzzle.data.puzzle.initialPly;
            foreach (var p in parts)
            {
                if (p.uci != null)
                {
                    string uci = p.uci.ToString();
                    lichess.UCIMoves.Add(new UCIChessMove(uci));
                }
                if (lichess.UCIMoves.Count >= lichess.StartingPly) break;
            }
            return lichess;
        }
    }
}
