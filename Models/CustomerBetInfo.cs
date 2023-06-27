using System;
namespace Star.Models
{
    public class CustomerBetInfo
    {
        public string Date { get; set; }
        public string CustomerName { get; set; }
        public List<ColumnBet> SerialBetList { get; set; }
        public List<ColumnBet> ColumnBetList { get; set; }
        public List<CarBet> CarBetList { get; set; }
        public Report Report { get; set; }
    }
}

