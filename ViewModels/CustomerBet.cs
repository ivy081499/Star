using System;
using Star.Models;

namespace Star.ViewModels
{
	public class CustomerBet
	{
		public Customer Customer { get; set; }

		public int TotalTwoStar { get; set; }
		public int TotalThreeStar { get; set; }
		public int TotalFourStar { get; set; }
		public int TotalCarSet{ get; set; }

		public List<BetInfo> BetInfoList { get; set; }
		public List<CarSetInfo> CarSetInfoList { get; set; }
	}
}

