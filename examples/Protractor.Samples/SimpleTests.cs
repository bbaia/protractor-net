using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Chrome;

namespace Protractor.Samples
{
    [TestFixture]
    public class SimpleTests
    {
        private IWebDriver driver;

        [SetUp]
        public void SetUp()
        {
            // Using NuGet Package 'PhantomJS'
            driver = new PhantomJSDriver();

            // Using NuGet Package 'WebDriver.ChromeDriver.win32'
            //driver = new ChromeDriver();

            driver.Manage().Timeouts().SetScriptTimeout(TimeSpan.FromSeconds(5));
        }

        [TearDown]
        public void TearDown()
        {
            driver.Dispose();
        }

        [Test]
        public void ShouldGreetUsingBinding()
        {
            IWebDriver ngDriver = new NgWebDriver(driver);
            ngDriver.Url = "http://www.angularjs.org";
            ngDriver.FindElement(NgBy.Input("yourName")).SendKeys("Julie");
            Assert.AreEqual("Hello Julie!", ngDriver.FindElement(NgBy.Binding("{{yourName}}")).Text);
        }

        [Test]
        public void ShouldListTodos()
        {
            IWebDriver ngDriver = new NgWebDriver(driver);
            ngDriver.Url = "http://www.angularjs.org";
            var elements = ngDriver.FindElements(NgBy.Repeater("todo in todos"));
            Assert.AreEqual("build an angular app", elements[1].Text);
        }
    }
}
