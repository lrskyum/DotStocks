using System.Threading.Tasks;
using DotStocks.Controllers;
using NUnit.Framework;

namespace NUnitTests
{
    public class Tests
    {
        private StocksController _controller;

        [SetUp]
        public void Setup()
        {
            _controller = new StocksController();
        }

        [Test]
        public async Task Test1()
        {
            await _controller.GetQuotesAsync("IBM");
            Assert.Pass();
        }
    }
}