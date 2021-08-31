using System.Threading.Tasks;
using DotStocks.Controllers;
using DotStocks.Services;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace NUnitTests
{
    [TestFixture]
    public class Tests
    {
        private IQuoteService _service;
        private StocksController _controller;
        
        // private Mock serviceMock = Mock.Get<IQuoteService>();

        [SetUp]
        public void Setup()
        {
            // serviceMock.Setups();
            _service = new QuoteService();
            _controller = new StocksController(_service);
        }

        [Test]
        public void Test1()
        {
            Assert.NotNull(_service.GetQuotes2("IBM"));
        }

        [Test]
        public async Task Test2()
        {
            Assert.NotNull(await _controller.GetQuotesAsync("IBM"));
        }
    }
}