using System;
using Star.Enums;
using Star.Models;
using Star.ResponseModel;
using Star.ViewModels;

namespace Star.Helper
{
    public static class CountingHelper
    {
        public static TimeSpan Range = TimeSpan.FromDays(30);

        public static void Calculate(CustomerBet customerBet)
        {

            int carSetBase = customerBet.Customer.LotteryType switch
            {
                Enums.LotteryType.Taiwan539 => 38,
                Enums.LotteryType.TaiwanLottery => 48,
                Enums.LotteryType.HK => 48,
                _ => 0,
            };

            float totalTwoStar = 0;
            float totalThreeStar = 0;
            float totalFourStar = 0;
            float totalCarSet = 0;

            customerBet.BetInfoList.ForEach((betInfo) =>
            {
                totalTwoStar += GenerateCombinations(betInfo.ColumnList, 2) * betInfo.TwoStarOdds;
                totalThreeStar += GenerateCombinations(betInfo.ColumnList, 3) * betInfo.ThreeStarOdds;
                totalFourStar += GenerateCombinations(betInfo.ColumnList, 4) * betInfo.FourStarOdds;

            });

            customerBet.CarSetInfoList.ForEach((carSet) =>
            {
                totalCarSet += (carSet.Odds * carSetBase);
            });

            int TotalBetDollars = Convert.ToInt32(
                totalTwoStar * customerBet.Customer.TwoStar +
                totalThreeStar * customerBet.Customer.ThreeStar +
                totalFourStar * customerBet.Customer.FourStar +
                totalCarSet * customerBet.Customer.CarSet);

            int toalBonusDollars = Convert.ToInt32(
                customerBet.BetStatistics.TwoStarBonus * customerBet.Customer.TwoStarBonus +
                customerBet.BetStatistics.ThreeStarBonus * customerBet.Customer.ThreeStarBonus +
                customerBet.BetStatistics.FourStarBonus * customerBet.Customer.FourStarBonus);

            customerBet.BetStatistics.TotalBetDollars = TotalBetDollars;
            customerBet.BetStatistics.TotalBonusDollars = toalBonusDollars;
            customerBet.BetStatistics.WinLoseDollars = toalBonusDollars - TotalBetDollars;
        }

        public static BetStatistics GetBetStatistics(List<BetInfo> betInfos, List<CarSetInfo> carSetInfos, Customer customer)
        {
            int carSetBase = customer.LotteryType switch
            {
                Enums.LotteryType.Taiwan539 => 38,
                Enums.LotteryType.TaiwanLottery => 48,
                Enums.LotteryType.HK => 48,
                _ => 0,
            };

            float totalTwoStar = 0;
            float totalThreeStar = 0;
            float totalFourStar = 0;
            float totalCarSet = 0;

            betInfos.ForEach((betInfo) =>
            {
                totalTwoStar += GenerateCombinations(betInfo.ColumnList, 2) * betInfo.TwoStarOdds;
                totalThreeStar += GenerateCombinations(betInfo.ColumnList, 3) * betInfo.ThreeStarOdds;
                totalFourStar += GenerateCombinations(betInfo.ColumnList, 4) * betInfo.FourStarOdds;

            });

            carSetInfos.ForEach((carSet) =>
            {
                totalCarSet += (carSet.Odds * carSetBase);
            });

            int TotalBetDollars = Convert.ToInt32(
                totalTwoStar * customer.TwoStar +
                totalThreeStar * customer.ThreeStar +
                totalFourStar * customer.FourStar +
                totalCarSet * customer.CarSet);

            BetStatistics result = new BetStatistics()
            {
                TotalTwoStar = totalTwoStar.ToString("n2"),
                TotalThreeStar = totalThreeStar.ToString("n2"),
                TotalFourStar = totalFourStar.ToString("n2"),
                TotalCarSet = totalCarSet.ToString("n2"),
                TotalBetDollars = TotalBetDollars,
                WinLoseDollars = TotalBetDollars,
                FourStarBonus = 0,
                TwoStarBonus = 0,
                ThreeStarBonus = 0,
                TotalBonusDollars = 0,
            };


            return result;
        }

        public static int GenerateCombinations(List<Column> columns, int stars)
        {
            List<List<int>> combinations = new List<List<int>>();
            HashSet<int[]> generatedCombinations = new HashSet<int[]>(new ArrayEqualityComparer<int>());

            GenerateCombinationsHelper(columns, stars, new List<int>(), 0, combinations, generatedCombinations);

            return combinations.Count;
        }

        static void GenerateCombinationsHelper(List<Column> columns, int count, List<int> currentCombination, int currentColumnIndex, List<List<int>> combinations, HashSet<int[]> generatedCombinations)
        {
            if (currentCombination.Count == count)
            {
                combinations.Add(new List<int>(currentCombination));
                return;
            }

            if (currentColumnIndex >= columns.Count)
            {
                return;
            }

            Column currentColumn = columns[currentColumnIndex];

            foreach (int num in currentColumn.Numbers)
            {
                List<int> updatedCombination = new List<int>(currentCombination);
                updatedCombination.Add(num);

                int[] combinationArray = updatedCombination.ToArray();
                if (!generatedCombinations.Contains(combinationArray))
                {
                    generatedCombinations.Add(combinationArray);
                    GenerateCombinationsHelper(columns, count, updatedCombination, currentColumnIndex + 1, combinations, generatedCombinations);
                }
            }

            GenerateCombinationsHelper(columns, count, currentCombination, currentColumnIndex + 1, combinations, generatedCombinations);
        }
    }

    class ArrayEqualityComparer<T> : IEqualityComparer<T[]>
    {
        public bool Equals(T[] x, T[] y)
        {
            if (ReferenceEquals(x, y))
                return true;
            if (x == null || y == null)
                return false;
            if (x.Length != y.Length)
                return false;

            for (int i = 0; i < x.Length; i++)
            {
                if (!EqualityComparer<T>.Default.Equals(x[i], y[i]))
                    return false;
            }

            return true;
        }

        public int GetHashCode(T[] obj)
        {
            unchecked
            {
                int hash = 17;
                foreach (T item in obj)
                {
                    hash = hash * 23 + (item?.GetHashCode() ?? 0);
                }
                return hash;
            }
        }

    }

}

