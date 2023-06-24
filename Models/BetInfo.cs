using System;
namespace Star.Models
{
    /// <summary>
    /// 單筆投注內容
    /// </summary>
	public class BetInfo
    {
        public string Id { get; set; }
        public List<Column> ColumnList { get; set; }
        public float TwoStarOdds { get; set; }
        public float ThreeStarOdds { get; set; }
        public float FourStarOdds { get; set; }
        public string BetContent  { get; set; }
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
        public string Id { get; set; }
        public int carSetNumber  { get; set; }
        public float Odds { get; set; }
    }
}

