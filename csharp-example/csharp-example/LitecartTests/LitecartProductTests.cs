using NUnit.Framework;

namespace csharp_example
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class LitecartProductTests : TestBase
    {
        protected MainPage mainPage;
        protected ProductPage productPage;
        protected BasketPage basketPage;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            mainPage = new MainPage(driver);
            productPage = new ProductPage(driver);
            basketPage = new BasketPage(driver);
        }

        [Test]
        public void LitecartBasketProductTest()
        {
            mainPage.Open();

            for (int i = 0; i < 3; i++)
            {
                mainPage.OpenProductPage();

                if (productPage.IsProductWithOptions())
                {
                    productPage.SelectSmallProductSize();
                }

                productPage.AddProductToBasketWithWait();

                mainPage.Open();
            }

            mainPage.OpenBasket();

            while (basketPage.IsEmptyBasketMessageAbsent())
            {
                if (basketPage.IsSliderProductsPresent())
                {
                    basketPage.RemoveProductWithWait(0); 
                }
                else
                {
                    basketPage.ClickRemoveProduct();
                }
            }
        }
    }
}
