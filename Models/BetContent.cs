using System;
namespace Star.Models
{
    public class BetContent
    {
        public BetContentType BetContentType { get; set; }
        public object ParsedContent { get; set; }
    }

    public enum BetContentType
    {
        Serial,
        Column,
        Car,
    }
}

