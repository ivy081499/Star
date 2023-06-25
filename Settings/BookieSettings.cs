using System;
using Star.Enums;
using Star.Models;

namespace Star.Settings
{
    public class BookieSettings
    {
        public List<BookieSetting> BookieSettingList { get; set; }

        public BookieSettings()
        {
            BookieSettingList.Add(new BookieSetting()
            {
                BookieType = BookieType.小惠,
                TwoStar = 74,
                ThreeStar = 64,
                FourStar = 55,
                CarSet = 73.5,
                TwoStarBonus = 5300,
                ThreeStarBonus = 56000,
                FourStarBonus = 750000,
            });

            BookieSettingList.Add(new BookieSetting()
            {
                BookieType = BookieType.楊董,
                TwoStar = 74,
                ThreeStar = 64,
                FourStar = 55,
                CarSet = 74,
                TwoStarBonus = 5300,
                ThreeStarBonus = 56000,
                FourStarBonus = 750000,
            });
        }

    }

    public class BookieSetting
    {
        public BookieType BookieType { get; set; }
        public int TwoStar { get; set; }
        public int ThreeStar { get; set; }
        public int FourStar { get; set; }
        public double CarSet { get; set; }
        public int TwoStarBonus { get; set; }
        public int ThreeStarBonus { get; set; }
        public int FourStarBonus { get; set; }
    }
}

