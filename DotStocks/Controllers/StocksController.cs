using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DotStocks.Services;
using DotStocks.Vo;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DotStocks.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StocksController : ControllerBase
    {
        private const String AlphaVantageKey = "EJSFNVGS7Q00C4N6";
        private readonly HttpClient _client = new HttpClient();

        private IQuoteService _quoteService;

        private String GetAlphaVantageUri(String symbol) =>
            $"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY_ADJUSTED&symbol={symbol}&apikey={AlphaVantageKey}";


        public StocksController(IQuoteService quoteService)
        {
            _quoteService = quoteService;
        }

        protected virtual void OnError(object? sender, EventArgs errorEventArgs)
        {
            Console.WriteLine("ERROR Args: " + errorEventArgs);
            Environment.Exit(1);
        }

        [HttpGet]
        public Task<IList<Quote>> GetQuotesAsync(String symbol)
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
                    var list = _quoteService.GetQuotes(series.Children<JToken>());
                    return list;
                });
            return jobj;
        }
    }
}