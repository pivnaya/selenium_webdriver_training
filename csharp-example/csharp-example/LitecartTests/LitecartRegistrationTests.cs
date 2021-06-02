using NUnit.Framework;
using OpenQA.Selenium;
using System;

namespace csharp_example
{
    public class LitecartRegistrationTests : LitecartTastBase
    {
        [Test]
        public void LitecartRegistrationTest()
        {
            Random rnd = new Random();
            int rndValue = rnd.Next(0, 100);
            string email = $"testik{rndValue}@email.test";
            string password = "testikpass";

            driver.Url = "http://localhost/litecart/en/create_account";

            driver.FindElement(By.Name("firstname")).SendKeys("Testik");
            driver.FindElement(By.Name("lastname")).SendKeys("Testovich");
            driver.FindElement(By.Name("address1")).SendKeys("Testik Address");
            driver.FindElement(By.Name("postcode")).SendKeys("12345");
            driver.FindElement(By.Name("city")).SendKeys("Testik Town");
            (driver as IJavaScriptExecutor).ExecuteScript("arguments[0].value = 'US'; arguments[0].dispatchEvent(new Event('change'))",
                driver.FindElement(By.Name("country_code")));
            driver.FindElement(By.Name("email")).SendKeys(email);
            driver.FindElement(By.Name("phone")).SendKeys("123456789");
            driver.FindElement(By.Name("password")).SendKeys(password);
            driver.FindElement(By.Name("confirmed_password")).SendKeys(password);

            driver.FindElement(By.Name("create_account")).Click();

            Logout();
            Login(email, password);
            Logout();
        }
    }
}
