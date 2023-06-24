using System;
using Star.Enums;

namespace Star.RequestModel
{
    public class BetRequestModel
    {
        public DateTime Date { get; set; }
        public BookieType Bookie { get; set; }
        public string CustomerName { get; set; }
        public string BetContent { get; set; }
        public float TwoStarOdds { get; set; }
        public float ThreeStarOdds { get; set; }
        public float FourStarOdds { get; set; }
        public int PaperNumber { get; set; }
    }

    public class CarSetRequestModel
    {
        public DateTime Date { get; set; }
        public BookieType Bookie { get; set; }
        public string CustomerName { get; set; }
        public int CarSetNumber { get; set; }
        public float Odds { get; set; }
        public int PaperNumber { get; set; }
    }
}

