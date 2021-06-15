using NUnit.Framework;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace csharp_example
{
    public class AdminPanelCountryTests : AdminPanelTestBase
    {
        [OneTimeSetUp]
        public void SetUp()
        {
            Login();
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

            foreach (string link in linksWithZone)
            {
                driver.Url = link;

                List<string> zones = new List<string>();

                IWebElement table = driver.FindElement(By.CssSelector("#table-zones.dataTable"));
                IList<IWebElement> zoneRows = table.FindElements(By.CssSelector("tr:not(.header)"));
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
                IList<IWebElement> zoneRows = table.FindElements(By.CssSelector("tr:not(.header)"));
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
