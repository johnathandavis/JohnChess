using System;
using System.Collections.Generic;
using System.Text;

namespace JohnChess
{
    public struct Position
    {
        public Position(File file, Rank rank)
        {
            Rank = rank;
            File = file;
        }

        public Rank Rank { get; }
        public File File { get; }

        public int RowX
        {
            get
            {
                return (int)File - 1;
            }
        }
        public int ColY
        {
            get
            {
                return (int)Rank - 1;
            }
        }
        public override string ToString()
        {
            // Should be something like:
            // F5 (File first, then Rank)
            return File.ToString().ToLower() + ((int)Rank).ToString();
        }

        public static bool IsOnBoard(Position position)
        {
            return IsOnBoard(position.Rank, position.File);
        }
        public static bool IsOnBoard(Rank r, File f)
        {
            return IsOnBoard((int)r, (int)f);
        }
        public static bool IsOnBoard(int r, int f)
        {
            return (r >= 1 && r <= 8) &&
                (f >= 1 && f <= 8);
        }
    }
}
