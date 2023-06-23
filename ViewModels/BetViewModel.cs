using System;
using Star.Enums;
using Star.Settings;

namespace Star.ViewModels
{
    public class BetViewModel
    {
        public CustomerSettings CustomerSettings { get; set; }

        public List<BookieViewModel> BookieList { get; set; }
    }
}

