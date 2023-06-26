using System;
namespace Star.Models
{
    public class Report
    {
        public string TotalTwoStar { get; set; }
        public string TotalThreeStar { get; set; }
        public string TotalFourStar { get; set; }
        public string TotalCarSet { get; set; }
        public string TotalBetMoney { get; set; }

        public double TwoStarBonus { get; set; }
        public double ThreeStarBonus { get; set; }
        public double FourStarBonus { get; set; }
        public string TotalBonusMoney { get; set; }

        public string WinLoseMoney { get; set; }

    }
}

