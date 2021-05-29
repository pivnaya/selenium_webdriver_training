using NUnit.Framework;
using OpenQA.Selenium;

namespace csharp_example
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class GoogleTests : TestBase
    {  
        [Test]
        public void SearchTest()
        {
            driver.Url = "http://www.google.com/";
            driver.FindElement(By.Name("q")).SendKeys("webdriver" + Keys.Enter);
            Assert.AreEqual("webdriver - Поиск в Google", driver.Title);
        }
    }
}