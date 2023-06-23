using System;
namespace Star.Models
{
    /// <summary>
    /// 單筆投注內容
    /// </summary>
	public class BetInfo
    {
        public List<Column> ColumnList { get; set; }
        public float TwoStarOdds { get; set; }
        public float ThreeStarOdds { get; set; }
        public float FourStarOdds { get; set; }
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

    public class CarSetInfo
    {
        public int BallNumber { get; set; }
        public float Odds { get; set; }
    }
}

