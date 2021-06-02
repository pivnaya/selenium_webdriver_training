using NUnit.Framework;
using OpenQA.Selenium;
using System.Collections.Generic;

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
    }
}
