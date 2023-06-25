using System;
namespace Star.ViewModels
{
    public class TodayBetViewModel
    {
        public DateTime Today { get; set; }

        public List<CustomerDailyBet> CustomerBetList { get; set; }
    }
}

