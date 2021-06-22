using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Safari;
using OpenQA.Selenium.Remote;

namespace csharp_example
{
    public class TestBase
    {
        protected IWebDriver driver;
        protected AppHelper appHelper;

        [OneTimeSetUp]
        public void Start()
        {
            driver = GetDriver("chrome");
            appHelper = new AppHelper(driver);
        }

        [OneTimeTearDown]
        public void Stop()
        {
            driver?.Quit();
        }

        public IWebDriver GetDriver(string browser)
        {
            IWebDriver driver = null;

            switch (browser)
            {
                case "chrome":
                    ChromeOptions options = new ChromeOptions();
                    options.PageLoadStrategy = PageLoadStrategy.Normal;
                    options.AddArgument("--start-maximized");
                    #region трафик через прокси
                    //Proxy proxy = new Proxy();
                    //proxy.Kind = ProxyKind.Manual;
                    //proxy.HttpProxy = "localhost:8866";
                    //proxy.SslProxy = "localhost:8866";
                    //options.Proxy = proxy;
                    #endregion
                    driver = new ChromeDriver(options);
                    break;
                case "firefox":
                    driver = new FirefoxDriver();
                    driver.Manage().Window.Maximize();
                    break;
                case "firefox nightly":
                    FirefoxDriverService service = FirefoxDriverService.CreateDefaultService();
                    service.FirefoxBinaryPath = @"C:\Program Files\Firefox Nightly\firefox.exe";
                    driver = new FirefoxDriver(service);
                    break;
                case "edge":
                    driver = new EdgeDriver();
                    break;
                case "remote":
                    driver = new RemoteWebDriver(new Uri("http://192.168.0.221:4444/wd/hub"), new ChromeOptions());
                    //driver = new RemoteWebDriver(new Uri("http://192.168.0.221:4444/wd/hub"), new FirefoxOptions());
                    //driver = new RemoteWebDriver(new Uri("http://192.168.0.221:4444/wd/hub"), new InternetExplorerOptions());
                    break;
                case "cloud":
                    var capability = new SafariOptions();
                    capability.AddAdditionalCapability("os_version", "Big Sur");
                    capability.AddAdditionalCapability("browser", "safari");
                    capability.AddAdditionalCapability("browser_version", "latest");
                    capability.AddAdditionalCapability("os", "OS X");
                    capability.AddAdditionalCapability("browserstack.user", "пользователь");
                    capability.AddAdditionalCapability("browserstack.key", "ключ");
                    driver = new RemoteWebDriver(new Uri("https://hub-cloud.browserstack.com/wd/hub/"), capability);
                    break;
            }

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(500);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(10);

            return driver;
        }
    }
}
