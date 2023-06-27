using System;
namespace Star.Models
{
    public class BookieBetInfo
    {
        public string Date { get; set; }
        public List<PaperBet> PaperBetList { get; set; }
        public Report Report { get; set; }

    }

    public class PaperBet
    {
        public List<ColumnBet> SerialBetList { get; set; }
        public List<ColumnBet> ColumnBetList { get; set; }
        public List<CarBet> CarBetList { get; set; }
        public Report Report { get; set; }
        public int Number { get; set; }
    }
}

