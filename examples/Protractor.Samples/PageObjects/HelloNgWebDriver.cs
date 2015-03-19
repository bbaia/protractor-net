namespace Protractor.Samples.PageObjects
{
    using System;
    using NUnit.Framework;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;

    [TestFixture]
    public class NavigationTest
    {
        private IWebDriver _driver;
        private NgWebDriver _ngWebDriver;
        const string Url = "https://docs.angularjs.org/api";

        [TestFixtureSetUp]
        public void TestSetUp()
        {
            _driver = new ChromeDriver();

            //Meet the trick here
            _ngWebDriver = new NgWebDriver(_driver, "[ng-app='docsApp']");
            _ngWebDriver.Navigate().GoToUrl(Url);
            _ngWebDriver.Manage().Window.Maximize();
            //The script timeout is almost essential since most of protractor mechanism are dependent of client side script.
            _ngWebDriver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(10));

        }

        [TestFixtureTearDown]
        public void QuitDriver()
        {
            _ngWebDriver.Quit();
        }

        /// <summary>
        /// Sample test to demonstrate the use NgWebDriver with angular page.
        /// </summary>
        [Test]
        public void HelloNgDriver()
        {

            NgWebElement ngElement = _ngWebDriver.FindElement(NgBy.Model("q"));
            ngElement.Clear();
            ngElement.SendKeys("Hello NgWebDriver");
        }

        /// <summary>
        /// Sample test to demonstrate the use wrapper driver with angular and non-angular hybrid page.
        /// </summary>
        [Test]
        public void HelloNgWrapperDriver()
        {
            IWebElement element = _ngWebDriver.WrappedDriver.FindElement(By.CssSelector("[ng-change='search(q)']"));
            
            element.Clear();
            element.SendKeys("Hello Wrapper Driver");
        }
    }
}
