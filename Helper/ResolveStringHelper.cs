using System;
using System.Data.Common;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Star.Models;

namespace Star.Helper
{
    public static class ResolveStringHelper
    {
        public static string SplitChar = "X";

        public static bool CheckBetContent(string value, out BetContent betContent)
        {
            betContent = new BetContent();

            //車組
            if (value.Length == 2)
            {
                if (int.TryParse(value, out int number))
                {
                    betContent.BetContentType = BetContentType.Car;
                    betContent.ParsedContent = value;
                    return true;
                }
            }
            //連碰
            else if (CheckSerialNumberFormat(value))
            {
                List<List<string>> parsedContent = new List<List<string>>();

                List<string> numbers = ParseToNumbers(value);
                numbers.ForEach(o => parsedContent.Add(new List<string>() { o }));

                betContent.BetContentType = BetContentType.Serial;
                betContent.ParsedContent = value;

                return true;
            }
            //排碰
            else if (value.IndexOf("x") > -1 || value.IndexOf(SplitChar) > -1 || value.IndexOf("*") > -1)
            {
                value = value.Replace("x", SplitChar).Replace("*", SplitChar);

                var columns = value.Split(SplitChar);
                List<List<string>> parsedContent = new List<List<string>>();

                foreach (string column in columns)
                {
                    if (!CheckSerialNumberFormat(column))
                    {
                        return false;
                    }

                    var numbers = ParseToNumbers(column);

                    //每排內的多個數字
                    parsedContent.Add(numbers);

                }

                betContent.BetContentType = BetContentType.Column;
                betContent.ParsedContent = parsedContent;

            }

            return false;
        }

        private static bool CheckSerialNumberFormat(string value)
        {
            if (int.TryParse(value, out int number) && value.Length % 2 == 0)
                return true;
            return false;
        }

        private static List<string> ParseToNumbers(string value)
        {
            List<string> numbers = new List<string>();

            int count = value.Length / 2;

            for (int i = 0; i < count; i++)
            {
                string number = value.Substring(0, 2);
                numbers.Add(number);

                value = value.Remove(0, 2);
            }

            return numbers;
        }
    }
}

