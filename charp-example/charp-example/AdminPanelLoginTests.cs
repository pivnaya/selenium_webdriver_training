using NUnit.Framework;
using OpenQA.Selenium;

namespace csharp_example
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class AdminPanelLoginTests : TestBase
    {
        [Test]
        public void AdminPanelLoginTest()
        {
            driver.Url = "http://localhost/litecart/admin/";
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Click();
        }
    }
}
