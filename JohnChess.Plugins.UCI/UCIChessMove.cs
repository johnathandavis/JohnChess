using System;
using System.Collections.Generic;
using System.Text;

namespace JohnChess.Plugins.UCI
{
    public class UCIChessMove
    {
        public UCIChessMove(string text)
        {
            TextValue = text;
            From = text.Substring(0, 2);
            To = text.Substring(2, 2);
            if (text.Length == 5)
            {
                PromotionChar = text.Substring(4, 1);
            }
        }

        public string TextValue { get; }
        public string From { get; }
        public string To { get; }
        public string PromotionChar { get; }

        public override string ToString()
        {
            return TextValue;
        }
    }
}
