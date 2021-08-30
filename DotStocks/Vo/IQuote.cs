namespace DotStocks.Vo
{
    public interface IQuote
    {
        PastTime CloseDate { get; }
        QuoteValue Adjusted { get; }
    }
}