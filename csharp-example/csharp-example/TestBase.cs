using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace csharp_example
{
    public class TestBase
    {
        public static ThreadLocal<IWebDriver> tlDriver = new ThreadLocal<IWebDriver>();
        public IWebDriver driver;
        public WebDriverWait wait;

        [SetUp]
        public void Start()
        {
            GetDriver();
        }

        [TearDown]
        public void Stop()
        {
            driver.Quit();
            driver = null;
        }

        public IWebDriver GetDriver()
        {
            if (!tlDriver.IsValueCreated)
            {
                #region запуск Chrome
                ChromeOptions options = new ChromeOptions();
                options.PageLoadStrategy = PageLoadStrategy.Normal;
                options.AddArgument("--start-maximized");
                driver = new ChromeDriver(options);
                #endregion

                #region запуск FF
                //driver = new FirefoxDriver();
                //driver.Manage().Window.Maximize();
                #endregion

                #region запуск FF Nightly
                //FirefoxDriverService service = FirefoxDriverService.CreateDefaultService();
                //service.FirefoxBinaryPath = @"C:\Program Files\Firefox Nightly\firefox.exe";
                //driver = new FirefoxDriver(service);
                #endregion

                #region запуск Edge
                //driver = new EdgeDriver();
                #endregion

                tlDriver.Value = driver;
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(500);
                driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(10);
            }
            return tlDriver.Value;
        }

        public bool IsElementPresent(By by)
        {
            return driver.FindElements(by).Count > 0;
        }

        public void WaitUntilInvisible(By by)
        {
            new WebDriverWait(driver, TimeSpan.FromSeconds(5)).Until(ExpectedConditions.InvisibilityOfElementLocated(by));
        }
    }
}
