using System;
using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Safari;
using OpenQA.Selenium.Support.UI;
using System.Linq;
using OpenQA.Selenium.Remote;

namespace csharp_example
{
    public class TestBase
    {
        public IWebDriver driver;
        public WebDriverWait wait;

        [OneTimeSetUp]
        public void Start()
        {
            driver = GetDriver();
        }

        [OneTimeTearDown]
        public void Stop()
        {
            driver?.Quit();
        }

        public IWebDriver GetDriver()
        {
            #region запуск Chrome
            ChromeOptions options = new ChromeOptions();
            options.PageLoadStrategy = PageLoadStrategy.Normal;
            options.AddArgument("--start-maximized");

            //трафик через прокси
            //Proxy proxy = new Proxy();
            //proxy.Kind = ProxyKind.Manual;
            //proxy.HttpProxy = "localhost:8866";
            //proxy.SslProxy = "localhost:8866";
            //options.Proxy = proxy;

            var driver = new ChromeDriver(options);
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

            #region удаленный запуск
            //driver = new RemoteWebDriver(new Uri("http://192.168.0.221:4444/wd/hub"), new ChromeOptions());
            //driver = new RemoteWebDriver(new Uri("http://192.168.0.221:4444/wd/hub"), new FirefoxOptions());
            //driver = new RemoteWebDriver(new Uri("http://192.168.0.221:4444/wd/hub"), new InternetExplorerOptions());
            #endregion

            #region запуск в облаке
            //var capability = new SafariOptions();
            //capability.AddAdditionalCapability("os_version", "Big Sur");
            //capability.AddAdditionalCapability("browser", "safari");
            //capability.AddAdditionalCapability("browser_version", "latest");
            //capability.AddAdditionalCapability("os", "OS X");
            //capability.AddAdditionalCapability("browserstack.user", "пользователь");
            //capability.AddAdditionalCapability("browserstack.key", "ключ");
            //driver = new RemoteWebDriver(new Uri("https://hub-cloud.browserstack.com/wd/hub/"), capability);
            #endregion

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(500);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(10);

            return driver;
        }

        public bool IsElementPresent(By by)
        {
            return driver.FindElements(by).Count > 0;
        }

        public void WaitUntilInvisible(By by)
        {
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(by));
        }

        public void WaitUntilTextPresent(IWebElement element, string text)
        {
            wait.Until(ExpectedConditions.TextToBePresentInElement(element, text));
        }

        public void WaitUntilNumberLess(By locator, int count)
        {
            wait.Until(d => d.FindElements(locator).Count < count);
        }

        public static Func<IWebDriver, string> AnyWindowOtherThan(ICollection<string> oldWindows)
        {
            return (driver) =>
            {
                return driver.WindowHandles.ToList().Except(oldWindows).FirstOrDefault();
            };
        }

        public string WaitForWindowOtherThan(ICollection<string> oldWindows)
        {
            return wait.Until(AnyWindowOtherThan(oldWindows));
        }

        public void WaitUntilVisible(By by)
        {
            wait.Until(ExpectedConditions.ElementIsVisible(by));
        }
    }
}
