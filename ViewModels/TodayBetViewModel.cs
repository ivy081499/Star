using System;
using Star.Models;

namespace Star.ViewModels
{
    public class TodayBetViewModel
    {
        public DateTime Today { get; set; }

        public List<CustomerBetInfo> CustomerBetInfoList { get; set; }
    }
}

