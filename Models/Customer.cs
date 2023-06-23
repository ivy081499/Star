using System;
using Star.Enums;

namespace Star.Models
{
    public class Customer
    {
        public string Name { get; set; }
        public LotteryType LotteryType { get; set; }
        public int TwoStar { get; set; }
        public int ThreeStar { get; set; }
        public int FourStar { get; set; }
        public int CarSet { get; set; }
        public int TwoStarBonus { get; set; }
        public int ThreeStarBonus { get; set; }
        public int FourStarBonus { get; set; }
    }
}

