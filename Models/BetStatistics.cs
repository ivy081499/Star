using System;
namespace Star.Models
{
    public class BetStatistics
    {
        public string TotalTwoStar { get; set; }
        public string TotalThreeStar { get; set; }
        public string TotalFourStar { get; set; }
        public string TotalCarSet { get; set; }
        public int TotalBetDollars { get; set; }

        public float TwoStarBonus { get; set; }
        public float ThreeStarBonus { get; set; }
        public float FourStarBonus { get; set; }
        public int TotalBonusDollars { get; set; }

        public int WinLoseDollars { get; set; }

    }
}

