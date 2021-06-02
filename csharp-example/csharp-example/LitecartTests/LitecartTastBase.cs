using OpenQA.Selenium;

namespace csharp_example
{
    public class LitecartTastBase : TestBase
    {
        public void Logout()
        {
            driver.Url = "http://localhost/litecart/en/logout";
        }

        public void Login(string login, string password)
        {
            driver.FindElement(By.Name("email")).SendKeys(login);
            driver.FindElement(By.Name("password")).SendKeys(password);
            driver.FindElement(By.Name("login")).Click();
        }
    }
}
