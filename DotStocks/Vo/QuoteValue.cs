using System;

namespace DotStocks.Vo
{
    public class QuoteValue
    {
        public QuoteValue(double quote)
        {
            if (quote < 0)
            {
                throw new ArgumentException($"Value must not be negative: {quote}");
            }
            Value = quote;
        }

        public double Value { get; }
    }
}