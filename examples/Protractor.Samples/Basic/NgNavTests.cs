using System;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;

using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.PhantomJS;
using System.Collections.ObjectModel;

namespace Protractor.Test
{
    [TestFixture]
    public class NgNavTests
    {
        private StringBuilder verificationErrors = new StringBuilder();
        private IWebDriver driver;
        private NgWebDriver ngDriver;
        private String base_url = "http://www.java2s.com/Tutorials/AngularJSDemo/n/ng_repeat_start_and_ng_repeat_end_example.htm";

        [SetUp]
        public void SetUp()
        {
            // driver = new InternetExplorerDriver();
            driver = new PhantomJSDriver();
            driver.Manage().Timeouts().SetScriptTimeout(TimeSpan.FromSeconds(60));
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
            Assert.IsEmpty(verificationErrors.ToString());
        }

        [Test]
        public void ShouldFindAllRepeaterRows()
        {
            ngDriver.Navigate().GoToUrl(base_url);
            ReadOnlyCollection<NgWebElement> elements = ngDriver.FindElements(NgBy.Repeater("definition in definitions"));
            Assert.IsTrue(elements[0].Displayed);
            StringAssert.AreEqualIgnoringCase(elements[0].Text, "Foo");
        }

    }
}
