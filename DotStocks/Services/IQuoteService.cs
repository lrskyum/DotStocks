using System.Collections.Generic;
using DotStocks.Vo;
using Newtonsoft.Json.Linq;

namespace DotStocks.Services
{
    public interface IQuoteService
    {
        IList<Quote> GetQuotes(IEnumerable<JToken> quotesJson);
    }
}