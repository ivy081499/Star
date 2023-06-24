using System;
namespace Star.RequestModel
{
    public class ModifyBonusRequestModel
    {
        public DateTime Date { get; set; }
        public string CustomerName { get; set; }

        public string TwoStarBonus1 { get; set; }
        public string ThreeStarBonus1 { get; set; }
        public string FourStarBonus1 { get; set; }

        public string TwoStarBonus2 { get; set; }
        public string ThreeStarBonus2 { get; set; }
        public string FourStarBonus2{ get; set; }

    }
}

