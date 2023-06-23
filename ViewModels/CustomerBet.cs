using System;
using Star.Enums;
using Star.Models;

namespace Star.ViewModels
{
	public class CustomerBet
	{
		public DateTime Date { get; set; }
		public string BookieType { get; set; }
		public Customer Customer { get; set; }

		public int TotalTwoStar { get; set; }
		public int TotalThreeStar { get; set; }
		public int TotalFourStar { get; set; }
		public int TotalCarSet{ get; set; }
		public int TotalBetDollars { get; set; }

		public int TwoStarBonus { get; set; }
		public int ThreeStarBonus { get; set; }
		public int FourStarBonus { get; set; }
		public int TotalBonusDollars { get; set; }

		public List<BetInfo> BetInfoList { get; set; }
		public List<CarSetInfo> CarSetInfoList { get; set; }
	}
}

