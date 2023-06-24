using System;
using Star.Enums;
using Star.Helper;
using Star.Models;

namespace Star.ViewModels
{
    public class CustomerBet
    {
        public DateTime Date { get; set; }
        public string BookieText { get; set; }
        public BookieType Bookie { get; set; }
        public int PaperNumber { get; set; }
        public Customer Customer { get; set; }
        public BetStatistics BetStatistics { get; set; }
        public List<BetInfo> BetInfoList { get; set; }
        public List<CarSetInfo> CarSetInfoList { get; set; }
    }

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

