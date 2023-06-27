using System;
using Star.Enums;

namespace Star.Models
{
    public class BaseBetInfo
    {
        public string Id { get; set; }
        public string CustomerName { get; set; }
        public string Date { get; set; }
        public BookieType BookieType { get; set; }
        public string Bookie { get { return this.BookieType.ToString(); } }
        public int PaperNumber { get; set; }
        public OddsInfo OddsInfo { get; set; }
        public string RawContent { get; set; }
    }

    //排碰、連碰
    public class ColumnBet : BaseBetInfo
    {
        public List<List<string>> Content { get; set; }
    }

    //車組
    public class CarBet : BaseBetInfo
    {
        public string Content { get; set; }
    }

}

