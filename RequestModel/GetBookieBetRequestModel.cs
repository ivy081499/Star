using System;
using Star.Enums;

namespace Star.RequestModel
{
	public class GetBookieBetRequestModel
	{
		public BookieType BookieType { get; set; }

		public DateTime Date { get; set; }
	}
}

