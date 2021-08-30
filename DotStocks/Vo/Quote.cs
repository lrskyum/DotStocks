using System;

namespace DotStocks.Vo
{
    public class Quote : IQuote
    {
        public PastTime CloseDate { get; }
        public QuoteValue Adjusted { get; }

        public Quote(Quote quote)
        {
            CloseDate = quote.CloseDate;
            Adjusted = quote.Adjusted;
        }

        public Quote(DateTime closeDate, double adjusted)
            : this(new PastTime(closeDate), new QuoteValue(adjusted))
        {
        }

        public Quote(PastTime closeDate, QuoteValue adjusted)
        {
            CloseDate = closeDate;
            Adjusted = adjusted;
        }

        public Quote WithCloseDate(DateTime value)
        {
            return new Quote(value, Adjusted.Value);
        }

        public Quote WithAdjusted(double value)
        {
            return new Quote(CloseDate.QuoteTime, value);
        }
    }
}