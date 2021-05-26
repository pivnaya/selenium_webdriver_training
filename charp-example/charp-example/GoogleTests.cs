using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

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
            wait.Until(ExpectedConditions.TitleIs("webdriver - Поиск в Google"));
        }
    }
}