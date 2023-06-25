using System;
using Star.Enums;

namespace Star.Models
{
    public class BaseBetInfo
    {
        public string Id { get; set; }
        public BookieType BookieType { get; set; }
        public string Bookie { get { return this.BookieType.ToString(); } }
        public int PaperNumber  { get; set; }
    }

    /// <summary>
    /// 單筆投注內容
    /// </summary>
	public class BetInfo : BaseBetInfo
    {
        public List<Column> ColumnList { get; set; }
        public float TwoStarOdds { get; set; }
        public float ThreeStarOdds { get; set; }
        public float FourStarOdds { get; set; }
        public string BetContent { get; set; }
    }

    /// <summary>
    /// 單排
    /// </summary>
    public class Column
    {
        /// <summary>
        /// 單排內的數字清單
        /// </summary>
        public List<int> Numbers { get; set; }
    }

    public class CarSetInfo : BaseBetInfo
    {
        public int CarSetNumber { get; set; }
        public float Odds { get; set; }
    }
}

