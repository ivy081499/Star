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

        public List<Customer> CustomerList { get;  }

        public CustomerSettings()
        {
            CustomerList = new List<Customer>();

            CustomerList.Add(new Customer()
            {
                LotteryType = LotteryType.Taiwan539,
                Name = "小雲",
                TwoStar = 77,
                ThreeStar = 77,
                FourStar = 77,
                CarSet = 77,
                TwoStarBonus = this.TwoStarBonus,
                ThreeStarBonus = this.ThreeStarBonus,
                FourStarBonus = this.FourStarBonus,
            });

            CustomerList.Add(new Customer()
            {
                LotteryType = LotteryType.Taiwan539,
                Name = "寶蓮",
                TwoStar = 80,
                ThreeStar = 80,
                FourStar = 80,
                CarSet = 80,
                TwoStarBonus = this.TwoStarBonus,
                ThreeStarBonus = this.ThreeStarBonus,
                FourStarBonus = this.FourStarBonus,
            });

            CustomerList.Add(new Customer()
            {
                LotteryType = LotteryType.Taiwan539,
                Name = "羅老師",
                TwoStar = 78,
                ThreeStar = 78,
                FourStar = 78,
                CarSet = 78,
                TwoStarBonus = this.TwoStarBonus,
                ThreeStarBonus = this.ThreeStarBonus,
                FourStarBonus = this.FourStarBonus,
            });

            CustomerList.Add(new Customer()
            {
                LotteryType = LotteryType.Taiwan539,
                Name = "宥炘",
                TwoStar = 80,
                ThreeStar = 70,
                FourStar = 70,
                CarSet = 80,
                TwoStarBonus = this.TwoStarBonus,
                ThreeStarBonus = this.ThreeStarBonus,
                FourStarBonus = this.FourStarBonus,
            });

            CustomerList.Add(new Customer()
            {
                LotteryType = LotteryType.Taiwan539,
                Name = "呂小姐",
                TwoStar = 80,
                ThreeStar = 80,
                FourStar = 80,
                CarSet = 80,
                TwoStarBonus = this.TwoStarBonus,
                ThreeStarBonus = this.ThreeStarBonus,
                FourStarBonus = this.FourStarBonus,
            });

            CustomerList.Add(new Customer()
            {
                LotteryType = LotteryType.Taiwan539,
                Name = "阿枝兄",
                TwoStar = 80,
                ThreeStar = 80,
                FourStar = 80,
                CarSet = 80,
                TwoStarBonus = this.TwoStarBonus,
                ThreeStarBonus = this.ThreeStarBonus,
                FourStarBonus = this.FourStarBonus,
            });

            CustomerList.Add(new Customer()
            {
                LotteryType = LotteryType.Taiwan539,
                Name = "楊大哥",
                TwoStar = 80,
                ThreeStar = 80,
                FourStar = 80,
                CarSet = 80,
                TwoStarBonus = this.TwoStarBonus,
                ThreeStarBonus = this.ThreeStarBonus,
                FourStarBonus = this.FourStarBonus,
            });

            CustomerList.Add(new Customer()
            {
                LotteryType = LotteryType.Taiwan539,
                Name = "誠",
                TwoStar = 80,
                ThreeStar = 80,
                FourStar = 80,
                CarSet = 80,
                TwoStarBonus = this.TwoStarBonus,
                ThreeStarBonus = this.ThreeStarBonus,
                FourStarBonus = this.FourStarBonus,
            });

            CustomerList.Add(new Customer()
            {
                LotteryType = LotteryType.Taiwan539,
                Name = "阿順",
                TwoStar = 80,
                ThreeStar = 80,
                FourStar = 80,
                CarSet = 80,
                TwoStarBonus = this.TwoStarBonus,
                ThreeStarBonus = this.ThreeStarBonus,
                FourStarBonus = this.FourStarBonus,
            });

            CustomerList.Add(new Customer()
            {
                LotteryType = LotteryType.Taiwan539,
                Name = "可可",
                TwoStar = 80,
                ThreeStar = 80,
                FourStar = 80,
                CarSet = 80,
                TwoStarBonus = this.TwoStarBonus,
                ThreeStarBonus = this.ThreeStarBonus,
                FourStarBonus = this.FourStarBonus,
            });

            CustomerList.Add(new Customer()
            {
                LotteryType = LotteryType.Taiwan539,
                Name = "阿芬",
                TwoStar = 78,
                ThreeStar = 78,
                FourStar = 78,
                CarSet = 78,
                TwoStarBonus = this.TwoStarBonus,
                ThreeStarBonus = this.ThreeStarBonus,
                FourStarBonus = this.FourStarBonus,
            });

        }
    }
}

