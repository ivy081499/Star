using System;
using Star.Models;

namespace Star.Helper
{
	public class CountingHelper
	{
        static int GenerateCombinations(List<Column> columns, int count)
        {
            List<List<int>> combinations = new List<List<int>>();
            HashSet<int[]> generatedCombinations = new HashSet<int[]>(new ArrayEqualityComparer<int>());

            GenerateCombinationsHelper(columns, count, new List<int>(), 0, combinations, generatedCombinations);

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

