using System;
using Star.Models;

namespace Star.ResponseModel
{
    public class BookieBetModel
    {
        public DateTime Date { get; set; }
        public List<BookiePaper> BookiePapaerList { get; set; }
        public int WinLoseDollars { get; set; }
    }

    public class BookiePaper
    {
        public int PaperNumber { get; set; }

        public List<BetInfo> BetInfoList { get; set; }

        public List<CarSetInfo> CarSetInfoList { get; set; }

        public BetStatistics BetStatistics { get; set; }
    }
}

