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
        private StocksController _controller;
        private Mock serviceMock = Mock.Get<IQuoteService>();

        [SetUp]
        public void Setup()
        {
            serviceMock.Setups()
            _controller = new StocksController(serviceMock);
        }

        [Test]
        public async Task Test1()
        {
            Assert.NotNull(await _controller.GetQuotesAsync("IBM"));
        }
    }
}