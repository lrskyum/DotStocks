using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DotStocks.Controllers
{
    public class Quote
    {
        private readonly DateTime closeDate;
        private readonly double adjusted;

        public Quote(DateTime closeDate, double adjusted)
        {
            this.closeDate = closeDate;
            this.adjusted = adjusted;
        }

        public DateTime CloseDate => closeDate;

        public double Adjusted => adjusted;
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
                var quoteDate = quote.Name;
                foreach (var value in quote.Value.OfType<JProperty>())
                {
                    // var y = value switch 
                    // {
                    //     { Name: "1. "}
                    //     Na
                    // };
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