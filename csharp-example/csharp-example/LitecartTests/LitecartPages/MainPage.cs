using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace csharp_example
{
    public class MainPage : PageBase
    {
        public MainPage(IWebDriver driver) : base(driver)
        {
        }

        [FindsBy(How = How.ClassName, Using = "product")]
        protected IWebElement ProductBlock { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[contains(.,'Checkout')]")]
        protected IWebElement BasketButton { get; set; }

        public void Open()
        {
            driver.Url = "http://localhost/litecart/";
        }

        public void OpenProductPage()
        {
            ProductBlock.Click();
        }

        public void OpenBasket()
        {
            BasketButton.Click();
        }
    }
}
