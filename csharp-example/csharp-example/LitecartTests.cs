using NUnit.Framework;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace csharp_example
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class LitecartTests : TestBase
    {
        [Test]
        public void LitecartProductStickersTest()
        {
            driver.Url = "http://localhost/litecart/";

            ICollection<IWebElement> products = driver.FindElements(By.ClassName("product"));

            foreach (IWebElement product in products)
            {
                ICollection<IWebElement> stickers = product.FindElements(By.ClassName("sticker"));
                Assert.AreEqual(1, stickers.Count);
            }
        }
    }
}
