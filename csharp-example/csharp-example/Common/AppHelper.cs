using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace csharp_example
{
    public class AppHelper
    {
        protected IWebDriver driver;
        protected WebDriverWait wait;

        public AppHelper(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        public bool IsElementPresent(By by)
        {
            return driver.FindElements(by).Count > 0;
        }

        public void WaitUntilTextPresent(IWebElement element, string text)
        {
            wait.Until(ExpectedConditions.TextToBePresentInElement(element, text));
        }

        public void WaitUntilNumberLess(By locator, int count)
        {
            wait.Until(d => d.FindElements(locator).Count < count);
        }

        public void WaitUntilInvisible(By by)
        {
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(by));
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
