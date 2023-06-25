using System;
using Star.Models;

namespace Star.ResponseModel
{
    public class BookieBetModel
    {
        public string Date { get; set; }
        public List<BookiePaper> BookiePapaerList { get; set; }
        public Report DailyReport { get; set; }
    }

    public class BookiePaper
    {
        public int PaperNumber { get; set; }

        public List<BetInfo> BetInfoList { get; set; }

        public List<CarSetInfo> CarSetInfoList { get; set; }

        public Report PaperReport { get; set; }
    }
}

