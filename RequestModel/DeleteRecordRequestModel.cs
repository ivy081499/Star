using System;
namespace Star.RequestModel
{
	public class DeleteRecordRequestModel
	{
		public string Id { get; set; }
		public string CustomerName { get; set; }
		public int Bookie { get; set; }
		public DateTime Date { get; set; }
	}
}

