using System;
using Star.Enums;
using Star.Helper;
using Star.Models;

namespace Star.ViewModels
{
    public class CustomerDailyBet
    {
        public string Date { get; set; }
        public CustomerInfo Customer { get; set; }
        public Report DailyReport { get; set; }
        public List<BetInfo> BetInfoList { get; set; }
        public List<CarSetInfo> CarSetInfoList { get; set; }
    }

}

