using System;
using System.Collections.Generic;
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
        private IQuoteService _quoteService;

        public StocksController(IQuoteService quoteService)
        {
            _quoteService = quoteService;
        }

        [HttpGet]
        public Task<IList<Quote>> GetQuotesAsync(String symbol)
        {
            return _quoteService.GetQuotesAsync(symbol);
        }
    }
}