using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace csharp_example
{
    public class ProductPage : PageBase
    {
        public ProductPage(IWebDriver driver) : base(driver)
        {
        }

        [FindsBy(How = How.Name, Using = "add_cart_product")]
        protected IWebElement AddToBasketButton { get; set; }

        [FindsBy(How = How.Name, Using = "options[Size]")]
        protected IWebElement ProductOptions { get; set; }

        [FindsBy(How = How.CssSelector, Using = "span.quantity")]
        protected IWebElement BasketQuantity { get; set; }

        public bool IsProductWithOptions()
        {
            return appHelper.IsElementPresent(By.ClassName("options"));
        }

        public void SelectSmallProductSize()
        {
            new SelectElement(ProductOptions).SelectByText("Small");
        }

        public void AddProductToBasketWithWait()
        {
            int quantity = int.Parse(BasketQuantity.Text);
            AddToBasketButton.Click();
            appHelper.WaitUntilTextPresent(BasketQuantity, (quantity + 1).ToString());
        }
    }
}
