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
            return File.ToString() + ((int)Rank).ToString();
        }
    }
}
