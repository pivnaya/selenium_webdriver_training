using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System.Collections.Generic;

namespace csharp_example
{
    public class BasketPage : PageBase
    {
        public BasketPage(IWebDriver driver) : base(driver)
        {
        }

        [FindsBy(How = How.CssSelector, Using = "li.shortcut")]
        protected IList<IWebElement> SliderProductsList { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".dataTable td.item")]
        protected IList<IWebElement> TableProductsList { get; set; }

        [FindsBy(How = How.Name, Using = "remove_cart_item")]
        protected IWebElement RemoveProductButton { get; set; }

        [FindsBy(How = How.XPath, Using = "//em[contains(.,'There are no items in your cart')]")]
        protected IList<IWebElement> EmptyBasketMessages { get; set; }

        public bool IsEmptyBasketMessageAbsent()
        {
            return EmptyBasketMessages.Count == 0;
        }

        public IList<IWebElement> GetSliderProducts()
        {
            return SliderProductsList;
        }

        public bool IsSliderProductsPresent()
        {
            return GetSliderProducts().Count > 0;
        }

        public void RemoveProductWithWait(int index)
        {
            IList<IWebElement> sliderProducts = GetSliderProducts();
            sliderProducts[index].Click();
            ClickRemoveProduct();
            appHelper.WaitUntilNumberLess(By.CssSelector(".dataTable td.item"), TableProductsList.Count);
        }

        public void ClickRemoveProduct()
        {
            RemoveProductButton.Click();
        }
    }
}
