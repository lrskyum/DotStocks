using System.Threading;
using DotStocks.Controllers;
using Microsoft.AspNetCore.Mvc;
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
        public void Test1()
        {
            _controller.getQuotes("IBM");
            Thread.Sleep(200000);
            Assert.Pass();
        }
    }
}