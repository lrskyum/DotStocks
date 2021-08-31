using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DotStocks.Vo;

namespace DotStocks.Services
{
    public interface IQuoteService
    {
        IList<Quote> GetQuotes(String symbol);

        Task<IList<Quote>> GetQuotesAsync(String symbol);
    }
}