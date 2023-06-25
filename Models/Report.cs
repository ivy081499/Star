using System;
namespace Star.Models
{
    public class Report
    {
        public string TotalTwoStar { get; set; }
        public string TotalThreeStar { get; set; }
        public string TotalFourStar { get; set; }
        public string TotalCarSet { get; set; }
        public int TotalBetMoney { get; set; }

        public double TwoStarBonus { get; set; }
        public double ThreeStarBonus { get; set; }
        public double FourStarBonus { get; set; }
        public int TotalBonusMoney { get; set; }

        public int WinLoseMoney { get; set; }

    }
}

