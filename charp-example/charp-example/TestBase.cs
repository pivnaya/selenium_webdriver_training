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
                //запуск Chrome
                ChromeOptions options = new ChromeOptions();
                options.AddArgument("--start-maximized");
                driver = new ChromeDriver(options);

                //запуск FF
                //driver = new FirefoxDriver();

                //запуск FF Nightly
                //FirefoxDriverService service = FirefoxDriverService.CreateDefaultService();
                //service.FirefoxBinaryPath = @"C:\Program Files\Firefox Nightly\firefox.exe";
                //driver = new FirefoxDriver(service);

                //запуск Edge
                //driver = new EdgeDriver();

                tlDriver.Value = driver;
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            }
            return tlDriver.Value;
        }
    }
}
