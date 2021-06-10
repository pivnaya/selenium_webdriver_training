using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;

namespace csharp_example
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class AdminPanelTests : AdminPanelTestBase
    {
        [OneTimeSetUp]
        public void SetUp()
        {
            Login();
        }

        [Test]
        public void AdminPanelMenuTest()
        {
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

        [Test]
        public void AdminPanelCountriesTest()
        {
            driver.Url = "http://localhost/litecart/admin/?app=countries&doc=countries";

            List<string> countries = new List<string>();
            List<string> linksWithZone = new List<string>();

            IList<IWebElement> countryRows = driver.FindElements(By.CssSelector("tr.row"));
            foreach (IWebElement row in countryRows)
            {
                IList<IWebElement> cells = row.FindElements(By.TagName("td"));
                countries.Add(cells[4].Text);

                if (!cells[5].Text.Equals("0"))
                {
                    string href = cells[4].FindElement(By.TagName("a")).GetAttribute("href");
                    linksWithZone.Add(href);
                }
            }

            List<string> sortedContries = new List<string>(countries);
            sortedContries.Sort();

            Assert.AreEqual(countries, sortedContries);

            foreach(string link in linksWithZone)
            {
                driver.Url = link;

                List<string> zones = new List<string>();

                IWebElement table = driver.FindElement(By.CssSelector("#table-zones.dataTable"));
                IList<IWebElement>  zoneRows = table.FindElements(By.CssSelector("tr:not(.header)"));
                foreach (IWebElement row in zoneRows)
                {
                    IList<IWebElement> cells = row.FindElements(By.TagName("td"));

                    string name = cells[2].Text;

                    if (name != string.Empty)
                    {
                        zones.Add(name);
                    } 
                }

                List<string> sortedZones = new List<string>(zones);
                sortedZones.Sort();

                Assert.AreEqual(zones, sortedZones);
            }
        }

        [Test]
        public void AdminPanelGeoZonesTest()
        {
            driver.Url = "http://localhost/litecart/admin/?app=geo_zones&doc=geo_zones";


            List<string> links = new List<string>();

            IList<IWebElement> countryRows = driver.FindElements(By.CssSelector("tr.row"));
            foreach (IWebElement row in countryRows)
            {
                IList<IWebElement> cells = row.FindElements(By.TagName("td"));
                string href = cells[2].FindElement(By.TagName("a")).GetAttribute("href");
                links.Add(href);
            }

            foreach (string link in links)
            {
                driver.Url = link;

                List<string> zones = new List<string>();

                IWebElement table = driver.FindElement(By.CssSelector("#table-zones.dataTable"));
                IList<IWebElement>  zoneRows = table.FindElements(By.CssSelector("tr:not(.header)"));
                foreach (IWebElement row in zoneRows)
                {
                    IList<IWebElement> cells = row.FindElements(By.TagName("td"));

                    if (cells.Count > 1)
                    {
                        string name = cells[2].FindElement(By.CssSelector("option[selected=selected]")).Text;
                        zones.Add(name);
                    }
                }

                List<string> sortedZones = new List<string>(zones);
                sortedZones.Sort();

                Assert.AreEqual(zones, sortedZones);
            }
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

            new SelectElement(driver.FindElement(By.Name("manufacturer_id"))).SelectByIndex(1);
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
        public void AdminPanelOpenCountryTest()
        {
            driver.Url = "http://localhost/litecart/admin/?app=countries&doc=countries";

            driver.FindElement(By.XPath("//a[contains(.,'Add New Country')]")).Click();

            IList<IWebElement> links = driver.FindElements(By.CssSelector("td>a[target=_blank]"));

            foreach (IWebElement link in links)
            {
                string originalWindow = driver.CurrentWindowHandle;
                ICollection<string> oldWindows = driver.WindowHandles;

                link.Click();

                string newWindow = WaitForWindowOtherThan(oldWindows);

                driver.SwitchTo().Window(newWindow);
                driver.Close();

                driver.SwitchTo().Window(originalWindow);
            }
        }
    }
}
