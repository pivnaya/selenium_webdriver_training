using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace csharp_example
{
    public class PageBase
    {
        protected IWebDriver driver;
        protected AppHelper appHelper;

        public PageBase(IWebDriver driver)
        {
            this.driver = driver;
            PageFactory.InitElements(driver, this);
            appHelper = new AppHelper(driver);
        }
    }
}
