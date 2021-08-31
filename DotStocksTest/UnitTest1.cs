using System.Collections.Generic;
using System.Threading.Tasks;
using DotStocks.Controllers;
using DotStocks.Services;
using DotStocks.Vo;
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
        private Mock<IQuoteService> _serviceMock = new();

        [SetUp]
        public void Setup()
        {
            _service = new QuoteService();
            _controller = new StocksController(_service);
        }

        [Test]
        public async Task ShouldTestAsync()
        {
            // given
            _serviceMock.Setup(m => m.GetQuotes("IBM")).Returns(new List<Quote>());
            _service = _serviceMock.Object;
            
            // when
            
            // then
            Assert.NotNull(await _controller.GetQuotesAsync("IBM"));
        }
        
        [Test]
        public void ShouldTest()
        {
            // given
            _serviceMock.Setup(m => m.GetQuotes("IBM")).Returns(new List<Quote>());
            
            // when
            
            // then
            Assert.NotNull(_service.GetQuotes("IBM"));
        }
    }
}