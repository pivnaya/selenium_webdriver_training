using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace csharp_example
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class AdminPanelTests : AdminPanelTestBase
    {
        [Test]
        public void AdminPanelMenuTest()
        {
            Login();

            int i = 1;

            while (true)
            {
                try
                {
                    IWebElement app = driver.FindElement(By.CssSelector($"li#app-:nth-child({i})"));
                    app.Click();
                    Assert.IsTrue(IsElementPresent(By.TagName("h1")));

                    int j = 2;

                    while (true)
                    {
                        try
                        {
                            IWebElement doc = driver.FindElement(By.CssSelector($"li[id^=doc-]:nth-child({j})"));
                            doc.Click();
                            Assert.IsTrue(IsElementPresent(By.TagName("h1")));
                        }
                        catch (NoSuchElementException)
                        {
                            break;
                        }

                        j++;
                    }

                }
                catch (NoSuchElementException)
                {
                    break;
                }

                i++;
            }
        }
    }
}
