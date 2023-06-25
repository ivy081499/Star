using System;
using Star.Enums;
using Star.Models;

namespace Star.Settings
{
    public class BookieSettings
    {
        public List<BookieInfo> BookieSettingList = new List<BookieInfo>();

        public BookieSettings()
        {
            BookieSettingList.Add(new BookieInfo()
            {
                BookieType = BookieType.小惠,
                Cost539 = new CostDefinition()
                {
                    TwoStarPrice = 74,
                    ThreeStarPrice = 64,
                    FourStarPrice = 55,
                    CarSetPrice = 73.5,
                    TwoStarBonus = 5300,
                    ThreeStarBonus = 56000,
                    FourStarBonus = 750000,
                }
            });

            BookieSettingList.Add(new BookieInfo()
            {
                BookieType = BookieType.楊董,
                Cost539 = new CostDefinition()
                {
                    TwoStarPrice = 74,
                    ThreeStarPrice = 64,
                    FourStarPrice = 55,
                    CarSetPrice = 74,
                    TwoStarBonus = 5300,
                    ThreeStarBonus = 56000,
                    FourStarBonus = 750000,
                }
            });
        }

    }


}

