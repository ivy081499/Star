using System;
using Star.Enums;
using Star.Models;

namespace Star.Settings
{
    public class CustomerSettings
    {
        public int TwoStarBonus = 5300;
        public int ThreeStarBonus = 57000;
        public int FourStarBonus = 750000;

        public List<CustomerInfo> CustomerList { get; }

        public CustomerSettings()
        {
            CustomerList = new List<CustomerInfo>();

            CustomerList.Add(new CustomerInfo()
            {
                Name = "小雲",
                Cost539 = new CostDefinition()
                {
                    TwoStarPrice = 77,
                    ThreeStarPrice = 77,
                    FourStarPrice = 77,
                    CarSetPrice = 77,
                    TwoStarBonus = this.TwoStarBonus,
                    ThreeStarBonus = this.ThreeStarBonus,
                    FourStarBonus = this.FourStarBonus,
                }
            });

            CustomerList.Add(new CustomerInfo()
            {
                Name = "寶蓮",
                Cost539 = new CostDefinition()
                {
                    TwoStarPrice = 80,
                    ThreeStarPrice = 80,
                    FourStarPrice = 80,
                    CarSetPrice = 80,
                    TwoStarBonus = this.TwoStarBonus,
                    ThreeStarBonus = this.ThreeStarBonus,
                    FourStarBonus = this.FourStarBonus,
                }
            });

            CustomerList.Add(new CustomerInfo()
            {
                Name = "羅老師",
                Cost539 = new CostDefinition()
                {
                    TwoStarPrice = 78,
                    ThreeStarPrice = 78,
                    FourStarPrice = 78,
                    CarSetPrice = 78,
                    TwoStarBonus = this.TwoStarBonus,
                    ThreeStarBonus = this.ThreeStarBonus,
                    FourStarBonus = this.FourStarBonus,
                }
            });

            CustomerList.Add(new CustomerInfo()
            {
                Name = "宥炘",
                Cost539 = new CostDefinition()
                {
                    TwoStarPrice = 80,
                    ThreeStarPrice = 70,
                    FourStarPrice = 70,
                    CarSetPrice = 80,
                    TwoStarBonus = this.TwoStarBonus,
                    ThreeStarBonus = this.ThreeStarBonus,
                    FourStarBonus = this.FourStarBonus,
                }
            });

            CustomerList.Add(new CustomerInfo()
            {
                Name = "呂小姐",
                Cost539 = new CostDefinition()
                {
                    TwoStarPrice = 80,
                    ThreeStarPrice = 80,
                    FourStarPrice = 80,
                    CarSetPrice = 80,
                    TwoStarBonus = this.TwoStarBonus,
                    ThreeStarBonus = this.ThreeStarBonus,
                    FourStarBonus = this.FourStarBonus,
                }
            });

            CustomerList.Add(new CustomerInfo()
            {
                Name = "阿枝兄",
                Cost539 = new CostDefinition()
                {
                    TwoStarPrice = 80,
                    ThreeStarPrice = 80,
                    FourStarPrice = 80,
                    CarSetPrice = 80,
                    TwoStarBonus = this.TwoStarBonus,
                    ThreeStarBonus = this.ThreeStarBonus,
                    FourStarBonus = this.FourStarBonus,
                }
            });

            CustomerList.Add(new CustomerInfo()
            {
                Name = "楊大哥",
                Cost539 = new CostDefinition()
                {
                    TwoStarPrice = 80,
                    ThreeStarPrice = 80,
                    FourStarPrice = 80,
                    CarSetPrice = 80,
                    TwoStarBonus = this.TwoStarBonus,
                    ThreeStarBonus = this.ThreeStarBonus,
                    FourStarBonus = this.FourStarBonus,
                }
            });

            CustomerList.Add(new CustomerInfo()
            {
                Name = "誠",
                Cost539 = new CostDefinition()
                {
                    TwoStarPrice = 80,
                    ThreeStarPrice = 80,
                    FourStarPrice = 80,
                    CarSetPrice = 80,
                    TwoStarBonus = this.TwoStarBonus,
                    ThreeStarBonus = this.ThreeStarBonus,
                    FourStarBonus = this.FourStarBonus,
                }
            });

            CustomerList.Add(new CustomerInfo()
            {
                Name = "阿順",
                Cost539 = new CostDefinition()
                {
                    TwoStarPrice = 80,
                    ThreeStarPrice = 80,
                    FourStarPrice = 80,
                    CarSetPrice = 80,
                    TwoStarBonus = this.TwoStarBonus,
                    ThreeStarBonus = this.ThreeStarBonus,
                    FourStarBonus = this.FourStarBonus,
                }
            });

            CustomerList.Add(new CustomerInfo()
            {
                Name = "可可",
                Cost539 = new CostDefinition()
                {
                    TwoStarPrice = 80,
                    ThreeStarPrice = 80,
                    FourStarPrice = 80,
                    CarSetPrice = 80,
                    TwoStarBonus = this.TwoStarBonus,
                    ThreeStarBonus = this.ThreeStarBonus,
                    FourStarBonus = this.FourStarBonus,
                }
            });

            CustomerList.Add(new CustomerInfo()
            {
                Name = "阿芬",
                Cost539 = new CostDefinition()
                {
                    TwoStarPrice = 78,
                    ThreeStarPrice = 78,
                    FourStarPrice = 78,
                    CarSetPrice = 78,
                    TwoStarBonus = this.TwoStarBonus,
                    ThreeStarBonus = this.ThreeStarBonus,
                    FourStarBonus = this.FourStarBonus,
                }
            });

        }
    }
}

