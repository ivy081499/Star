using System;
using System.Text.RegularExpressions;
using Star.Models;

namespace Star.Helper
{
    public static class ResolveStringHelper
    {
        public static string SplitChar = "X";

        public static Regex ResolveRegex = new Regex(@"(\d{2})+[Xx]+(.)+");

        public static bool CheckBetContent(string value, out List<Column> columns)
        {
            columns = new List<Column>();

            if (!ResolveRegex.IsMatch(value))
            {
                return false;
            }

            value = value.Replace("x", SplitChar);

            List<string> columnStringList = value.Split(SplitChar).ToList();

            foreach (string column in columnStringList)
            {
                List<int> numbers = new List<int>();

                column.Split('.').ToList().ForEach(o =>
                {
                    numbers.Add(int.Parse(o));
                });

                columns.Add(new Column()
                {
                    Numbers = numbers,
                });
            }

            return true;

        }
    }
}

