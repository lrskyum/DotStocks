using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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

    class AlphaVantageResponseDto
    {
    }

    [ApiController]
    [Route("[controller]")]
    public class StocksController : ControllerBase
    {
        private const String alphaVantageKey = "EJSFNVGS7Q00C4N6";

        private Uri getAlphaVantageUrl(String symbol) =>
            new Uri(
                $"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY_ADJUSTED&symbol={symbol}&apikey={alphaVantageKey}");

        [HttpGet]
        public Task<String> getQuotes(String symbol)
        {
            String res = null;
            var url = getAlphaVantageUrl(symbol);
            var httClient = new HttpClient();
            Task<HttpResponseMessage> trm = httClient.GetAsync(url);
            
            trm.ContinueWith(responseTask =>
            {
                var response = responseTask.Result;

                // return response.Content.ReadAsStringAsync().ContinueWith(jsonTask =>
                // {
                //     var json = jsonTask.Result;
                //     String result = JsonConvert.DeserializeObject<String>(json);
                //     return result;
                // });
            });
            return null;
        }
    }
}