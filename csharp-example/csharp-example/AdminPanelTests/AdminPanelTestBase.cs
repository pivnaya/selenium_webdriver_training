using OpenQA.Selenium;

namespace csharp_example
{
    public class AdminPanelTestBase : TestBase
    {
        public void Login()
        {
            driver.Url = "http://localhost/litecart/admin/";
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Click();

            appHelper.WaitUntilInvisible(By.Id("loader"));
        }
    }
}
