using System;
namespace Star.Helper
{
    public static class Extensions
    {
        public static string ToDateFormat(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }

        public static string ToStarFormat(this double value)
        {
            return value.ToString("N2");
        }

    }
}

