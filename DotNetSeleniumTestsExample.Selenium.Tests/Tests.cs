using System.Threading;
using DotNetCypressTestsExample.Selenium.Tests.Setup;
using NUnit.Framework;

namespace DotNetCypressTestsExample.Selenium.Tests
{
    [TestFixture]
    public class Tests : UITest
    {
        [Test]
        public void Test1()
        {
            _chromeDriver.Navigate().GoToUrl("localhost:5000");
            Thread.Sleep(5000);
        }
    }
    
}