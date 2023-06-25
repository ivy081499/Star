using System;
namespace Star.RequestModel
{
    public class ModifyBonusRequestModel
    {
        public DateTime Date { get; set; }
        public string CustomerName { get; set; }

        public string TwoStarBonus { get; set; }
        public string ThreeStarBonus { get; set; }
        public string FourStarBonus { get; set; }
    }
}

