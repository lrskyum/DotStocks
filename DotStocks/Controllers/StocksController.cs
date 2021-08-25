using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ErrorEventArgs = Newtonsoft.Json.Serialization.ErrorEventArgs;

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

        [HttpGet]
        public Task<dynamic> GetQuotesAsync(String symbol)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.Error += OnError;

            var uri = GetAlphaVantageUri(("IBM"));
            return _client.GetAsync(uri).ContinueWith(responseTask =>
            {
                var response = responseTask.Result;
                return response.Content.ReadAsStringAsync().ContinueWith(jsonTask =>
                {
                    var json = jsonTask.Result;
                    var str = JsonConvert.DeserializeObject<dynamic>(json, new JsonSerializerSettings
                    {
                        MaxDepth = Int32.MaxValue
                    });
                    return str;
                    // return JsonConvert.DeserializeObject<JObject>(json);
                });
            }).Unwrap();
        }
    }
}