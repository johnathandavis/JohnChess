using System;
using System.Collections.Generic;
using System.Text;

using JohnChess.AI.Evaluation;
using JohnChess.AI.Enumeration;

namespace JohnChess.AI.JohnJohn.Evaluation
{
    public class PawnPositionEvaluation
    {
        public double InitialValue { get; set; }
        public double RankBonusCount { get; set; }
        public double RankBonusValue { get; set; }
        public double FileBonusCount { get; set; }
        public double FileBonusValue { get; set; }
        public double ProtectionIndexCount { get; set; }
        public double ProtectionIndexValue { get; set; }
        public bool IsDoubled { get; set; }
        public double DoubledPenalty { get; set; }
        public bool IsPassed { get; set; }
        public double PassedBonus { get; set; }

        public double TotalValue
        {
            get
            {
                return InitialValue + RankBonusValue + FileBonusValue
                    + ProtectionIndexValue + PassedBonus
                    - DoubledPenalty;
            }
        }
    }
}
