using System;
using System.Collections.Generic;
using System.Linq;
using DotStocks.Vo;
using Newtonsoft.Json.Linq;

namespace DotStocks.Services
{
    public class QuoteService : IQuoteService
    {
        public IList<Quote> GetQuotes(IEnumerable<JToken> quotesJson)
        {
            IList<Quote> list = new List<Quote>(1000);
            foreach (var quote in quotesJson.OfType<JProperty>())
            {
                var quoteDate = DateTime.Parse(quote.Name);
                var detail = quote.Value;
                var open = detail["1. open"].ToObject<Double>();
                var high = detail["2. high"].ToObject<Double>();
                var low = detail["3. low"].ToObject<Double>();
                var close = detail["4. close"].ToObject<Double>();
                var adjusted = detail["5. adjusted close"].ToObject<Double>();
                var volume = detail["6. volume"].ToObject<Double>();
                var dividend = detail["7. dividend amount"].ToObject<Double>();
                var splint = detail["8. split coefficient"].ToObject<Double>();
                list.Add(new Quote(quoteDate, adjusted));
            }

            return list;
        }
    }
}