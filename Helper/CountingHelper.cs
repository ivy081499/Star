using System;
using Star.Enums;
using Star.Models;
using Star.ResponseModel;
using Star.Settings;
using Star.ViewModels;

namespace Star.Helper
{
    public static class CountingHelper
    {
        public static TimeSpan Range = TimeSpan.FromDays(30);

        public static void ReCalulateReport(Report report, CostDefinition costDefinition)
        {
            int TotalBonusMoney = Convert.ToInt32(
                report.TwoStarBonus * costDefinition.TwoStarBonus +
                report.ThreeStarBonus * costDefinition.ThreeStarBonus +
                report.FourStarBonus * costDefinition.FourStarBonus);

            report.TotalBonusMoney = TotalBonusMoney;
            report.WinLoseMoney = report.TotalBonusMoney - report.TotalBetMoney;
        }

        public static Report GetDailyReport(IEnumerable<BetInfo> betInfos, IEnumerable<CarSetInfo> carSetInfos, CostDefinition costDefinition)
        {
            LotteryType lotteryType = LotteryType.Taiwan539;

            int carSetBase = lotteryType switch
            {
                Enums.LotteryType.Taiwan539 => 38,
                Enums.LotteryType.TaiwanLottery => 48,
                Enums.LotteryType.HK => 48,
                _ => 0,
            };

            double totalTwoStar = 0;
            double totalThreeStar = 0;
            double totalFourStar = 0;
            double totalCarSet = 0;

            betInfos.ToList().ForEach((betInfo) =>
            {
                totalTwoStar += GenerateCombinations(betInfo.ColumnList, 2) * betInfo.TwoStarOdds;
                totalThreeStar += GenerateCombinations(betInfo.ColumnList, 3) * betInfo.ThreeStarOdds;
                totalFourStar += GenerateCombinations(betInfo.ColumnList, 4) * betInfo.FourStarOdds;

            });

            carSetInfos.ToList().ForEach((carSet) =>
            {
                totalCarSet += (carSet.Odds * carSetBase);
            });

            int TotalBetDollars = Convert.ToInt32(
                totalTwoStar * costDefinition.TwoStarPrice +
                totalThreeStar * costDefinition.ThreeStarPrice +
                totalFourStar * costDefinition.FourStarPrice +
                totalCarSet * costDefinition.CarSetPrice);

            Report result = new Report()
            {
                TotalTwoStar = totalTwoStar.ToString("n2"),
                TotalThreeStar = totalThreeStar.ToString("n2"),
                TotalFourStar = totalFourStar.ToString("n2"),
                TotalCarSet = totalCarSet.ToString("n2"),
                TotalBetMoney = TotalBetDollars,
                WinLoseMoney = 0 - TotalBetDollars,
                FourStarBonus = 0,
                TwoStarBonus = 0,
                ThreeStarBonus = 0,
                TotalBonusMoney = 0,
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

