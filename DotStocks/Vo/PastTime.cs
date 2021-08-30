using System;

namespace DotStocks.Vo
{
    public class PastTime
    {
        public DateTime QuoteTime { get; }

        public PastTime(DateTime quoteTime)
        {
            if (QuoteTime > DateTime.Now)
            {
                throw new ArgumentException($"Date must be in the past: {quoteTime}");
            }

            QuoteTime = quoteTime;
        }
    }
}