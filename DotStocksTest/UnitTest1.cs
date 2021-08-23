using DotStocks.Controllers;
using NUnit.Framework;

namespace NUnitTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            StocksController controller = new StocksController();
            
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}