using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;

namespace csharp_example
{
    public class AdminPanelProductTests : AdminPanelTestBase
    {
        [OneTimeSetUp]
        public void SetUp()
        {
            Login();
        }

        [Test]
        public void AdminPanelAddProductTest()
        {
            driver.Url = "http://localhost/litecart/admin/?app=catalog&doc=catalog";

            driver.FindElement(By.XPath("//a[contains(.,'Add New Product')]")).Click();

            WaitUntilVisible(By.CssSelector("li.active"));
            By productNameLocator = By.Name("name[en]");
            WaitUntilVisible(productNameLocator);

            Random rnd = new Random();
            int rndValue = rnd.Next(0, 100);
            string productName = $"TestName{rndValue}";

            driver.FindElement(productNameLocator).SendKeys(productName);
            driver.FindElement(By.Name("code")).SendKeys("123");
            driver.FindElement(By.CssSelector("[name^= product_groups]")).Click();
            IWebElement quantity = driver.FindElement(By.Name("quantity"));
            quantity.Clear();
            quantity.SendKeys("10");

            string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testpic.jpg");
            driver.FindElement(By.Name("new_images[]")).SendKeys(file);

            string dateFrom = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            string dateTo = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            (driver as IJavaScriptExecutor).ExecuteScript($"arguments[0].value = '{dateFrom}'",
                driver.FindElement(By.Name("date_valid_from")));
            (driver as IJavaScriptExecutor).ExecuteScript($"arguments[0].value = '{dateTo}'",
                driver.FindElement(By.Name("date_valid_to")));

            driver.FindElement(By.XPath("//a[contains(.,'Information')]")).Click();

            new SelectElement(driver.FindElement(By.Name("manufacturer_id"))).SelectByText("ACME Corp.");
            driver.FindElement(By.Name("keywords")).SendKeys("test");
            driver.FindElement(By.Name("short_description[en]")).SendKeys("Short test description");
            driver.FindElement(By.ClassName("trumbowyg-editor")).SendKeys("Very long test description");
            driver.FindElement(By.Name("head_title[en]")).SendKeys("Test title");
            driver.FindElement(By.Name("meta_description[en]")).SendKeys("Test metadata");

            driver.FindElement(By.XPath("//a[contains(.,'Prices')]")).Click();

            IWebElement price = driver.FindElement(By.Name("purchase_price"));
            price.Clear();
            price.SendKeys("100");
            new SelectElement(driver.FindElement(By.Name("purchase_price_currency_code"))).SelectByValue("USD");
            driver.FindElement(By.Name("prices[USD]")).SendKeys("50");
            driver.FindElement(By.Name("prices[EUR]")).SendKeys("10");

            driver.FindElement(By.Name("save")).Click();

            Assert.IsTrue(IsElementPresent(By.XPath($"//a[contains(.,'{productName}')]")), "Товара нет на странице!");
        }

        [Test]
        public void AdminPanelProductPageLogTest()
        {
            driver.Url = "http://localhost/litecart/admin/?app=catalog&doc=catalog&category_id=1";

            List<string> links = new List<string>();

            IList<IWebElement> productLinks = driver.FindElements(By.XPath("//tr[@class = 'row']//a[contains(@href, 'product_id') and @title='Edit']"));

            foreach (IWebElement productLink in productLinks)
            {
                links.Add(productLink.GetAttribute("href"));
            }

            foreach (string link in links)
            {
                driver.Url = link;
                Assert.IsEmpty(driver.Manage().Logs.GetLog("browser"));
            }
        }
    }
}
