using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
        
        private String alphaVantageTemplate =
            "https://www.alphavantage.co/query?function=TIME_SERIES_DAILY_ADJUSTED&symbol=${symbol}&apikey=${key}";

        [HttpGet]
        public async IEnumerable<List<Quote>> getQuotes(String symbol)
        {
            var httpClient = new HttpClient();
            await httpClient.GetAsync()
        }
    }
}