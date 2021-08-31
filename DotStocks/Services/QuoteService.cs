using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DotStocks.Vo;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DotStocks.Services
{
    public class QuoteService : IQuoteService
    {
        private const String AlphaVantageKey = "EJSFNVGS7Q00C4N6";
        private readonly HttpClient _client = new HttpClient();

        private String GetAlphaVantageUri(String symbol) =>
            $"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY_ADJUSTED&symbol={symbol}&apikey={AlphaVantageKey}";

        protected virtual void OnError(object? sender, EventArgs errorEventArgs)
        {
            Console.WriteLine("ERROR Args: " + errorEventArgs);
            Environment.Exit(1);
        }

        public IList<Quote> GetQuotes(String symbol)
        {
            return GetQuotesAsync(symbol).GetAwaiter().GetResult();
        }

        public Task<IList<Quote>> GetQuotesAsync(String symbol)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.Error += OnError;

            var uri = GetAlphaVantageUri(symbol);
            var jobj = _client.GetAsync(uri)
                .ContinueWith(responseTask =>
                {
                    var response = responseTask.Result;
                    return response.Content.ReadAsStringAsync();
                })
                .Unwrap()
                .ContinueWith(jsonTask =>
                {
                    var json = jsonTask.Result;
                    return JsonConvert.DeserializeObject<JObject>(json);
                })
                .ContinueWith(deserializeTask =>
                {
                    var serviceResponse = deserializeTask.Result;
                    var series = serviceResponse["Time Series (Daily)"];
                    return GetQuotes(series.Children<JToken>());
                });
            return jobj;
        }

        private IList<Quote> GetQuotes(IEnumerable<JToken> quotesJson)
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