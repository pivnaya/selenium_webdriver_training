using NUnit.Framework;
using OpenQA.Selenium;

namespace csharp_example
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class AdminPanelLoginTests : AdminPanelTestBase
    {
        [Test]
        public void AdminPanelLoginTest()
        {
            Login();
            Assert.IsTrue(appHelper.IsElementPresent(By.CssSelector("[title = Logout]")));
        }
    }
}
