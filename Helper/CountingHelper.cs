using System;
using Star.Enums;
using Star.Models;
using Star.Settings;
using Star.ViewModels;

namespace Star.Helper
{
    public static class CountingHelper
    {
        public static TimeSpan Range = TimeSpan.FromDays(30);

        public static void SetReport(Report report, CostDefinition costDefinition, List<PaperBet> paperBetList)
        {
            report ??= new Report();

            double totalTwoStar = paperBetList.Sum(o => Convert.ToDouble(o.Report.TotalTwoStar));
            double totalThreeStar = paperBetList.Sum(o => Convert.ToDouble(o.Report.TotalThreeStar));
            double totalFourStar = paperBetList.Sum(o => Convert.ToDouble(o.Report.TotalFourStar));
            double totalCarSet = paperBetList.Sum(o => Convert.ToDouble(o.Report.TotalCarSet));

            report.TotalTwoStar = totalTwoStar.ToStarFormat();
            report.TotalThreeStar = totalThreeStar.ToStarFormat();
            report.TotalFourStar = totalFourStar.ToStarFormat();
            report.TotalCarSet = totalCarSet.ToStarFormat();

            int totalBetMoney = Convert.ToInt32(
                totalTwoStar * costDefinition.TwoStarPrice +
                totalThreeStar * costDefinition.ThreeStarPrice +
                totalFourStar * costDefinition.FourStarPrice +
                totalCarSet * costDefinition.CarSetPrice);

            int totalBonusMoney = Convert.ToInt32(
               report.TwoStarBonus * costDefinition.TwoStarBonus +
               report.ThreeStarBonus * costDefinition.ThreeStarBonus +
               report.FourStarBonus * costDefinition.FourStarBonus);

            report.TotalBetMoney = totalBetMoney.ToString();
            report.TotalBonusMoney = totalBonusMoney.ToString();
            report.WinLoseMoney = (totalBonusMoney - Convert.ToInt32(totalBetMoney)).ToString("N0");
        }

        public static void SetReport(List<ColumnBet> SerialBetList, List<ColumnBet> ColumnBetList, List<CarBet> CarBetList,
           Report report, CostDefinition costDefinition)
        {
            report ??= new Report();

            int carSetBase = GetCarSetBase();

            double totalTwoStar = 0;
            double totalThreeStar = 0;
            double totalFourStar = 0;
            double totalCarSet = 0;

            SerialBetList.ToList().ForEach(bet => Sum(bet, ref totalTwoStar, ref totalThreeStar, ref totalFourStar));

            ColumnBetList.ToList().ForEach(bet => Sum(bet, ref totalTwoStar, ref totalThreeStar, ref totalFourStar));

            CarBetList.ToList().ForEach((carSet) => { totalCarSet += carSet.OddsInfo.TwoStar * carSetBase; });

            int totalBetMoney = Convert.ToInt32(
                totalTwoStar * costDefinition.TwoStarPrice +
                totalThreeStar * costDefinition.ThreeStarPrice +
                totalFourStar * costDefinition.FourStarPrice +
                totalCarSet * costDefinition.CarSetPrice);

            int totalBonusMoney = Convert.ToInt32(
               report.TwoStarBonus * costDefinition.TwoStarBonus +
               report.ThreeStarBonus * costDefinition.ThreeStarBonus +
               report.FourStarBonus * costDefinition.FourStarBonus);

            report.TotalTwoStar = totalTwoStar.ToStarFormat();
            report.TotalThreeStar = totalThreeStar.ToStarFormat();
            report.TotalFourStar = totalFourStar.ToStarFormat();
            report.TotalCarSet = totalCarSet.ToStarFormat();
            report.TotalBetMoney = totalBetMoney.ToString();
            report.TotalBonusMoney = totalBonusMoney.ToString();
            report.WinLoseMoney = (totalBonusMoney - Convert.ToInt32(totalBetMoney)).ToString("N0");
        }

        public static int GenerateCombinations(List<List<string>> columns, int stars)
        {
            List<List<string>> combinations = new List<List<string>>();
            HashSet<string[]> generatedCombinations = new HashSet<string[]>(new ArrayEqualityComparer<string>());

            GenerateCombinationsHelper(columns, stars, new List<string>(), 0, combinations, generatedCombinations);

            return combinations.Count;
        }

        static void GenerateCombinationsHelper(List<List<string>> columns, int count, List<string> currentCombination, int currentColumnIndex, List<List<string>> combinations, HashSet<string[]> generatedCombinations)
        {
            if (currentCombination.Count == count)
            {
                combinations.Add(new List<string>(currentCombination));
                return;
            }

            if (currentColumnIndex >= columns.Count)
            {
                return;
            }

            List<string> currentColumn = columns[currentColumnIndex];

            foreach (string num in currentColumn)
            {
                List<string> updatedCombination = new List<string>(currentCombination);
                updatedCombination.Add(num);

                string[] combinationArray = updatedCombination.ToArray();
                if (!generatedCombinations.Contains(combinationArray))
                {
                    generatedCombinations.Add(combinationArray);
                    GenerateCombinationsHelper(columns, count, updatedCombination, currentColumnIndex + 1, combinations, generatedCombinations);
                }
            }

            GenerateCombinationsHelper(columns, count, currentCombination, currentColumnIndex + 1, combinations, generatedCombinations);
        }

        private static int GetCarSetBase()
        {
            LotteryType lotteryType = LotteryType.Taiwan539;

            return lotteryType switch
            {
                Enums.LotteryType.Taiwan539 => 38,
                Enums.LotteryType.TaiwanLottery => 48,
                Enums.LotteryType.HK => 48,
                _ => throw new Exception("LotteryType錯誤"),
            };
        }

        private static void Sum(ColumnBet bet, ref double totalTwoStar, ref double totalThreeStar, ref double totalFourStar)
        {
            totalTwoStar += GenerateCombinations(bet.Content, 2) * bet.OddsInfo.TwoStar;
            totalThreeStar += GenerateCombinations(bet.Content, 3) * bet.OddsInfo.ThreeStar;
            totalFourStar += GenerateCombinations(bet.Content, 4) * bet.OddsInfo.FourStar;
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

