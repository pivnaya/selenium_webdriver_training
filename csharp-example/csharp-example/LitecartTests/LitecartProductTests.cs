using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;

namespace csharp_example
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class LitecartTests : TestBase
    {
        [SetUp]
        public void SetUp()
        {
            driver.Url = "http://localhost/litecart/";
        }

        [Test]
        public void LitecartProductStickersTest()
        {
            ICollection<IWebElement> products = driver.FindElements(By.ClassName("product"));

            foreach (IWebElement product in products)
            {
                ICollection<IWebElement> stickers = product.FindElements(By.ClassName("sticker"));
                Assert.AreEqual(1, stickers.Count);
            }
        }

        [Test]
        public void LitecartOpenProductTest()
        {
            IList<IWebElement> campaignsProducts = driver.FindElements(By.CssSelector("#box-campaigns .product"));

            IWebElement product = campaignsProducts[0];

            string nameOnMain = product.FindElement(By.ClassName("name")).Text;

            IWebElement priceOnMain = product.FindElement(By.ClassName("regular-price"));
            string valuePriceOnMain = priceOnMain.Text;
            Color colorPriceOnMain = new Color(priceOnMain.GetCssValue("color"));
            Font fontPriceOnMain = new Font(priceOnMain);

            IWebElement discountPriceOnMain = product.FindElement(By.ClassName("campaign-price"));
            string valueDiscountPriceOnMain = discountPriceOnMain.Text;
            Color colorDiscountPriceOnMain = new Color(discountPriceOnMain.GetCssValue("color"));
            Font fontDiscountPriceOnMain = new Font(discountPriceOnMain);

            Assert.Multiple(() =>
            {
                Assert.True(colorPriceOnMain.IsGray(), "Цена на главной не серая!");
                Assert.True(fontPriceOnMain.IsStrikeThrough(), "Цена на главной не зачеркнута!");
                Assert.True(colorDiscountPriceOnMain.IsRed(), "Цена со скидкой на главной не красная!");
                Assert.True(fontDiscountPriceOnMain.IsBold(), "Цена со скидкой на главной не жирная!");
                Assert.Greater(fontDiscountPriceOnMain.Size, fontPriceOnMain.Size, "Цена со скидкой на главной не больше обычной цены!");
            });

            product.Click();

            string nameOnProductPage = driver.FindElement(By.CssSelector("[itemprop=name]")).Text;

            IWebElement priceOnProductPage = driver.FindElement(By.ClassName("regular-price"));
            string valuePriceOnProductPage = priceOnProductPage.Text;
            Color colorPriceOnProductPage = new Color(priceOnProductPage.GetCssValue("color"));
            Font fontPriceOnProductPage = new Font(priceOnProductPage);

            IWebElement discountPriceOnProductPage = driver.FindElement(By.ClassName("campaign-price"));
            string valueDiscountPriceOnProductPage = discountPriceOnProductPage.Text;
            Color colorDiscountPriceOnProductPage = new Color(discountPriceOnProductPage.GetCssValue("color"));
            Font fontDiscountPriceOnProductPage = new Font(discountPriceOnProductPage);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(nameOnMain, nameOnProductPage, "Названия на главной и странице товара отличаются!");
                Assert.AreEqual(valuePriceOnMain, valuePriceOnProductPage, "Цена на главной и странице товара отличаются!");
                Assert.AreEqual(valueDiscountPriceOnMain, valueDiscountPriceOnProductPage, "Цена со скидкой на главной и странице товара отличаются!");
                Assert.True(colorPriceOnProductPage.IsGray(), "Цена на странице товара не серая!");
                Assert.True(fontPriceOnProductPage.IsStrikeThrough(), "Цена на странице товара не зачеркнута!");
                Assert.True(colorDiscountPriceOnProductPage.IsRed(), "Цена со скидкой на странице товара не красная!");
                Assert.True(fontDiscountPriceOnProductPage.IsBold(), "Цена со скидкой на странице товара не жирная!");
                Assert.Greater(fontDiscountPriceOnProductPage.Size, fontPriceOnProductPage.Size, "Цена со скидкой на странице товара не больше обычной цены!");
            });
        }

        [Test]
        public void LitecartBasketProductTest()
        {
            for(int i = 0; i < 3; i++)
            {
                driver.FindElement(By.ClassName("product")).Click();

                if (IsElementPresent(By.ClassName("options")))
                {
                    new SelectElement(driver.FindElement(By.Name("options[Size]"))).SelectByIndex(1);
                }

                int quantity = int.Parse(driver.FindElement(By.CssSelector("span.quantity")).Text);

                driver.FindElement(By.Name("add_cart_product")).Click();

                WaitUntilTextPresent(driver.FindElement(By.CssSelector("span.quantity")), (quantity + 1).ToString());

                driver.Url = "http://localhost/litecart/";
            }

            driver.FindElement(By.XPath("//a[contains(.,'Checkout')]")).Click();

            while (driver.FindElements(By.XPath("//em[contains(.,'There are no items in your cart')]")).Count == 0)
            {
                IList<IWebElement> sliderProducts = driver.FindElements(By.CssSelector("li.shortcut"));
                if (sliderProducts.Count > 0)
                {
                    IList<IWebElement> tableProducts = driver.FindElements(By.CssSelector(".dataTable td.item"));

                    sliderProducts[0].Click();
                
                    driver.FindElement(By.Name("remove_cart_item")).Click();

                    WaitUntilNumberLess(By.CssSelector(".dataTable td.item"), tableProducts.Count);
                }
                else
                {
                    driver.FindElement(By.Name("remove_cart_item")).Click();
                }
            }
        }
    }
}
