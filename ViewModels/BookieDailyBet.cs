using System;
using Star.Models;

namespace Star.ViewModels
{
	public class BookieDailyBet
	{
        public DateTime Date { get; set; }
        public List<PaperInfo> PaperList { get; set; }

    }

    public class PaperInfo {
        public int PaperNumber { get; set; }
        public Report DailyReport { get; set; }
        public List<BetInfo> BetInfoList { get; set; }
        public List<CarSetInfo> CarSetInfoList { get; set; }
    }
}

