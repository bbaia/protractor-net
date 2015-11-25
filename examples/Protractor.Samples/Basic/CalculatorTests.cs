using System;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using System.Collections.ObjectModel;

// origin: https://github.com/anthonychu/Protractor-Net-Demo/tree/master/Protractor-Net-Demo

namespace Protractor.Test
{
    [TestFixture]
    public class CalculatorTests
    {
        private StringBuilder verificationErrors = new StringBuilder();
        private IWebDriver driver;
        private NgWebDriver ngDriver;
        private String base_url = "http://juliemr.github.io/protractor-demo/";

        [SetUp]
        public void SetUp()
        {
            driver = new PhantomJSDriver();
            driver.Manage().Timeouts().SetScriptTimeout(TimeSpan.FromSeconds(5));
            ngDriver = new NgWebDriver(driver);
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                driver.Quit();
            }
            catch (Exception) { } /* Ignore cleanup errors */
            Assert.AreEqual("", verificationErrors.ToString());
        }

        
                    
        [Test]
        public void ShouldSetUrl()
        {
            ngDriver.Url = base_url;
            StringAssert.AreEqualIgnoringCase(ngDriver.Title, "Super Calculator");
        }

        [Test]
        public void ShouldFindModel()
        {
            ngDriver.Navigate().GoToUrl(base_url);
            IWebElement element = ngDriver.FindElement(NgBy.Model("first"));
            Assert.IsTrue(((NgWebElement)element).Displayed);
        }

        [Test]
        public void ShouldFindByOptions()
        {
            ngDriver.Navigate().GoToUrl(base_url);
            ReadOnlyCollection<NgWebElement> elements = ngDriver.FindElements(NgBy.Options("value for (key, value) in operators"));
            Assert.AreEqual(((NgWebElement)elements[0]).Text, "+");
        }

        [Test]
        public void ShouldFindBySelectedOption()
        {
            ngDriver.Navigate().GoToUrl(base_url);
            IWebElement element = ngDriver.FindElement(NgBy.SelectedOption("operator"));
            Assert.AreEqual(((NgWebElement)element).Text, "+");
        }
        
        [Test]
        public void ShouldFindButtonText()
        {
            ngDriver.Navigate().GoToUrl(base_url);
            IWebElement element = ngDriver.FindElement(NgBy.ButtonText("Go!"));
            Assert.IsTrue(((NgWebElement)element).Displayed);
        }
        [Test]
        public void ShouldFindPartialButtonText()
        {
            ngDriver.Navigate().GoToUrl(base_url);
            IWebElement element = ngDriver.FindElement(NgBy.PartialButtonText("Go"));
            Assert.IsTrue(((NgWebElement)element).Displayed);
        }
        [Test]
        public void ShouldAdd()
        {
            ngDriver.Navigate().GoToUrl(base_url);
            var first = ngDriver.FindElement(NgBy.Input("first"));
            var second = ngDriver.FindElement(NgBy.Input("second"));
            var goButton = ngDriver.FindElement(By.Id("gobutton"));

            first.SendKeys("1");
            second.SendKeys("2");
            goButton.Click();
            var latestResult = ngDriver.FindElement(NgBy.Binding("latest")).Text;

            Assert.AreEqual(latestResult, "3");
        }

    }
}
