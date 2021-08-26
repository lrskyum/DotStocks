using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DotStocks.Controllers
{
    public interface IQuote
    {
        PastTime CloseDate { get; }
        QuoteValue Adjusted { get; }
    }

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

    [ApiController]
    [Route("[controller]")]
    public class StocksController : ControllerBase
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

        private IList<Quote> GetQuotes(IEnumerable<JToken> quotesJson)
        {
            foreach (var quote in quotesJson.OfType<JProperty>())
            {
                var quoteDate = DateTime.Parse(quote.Name);
                Quote? value = null; 
                foreach (var property in (quote.Value as JObject)?.Properties()!)
                {
                    // switch (property)
                    // {
                    //     case "1. open" => value = new Quote(quoteDate, property.Value);
                    // }
                }
            }

            return new List<Quote>();
        }

        [HttpGet]
        public Task<JToken> GetQuotesAsync(String symbol)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.Error += OnError;

            var uri = GetAlphaVantageUri(("IBM"));
            var jobj = _client.GetAsync(uri)
                .ContinueWith(responseTask =>
                {
                    var response = responseTask.Result;
                    return response.Content.ReadAsStringAsync().ContinueWith(jsonTask =>
                    {
                        var json = jsonTask.Result;
                        return JsonConvert.DeserializeObject<JObject>(json);
                    });
                })
                .Unwrap()
                .ContinueWith(deserializeTask =>
                {
                    var serviceResponse = deserializeTask.Result;
                    var series = serviceResponse["Time Series (Daily)"];
                    var x = GetQuotes(series.Children<JToken>());
                    return series;
                });
            return jobj;
        }
    }
}