using System;
using System.Net.Http;
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
        private const String alphaVantageKey = "EJSFNVGS7Q00C4N6";

        private String GetAlphaVantageTemplate(String symbol) =>
            $"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY_ADJUSTED&symbol={symbol}&apikey={alphaVantageKey}";

        [HttpGet]
        public async Task<JObject> GetQuotesAsync(String symbol)
        {
            JObject result = null;
            var uri = GetAlphaVantageTemplate(("IBM"));
            await new HttpClient().GetAsync(uri).ContinueWith(responseTask =>
            {
                var response = responseTask.Result;
                return response.Content.ReadAsStringAsync().ContinueWith(jsonTask =>
                {
                    var json = jsonTask.Result;
                    result = JsonConvert.DeserializeObject<JObject>(json);
                });
            }).Unwrap();
            return result;
        }
    }
}